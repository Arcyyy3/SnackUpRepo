// App.xaml.cs
using Syncfusion.Licensing;
using System.Diagnostics;

namespace SnackUpClient
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                System.Diagnostics.Debug.WriteLine("App Initialized");

                SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCfEx3QHxbf1x1ZFNMYlVbQXRPIiBoS35Rc0ViWHlfeHZcQ2RfUUVx");
                System.Diagnostics.Debug.WriteLine("Syncfusion License Registered");

                MainPage = new AppShell();
                System.Diagnostics.Debug.WriteLine("MainPage set to AppShell");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during App initialization: {ex}");
                throw;
            }
        }
    }
}
