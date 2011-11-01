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
		public ActivityInventoryView()
		{
            App.LoadStaticResources(this);
			this.InitializeComponent();
		}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void Button_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var vm = DataContext as ActivityInventoryViewModel;
            if (vm != null)
            {
                var activity = (Activity)activitiesListBox.SelectedItem;
                vm.SelectActivityCommand.Execute(activity);
            }
        }

        private void DeleteActivity_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ActivityInventoryViewModel;
            if (vm != null)
            {
                var activity = (Activity)activitiesListBox.SelectedItem;
                vm.DeleteActivityCommand.Execute(activity);
            }
        }

        private void ArchiveActivity_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ActivityInventoryViewModel;
            if (vm != null)
            {
                var activity = (Activity)activitiesListBox.SelectedItem;
                vm.ArchiveActivityCommand.Execute(activity);
            }
        }
        
	}
}