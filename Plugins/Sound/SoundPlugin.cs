using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using WorkBalance.Contracts;
using WorkBalance;
using System.Media;
using System.IO;
using System.Windows.Media;

namespace Sound
{
    [Export(typeof(IPlugin))]
    public class SoundPlugin : IPlugin, IPartImportsSatisfiedNotification
    {
        [Import]
        ITimer Timer { get; set; }

        MediaPlayer _player;
        Uri _alarmSound;

        public void OnImportsSatisfied()
        {
            Timer.StateChanged += HandleStateChanged;
            _player = new MediaPlayer();
            _alarmSound = new Uri(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Sounds", "alarm-2.0.wma"));
        }

        void HandleStateChanged(object sender, TimerStateChangedEventArgs e)
        {
            switch (e.Transition)
            {
                //case TimerStateTransition.ReadyToSprint:
                //    break;
                //case TimerStateTransition.SprintToReady:
                //    break;
                //case TimerStateTransition.SprintToHomeStraight:
                //    break;
                //case TimerStateTransition.HomeStraightToReady:
                //    break;
                case TimerStateTransition.HomeStraightToBreak:
                    _player.Open(_alarmSound);
                    _player.Play();
                    break;
                //case TimerStateTransition.BreakToReady:
                //    break;
                //case TimerStateTransition.BreakToBreakOverrun:
                //    break;
                //case TimerStateTransition.BreakOverrunToReady:
                //    break;
                default:
                    break;
            }
        }
    }
}
