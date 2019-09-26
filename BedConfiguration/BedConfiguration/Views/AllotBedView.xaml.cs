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
using AllotBedModuleLib;
using static AllotBedModuleLib.AllotBedViewModel;

namespace BedConfiguration.Views
{
    /// <summary>
    /// Interaction logic for AllotBedView.xaml
    /// </summary>
    public partial class AllotBedView : Page
    {
        public AllotBedView()
        {
            InitializeComponent();
        }

        public string PatientId { get; private set; }


        private void Submit_button_Copy_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate("HomePageView.xaml");
        }
    }
}
