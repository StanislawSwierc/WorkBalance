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

using GalaSoft.MvvmLight;
using WorkBalance.ViewModel;
using System;
using WorkBalance.Repositories;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using GalaSoft.MvvmLight.Messaging;

namespace WorkBalance
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator: IDisposable
    {
        private const string c_Storage = "WorkBalance.db4o";
        private IUnityContainer m_Container;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time services and viewmodels
            }
            else
            {
                // Code runs "for real"
            }
            m_Container = CreateContainer();
        }

        private IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Register repositories
                container.RegisterType<IActivityRepository, Repositories.Design.ActivityRepository>();
                container.RegisterType<IActivityTagRepository, Repositories.Design.ActivityTagRepository>();
                container.RegisterType<IInterruptionRepository, Repositories.Design.InterruptionRepository>();
                container.RegisterType<IInterruptionRecordRepository, Repositories.Design.InterruptionRecordRepository>();
                container.RegisterType<IInterruptionTagRepository, Repositories.Design.InterruptionTagRepository>();
                container.RegisterType<ISprintRepository, Repositories.Design.SprintRepository>();
            }
            else
            {
                // Register repositories
                container.RegisterInstance<Db4objects.Db4o.IObjectContainer>(Db4objects.Db4o.Db4oFactory.OpenFile(c_Storage));
                container.RegisterType<IActivityRepository, Repositories.Db4o.ActivityRepository>();
                container.RegisterType<IActivityTagRepository, Repositories.Db4o.ActivityTagRepository>();
                container.RegisterType<IInterruptionRepository, Repositories.Db4o.InterruptionRepository>();
                container.RegisterType<IInterruptionRecordRepository, Repositories.Db4o.InterruptionRecordRepository>();
                container.RegisterType<IInterruptionTagRepository, Repositories.Db4o.InterruptionTagRepository>();
                container.RegisterType<ISprintRepository, Repositories.Db4o.SprintRepository>();
            }
            
            // Register messenger
            container.RegisterInstance<IMessenger>(new Messenger());

            // Define the lifetime of the ViewModels
            container.RegisterType<MainViewModel>(new ContainerControlledLifetimeManager());
            
            return container;
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
        public MainViewModel Main { get { return m_Container.Resolve<MainViewModel>(); } }
    }
}