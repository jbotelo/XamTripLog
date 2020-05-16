using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TripLog.Services;
using TripLog.ViewModels;
using TripLog.Models;

namespace TripLog.Tests
{
    [TestFixture]
    public class NewEntryViewModelTests
    {
        private Mock<INavService> _navMock;
        private Mock<ITripLogDataService> _dataMock;
        private Mock<ILocationService> _lockMock;
        private NewEntryViewModel _vm;

        [SetUp]
        public void Setup()
        {
            _navMock = new Mock<INavService>();
            _dataMock = new Mock<ITripLogDataService>();
            _lockMock = new Mock<ILocationService>();


            _navMock.Setup(nv => nv.GoBack()).Verifiable();
            _dataMock.Setup(d => d.AddEntryAsync(It.Is<TripLogEntry>(entry =>
            entry.Title == "Mock Entry"))).Verifiable();
            _lockMock.Setup(ls => ls.GetGeoCoordinatesAsync())
                .ReturnsAsync(new GeoCoords
                {
                    Latitude = 123,
                    Longitude = 321
                });

            _vm = new NewEntryViewModel(_navMock.Object, _lockMock.Object,
                _dataMock.Object);
        }

        [Test]
        public void Init_EntryIsSetWithGeoCoordinates()
        {
            // Arrange
            _vm.Latitude = 0.0;
            _vm.Longitude = 0.0;

            // Act
            _vm.Init();

            // Assert
            Assert.AreEqual(123, _vm.Latitude);
            Assert.AreEqual(321, _vm.Longitude);
        }

        [Test]
        public void SaveCommand_TitleIsEmpty_CanExecuteReturnsFalse()
        {
            // Arrange
            _vm.Title = "";

            // Act
            var canSave = _vm.SaveCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canSave);
        }

        [Test]
        public void SaveCommand_AddsEntryToTripLogBackend()
        {
            // Arrange
            _vm.Title = "Mock Entry";

            // Act
            _vm.SaveCommand.Execute(null);

            // Assert
            _dataMock.Verify(d => d.AddEntryAsync(It.Is<TripLogEntry>(entry =>
            entry.Title == "Mock Entry")), Times.Once);
        }

        [Test]
        public void SaveCommand_NavigatesBack()
        {
            // Arrange
            _vm.Title = "Mock Entry";

            // Act
            _vm.SaveCommand.Execute(null);

            // Assert
            _navMock.Verify(nv => nv.GoBack(), Times.Once);
        }
    }
}
