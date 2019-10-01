using MVVMUtilityLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DischargePatientModuleLib
{
    public class DischargePatientViewModel : INotifyPropertyChanged
    {
        public DischargePatientViewModel()
        {
           
            SubmitCommand = new DelegateCommand((object obj) => { this.DischargePatient(PatientId); },
                (object obj) => { return true; });

        }


        public ICommand SubmitCommand { get; set; }
        public string patientId;

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string PatientId
        {
            get { return patientId; }

            set
            {
                patientId = value;
                OnPropertyChanged(PatientId);
            }
        }


        void Reset()
        {
            PatientId = "";
        }


        public bool DischargePatient(string PatientId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add
                (new MediaTypeWithQualityHeaderValue("application/json"));
            string PatientDischargeURL = "http://localhost:58905/api/BedConfiguration/";
            var responseMessage = client.PostAsJsonAsync
                (PatientDischargeURL+"DischargePatient",PatientId).Result;

            var result=responseMessage.IsSuccessStatusCode;
            if (responseMessage.IsSuccessStatusCode)
            {
                MessageBox.Show("Patient discharged");
                  Reset();
            }
            return result;
           
        }
    }
}
