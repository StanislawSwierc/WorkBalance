using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

namespace WorkBalance
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ViewModelLocator m_Locator;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            m_Locator = (ViewModelLocator)Resources["Locator"];
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            m_Locator.Dispose();
        }

        /// <summary>
        /// Loads static resources to the Resources collection of the element
        /// </summary>
        /// <remarks>
        /// http://stackoverflow.com/questions/3317902/resource-with-the-name-locator-cannot-be-found-error-when-using-mvvm-light-u
        /// </remarks>
        /// <param name="element">Element where the resources need to be loaded</param>
        public static void LoadStaticResources(Control element)
        {
            if (DesignerProperties.GetIsInDesignMode(element))
            {
                if (AppDomain.CurrentDomain.BaseDirectory.Contains("Blend 4"))
                {
                    element.Resources.Add("Locator", new ViewModelLocator());
                }
            } 
        }
    }
}
