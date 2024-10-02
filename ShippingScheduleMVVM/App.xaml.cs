using ShippingScheduleMVVM.Models;
using ShippingScheduleMVVM.ViewModels; 
using ShippingScheduleMVVM.Views; 
using System;  
using System.Runtime; 
using System.Windows;  

namespace ShippingScheduleMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string db_username = ShippingScheduleMVVM.Properties.Settings.Default.db_username;
        public static readonly string db_table = ShippingScheduleMVVM.Properties.Settings.Default.db_table;
        public static readonly string db_ip = ShippingScheduleMVVM.Properties.Settings.Default.db_ip;
        public static readonly string db_password = ShippingScheduleMVVM.Properties.Settings.Default.db_password;
        public static readonly string db_connectionString = $"Data Source={db_ip};Initial Catalog={db_table};User Id={db_username};Password={db_password};Persist Security Info=True;TrustServerCertificate=True;Connection Timeout=10";

        public static User loggedUser = new();
        public static double InitialDPI = 1;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CheckLogin();
        }
        
        public App()
        {
            // Defines where to store JIT profiles
            ProfileOptimization.SetProfileRoot(@"C:\temp");

            // Enables Multicore JIT with the specified profile
            ProfileOptimization.StartProfile("Startup.Profile");
        }

        private void CheckLogin()
        {
            if (App.loggedUser.GetSavedUserInformation())
            {
                Tuple<bool, string, User?> response = loggedUser.Login();

                if (response.Item1 == true)
                {
                    if (response.Item3 != null) { loggedUser = response.Item3; }
                    MainWindow mainWindow = new MainWindow()
                    {
                        DataContext = new MainWindowViewModel(),
                    };

                    MainWindow = mainWindow;
                    MainWindow.Show();
                }

                if (response.Item1 == false)
                {
                    MainWindow = new LoginWindow()
                    {
                        DataContext = new LoginViewModel(),
                    };
                    MainWindow.Show();
                }
            }
            else
            {
                MainWindow = new LoginWindow()
                {
                    DataContext = new LoginViewModel(),
                };
                MainWindow.Show();
            }
        }
    }
}
