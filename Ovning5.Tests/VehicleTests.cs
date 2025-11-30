using Ovning5.Models;

namespace Ovning5.Tests
{
	/*
	 Strukturera testen enligt principen.
		1. Arrange här sätter ni upp det som ska testas, instansierar objekt och inputs
		2. Act här anropar ni metoden som ska testas
		3. Assert här kontrollerar ni att ni fått det förväntade resultatet

		Tänk även på att namnge testen på ett förklarande sätt. När ett test inte går igenom vill vi
		veta vad som inte fungerat enbart genom att se på testmetod namnet.

			Exempelvis:
				[MethodName_StateUnderTest_ExpectedBehavior]
				Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()

	*/

	public class VehicleTests
	{
		[Fact]
		public void Vehicle_NegativeWheels_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Car("abc123", wheels: -1, color: VehicleColor.Yellow, selfDriving: false));
		}

		[Theory]
		[InlineData("abC123", "ABC123")]
		[InlineData("123xyz", "123XYZ")]
		[InlineData("TeSt-456", "TEST-456")]
		public void Vehicle_LowerCaseIdentifying_ConvertsToUpperCase(string id, string expected)
		{
			Car car = new Car(id, wheels: 4, VehicleColor.Red, selfDriving: false);
			Assert.Equal(expected, car.Identifier);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Vehicle_InvalidIdentifying_ThrowsArgumentException(string invalidId)
		{
			Assert.Throws<ArgumentException>(() => new Car(invalidId, wheels: 4, VehicleColor.Green, selfDriving: true));
		}

		// Test constructor for each derived class of Vehicle
		[Fact]
		public void Car_ValidParameters_ObjectCreated()
		{
			string id = "TEST123";
			int wheels = 4;
			VehicleColor color = VehicleColor.Blue;
			bool isSelfDriving = false;

			Car car = new Car(id, wheels, color, selfDriving: isSelfDriving);

			Assert.Equal(id, car.Identifier);
			Assert.Equal(wheels, car.Wheels);
			Assert.Equal(color, car.Color);
			Assert.Equal(isSelfDriving, car.SelfDriving);
		}

		// Test constructor for each derived class of Vehicle

		[Fact]
		public void Airplane_ValidParameters_ObjectCreated()
		{
			string id = "PLANE123";
			int wheels = 6;
			VehicleColor color = VehicleColor.White;
			int engines = 2;

			Airplane plane = new Airplane(id, wheels, color, engines);

			Assert.Equal(id, plane.Identifier);
			Assert.Equal(wheels, plane.Wheels);
			Assert.Equal(color, plane.Color);
			Assert.Equal(engines, plane.Engines);
		}

		[Fact]
		public void Bus_ValidParameters_ObjectCreated()
		{
			string id = "BUS123";
			int wheels = 8;
			VehicleColor color = VehicleColor.Yellow;
			int seats = 50;

			Bus bus = new Bus(id, wheels, color, seats);

			Assert.Equal(id, bus.Identifier);
			Assert.Equal(wheels, bus.Wheels);
			Assert.Equal(color, bus.Color);
			Assert.Equal(seats, bus.Seats);
		}

		[Fact]
		public void Boat_ValidParameters_ObjectCreated()
		{
			string id = "BOAT23";
			int wheels = 0;
			VehicleColor color = VehicleColor.Black;
			int length = 30;

			Boat boat = new Boat(id, wheels, color, length);

			Assert.Equal(id, boat.Identifier);
			Assert.Equal(wheels, boat.Wheels);
			Assert.Equal(color, boat.Color);
			Assert.Equal(length, boat.Length);
		}

		[Fact]
		public void Motorcycle_ValidParameters_ObjectCreated()
		{
			string id = "MOTO23";
			int wheels = 2;
			VehicleColor color = VehicleColor.Red;
			int cylinderVolume = 600;

			Motorcycle mc = new Motorcycle(id, wheels, color, cylinderVolume);

			Assert.Equal(id, mc.Identifier);
			Assert.Equal(wheels, mc.Wheels);
			Assert.Equal(color, mc.Color);
			Assert.Equal(cylinderVolume, mc.CylinderVolume);
		}

		// Test unique properties for the derived classes
		[Fact]
		public void Airplane_NegativeEngineCount_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Airplane("plane123", wheels: 6, VehicleColor.White, engines: -2));
		}

		[Fact]
		public void Bus_NegativeSeatCount_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Bus("bus123", wheels: 8, VehicleColor.Yellow, seats: -4));
		}

		[Fact]
		public void Boat_NegativeLength_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Boat("boat123", wheels: 0, VehicleColor.Black, length: -5));
		}

		[Fact]
		public void Motorcycle_NegativeCylinderVolume_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Motorcycle("moto123", wheels: 2, VehicleColor.Red, cylinderVolume: -250));
		}
	}
}