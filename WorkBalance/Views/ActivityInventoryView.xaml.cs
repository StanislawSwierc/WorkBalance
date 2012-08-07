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
using System.ComponentModel.Composition;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for ActivityInventoryView.xaml
    /// </summary>
    public partial class ActivityInventoryView : UserControl
    {
        public ActivityInventoryViewModel ViewModel { get { return (ActivityInventoryViewModel) this.DataContext; } }

        public ActivityInventoryView()
        {
            App.LoadStaticResources(this);
            this.InitializeComponent();
            activitiesListBox.SelectionChanged += activitiesListBox_SelectionChanged;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) )
            {
                // Invoke once to initialize ViewModel
                activitiesListBox_SelectionChanged(null, null);
            }
        }

        private void activitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                // It would be better if two lists were compare against each other but that should also work.
                ViewModel.SelectedActivities = activitiesListBox.SelectedItems.Cast<Activity>().ToList();
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.DeleteActivityCommand.Execute(null);
        }
    }
}