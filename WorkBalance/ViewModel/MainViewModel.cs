
using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.ComponentModel.Composition;
using WorkBalance.Utilities;
using WorkBalance.Domain;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Xaml;
using System.Reactive.Concurrency;
using WorkBalance.Repositories;
using WorkBalance.Contracts;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace WorkBalance.ViewModel
{
    [Export]
    public class MainViewModel : ViewModelBase
    {
        [Import]
        public Timer Timer { get; set; }

        [ImportMany]
        IEnumerable<IPlugin> Plugins { get; set; }

        [Import]
        public ActivityInventoryViewModel ActivityInventory { get; set; }

        public MainViewModel()
        {
            // Translate state change notification and propagate it to the user interface
        }
    }
}