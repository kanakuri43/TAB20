using Prism.Ioc;
using System.Windows;
using TAB20.Views;

namespace TAB20
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<JournalEditor>();
            containerRegistry.RegisterForNavigation<FinancialStatements>();
        }
    }
}
