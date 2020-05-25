using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using DomainLibrary.Repositories;
using FluentAssertions;

namespace DomainLibrary.Domain.Tests
{
    [TestClass()]
    public class RunningSessionTests
    {
        [TestMethod()]
        public void RunningSessionTestAverageSpeedTest()
        {
            //Arrange
            var runSes = new RunningSession(new DateTime(1, 1, 1), 100, new TimeSpan(1, 0, 0), null, TrainingType.Interval, null);
            var expected = 100;
            //Assert
            runSes.AverageSpeed.Should().Equals(expected);
        }
    }
}