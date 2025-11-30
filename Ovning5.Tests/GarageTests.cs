using Ovning5.Managers;
using Ovning5.Models;

namespace Ovning5.Tests
{
	public class GarageTests
	{
		// Constructor

		[Theory]
		[InlineData(0)]
		[InlineData(-5)]
		public void Constructor_InvalidCapacity_ThrowsArgumentException(int capacity)
		{
			Assert.Throws<ArgumentException>(() => new Garage<Car>(capacity));
		}

		[Fact]
		public void Constructor_ValidCapacity_GarageInitialized()
		{
			int capacity = 5;
			var garage = new Garage<Vehicle>(capacity);
			Assert.Equal(capacity, garage.Capacity);
		}

		[Fact]
		public void Constructor_EmptyName_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => new Garage<Vehicle>(5, ""));
		}

		[Fact]
		public void Property_Name_SetAndGetSuccessfully()
		{
			string garageName = "My Garage";
			var garage = new Garage<Vehicle>(5, garageName);
			Assert.Equal(garageName, garage.Name);
		}

		// Enumeration

		[Fact]
		public void GetEnumerator_WithVehicles_ReturnsAllVehicles()
		{
			var garage = new Garage<Vehicle>(3);
			var car1 = new Car("CAR1", wheels: 4, VehicleColor.Red, selfDriving: false);
			var car2 = new Car("CAR2", wheels: 4, VehicleColor.Blue, selfDriving: true);
			var bus1 = new Bus("BUS1", wheels: 6, VehicleColor.Yellow, seats: 40);

			garage.Add(car1);
			garage.Add(car2);
			garage.Add(bus1);
			var vehicles = garage.ToList();

			Assert.Contains(car1, vehicles);
			Assert.Contains(car2, vehicles);
			Assert.Contains(bus1, vehicles);
			Assert.Equal(3, vehicles.Count);
		}

		// Add(T vehicle)

		[Fact]
		public void AddVehicle_ValidVehicle_ReturnsTrue()
		{
			var garage = new Garage<Car>(2);
			var car = new Car("ABC123", wheels: 4, VehicleColor.Black, selfDriving: false);
			Assert.True(garage.Add(car));
			Assert.True(garage.Contains("ABC123"));
		}

		[Fact]
		public void AddVehicle_VehicleIsNull_ThrowsArgumentNullException()
		{
			var garage = new Garage<Car>(2);
			Assert.Throws<ArgumentNullException>(() => garage.Add(null));
		}

		[Fact]
		public void AddVehicle_VehicleAlreadyInGarage_ReturnsFalse()
		{
			var garage = new Garage<Car>(2);
			var car = new Car("car123", wheels: 4, VehicleColor.Black, selfDriving: false);
			garage.Add(car);
			Assert.False(garage.Add(car));
		}

		[Fact]
		public void AddVehicle_GarageIsFull_ReturnsFalse()
		{
			var garage = new Garage<Car>(1);
			var car = new Car("car123", wheels: 4, VehicleColor.Black, selfDriving: false);
			var car2 = new Car("othercar", wheels: 4, VehicleColor.Black, selfDriving: false);

			garage.Add(car);
			Assert.False(garage.Add(car2));
		}

		// Contains

		[Fact]
		public void Contains_IdInLowerCase_ReturnsTrue()
		{
			var garage = new Garage<Car>(2);
			var car = new Car("abc123", wheels: 4, VehicleColor.Black, selfDriving: false);
			garage.Add(car);
			Assert.True(garage.Contains("abc123"));
		}

		[Fact]
		public void Contains_IdIsNull_ThrowsArgumentNullException()
		{
			var garage = new Garage<Car>(2);
			Assert.Throws<ArgumentNullException>(() => garage.Contains(null));
		}

		[Fact]
		public void Contains_IdNotInGarage_ReturnsFalse()
		{
			var garage = new Garage<Car>(2);
			Assert.False(garage.Contains("missing"));
		}

		// Remove(string identifier)
		[Fact]
		public void Remove_IdIsNull_ThrowsArgumentNullException()
		{
			var garage = new Garage<Car>(2);
			Assert.Throws<ArgumentNullException>(() => garage.Remove(null));
		}

		[Fact]
		public void Remove_IdExistsInGarage_ReturnsTrue()
		{
			string carID = "DEF456";
			var garage = new Garage<Car>(2);
			var car = new Car(carID, wheels: 4, VehicleColor.White, selfDriving: true);
			garage.Add(car);
			bool removed = garage.Remove(carID);
			Assert.True(removed);
			Assert.False(garage.Contains(carID));
		}

		[Fact]
		public void Remove_IdNotInGarage_ReturnsFalse()
		{
			var garage = new Garage<Car>(2);

			Assert.False(garage.Remove("notingarage"));
		}

		// GetVehicle(string identifier)
		[Fact]
		public void GetVehicle_IdExists_ReturnsVehicle()
		{
			string carID = "ABC123";

			var garage = new Garage<Car>(2);
			var car = new Car(carID, wheels: 4, VehicleColor.Black, selfDriving: false);
			var otherCar = new Car("someid", wheels: 2, VehicleColor.Green, selfDriving: false);
			garage.Add(car);
			garage.Add(otherCar);
			Assert.Equal(car, garage.GetVehicle("abc123"));
		}

		[Fact]
		public void GetVehicle_IdNotInGarage_ReturnsNull()
		{
			var garage = new Garage<Car>(2);
			Assert.Null(garage.GetVehicle("notingarage"));
		}
	}
}