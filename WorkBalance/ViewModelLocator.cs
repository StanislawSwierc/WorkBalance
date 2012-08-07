/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:WorkBalance"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/


using MefContrib.Hosting;
using WorkBalance.Domain;
using WorkBalance.ViewModel;
using System;
using System.Linq;
using WorkBalance.Repositories;
using System.Reflection;
using System.Collections.Generic;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using WorkBalance.Utilities;
using ReactiveUI;

namespace WorkBalance
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator : IDisposable
    {
        private const string c_Storage = "WorkBalance.db4o";
        private CompositionContainer m_Container;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                m_Container = new CompositionContainer(
                    new DesignTimeCatalog(
                        new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly())));
            }
            else
            {
#if Db4o
                Db4objects.Db4o.Db4oFactory.Configure().CallConstructors(true);
                var server = Db4objects.Db4o.CS.Db4oClientServer.OpenServer(c_Storage, 0);
                var factory = new Func<ExportProvider, IDomainContext>(
                    ep => new Domain.Db4o.Db4oDomainContext(server.OpenClient()));
#elif Ef
                var factory = new DelegateDomainContextFactory(() => new Domain.Ef.EfDomainContext());
#endif

                var provider = new FactoryExportProvider();
                provider.Register<IDomainContext>(factory);

                m_Container = new CompositionContainer(
                    new DesignTimeCatalog(
                        new AggregateCatalog(
                            new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()),
                            new DirectoryCatalog("Plugins"))),
                    provider);

            }
            m_Container.ComposeExportedValue<IMessageBus>(new MessageBus());

        }

        public void Dispose()
        {
            if (m_Container != null)
            {
                m_Container.Dispose();
            }
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        public MainViewModel Main { get { return m_Container.GetExportedValue<MainViewModel>(); } }

        public ActivityInventoryViewModel ActivityInventory { get { return m_Container.GetExportedValue<ActivityInventoryViewModel>(); } }

        public CreateActivityViewModel CreateActivity { get { return m_Container.GetExportedValue<CreateActivityViewModel>(); } }

        public EditActivityViewModel EditActivity { get { return m_Container.GetExportedValue<EditActivityViewModel>(); } }

        public MainWindow MainWindow { get { return m_Container.GetExportedValue<MainWindow>(); } }

        public HistoryViewModel History { get { return m_Container.GetExportedValue<HistoryViewModel>(); } }
    }
}