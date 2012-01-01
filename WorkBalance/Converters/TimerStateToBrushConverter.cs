using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Diagnostics.Contracts;

namespace WorkBalance.Converters
{
    class TimerStateToBrushConverter : IValueConverter
    {
        #region Properties

        public Brush ReadyBrush { get; set; }
        public Brush SprintBrush { get; set; }
        public Brush HomeStraightBrush { get; set; }
        public Brush BreakBrush { get; set; }
        public Brush BreanOverrunBrush { get; set; }

        #endregion

        #region IValueConverter Implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // TODO: Comment out that contract
            Contract.Requires(value != null);
            Contract.Requires(value is TimerState);
            Contract.Requires(targetType.IsAssignableFrom(typeof(Brush)));

            TimerState state = (TimerState)value;
            return Convert(state);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        private Brush Convert(TimerState state)
        {
            Brush result = null;
            switch (state)
            {
                case TimerState.Ready:
                    result = ReadyBrush;
                    break;
                case TimerState.Sprint:
                    result = SprintBrush;
                    break;
                case TimerState.HomeStraight:
                    result = HomeStraightBrush;
                    break;
                case TimerState.Break:
                    result = BreakBrush;
                    break;
                case TimerState.BreakOverrun:
                    result = BreanOverrunBrush;
                    break;
                default:
                    break;
            }
            return result;
        }

        #endregion
    }
}
