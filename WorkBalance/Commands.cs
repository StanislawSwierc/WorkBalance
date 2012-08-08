using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel.Composition;
using ReactiveUI;
using ReactiveUI.Xaml;
using System.Reactive;
using WorkBalance.Utilities;

namespace WorkBalance
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Commands: IPartImportsSatisfiedNotification
    {
        [Import]
        public IMessageBus MessageBus;

        [KeyGestureCommandExport(Key.T, ModifierKeys.Control)]
        public ReactiveCommand ToggleTimerCommand { get; private set; }

        public void OnImportsSatisfied()
        {
            ToggleTimerCommand = new ReactiveCommand();
            ToggleTimerCommand.Subscribe(o => MessageBus.SendMessage<Unit>(Unit.Default, Notifications.ToggleTimer));
        }
    }


    
}
