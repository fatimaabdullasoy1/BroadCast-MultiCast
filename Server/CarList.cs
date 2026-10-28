using System.Collections.Generic;
using System.Linq;
using Shared;

namespace Server
{
    public class CarList
    {
        private List<Car> cars = new List<Car>
        {
            new Car
            {
                Id = 1,
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                VIN = "VIN001",
                Color = "Black"
            },
            new Car
            {
                Id = 2,
                Make = "BMW",
                Model = "M4",
                Year = 2024,
                VIN = "VIN002",
                Color = "White"
            }
        };

        public Car? GetById(int id)
        {
            return cars.FirstOrDefault(c => c.Id == id);
        }

        public List<Car> GetAll()
        {
            return cars;
        }

        public bool Add(Car car)
        {
            if (cars.Any(c => c.Id == car.Id))
                return false;

            cars.Add(car);
            return true;
        }

        public bool Update(Car car)
        {
            Car? oldCar = GetById(car.Id);

            if (oldCar == null)
                return false;

            oldCar.Make = car.Make;
            oldCar.Model = car.Model;
            oldCar.Year = car.Year;
            oldCar.VIN = car.VIN;
            oldCar.Color = car.Color;

            return true;
        }

        public bool Delete(int id)
        {
            Car? car = GetById(id);

            if (car == null)
                return false;

            cars.Remove(car);
            return true;
        }
    }
}