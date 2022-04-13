using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace TPW.Presentation.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region code responsible for notifying View about changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private string title;
        public string Title { 
            get { return title; } 
            set 
            { 
                title = value; 
                OnPropertyChanged(); 
            } 
        }

        public ICommand HelloButtonClick { get; set; }


        public MainViewModel()
        {
            Title = "😺";
            HelloButtonClick = new RelayCommand(() =>
            {
                this.Title = "Hello!";
            });
        }
    }
}

// Frequently used in mvvm model
public class RelayCommand : ICommand
{
    private readonly Action _handler;
    private bool _isEnabled;

    public RelayCommand(Action handler)
    {
        _handler = handler;
        _isEnabled = true;
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