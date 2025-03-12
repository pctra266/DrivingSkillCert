using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DrivingSkillCert.Commands
{
    internal class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public Action<object> _Excute {  get; set; }
        public Func<object, bool> _CanExcute { get; set; }
        public RelayCommand(Action<object> excute, Func<object, bool> canExcute)
        {
            _Excute = excute;
            _CanExcute = canExcute;
        }

        public bool CanExecute(object? parameter)
        {
            return _CanExcute(parameter);
        }

        public void Execute(object? parameter)
        {
            _Excute(parameter);
        }
    }
}
