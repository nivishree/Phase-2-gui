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
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class ViewModels 
    {
      

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
        public void Alert()
        {
            ClickCommand = new DelegateCommand((object obj) => { this.Click(GenerateVitals(), 6); }, (object obj) => { return true; });
            //ClickCommand1 = new DelegateCommand((object obj) => { this.Click(value2, 5); }, (object obj) => { return true; });
            //ClickCommand2 = new DelegateCommand((object obj) => { this.Click(value3, 4); }, (object obj) => { return true; });
            //ClickCommand3 = new DelegateCommand((object obj) => { this.Click(value4, 3); }, (object obj) => { return true; });
            //ClickCommand4 = new DelegateCommand((object obj) => { this.Click(value5, 2); }, (object obj) => { return true; });
            //ClickCommand5 = new DelegateCommand((object obj) => { this.Click(value6, 1); }, (object obj) => { return true; });
            Shortcuts = new ObservableCollection<ProcessShortcut>()
        {
        new ProcessShortcut(){DisplayName = "Bed 6",ColorChange=Bed(GenerateVitals()),clickCommand=ClickCommand,Bedno =6},
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
        public int ReturnValue(string response)
        {
            int value = 1;
            if (response != "Healthy")
            {
                value = 0;
            }
            else
                value = 1;

            return value;
        }

        public int GenerateVitals()
        {
            int value = 1;
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client
                .GetAsync("http://localhost:58905/api/BedConfiguration/GetPatientsFromBedAllotment").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var patientList = JsonConvert.DeserializeObject<List<string>>(content);

            foreach (var patientId in patientList)
            {
                var response1 = client
                    .PostAsJsonAsync("http://localhost:1080/api/PatientMonitoring/GeneratePatientVitals/", patientId)
                    .Result;
                if (response1.IsSuccessStatusCode)
                {
                    var response2 = client
                        .PostAsJsonAsync("http://localhost:1080/api/PatientAlert/PatientVitalsAlertUponValidation/",
                            patientId).Result;
                    var content1 = response2.Content.ReadAsStringAsync().Result;
                     value = ReturnValue(content1);

                }
                else
                    Console.WriteLine("Generate Error code= " + response.StatusCode);
            }

            return value;
        }
    }
}
    

