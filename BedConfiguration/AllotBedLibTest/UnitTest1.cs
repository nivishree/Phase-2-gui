using System;
using System.Collections.Generic;
using AllotBedModuleLib;
using EnableVitalsClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VitalSigns;

namespace AllotBedLibTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Given_PatientId_And_Vitals_To_api_should_return_Success()
        {
            AllotBedViewModel allotBed = new AllotBedViewModel();
            SetVitals set = new SetVitals();
            set.PatientId = "veena";
            set.VitalsSigns = new List<VitalSign>();
            set.VitalsSigns.Add(new VitalSign { IsEnabled = true, Type = 0 });
            bool expected = true;
            bool actual = allotBed.AllotBed(set.PatientId,set);
            Assert.AreEqual(expected,actual);
        }
    }
}
