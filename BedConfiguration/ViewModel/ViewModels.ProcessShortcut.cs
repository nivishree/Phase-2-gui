using System.ComponentModel;
using System.Windows.Input;

namespace ViewModel
{
    public partial class ViewModels
    {
        public class ProcessShortcut : INotifyPropertyChanged
        {
            void OnPropertyChanged(string propertyname)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
                }
            }

            private string _colorChange;

            private ICommand _command;
            public event PropertyChangedEventHandler PropertyChanged;
            public string DisplayName { get; set; }
            public int BedNo { get; set; }
            public bool Status { get; set; }

            public string PatientID { get; set; }

            public string ColorChange
            {
                get { return this._colorChange; }
                set
                {
                    this._colorChange = value;
                    this.OnPropertyChanged("ColorChange");
                }
            }

            public ICommand BedClickCommand
            {
                get { return this._command; }
                set
                { 
                    this._command = value;
                    this.OnPropertyChanged("BedClickCommand");
                }
            }
        }
    }
}