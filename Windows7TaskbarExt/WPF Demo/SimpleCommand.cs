namespace Huseyint.Windows7.WPF.Demo
{
    using System;
    using System.Windows.Input;

    // Source: http://marlongrech.wordpress.com/2008/11/26/avoiding-commandbinding-in-the-xaml-code-behind-files/
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Predicate<object> CanExecuteDelegate { get; set; }

        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteDelegate != null)
            {
                return this.CanExecuteDelegate(parameter);
            }

            return true;
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteDelegate != null)
            {
                this.ExecuteDelegate(parameter);
            }
        }
    }
}