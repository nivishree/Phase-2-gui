using System;
using System.Collections.Generic;
using System.Net.Http;
using AllotBedModuleLib;
using EnableVitalsClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMUtilityLib;
using VitalSigns;

namespace AllotBedLibTest
{
    [TestClass]
    public class AllotBedTest
    {
        [TestMethod]
        public void Given_PatientId_And_Vitals_To_api_should_return_Success()
        {


            AllotBedViewModel allotBed = new AllotBedViewModel();
            SetVitals set = new SetVitals();
            set.PatientId = "pox";
            set.VitalsSigns = new List<VitalSign>();
            set.VitalsSigns.Add(new VitalSign { IsEnabled = true, Type = 0 });
            bool expected = false;
            //HttpClient client = new HttpClient();
            //var response = client.PostAsJsonAsync
            //    ("http://localhost:58905/api/BedConfiguration/ConfigureBed", 46).Result;
            bool actual = allotBed.AllotBed(set.PatientId, set);
            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void Checkbox1GetTest()
        {
            AllotBedViewModel bed = new AllotBedViewModel();
            var expected = false;
            bed.Checkbox1 = false;
            var actual = bed.Checkbox1;
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void Checkbox2GetTest()
        {
            AllotBedViewModel bed = new AllotBedViewModel();
            var expected = false;
            bed.Checkbox2 = false;
            var actual = bed.Checkbox2;
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void Checkbox3GetTest()
        {
            AllotBedViewModel allotBed = new AllotBedViewModel();
            var expected = false;
            allotBed.Checkbox3 = false;
            var actual = allotBed.Checkbox3;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void SubmitCommandTest()
        {
            AllotBedViewModel bed = new AllotBedViewModel();
            bed.SubmitCommand = new DelegateCommand((object obj) => { this.Add(); },
                (object obj) => { return true; });


            var x = bed.SubmitCommand;

            Assert.AreEqual(bed.SubmitCommand, x);
        }
        public int Add()
        {
            return 2 * 2;
        }
    }
}
