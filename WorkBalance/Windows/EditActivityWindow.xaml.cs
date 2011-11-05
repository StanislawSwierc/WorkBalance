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
using WorkBalance.Domain;
using WorkBalance.ViewModel;

namespace WorkBalance.Windows
{
    /// <summary>
    /// Interaction logic for CreateActivityWindow.xaml
    /// </summary>
    public partial class EditActivityWindow : Window
    {
        private EditActivityViewModel _ViewModel;

        public EditActivityWindow()
        {
            App.LoadStaticResources(this);
            InitializeComponent();
        }

        private void expectedEffortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var text = textBox.Text;
            bool modify = false;

            if (text.Length > 1)
            {
                text = text.Substring(0, 1);
                modify = true;
            }
            if (text.Length == 1 && (text[0] < '0' || '9' < text[0]))
            {
                text = string.Empty;
                modify = true;
            }
            if (modify)
            {
                textBox.Text = text;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) && _ViewModel == null)
            {
                _ViewModel = (EditActivityViewModel)this.DataContext;
                _ViewModel.Activity = Activity;
            }
            nameTextBox.Focus();
        }

        public Activity Activity { get; set; }
    }
}
