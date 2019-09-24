using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Diagnostics;
using MVVMUtilityLib;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class Alert : System.ComponentModel.INotifyPropertyChanged


    {
        public int Value1()
        {
            ViewModels obj = new ViewModels();
           return obj.GenerateVitals();
            
        }

        private ObservableCollection<ProcessShortcut> shortcuts;
        public ObservableCollection<ProcessShortcut> Shortcuts
        {
            get { return this.shortcuts; }
            set
            {
                {
                    this.shortcuts = value;
                    this.OnPropertyChanged("Shortcuts");
                }
            }
        }
        public Alert()
        {
            ClickCommand = new DelegateCommand((object obj) => { this.Click(Value1(), 6); }, (object obj) => { return true; });
            //ClickCommand1 = new DelegateCommand((object obj) => { this.Click(value2, 5); }, (object obj) => { return true; });
            //ClickCommand2 = new DelegateCommand((object obj) => { this.Click(value3, 4); }, (object obj) => { return true; });
            //ClickCommand3 = new DelegateCommand((object obj) => { this.Click(value4, 3); }, (object obj) => { return true; });
            //ClickCommand4 = new DelegateCommand((object obj) => { this.Click(value5, 2); }, (object obj) => { return true; });
            //ClickCommand5 = new DelegateCommand((object obj) => { this.Click(value6, 1); }, (object obj) => { return true; });
            Shortcuts = new ObservableCollection<ProcessShortcut>()
        {
        new ProcessShortcut(){DisplayName = "Bed 6",ColorChange=Bed(Value1()),clickCommand=ClickCommand,Bedno =6},
        //new ProcessShortcut(){DisplayName = "Bed 5",ColorChange=Bed(value2),clickCommand=ClickCommand1,Bedno =5},
        //new ProcessShortcut(){DisplayName = "Bed 4",ColorChange=Bed(value3),clickCommand=ClickCommand2,Bedno=4},
        //new ProcessShortcut(){DisplayName="Bed 3",ColorChange=Bed(value4),clickCommand=ClickCommand3,Bedno=3},
        //new ProcessShortcut(){DisplayName="Bed 2",ColorChange=Bed(value5),clickCommand=ClickCommand4,Bedno=2},
        //new ProcessShortcut(){DisplayName="Bed 1",ColorChange=Bed(value6),clickCommand=ClickCommand5,Bedno=1},
         };
        }
        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommand1 { get; set; }
        public ICommand ClickCommand2 { get; set; }
        public ICommand ClickCommand3 { get; set; }
        public ICommand ClickCommand4 { get; set; }
        public ICommand ClickCommand5 { get; set; }

        public string Bed(int value)
        {
            if (value == 0)
            {
                return "Red";
            }
            else
                return "White";

        }
        public string Click(int value1, int bed)
        {

            if (value1 == 0)
            {
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show("Do you want to turn off the alert", "Patient is in emergnecy in " + bed, buttons);
                if (result == MessageBoxResult.Yes)
                {
                    //List<ProcessShortcut>() 
                    foreach (ProcessShortcut item in Shortcuts)
                    {
                        if (item.Bedno == bed)
                        {
                            item.ColorChange = "White";
                            OnPropertyChanged(item.ColorChange);
                        }
                    }

                }
                else
                {
                    Bed(0);
                }
            }
            if (value1 == 1)
            {
                MessageBox.Show("Patient is healthy");

            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        #region Behaviours
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion




        public class ProcessShortcut : INotifyPropertyChanged
        {
            void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            private string _colorchange;

            public event PropertyChangedEventHandler PropertyChanged;

            public string DisplayName { get; set; }
            public string ProcessName { get; set; }
            public string ColorChange
            {
                get { return this._colorchange; }
                set
                {

                    {
                        this._colorchange = value;
                        this.OnPropertyChanged("ColorChange");
                    }
                }
            }

            public string ButtonClick { get; set; }
            public ICommand clickCommand { get; set; }
            public int Bedno { get; set; }
        }




        public bool CanExecute
        {
            get
            {
                // check if executing is allowed, i.e., validate, check if a process is running, etc. 
                return true;
            }
        }


    }
}

