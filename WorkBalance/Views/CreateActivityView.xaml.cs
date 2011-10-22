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

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for CreateActivityView.xaml
    /// </summary>
    public partial class CreateActivityView : UserControl
    {
        public CreateActivityView()
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
        
    }
}