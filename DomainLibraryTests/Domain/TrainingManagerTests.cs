using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using DomainLibrary.Repositories;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;

namespace DomainLibrary.Domain.Tests
{
    [TestClass()]
    public class TrainingManagerTests
    {


        private TrainingManager SetUpForAddCyclingSessionExceptions()
        {

            var mockUoW = new Mock<IUnitOfWork>();

            mockUoW.Setup(x => x.CyclingTrainings.AddTraining(It.IsAny<CyclingSession>()));
            TrainingManager tm = new TrainingManager(new Mock<IUnitOfWork>().Object);
            return tm;
        }

        [TestMethod()]
        public void AddCyclingTrainingTest_InTheFuture_ThrowDomainException()
        {
            var tm = SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now.AddDays(1), null, TimeSpan.Zero,
               null, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Training is in the future");

        }

        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidDistance_ThrowDomainException()
        {
            var tm = SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now.AddDays(-1), 6000, TimeSpan.Zero,
               null, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Distance invalid value");

        }
        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidTime_ThrowDomainException()
        {
            var tm = SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now, null, new TimeSpan(-5),
               null, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Time invalid value");

        }
        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidSpeed_ThrowDomainException()
        {
            var tm = SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now, 500, new TimeSpan(1, 0, 0),
               75, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Average speed invalid value");

        }

        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidWatt_ThrowDomainException()
        {
            var tm = SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now, 500, new TimeSpan(1, 0, 0),
               75, 950, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Average speed invalid value");

        }

        private TrainingManager SetUpForAddRunningSessionExceptions()
        {
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.RunningTrainings.AddTraining(It.IsAny<RunningSession>()));
            TrainingManager tm = new TrainingManager(new Mock<IUnitOfWork>().Object);
            return tm;
        }

        [TestMethod()]
        public void AddRunningTrainingTest_InTheFuture_ThrowDomainException()
        {
            var tm = SetUpForAddRunningSessionExceptions();

            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now.AddDays(1), 10, new TimeSpan(5),
               null, TrainingType.Endurance, null))
                .Should().Throw<DomainException>().WithMessage("Training is in the future");

        }

        [TestMethod()]
        public void AddRunningTrainingTest_InvalidDistance_ThrowDomainException()
        {
            var tm = SetUpForAddRunningSessionExceptions();
            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now, 50001, new TimeSpan(5),
                         null, TrainingType.Endurance, null))
                    .Should().Throw<DomainException>().WithMessage("Distance invalid value");

        }
        [TestMethod()]
        public void AddRunningTrainingTest_InvalidTime_ThrowDomainException()
        {
            var tm = SetUpForAddRunningSessionExceptions();
            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now, 10, new TimeSpan(-5),
                        null, TrainingType.Endurance, null))
                        .Should().Throw<DomainException>().WithMessage("Time invalid value");

        }
        [TestMethod()]
        public void AddRunningTrainingTest_InvalidSpeed_ThrowDomainException()
        {
            var tm = SetUpForAddRunningSessionExceptions();

            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now, 10, new TimeSpan(5),
                         45, TrainingType.Endurance, null))
                .Should().Throw<DomainException>().WithMessage("Average speed invalid value");

        }

        [TestMethod()]
        public void GenerateMonthlyCyclingReportTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddRunningTrainingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GenerateMonthlyRunningReportTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveTrainingsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GenerateMonthlyTrainingsReportTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPreviousRunningSessionsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPreviousCyclingSessionsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllRunningSessionsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllCyclingSessionsTest()
        {
            Assert.Fail();
        }
    }
}