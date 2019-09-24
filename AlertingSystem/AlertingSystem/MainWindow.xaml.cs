using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ViewModel;
using System.Threading.Tasks;

namespace WpfTutorialSamples.ItemsControl.ItemsControlDataBindingSample
{
    public partial class MainWindow : Window
    {
       public MainWindow()
        {
            InitializeComponent();
            ViewModels vmRef=new ViewModels();
            vmRef.GenerateVitals();
        }
    }

   
}