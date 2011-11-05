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

using System.ComponentModel.Composition;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;
using WorkBalance.Domain;

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow : Window
    {
        [Import]
        public IMessageBus MessageBus { get; set; }

        private IDisposable CreateActivityWindowOpenSubscription;

        public MainWindow()
        {
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
            CreateActivityWindowOpenSubscription = MessageBus.Listen<Action>(Notifications.CreateActivityWindowOpen)
                .ObserveOnDispatcher()
                .Subscribe(OpenCreateActivityWindow);

            MessageBus.Listen<Activity>(Notifications.Edit)
                    .ObserveOnDispatcher()
                    .Subscribe(EditActivity);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (CreateActivityWindowOpenSubscription != null)
            {
                CreateActivityWindowOpenSubscription.Dispose();
                CreateActivityWindowOpenSubscription = null;
            }

        }

        private void OpenCreateActivityWindow(Action callback)
        {
            var window = new WorkBalance.Windows.CreateActivityWindow()
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner,
                Owner = this
            };
            using (MessageBus.Listen<Unit>(Notifications.CreateActivityWindowClose)
                .ObserveOnDispatcher()
                .Subscribe(u => window.Close()))
            {
                window.ShowDialog();
            }
            callback();
        }

        private void EditActivity(Activity activity)
        {
            var window = new WorkBalance.Windows.EditActivityWindow()
            {
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner,
                Owner = this,
                Activity = activity
            };
            using (MessageBus.Listen<Unit>(Notifications.EditActivityWindowClose)
                .ObserveOnDispatcher()
                .Subscribe(o => window.Close()))
            {
                window.ShowDialog();
            }
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

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.SendMessage<Unit>(Unit.Default, Notifications.CopyActivitiesToClipboard);
        }
    }
}
