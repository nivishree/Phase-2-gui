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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BedConfiguration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new HomePageView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new HomePageView();
        }

       
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Main.Content = new AllotBedView();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Main.Content = new DischargePatientView();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Main.Content = new StartMonitorView();
        }
    }
}