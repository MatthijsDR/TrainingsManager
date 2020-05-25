using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;

namespace DomainLibrary.Domain.Tests
{
    [TestClass()]
    public class CyclingSessionTests
    {
        [TestMethod()]
        public void CyclingSessionAverageSpeedTest()
        {

            //Arrange
            var runSes = new CyclingSession(new DateTime(1, 1, 1), 100, new TimeSpan(1, 0, 0),null, null, TrainingType.Interval, null, BikeType.IndoorBike);
            var expected = 100;
            //Assert
            runSes.AverageSpeed.Should().Equals(expected);
        }
    }
}