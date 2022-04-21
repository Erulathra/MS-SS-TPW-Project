using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TPW.Presentation.Model;

namespace TPW.Presentation.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel model;
        public ObservableCollection<Vector2> Circles { get; set; }

        public int BallsCount
        {
            get { return model.GetBallsCount(); }
            set
            {
                if (value >= 0)
                {
                    model.SetBallNumber(value);
                    OnPropertyChanged();
                }
            }
        }
        public ICommand IncreaseButton { get; }
        public ICommand DecreaseButton { get; }
        public ICommand StartSimulationButton { get; }
        public ICommand StopSimulationButton { get; }

        public MainViewModel()
        {
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
                model.StartSimulation();
            });
            StopSimulationButton = new RelayCommand(() =>
            {
                model.StopSimulation();
            });

            Circles = new ObservableCollection<Vector2>();

            //TODO: TEST
            Circles.Add(new Vector2(50, 50));
        }

        // Event for View update
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}


public class RelayCommand : ICommand
{
    private readonly Action _handler;
    private bool _isEnabled;

    public RelayCommand(Action handler)
    {
        _handler = handler;
        IsEnabled = true;
    }

    public bool IsEnabled
    {
        get { return _isEnabled; }
        set
        {
            if (value != _isEnabled)
            {
                _isEnabled = value;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
    }

    public bool CanExecute(object parameter)
    {
        return IsEnabled;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
        _handler();
    }
}