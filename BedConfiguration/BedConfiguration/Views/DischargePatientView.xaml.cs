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

namespace BedConfiguration.Views
{
    /// <summary>
    /// Interaction logic for DischargePatientView.xaml
    /// </summary>
    public partial class DischargePatientView : Page
    {
        public DischargePatientView()
        {
            InitializeComponent();
        }

        private void Submit_button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate("HomePageView.xaml");

        }
    }
}
