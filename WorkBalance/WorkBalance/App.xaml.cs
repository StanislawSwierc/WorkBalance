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
