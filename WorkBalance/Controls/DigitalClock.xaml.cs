using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics.Contracts;

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for DigitalClock.xaml
    /// </summary>
    public partial class DigitalClock : UserControl
    {
        private static readonly int[] SevenSegmentDisplayCode = new int[] { 0x7E, 0x30, 0x6D, 0x79, 0x33, 0x5B, 0x5F, 0x70, 0x7F, 0x7B };

        #region Properties

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(DigitalClock), new FrameworkPropertyMetadata(TimeSpan.MinValue, OnTimeChanged));


        public bool ShowInactiveSegments
        {
            get { return (bool)GetValue(ShowInactiveSegmentsProperty); }
            set { SetValue(ShowInactiveSegmentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowInactiveSegments.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowInactiveSegmentsProperty =
            DependencyProperty.Register("ShowInactiveSegments", typeof(bool), typeof(DigitalClock), new FrameworkPropertyMetadata(true, OnShowInactiveSegmentsChanged));



        public Brush InactiveSegmentsBrush
        {
            get { return (Brush)GetValue(InactiveSegmentsBrushProperty); }
            set { SetValue(InactiveSegmentsBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InactiveSegmentsBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveSegmentsBrushProperty =
            DependencyProperty.Register("InactiveSegmentsBrush", typeof(Brush), typeof(DigitalClock), new UIPropertyMetadata());


        public double GlowRadius
        {
            get { return (double)GetValue(GlowRadiusProperty); }
            set { SetValue(GlowRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GlowRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GlowRadiusProperty =
            DependencyProperty.Register("GlowRadius", typeof(double), typeof(DigitalClock), new UIPropertyMetadata(10.0));

        #endregion

        public DigitalClock()
        {
            this.InitializeComponent();

            m10 = new Path[7];
            m10[0] = m10g;
            m10[1] = m10f;
            m10[2] = m10e;
            m10[3] = m10d;
            m10[4] = m10c;
            m10[5] = m10b;
            m10[6] = m10a;

            m1 = new Path[8];
            m1[0] = m1g;
            m1[1] = m1f;
            m1[2] = m1e;
            m1[3] = m1d;
            m1[4] = m1c;
            m1[5] = m1b;
            m1[6] = m1a;

            s10 = new Path[7];
            s10[0] = s10g;
            s10[1] = s10f;
            s10[2] = s10e;
            s10[3] = s10d;
            s10[4] = s10c;
            s10[5] = s10b;
            s10[6] = s10a;

            s1 = new Path[8];
            s1[0] = s1g;
            s1[1] = s1f;
            s1[2] = s1e;
            s1[3] = s1d;
            s1[4] = s1c;
            s1[5] = s1b;
            s1[6] = s1a;
        }

        private Path[] m10;
        private Path[] m1;
        private Path[] s10;
        private Path[] s1;

        private static void OnShowInactiveSegmentsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var clock = (DigitalClock)sender;
            var value = (bool)e.NewValue;

            clock.Inactive.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        private static void OnTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var clock = (DigitalClock)sender;
            var time = (TimeSpan)e.NewValue;

            UpdateDisplay(Math.Abs(time.Minutes) / 10, clock.m10);
            UpdateDisplay(Math.Abs(time.Minutes) % 10, clock.m1);
            UpdateDisplay(Math.Abs(time.Seconds) / 10, clock.s10);
            UpdateDisplay(Math.Abs(time.Seconds) % 10, clock.s1);
        }

        private static void UpdateDisplay(int number, Path[] display)
        {
            Contract.Requires(number >= 0);
            Contract.Requires(number <= 9);
            Contract.Requires(display != null);
            Contract.Requires(display.Length == 7);

            int code = SevenSegmentDisplayCode[number];
            for (int i = 0; i < 7; i++)
            {
                display[i].Visibility = ((code & 1) == 1) ? Visibility.Visible : Visibility.Hidden;
                code >>= 1;
            }
        }
    }
}