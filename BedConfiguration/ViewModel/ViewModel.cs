using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using MVVMUtilityLib;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;

namespace ViewModel
{
    public partial class ViewModels : INotifyPropertyChanged
    {
        public int ReturnValue(string response)
        {
            int value = 1;
            if (response == "Healthy")
            {
                value = 1;
            }
            else
                value = 0;
            return value;
        }
        public int GenerateVitals(string patientId)
        {
            int value = 1;
            HttpClient client = new HttpClient();
            var response1 = client
                .PostAsJsonAsync("http://localhost:1080/api/PatientMonitoring/GeneratePatientVitals/", patientId)
                .Result;

            if (response1.IsSuccessStatusCode)
            {
                var response2 = client
                    .PostAsJsonAsync("http://localhost:1080/api/PatientAlert/PatientVitalsAlertUponValidation",
                        patientId).Result;
                var content1 = response2.Content.ReadAsStringAsync().Result;
                value = ReturnValue(content1);
            }
            else
                Console.WriteLine("Generate Error code= " + response1.StatusCode);
            return value;
        }

        private ObservableCollection<ProcessShortcut> shortcuts=new ObservableCollection<ProcessShortcut>();
        public ObservableCollection<ProcessShortcut> Shortcuts
        {
            get { return this.shortcuts; }
            set
                {
                    this.shortcuts = value;
                    this.OnPropertyChanged("Shortcuts");
                }
        }

        private BackgroundWorker backgroundWorker = null;

        public ViewModels()
        {
            Shortcuts = new ObservableCollection<ProcessShortcut>();
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            StartMonitoring = new DelegateCommand((object obj) =>
            {
                backgroundWorker.RunWorkerAsync();
            }, (object obj) => { return true; });
        }

        private bool bWait = false;

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Monitor();
            backgroundWorker.RunWorkerAsync();
        }

        private List<BedConfigTbl> bedList;
        private List<BedAllotmentTbl> patientList;
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bedList = GetDataFromBedConfiguration();
            patientList = GetDataFromBedAllotment();

            if (bWait)
            {
                Thread.Sleep(5000);
            }
            else
            {
                bWait = true;
            }
        }

        private List<BedAllotmentTbl> GetDataFromBedAllotment()
        {
            HttpClient client = new HttpClient();
            List<BedAllotmentTbl> patientList = new List<BedAllotmentTbl>();
            var response = client.GetAsync
                ("http://localhost:58905/api/BedConfiguration/GetAllFromBedAllotment").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsAsync<List<BedAllotmentTbl>>().Result;
                patientList = content;
            }
            return patientList;
        }

        private List<BedConfigTbl> GetDataFromBedConfiguration()
        {
            HttpClient client = new HttpClient();
            List<BedConfigTbl> bedList = new List<BedConfigTbl>();
            var response = client.GetAsync
                ("http://localhost:58905/api/BedConfiguration/GetAllFromBedConfiguration").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsAsync<List<BedConfigTbl>>().Result;
                bedList = content;
            }
            return bedList;
        }


        public void Monitor()
        {
            Shortcuts.Clear();
            //List<BedConfigTbl> bedList = GetDataFromBedConfiguration();
            //List<BedAllotmentTbl> patientList = GetDataFromBedAllotment();

            foreach (var bed in bedList)
            {
                string patientId = null;
                bool status = true;
                if (bed.BedAvailability == 0)
                {
                    status = false;
                }

                foreach (var patient in patientList)
                {
                    if (bed.BedNo == patient.BedNo)
                        patientId = patient.PatientId;
                }

                ClickCommand = new DelegateCommand((object obj) => { Click(bed.BedNo); },
                    (object obj) => { return true; });

                //Application.Current.Dispatcher.Invoke((System.Action) delegate
                //{
                    this.Shortcuts.Add(
                        new ProcessShortcut()
                        {
                            DisplayName = "Bed " + bed.BedNo,
                            ColorChange = Bed(GenerateVitals(patientId)),
                            BedNo = bed.BedNo,
                            PatientID = patientId,
                            BedClickCommand = ClickCommand,
                            Status = status
                        });
                //});
            }

        }


    public ICommand StartMonitoring { get; set; }
    public ICommand ClickCommand { get; set; }
    public string Bed(int value)
        {
            if (value == 0)
            {
                return "Red";
            }
            else
                return "#03fc07";
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void Click(int bed)
        {
            bool isYes = false;
            ProcessShortcut processShortcut = null;
            foreach (var item in Shortcuts)
            {
                if (item.BedNo == bed && item.ColorChange == "Red")
                {
                        MessageBoxButton msgBoxButton = MessageBoxButton.YesNo;
                        MessageBoxResult msgBoxResult = MessageBox.Show("Do you want to turn off the alert",
                            "Patient is in emergnecy in bed " + bed, msgBoxButton);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            processShortcut = item;
                            isYes = true;
                            break;
                        //item.ColorChange = "#03fc07";
                        //OnPropertyChanged(item.ColorChange);
                        }
                }
            }

            if (isYes && processShortcut!= null)
            {
                processShortcut.ColorChange = "#03fc07";
                OnPropertyChanged(processShortcut.ColorChange);
            }
        }

        #region Behaviours
            void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}

