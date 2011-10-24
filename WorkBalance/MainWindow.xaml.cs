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
using WorkBalance.Windows;
using WorkBalance.ViewModel;
using WorkBalance.Utilities;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel.Composition;

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow : Window
    {
        private readonly IMessenger m_Messenger;

        [ImportingConstructor]
        public MainWindow(IMessenger messenger)
        {
            m_Messenger = messenger;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_Messenger.Register(this, Notifications.CreateActivityWindowOpen, OpenCreateActivityWindow);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            m_Messenger.Unregister(this, Notifications.CreateActivityWindowOpen, OpenCreateActivityWindow);
        }

        private void OpenCreateActivityWindow()
        {
            var window = new WorkBalance.Windows.CreateActivityWindow();
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            window.Owner = this;
            Action action = () => window.Close();
            m_Messenger.Register(this, Notifications.CreateActivityWindowClose, action);
            window.ShowDialog();
            m_Messenger.Unregister(this, Notifications.CreateActivityWindowClose, action);
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CreateActivity_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.CreateActivityCommand.Execute(this);
            }
        }
    }
}
