﻿using System;
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
            _vm = (ActivityInventoryViewModel)this.DataContext;
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
            _vm.SelectActivityCommand.Execute(null);
        }

        private void DeleteActivity_Click(object sender, RoutedEventArgs e)
        {
            _vm.DeleteActivityCommand.Execute(null);
        }

        private void ArchiveActivity_Click(object sender, RoutedEventArgs e)
        {
            _vm.ArchiveActivityCommand.Execute(null);
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (activitiesListBox.SelectedItems.Count >= 0)
            {
                var text = activitiesListBox.SelectedItems.OfType<Activity>().Aggregate(
                    new StringBuilder(),
                    (sb, a) =>
                    {
                        sb.AppendLine(string.Format("{0}\t{1}\t{2}", a.Name, a.ExpectedEffort, a.ActualEffort));
                        sb.AppendLine(string.Join(" ", (a.Tags ?? Enumerable.Empty<ActivityTag>()).Select(t => t.Name).ToArray()));
                        return sb;
                    },
                    sb => sb.ToString());

                System.Windows.Clipboard.SetText(text, TextDataFormat.Text);
            }

        }

        private void activitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // It would be better if two lists were compare against each other but that should also work.
            _vm.SelectedActivities = activitiesListBox.SelectedItems.Cast<Activity>().ToList();
        }

    }
}