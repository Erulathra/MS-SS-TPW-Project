using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TPW.Presentation.Model;

namespace TPW.Presentation.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel model;
        public AsyncObservableCollection<BallPosition> Circles { get; set; }

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

        private string debug
        public ICommand IncreaseButton { get; }
        public ICommand DecreaseButton { get; }
        public ICommand StartSimulationButton { get; }
        public ICommand StopSimulationButton { get; }

		public MainViewModel()
        {
            Circles = new AsyncObservableCollection<BallPosition>();

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

                for (int i = 0; i < BallsCount; i++)
                {
                    Circles.Add(new BallPosition());
                }

                model.BallPositionChange += (sender, argv) =>
                {
                    Circles[argv.ball.id].ChangePosition(argv.ball.Position);
                };
                model.StartSimulation();
            });
            StopSimulationButton = new RelayCommand(() =>
            {
                model.StopSimulation();
            });


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

public class AsyncObservableCollection<T> : ObservableCollection<T>
{
    private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

    public AsyncObservableCollection()
    {
    }

    public AsyncObservableCollection(IEnumerable<T> list)
        : base(list)
    {
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (SynchronizationContext.Current == _synchronizationContext)
        {
            // Execute the CollectionChanged event on the current thread
            RaiseCollectionChanged(e);
        }
        else
        {
            // Raises the CollectionChanged event on the creator thread
            _synchronizationContext.Send(RaiseCollectionChanged, e);
        }
    }

    private void RaiseCollectionChanged(object param)
    {
        // We are in the creator thread, call the base implementation directly
        base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (SynchronizationContext.Current == _synchronizationContext)
        {
            // Execute the PropertyChanged event on the current thread
            RaisePropertyChanged(e);
        }
        else
        {
            // Raises the PropertyChanged event on the creator thread
            _synchronizationContext.Send(RaisePropertyChanged, e);
        }
    }

    private void RaisePropertyChanged(object param)
    {
        // We are in the creator thread, call the base implementation directly
        base.OnPropertyChanged((PropertyChangedEventArgs)param);
    }
}

public class BallPosition
{
    private Vector2 pos;
    public float X
    {
        get { return pos.X; }
        set { pos.X = value; }
    }
    public float Y
    {
        get { return pos.Y; }
        set { pos.Y = value; }
    }

    public BallPosition(float x, float y)
	{
		X = x;
		Y = y;
    }
    public BallPosition(Vector2 position)
    {
        X = position.X;
        Y = position.Y;
    }

	public BallPosition()
	{
        X = 0;
        Y = 0;
	}

    public void ChangePosition(Vector2 position)
	{
        this.X = position.X;
        this.Y = position.Y;
	}
}