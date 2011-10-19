using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

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
    }
}
