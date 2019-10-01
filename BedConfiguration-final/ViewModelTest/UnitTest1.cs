using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;

namespace ViewModelTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GivenTheStatusOfThePatients_BedColorShouldChange()
        {
            ViewModels bedColor = new ViewModels();
            string actual = bedColor.Bed(0);
            string expected = "Red";
            Assert.AreEqual(actual, expected);

        }

        [TestMethod]
        public void GivenTheVitals_ItShoudTheStatusOfThePatient()
        {
            ViewModels bedColor = new ViewModels();
            int actual = bedColor.ReturnValue("Healthy");
            int expected = 1;
            Assert.AreEqual(actual, expected);

        }

        [TestMethod]
        public void GivenThePatientId_VitalsShouldBeGeneratedRandomly()
        {
            ViewModels bedColor = new ViewModels();
            int actual = bedColor.GenerateVitals("6");
            int expected = 2;
            Assert.AreNotEqual(actual, expected);

        }

        [TestMethod]
        public void CheckBedAllotmentTblPatientId()
        {
            ViewModels models = new ViewModels();
            string expected = new Random().ToString();

            var target = new BedAllotmentTbl() { PatientId = expected };

            Assert.AreEqual(expected, target.PatientId);
        }

        [TestMethod]
        public void CheckBedConfigTbl()
        {
            ViewModels models = new ViewModels();
            int expected = new Random().Next();

            var target = new BedConfigTbl() { BedNo = expected };

            Assert.AreEqual(expected, target.BedNo);
        }

        [TestMethod]
        public void CheckBedConfigTblBedAvailability()
        {
            ViewModels models = new ViewModels();
            int expected = new Random().Next();

            var target = new BedConfigTbl() { BedAvailability = expected };

            Assert.AreEqual(expected, target.BedAvailability);
        }

        [TestMethod]
        public void CheckBedAllotmentTbl()
        {
            ViewModels models = new ViewModels();
            int expected = new Random().Next();

            var target = new BedAllotmentTbl() { BedNo = expected };

            Assert.AreEqual(expected, target.BedNo);
        }
    }
}
