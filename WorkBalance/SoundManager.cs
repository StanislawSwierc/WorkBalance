using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace WorkBalance
{
    [Export(typeof(IObserver<TimerState>))]
    public class SoundManager : IObserver<TimerState>
    {
        private readonly MediaPlayer _MediaPlayer = new MediaPlayer();
        private readonly Uri c_AlarmSound = new Uri(@"Sounds\alarm-2.0.wma", UriKind.Relative);

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(TimerState value)
        {
            _MediaPlayer.Stop();

            switch (value)
            {
                case TimerState.Ready:
                    break;
                case TimerState.Sprint:
                    break;
                case TimerState.Break:
                    _MediaPlayer.Open(c_AlarmSound);
                    _MediaPlayer.Play();
                    break;
                case TimerState.BreakOverrun:
                    _MediaPlayer.Open(c_AlarmSound);
                    _MediaPlayer.Play();
                    break;
                default:
                    break;
            }
        }
    }
}
