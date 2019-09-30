
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.Serialization;
using System.Windows.Forms;
using MVVMUtilityLib;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using EnableVitalsClass;
using VitalSigns;
using VitalsTypeLib;
using MessageBox = System.Windows.MessageBox;

namespace AllotBedModuleLib
{
    public partial class AllotBedViewModel  : INotifyPropertyChanged
    {
        public AllotBedViewModel()
        {
            SubmitCommand = new DelegateCommand((object obj) => { this.AllotBed(PatientId,set); },
                (object obj) => { return true; });
            //Listcommand1= new DelegateCommand((object obj) => { this.EnableVitals1(); },
            //    (object obj) => { return true; });
            //Listcommand2 = new DelegateCommand((object obj) => { this.EnableVitals2(); },
            //    (object obj) => { return true; });
            //Listcommand3 = new DelegateCommand((object obj) => { this.EnableVitals3(); },
            //   (object obj) => { return true; });

            set.VitalsSigns = new List<VitalSign>();


        }

        readonly SetVitals set = new SetVitals();

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SubmitCommand { get; set; }
        //public ICommand Listcommand1 { get; set; }
        //public ICommand Listcommand2 { get; set; }
        //public ICommand Listcommand3 { get; set; }

        public string _patientId;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string PatientId
        {
            get { return _patientId; }
            set
            {
                _patientId = value;
                OnPropertyChanged(PatientId);
            }
        }

     
        private bool _check1 = true;
        public bool Checkbox1
        {
            get { return _check1; }

            set
            {
                _check1 = value;
                OnPropertyChanged("Checkbox1");
            }
        }
        private bool _check2 = true;
        public bool Checkbox2
        {
            get { return _check2; }

            set
            {
                _check2 = value;
                OnPropertyChanged("Checkbox2");
            }
        }

        private bool _check3 = true;
        public bool Checkbox3
        {
            get { return _check3; }

            set
            {
                _check3 = value;
                OnPropertyChanged("Checkbox3");
            }
        }

        private void SendMessage(string parameter)
        {

            PatientId = "";
            Checkbox1 = true;
            Checkbox2 = true;
            Checkbox3 = true;
        }



        private void EnableVitals1()
        {
            if (Checkbox1)
            {
                set.PatientId = PatientId;

                set.VitalsSigns.Add(new VitalSign {IsEnabled = true, Type = 0});
            }
        }
        private void EnableVitals2()
        {
            if (Checkbox2)
            {
                set.PatientId = PatientId;
                set.VitalsSigns.Add(new VitalSign {IsEnabled = true, Type = VitalsTypeLib.VitalsType.PulseRate});
            }
        }
        private void EnableVitals3()
        {
            if (Checkbox3)
            {
                set.PatientId = PatientId;

                set.VitalsSigns.Add(new VitalSign {IsEnabled = true, Type = VitalsTypeLib.VitalsType.Temperature});
            }
        }


        public void Check()
        {
                set.PatientId = PatientId;

                EnableVitals1();
                EnableVitals2();
                EnableVitals3();

                //set.VitalsSigns.Add(new VitalSign { IsEnabled = true, Type = 0 });
                //set.VitalsSigns.Add(new VitalSign { IsEnabled = true, Type = VitalsTypeLib.VitalsType.PulseRate });
                //set.VitalsSigns.Add(new VitalSign { IsEnabled = true, Type = VitalsTypeLib.VitalsType.Temperature });
        }
        public bool AllotBed(string PatientId,SetVitals set)
        {
            bool x = false;

            if (PatientId != null)
            {
                Check();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add
                    (new MediaTypeWithQualityHeaderValue("application/json"));
                string BedConfigureURI = "http://localhost:58905/api/BedConfiguration/";
                var responseMessage = client.PostAsJsonAsync
                    (BedConfigureURI + "AllotBed", PatientId).Result;
                x = responseMessage.IsSuccessStatusCode;
                string PatientMonitoriURI = "http://localhost:1080/api/PatientMonitoring/";
                var httpResponse =
                    client.PostAsJsonAsync
                    (PatientMonitoriURI + "/EnablePatientVitals",
                        set).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("Patient alloted");
                    SendMessage("");
                }
                x = x && (httpResponse.IsSuccessStatusCode);
            }

            return x;
        }
    }
}

