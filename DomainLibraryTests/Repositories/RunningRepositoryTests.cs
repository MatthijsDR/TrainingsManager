using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using DomainLibraryTests;
using DomainLibrary.Domain;
using System.Linq;
using FluentAssertions;

namespace DataLayer.Repositories.Tests
{
    [TestClass()]
    public class RunningRepositoryTests
    {

        private RunningRepository _runRepo;
        private TrainingContextTest _context;
        private void SetRepoAndContext()
        {
            _context = new TrainingContextTest();
            _runRepo = new RunningRepository(_context);
        }
        [TestMethod()]
        public void AddTrainingTest()
        {
            //Arrange
            SetRepoAndContext();
            var runSes = new RunningSession(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            int initCount = _context.RunningSessions.Count();
            //Act
            _runRepo.AddTraining(runSes);
            _context.SaveChanges();
            //Assert
            Assert.AreEqual(initCount + 1, _context.RunningSessions.Count());

        }

        [TestMethod()]
        public void FindTest()
        {
            //Arrange
            SetRepoAndContext();
            var runSes = new RunningSession(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            _runRepo.AddTraining(runSes);
            _context.SaveChanges();

            //Act
            var actual = _runRepo.Find(1);

            //Assert  
            actual.Should().Equals(runSes);

        }

        [TestMethod()]
        public void FindAllTest()
        {
            //Arrange
            SetRepoAndContext();
            var runSes0 = new RunningSession(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null); 
            var runSes1 = new RunningSession(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes2 = new RunningSession(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes3 = new RunningSession(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes4 = new RunningSession(new DateTime(2020, 4, 19, 12, 30, 00), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            _runRepo.AddTraining(runSes0);
            _runRepo.AddTraining(runSes1);
            _runRepo.AddTraining(runSes2);
            _runRepo.AddTraining(runSes3);
            _runRepo.AddTraining(runSes4);
            _context.SaveChanges();
            //Act
            var actuals = _runRepo.FindAll();
            //Assert
            actuals.Count().Should().Be(5);

        }
        [TestMethod()]
        public void FindTest1()
        {
            //Arrange
            SetRepoAndContext();
            var runSes0 = new RunningSession(DateTime.Now, 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes1 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes2 = new RunningSession(DateTime.Now.AddDays(20), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes3 = new RunningSession(DateTime.Now.AddDays(-10), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes4 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            _runRepo.AddTraining(runSes0);
            _runRepo.AddTraining(runSes1);
            _runRepo.AddTraining(runSes2);
            _runRepo.AddTraining(runSes3);
            _runRepo.AddTraining(runSes4);
            _context.SaveChanges();
            //Act
            var actuals = _runRepo.Find(DateTime.Now, DateTime.Now.AddDays(21));
            //Assert
            actuals.Should().Contain(new[] { runSes1, runSes2 });
        }
        [TestMethod()]
        public void FindLatestSessionsTest()
        {
            //Arrange
            SetRepoAndContext();
            var runSes0 = new RunningSession(DateTime.Now, 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes1 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes2 = new RunningSession(DateTime.Now.AddDays(20), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes3 = new RunningSession(DateTime.Now.AddDays(-10), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            var runSes4 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), null, TrainingType.Endurance, null);
            _runRepo.AddTraining(runSes0);
            _runRepo.AddTraining(runSes1);
            _runRepo.AddTraining(runSes2);
            _runRepo.AddTraining(runSes3);
            _runRepo.AddTraining(runSes4);
            _context.SaveChanges();
            //Act
            var actuals = _runRepo.FindLatestSessions(2);
            //Assert
            actuals.Should().Contain(new[] { runSes4, runSes2 });
        }

        [TestMethod()]
        public void FindMaxSessionsTest()
        {
            //Arrange
            SetRepoAndContext();
            var runSes0 = new RunningSession(DateTime.Now, 5001, new TimeSpan(0, 25, 48), 4, TrainingType.Endurance, null);
            var runSes1 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), 15, TrainingType.Endurance, null);
            var runSes2 = new RunningSession(DateTime.Now.AddDays(20), 5000, new TimeSpan(0, 25, 48), 10, TrainingType.Endurance, null);
            var runSes3 = new RunningSession(DateTime.Now.AddDays(-10), 5000, new TimeSpan(0, 25, 48), 8, TrainingType.Endurance, null);
            var runSes4 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), 6, TrainingType.Endurance, null);
            _runRepo.AddTraining(runSes0);
            _runRepo.AddTraining(runSes1);
            _runRepo.AddTraining(runSes2);
            _runRepo.AddTraining(runSes3);
            _runRepo.AddTraining(runSes4);
            _context.SaveChanges();
            //Act
            var actuals = _runRepo.FindMaxSessions();
            //Assert
            actuals[0].Should().Be(runSes0);
            actuals[1].Should().Be(runSes1);

        }
        [TestMethod()]
        public void RemoveTrainingTest()
        {
            //Arrange
            SetRepoAndContext();
            var runSes0 = new RunningSession(DateTime.Now, 5001, new TimeSpan(0, 25, 48), 4, TrainingType.Endurance, null);
            var runSes1 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), 15, TrainingType.Endurance, null);
            var runSes2 = new RunningSession(DateTime.Now.AddDays(20), 5000, new TimeSpan(0, 25, 48), 10, TrainingType.Endurance, null);
            var runSes3 = new RunningSession(DateTime.Now.AddDays(-10), 5000, new TimeSpan(0, 25, 48), 8, TrainingType.Endurance, null);
            var runSes4 = new RunningSession(DateTime.Now.AddDays(1), 5000, new TimeSpan(0, 25, 48), 6, TrainingType.Endurance, null);
            _runRepo.AddTraining(runSes0);
            _runRepo.AddTraining(runSes1);
            _runRepo.AddTraining(runSes2);
            _runRepo.AddTraining(runSes3);
            _runRepo.AddTraining(runSes4);
            _context.SaveChanges();

            //Act
            _context = new TrainingContextTest(true);
            _runRepo = new RunningRepository(_context);
            _runRepo.RemoveTraining(1);
            //Assert
            _context.RunningSessions.Should().NotContain(runSes0);
        }
    }
}