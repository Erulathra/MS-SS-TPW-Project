using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TPW.Presentation.Model;

namespace TPW.Presentation.ViewModel
{
   //TODO: Michalina - When simulation is started, disable star simulation button.
   sealed public class MainViewModel : INotifyPropertyChanged
   {
      private readonly MainModel model;

      public MainViewModel()
      {
         Circles = new AsyncObservableCollection<ViewModelBallDecorator>();

         model = new MainModel();
         BallsCount = 5;

         IncreaseButton = new RelayCommand(() =>
         {
            BallsCount += 1;
         });
         DecreaseButton = new RelayCommand(() =>
         {
            BallsCount -= 1;
         });

         StartSimulationButton = new RelayCommand(() =>
         {
            model.SetBallNumber(BallsCount);

            for (var i = 0; i < BallsCount; i++)
            {
               Circles.Add(new ViewModelBallDecorator());
            }

            model.BallPositionChange += (sender, args) =>
            {
               if (Circles.Count <= 0) return;

               for (var i = 0; i < BallsCount; i++)
               {
                  Circles[args.SenderBall.ID].Position = args.SenderBall.Position;
                  Circles[args.SenderBall.ID].Radius = args.SenderBall.Radius;
               }
            };
            model.StartSimulation();
         });

         StopSimulationButton = new RelayCommand(() =>
         {
            model.StopSimulation();
            Circles.Clear();
            model.SetBallNumber(BallsCount);
         });
      }

      public AsyncObservableCollection<ViewModelBallDecorator> Circles { get; set; }

      public int BallsCount
      {
         get { return model.GetBallsCount(); }
         private set
         {
            if (value >= 0)
            {
               model.SetBallNumber(value);
               this.OnPropertyChanged();
            }
         }
      }

      public ICommand IncreaseButton { get; }
      public ICommand DecreaseButton { get; }
      public ICommand StartSimulationButton { get; }
      public ICommand StopSimulationButton { get; }

      // Event for View update
      public event PropertyChangedEventHandler? PropertyChanged;

      private void OnPropertyChanged([CallerMemberName] string caller = "")
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
      }
   }
}


public class RelayCommand : ICommand
{
   private readonly Action handler;
   private bool isEnabled;

   public RelayCommand(Action handler)
   {
      this.handler = handler;
      IsEnabled = true;
   }

   public bool IsEnabled
   {
      get { return isEnabled; }
      set
      {
         if (value != isEnabled)
         {
            isEnabled = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
         }
      }
   }

   public bool CanExecute(object parameter)
   {
      return IsEnabled;
   }

   public event EventHandler? CanExecuteChanged;

   public void Execute(object parameter)
   {
      handler();
   }
}