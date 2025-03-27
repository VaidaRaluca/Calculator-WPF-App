using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Predicate<T>? canExecute;

        public RelayCommand(Action<T> execute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute)); ;
            this.canExecute = null;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
           this.execute = execute ?? throw new ArgumentNullException(nameof(execute)); ;
           this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || (parameter is T typedParam && canExecute(typedParam));
        }
        public void Execute(object? parameter)
        {
            if (execute == null) return;

            if (parameter is T typedParam)
            {
                execute(typedParam);
            }
            else if (parameter == null && typeof(T).IsClass) 
            {
                execute(default!);
            }
            else
            {
                throw new ArgumentException($"Invalid parameter type. Expected {typeof(T)}, received {parameter?.GetType()}");
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

}
