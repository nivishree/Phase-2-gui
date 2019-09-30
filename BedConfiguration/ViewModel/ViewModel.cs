using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using MVVMUtilityLib;

namespace ViewModel
{
    public partial class ViewModels : INotifyPropertyChanged
    {
        public int ReturnValue(string response)
        {
            int value = response == "Healthy" ? 1 : 0;
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

        private ObservableCollection<ProcessShortcut> _shortcuts=new ObservableCollection<ProcessShortcut>();
        public ObservableCollection<ProcessShortcut> Shortcuts
        {
            get => _shortcuts;
            set
                {
                    _shortcuts = value;
                    OnPropertyChanged("Shortcuts");
                }
        }

        private readonly BackgroundWorker _backgroundWorker;

        public ViewModels()
        {
            Shortcuts = new ObservableCollection<ProcessShortcut>();
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            StartMonitoring = new DelegateCommand(obj =>
            {
                _backgroundWorker.RunWorkerAsync();
            }, obj => true);
        }

        private bool _bWait;

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Monitor();
            _backgroundWorker.RunWorkerAsync();
        }

        private List<BedConfigTbl> _bedList;
        private List<BedAllotmentTbl> _patientList;
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _bedList = GetDataFromBedConfiguration();
            _patientList = GetDataFromBedAllotment();

            if (_bWait)
            {
                Thread.Sleep(5000);
            }
            else
            {
                _bWait = true;
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
         

            foreach (var bed in _bedList)
            {
                string patientId = null;
                bool status = bed.BedAvailability != 0;

                foreach (var patient in _patientList)
                {
                    if (bed.BedNo == patient.BedNo)
                        patientId = patient.PatientId;
                }

                ClickCommand = new DelegateCommand(obj => { Click(bed.BedNo); },
                    obj => true);

             
                    Shortcuts.Add(
                        new ProcessShortcut
                        {
                            DisplayName = "Bed " + bed.BedNo,
                            ColorChange = Bed(GenerateVitals(patientId)),
                            BedNo = bed.BedNo,
                            PatientID = patientId,
                            BedClickCommand = ClickCommand,
                            Status = status
                        });
                
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
                            "Patient is in emergency in bed " + bed, msgBoxButton);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            processShortcut = item;
                            isYes = true;
                            break;
                        }
                }
            }

            if (isYes)
            {
                processShortcut.ColorChange = "#03fc07";
                OnPropertyChanged(processShortcut.ColorChange);
            }
        }

        #region Behaviours

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

