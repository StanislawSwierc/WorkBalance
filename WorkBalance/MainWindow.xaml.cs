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
using MahApps.Metro.Controls;
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
    [Export(typeof(ICreateActivityService))]
    public partial class MainWindow : MetroWindow, IPartImportsSatisfiedNotification, IEditActivityService, ICreateActivityService
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (CreateActivityWindowOpenSubscription != null)
            {
                CreateActivityWindowOpenSubscription.Dispose();
                CreateActivityWindowOpenSubscription = null;
            }
        }

        private bool? ShowCustomDialog(Window window)
        {
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            window.Owner = this;
            VisualStateManager.GoToElementState(LayoutRoot, "Disabled", true);
            var result = window.ShowDialog();
            VisualStateManager.GoToElementState(LayoutRoot, "Enabled", true);
            return result;
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

        #region Implementation of ICreateActivityService

        public Activity CreateActivity(IDomainContext context)
        {
            Activity activity = null;
            var window = new CreateActivityWindow();
            window.ViewModel.DomainContext = context;
            var dialogResult = ShowCustomDialog(window);
            if(dialogResult.HasValue && dialogResult.Value)
            {
                activity = window.ViewModel.Activity;
            }
            return activity;
        }

        #endregion
    }
}
