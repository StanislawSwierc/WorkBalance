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

namespace WorkBalance.Windows
{
    /// <summary>
    /// Interaction logic for CreateActivityWindow.xaml
    /// </summary>
    public partial class CreateActivityWindow : Window
    {
        public CreateActivityWindow()
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
            nameTextBox.Focus();
        }  
    }
}
