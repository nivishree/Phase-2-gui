using System;
using DischargePatientModuleLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DischargePatientLibTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void Given_PatientId_To_DischargePatientApi_api_should_return_true()
        {
            DischargePatientViewModel obj = new DischargePatientViewModel();
            bool expected = false;
            bool actualvalue = obj.DischargePatient("nivi");
            Assert.AreEqual(expected, actualvalue);
        }
        [TestMethod]
        public void Given_PatientId_Which_Does_Not_exist_To_DischargePatientApi_api_should_return_true()
        {

            DischargePatientViewModel obj = new DischargePatientViewModel();
            bool expected = false;
            bool actualvalue = obj.DischargePatient("pnull");
            Assert.AreEqual(expected, actualvalue);
        }

        [TestMethod]

        public void PatientId_Set_Check()
        {
            string expected = new Random().ToString();

            var target = new DischargePatientViewModel() { PatientId = expected };

            Assert.AreEqual(expected, target.PatientId);
        }
    }
}
