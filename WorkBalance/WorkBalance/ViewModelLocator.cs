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

namespace WorkBalance
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator: IDisposable
    {
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
                m_Container = CreateContainer();
                var ar = m_Container.Resolve<IActivityRepository>();
            }

            Main = new MainViewModel(null);
        }

        private IUnityContainer CreateContainer()
        {
            var result = new UnityContainer();
            m_Container.RegisterInstance<Db4objects.Db4o.IObjectContainer>(Db4objects.Db4o.Db4oFactory.OpenFile("WorkBalance.db4o"));
            m_Container.RegisterType<IActivityRepository, Repositories.Db4o.ActivityRepository>();
            m_Container.RegisterType<IActivityTagRepository, Repositories.Db4o.ActivityTagRepository>();
            m_Container.RegisterType<IInterruptionRepository, Repositories.Db4o.InterruptionRepository>();
            m_Container.RegisterType<IInterruptionRecordRepository, Repositories.Db4o.InterruptionRecordRepository>();
            m_Container.RegisterType<IInterruptionTagRepository, Repositories.Db4o.InterruptionTagRepository>();
            m_Container.RegisterType<ISprintRepository, Repositories.Db4o.SprintRepository>();
            return result;
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        public MainViewModel Main {get; private set;}

        public void Dispose()
        {
            if (m_Container != null)
            {
                m_Container.Dispose();
            }
        }
    }
}