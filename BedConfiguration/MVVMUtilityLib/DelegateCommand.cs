using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMUtilityLib
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _executeMethodAddress;
        private Func<object, bool> _canExecuteMethodAddress;

        public DelegateCommand(Action<object> executeMethodAddress, Func<object, bool> canExecuteMethodAddress)
        {
            this._executeMethodAddress = executeMethodAddress;
            this._canExecuteMethodAddress = canExecuteMethodAddress;
        }

        public DelegateCommand(Action<object> launchApp)
        {
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this._canExecuteMethodAddress.Invoke(parameter);
        }
        //CanExecute() will check that the command can be triggered or not. After getting true only Execute() will run

        void ICommand.Execute(object parameter)
        {
            this._executeMethodAddress.Invoke(parameter);
        }
    }
}
