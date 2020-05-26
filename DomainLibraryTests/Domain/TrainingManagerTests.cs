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
using DataLayer;
using DomainLibraryTests;
using System.Linq;

namespace DomainLibrary.Domain.Tests
{
    [TestClass()]
    public class TrainingManagerTests
    {
        public TrainingManager tm;

        private void SetUpForAddCyclingSessionExceptions()
        {

            var mockUoW = new Mock<IUnitOfWork>();

            mockUoW.Setup(x => x.CyclingTrainings.AddTraining(It.IsAny<CyclingSession>()));
            tm = new TrainingManager(new Mock<IUnitOfWork>().Object);
           
        }

        [TestMethod()]
        public void AddCyclingTrainingTest_InTheFuture_ThrowDomainException()
        {
            SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now.AddDays(1), null, TimeSpan.Zero,
               null, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Training is in the future");

        }

        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidDistance_ThrowDomainException()
        {
            SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now.AddDays(-1), 6000, TimeSpan.Zero,
               null, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Distance invalid value");

        }
        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidTime_ThrowDomainException()
        {
            SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now, null, new TimeSpan(-5),
               null, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Time invalid value");

        }
        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidSpeed_ThrowDomainException()
        {
            SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now, 500, new TimeSpan(1, 0, 0),
               75, null, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Average speed invalid value");

        }

        [TestMethod()]
        public void AddCyclingTrainingTest_InvalidWatt_ThrowDomainException()
        {
            SetUpForAddCyclingSessionExceptions();

            tm.Invoking(tm => tm.AddCyclingTraining(DateTime.Now, 500, new TimeSpan(1, 0, 0),
               75, 950, TrainingType.Endurance, null, BikeType.CityBike))
                .Should().Throw<DomainException>().WithMessage("Average speed invalid value");

        }

        private void SetUpForAddRunningSessionExceptions()
        {
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(x => x.RunningTrainings.AddTraining(It.IsAny<RunningSession>()));
            tm = new TrainingManager(new Mock<IUnitOfWork>().Object);
        }

        [TestMethod()]
        public void AddRunningTrainingTest_InTheFuture_ThrowDomainException()
        {
            SetUpForAddRunningSessionExceptions();

            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now.AddDays(1), 10, new TimeSpan(5),
               null, TrainingType.Endurance, null))
                .Should().Throw<DomainException>().WithMessage("Training is in the future");

        }

        [TestMethod()]
        public void AddRunningTrainingTest_InvalidDistance_ThrowDomainException()
        {
            SetUpForAddRunningSessionExceptions();
            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now, 50001, new TimeSpan(5),
                         null, TrainingType.Endurance, null))
                    .Should().Throw<DomainException>().WithMessage("Distance invalid value");

        }
        [TestMethod()]
        public void AddRunningTrainingTest_InvalidTime_ThrowDomainException()
        {
            SetUpForAddRunningSessionExceptions();
            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now, 10, new TimeSpan(-5),
                        null, TrainingType.Endurance, null))
                        .Should().Throw<DomainException>().WithMessage("Time invalid value");

        }
        [TestMethod()]
        public void AddRunningTrainingTest_InvalidSpeed_ThrowDomainException()
        {
            SetUpForAddRunningSessionExceptions();

            tm.Invoking(tm => tm.AddRunningTraining(DateTime.Now, 10, new TimeSpan(5),
                         45, TrainingType.Endurance, null))
                .Should().Throw<DomainException>().WithMessage("Average speed invalid value");

        }
        public void SetupForReportTests()
        {
            tm = new TrainingManager(new UnitOfWork(new TrainingContextTest()));
            tm.AddCyclingTraining(new DateTime(2020, 4, 21, 16, 00, 00), 40, new TimeSpan(1, 20, 00), 30, null, TrainingType.Endurance, null, BikeType.RacingBike);
            tm.AddCyclingTraining(new DateTime(2020, 4, 18, 18, 00, 00), 40, new TimeSpan(1, 42, 00), null, null, TrainingType.Recuperation, null, BikeType.RacingBike);
            tm.AddCyclingTraining(new DateTime(2020, 3, 19, 16, 45, 00), null, new TimeSpan(1, 0, 00), null, 219, TrainingType.Interval, "5x5 min 270", BikeType.IndoorBike);
            tm.AddRunningTraining(new DateTime(2020, 4, 17, 12, 30, 00), 5000, new TimeSpan(0, 27, 17), null, TrainingType.Endurance, null);
            tm.AddRunningTraining(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            tm.AddRunningTraining(new DateTime(2020, 3, 17, 12, 0, 00), 5000, new TimeSpan(0, 28, 10), null, TrainingType.Interval, "3x700m");
            tm.AddRunningTraining(new DateTime(2020, 3, 17, 11, 0, 00), 8000, new TimeSpan(0, 42, 10), null, TrainingType.Endurance, null);


        }

        [TestMethod()]
        public void GenerateMonthlyCyclingReportTest()
        {

            SetupForReportTests();

            var r = tm.GenerateMonthlyCyclingReport(2020,4);

            r.Rides.Select(x => x.When.Month).All( m => m == 4);
            r.Rides.Count.Should().Be(2);
            r.TotalCyclingDistance.Should().Be(40 + 40);
            r.TotalCyclingTrainingTime.Should().Be(TimeSpan.FromTicks(new TimeSpan(1, 20, 00).Ticks + new TimeSpan(1, 42, 00).Ticks));



        }

        [TestMethod()]
        public void GenerateMonthlyRunningReportTest()
        {
            SetupForReportTests();

            var r = tm.GenerateMonthlyRunningReport(2020, 4);

            r.Runs.Select(x => x.When.Month).All(m => m == 4);
            r.Runs.Count.Should().Be(2);
            r.TotalRunningDistance.Should().Be(5000 + 5000);
            r.TotalRunningTrainingTime.Should().Be(TimeSpan.FromTicks(new TimeSpan(0, 27, 17).Ticks + new TimeSpan(0, 25, 48).Ticks));


        }

        [TestMethod()]
        public void GenerateMonthlyTrainingsReportTest()
        {
            SetupForReportTests();

            var r = tm.GenerateMonthlyTrainingsReport(2020, 03);

            r.TotalSessions.Should().Be(3);
            r.Rides.Count.Should().Be(1);
            r.Runs.Count.Should().Be(2);
            r.TotalTrainingTime.Should().Be(TimeSpan.FromTicks(new TimeSpan(1, 0, 00).Ticks + new TimeSpan(0, 28, 10).Ticks + new TimeSpan(0, 42, 10).Ticks));

        }

    }
}