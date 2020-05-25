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
    public class CyclingRepositoryTests
    {
        private CyclingRepository _cycRepo;
        private TrainingContextTest _context;
        private void SetRepoAndContext()
        {
            _context = new TrainingContextTest();
            _cycRepo = new CyclingRepository(_context);
        }
        [TestMethod()]
        public void AddTrainingTest()
        {
            //Arrange
            SetRepoAndContext();
            var cycSes = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            int initCount = _context.CyclingSessions.Count();
            //Act
            _cycRepo.AddTraining(cycSes);
            _context.SaveChanges();
            //Assert
            Assert.AreEqual(initCount + 1, _context.CyclingSessions.Count());

        }

        [TestMethod()]
        public void FindTest()
        {
            //Arrange
            SetRepoAndContext();
            var cycSes = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            _cycRepo.AddTraining(cycSes);
            _context.SaveChanges();

            //Act
            var actual = _cycRepo.Find(1);

            //Assert  
            actual.Should().Equals(cycSes);

        }

        [TestMethod()]
        public void FindAllTest()
        {
            //Arrange
            SetRepoAndContext();
            var cycSes0 = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes1 = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes2 = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes3 = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes4 = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            _cycRepo.AddTraining(cycSes0);
            _cycRepo.AddTraining(cycSes1);
            _cycRepo.AddTraining(cycSes2);
            _cycRepo.AddTraining(cycSes3);
            _cycRepo.AddTraining(cycSes4);
            _context.SaveChanges();
            //Act
            var actuals = _cycRepo.FindAll();
            //Assert
            actuals.Count().Should().Be(5);

        }

        [TestMethod()]
        public void FindTest1()
        {
            //Arrange
            SetRepoAndContext();
            var cycSes0 = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes1 = new CyclingSession(DateTime.Now.AddDays(1), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes2 = new CyclingSession(DateTime.Now.AddDays(20), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes3 = new CyclingSession(DateTime.Now.AddDays(-10), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes4 = new CyclingSession(DateTime.Now.AddYears(1), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            _cycRepo.AddTraining(cycSes0);
            _cycRepo.AddTraining(cycSes1);
            _cycRepo.AddTraining(cycSes2);
            _cycRepo.AddTraining(cycSes3);
            _cycRepo.AddTraining(cycSes4);
            _context.SaveChanges();
            //Act
           var actuals = _cycRepo.Find(DateTime.Now, DateTime.Now.AddDays(21));
            //Assert
            actuals.Should().Contain(new[] { cycSes1, cycSes2 });
        }

        [TestMethod()]
        public void FindLatestSessionsTest()
        {
            //Arrange
            SetRepoAndContext();
            var cycSes0 = new CyclingSession(DateTime.Now, 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes1 = new CyclingSession(DateTime.Now.AddDays(1), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes2 = new CyclingSession(DateTime.Now.AddDays(20), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes3 = new CyclingSession(DateTime.Now.AddDays(-10), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes4 = new CyclingSession(DateTime.Now.AddYears(1), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            _cycRepo.AddTraining(cycSes0);
            _cycRepo.AddTraining(cycSes1);
            _cycRepo.AddTraining(cycSes2);
            _cycRepo.AddTraining(cycSes3);
            _cycRepo.AddTraining(cycSes4);
            _context.SaveChanges();
            //Act
           var actuals =  _cycRepo.FindLatestSessions(2);
            //Assert
            actuals.Should().Contain(new[] { cycSes4, cycSes2 });
        }

        [TestMethod()]
        public void FindMaxSessionsTest()
        {
            //Arrange
            SetRepoAndContext();
            var cycSes0 = new CyclingSession(DateTime.Now, 250, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes1 = new CyclingSession(DateTime.Now.AddDays(1), 45, new TimeSpan(5), 25, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes2 = new CyclingSession(DateTime.Now.AddDays(20), 45, new TimeSpan(5), 15, 750, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes3 = new CyclingSession(DateTime.Now.AddDays(-10), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes4 = new CyclingSession(DateTime.Now.AddYears(1), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            _cycRepo.AddTraining(cycSes0);
            _cycRepo.AddTraining(cycSes1);
            _cycRepo.AddTraining(cycSes2);
            _cycRepo.AddTraining(cycSes3);
            _cycRepo.AddTraining(cycSes4);
            _context.SaveChanges();
            //Act
            var actuals = _cycRepo.FindMaxSessions();
            //Assert
            actuals[0].Should().Be(cycSes0);
            actuals[1].Should().Be(cycSes1);
            actuals[2].Should().Be(cycSes2);

        }

        [TestMethod()]
        public void RemoveTrainingTest()
        {
            //Arrange
            SetRepoAndContext();
            var cycSes0 = new CyclingSession(DateTime.Now, 250, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes1 = new CyclingSession(DateTime.Now.AddDays(1), 45, new TimeSpan(5), 25, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes2 = new CyclingSession(DateTime.Now.AddDays(20), 45, new TimeSpan(5), 15, 750, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes3 = new CyclingSession(DateTime.Now.AddDays(-10), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            var cycSes4 = new CyclingSession(DateTime.Now.AddYears(1), 45, new TimeSpan(5), 15, 456, TrainingType.Endurance, null, BikeType.CityBike);
            _cycRepo.AddTraining(cycSes0);
            _cycRepo.AddTraining(cycSes1);
            _cycRepo.AddTraining(cycSes2);
            _cycRepo.AddTraining(cycSes3);
            _cycRepo.AddTraining(cycSes4);
            _context.SaveChanges();
            //Act
            _cycRepo.RemoveTraining(1);
            //Assert
            _context.CyclingSessions.Should().NotContain(cycSes0);
        }
    }
}