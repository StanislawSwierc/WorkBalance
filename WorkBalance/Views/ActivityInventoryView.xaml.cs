using System;
using System.Linq;
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
using System.ComponentModel;
using WorkBalance.ViewModel;
using WorkBalance.Domain;

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for ActivityInventoryView.xaml
    /// </summary>
    public partial class ActivityInventoryView : UserControl
    {
        private ActivityInventoryViewModel _vm;

        public ActivityInventoryView()
        {
            App.LoadStaticResources(this);
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
             if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
             {
                 _vm = (ActivityInventoryViewModel)this.DataContext;
             }
        }

        private void Button_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _vm.SelectActivityCommand.Execute(null);
        }

        private void activitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // It would be better if two lists were compare against each other but that should also work.
            _vm.SelectedActivities = activitiesListBox.SelectedItems.Cast<Activity>().ToList();
        }

    }
}