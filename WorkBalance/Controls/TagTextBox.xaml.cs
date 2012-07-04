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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkBalance.Controls
{
    /// <summary>
    /// Interaction logic for TagTextBox.xaml
    /// </summary>
    public partial class TagTextBox : UserControl
    {
        private bool _ignoreOnTagsChanged;
        public TagTextBox()
        {
            InitializeComponent();
            textBox.TextChanged += OnTextChanged;
        }

        public string[] Tags
        {
            get { return (string[])GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tags.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register("Tags", typeof(string[]), typeof(TagTextBox), new UIPropertyMetadata(new string[0], OnTagsChanged));

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;

            _ignoreOnTagsChanged = true;
            Tags = string.IsNullOrWhiteSpace(text)
                ? new string[0]
                : text.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            _ignoreOnTagsChanged = false;

        }

        public static void OnTagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = (TagTextBox)d;
            if (!sender._ignoreOnTagsChanged)
            {
                sender.textBox.Text = sender.Tags != null ? string.Join(" ", sender.Tags) : string.Empty;
            }
        }
    }
}
