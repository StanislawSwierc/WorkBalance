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
using WorkBalance.Services;
using WorkBalance.Windows;
using WorkBalance.ViewModel;
using WorkBalance.Utilities;

using System.ComponentModel.Composition;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;
using WorkBalance.Domain;
using WorkBalance.Views;

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    [Export(typeof(IEditActivityService))]
    public partial class MainWindow : Window, IPartImportsSatisfiedNotification, IEditActivityService
    {
        [Import]
        public IMessageBus MessageBus { get; set; }

        [ImportMany]
        Lazy<ICommand, IKeyGestureCommandMetadata>[] Commands { get; set; }

        private IDisposable CreateActivityWindowOpenSubscription;
        private HistoryWindow HistoryWindow;

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
            CreateActivityWindowOpenSubscription = MessageBus.Listen<Unit>(Notifications.CreateActivity)
                .ObserveOnDispatcher()
                .Subscribe(o => OpenCreateActivityWindow());
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (CreateActivityWindowOpenSubscription != null)
            {
                CreateActivityWindowOpenSubscription.Dispose();
                CreateActivityWindowOpenSubscription = null;
            }
        }

        private void OpenCreateActivityWindow()
        {
            var window = new WorkBalance.Windows.CreateActivityWindow();
            ShowCustomDialog(window);
        }

        private void ShowCustomDialog(Window window)
        {
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            window.Owner = this;
            VisualStateManager.GoToElementState(LayoutRoot, "Disabled", true);
            window.ShowDialog();
            VisualStateManager.GoToElementState(LayoutRoot, "Enabled", true);
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBus.SendMessage<Unit>(Unit.Default, Notifications.CopyActivitiesToClipboard);
        }

        private void OpenHistoryWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (HistoryWindow == null)
            {
                HistoryWindow = new HistoryWindow();
                HistoryWindow.Closed += (s, a) => HistoryWindow = null;
                HistoryWindow.Show();
            }
            else
            {
                HistoryWindow.Activate();
            }
            
        }

        public void OnImportsSatisfied()
        {
            foreach (var command in Commands)
            {
                this.InputBindings.Add(new InputBinding(command.Value, new KeyGesture(command.Metadata.Key, command.Metadata.ModifierKeys)));
            }
        }

        #region Implementation of IEditActivityService

        public void EditActivity(IDomainContext context, Activity activity)
        {
            var window = new EditActivityWindow();
            window.ViewModel.DomainContext = context;
            window.ViewModel.Activity = activity;
            ShowCustomDialog(window);
        }

        #endregion
    }
}
