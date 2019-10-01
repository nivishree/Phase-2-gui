using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitalSigns;

namespace EnableVitalsClass
{
    public class SetVitals
    {
        public string PatientId { get; set; }
        public List<VitalSign> VitalsSigns { get; set; }
    }
}
