using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using WorkBalance.ViewModel;

namespace WorkBalance.Views
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            App.LoadStaticResources(this);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Select current date
            this.callendar.SelectedDate = DateTime.Now.Date;

            //TODO: Move this to user settings
            dynamicFilterCheckBox.IsChecked = true;

            // Bind callendar to the ViewModel
            var vm = (HistoryViewModel)this.DataContext;
            vm.DatesFilter = this.callendar.SelectedDates;
        }
    }
}
