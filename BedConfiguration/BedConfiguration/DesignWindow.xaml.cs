using BedConfiguration.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BedConfiguration
{
    /// <summary>
    /// Interaction logic for DesignWindow.xaml
    /// </summary>
    public partial class DesignWindow : Window
    {
        public DesignWindow()
        {
            InitializeComponent();
            Main.NavigationService.Navigate(new HomePageView());
        }

        private void GotoHome(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new HomePageView());
        }

        private void GotoAllotBed(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new AllotBedView());
        }

        private void GotoDischargePatient(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new DischargePatientView());
        }

        private void GotoPatientMonitoring(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new StartMonitorView());
        }

    }
}
