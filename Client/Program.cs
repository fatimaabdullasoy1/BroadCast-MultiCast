using System;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Text.Json;
using Shared;

namespace Client
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nCAR MANAGEMENT");
                Console.WriteLine("1. Get all cars");
                Console.WriteLine("2. Get car by ID");
                Console.WriteLine("3. Add car");
                Console.WriteLine("4. Update car");
                Console.WriteLine("5. Delete car");
                Console.WriteLine("0. Exit");
                Console.Write("Choose pnly obne : ");

                string? choice = Console.ReadLine();

                if (choice == "0")
                    break;

                Command command = new Command();

                switch (choice)
                {
                    case "1":
                        command.Method = HttpMethods.GET;
                        command.Car.Id = 0;
                        break;

                    case "2":
                        command.Method = HttpMethods.GET;
                        Console.Write("Enter car ID: ");
                        command.Car.Id = int.Parse(Console.ReadLine()!);
                        break;

                    case "3":
                        command.Method = HttpMethods.POST;
                        command.Car = ReadCar();
                        break;

                    case "4":
                        command.Method = HttpMethods.PUT;
                        command.Car = ReadCar();
                        command.Car.Id = command.Car.Id;
                        break;

                    case "5":
                        command.Method = HttpMethods.DELETE;
                        Console.Write("Enter car ID: ");
                        command.Car.Id = int.Parse(Console.ReadLine()!);
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        continue;
                }

                SendCommand(command);
            }
        }

        static Car ReadCar()
        {
            Car car = new Car();

            Console.Write("ID: ");
            car.Id = int.Parse(Console.ReadLine()!);

            Console.Write("Make: ");
            car.Make = Console.ReadLine()!;

            Console.Write("Model: ");
            car.Model = Console.ReadLine()!;

            Console.Write("Year: ");
            car.Year = ushort.Parse(Console.ReadLine()!);

            Console.Write("VIN: ");
            car.VIN = Console.ReadLine()!;

            Console.Write("Color: ");
            car.Color = Console.ReadLine()!;

            return car;
        }

        static void SendCommand(Command command)
        {
            try
            {
                using TcpClient client = new TcpClient();

                client.Connect("127.0.0.1", 5000);

                using NetworkStream stream = client.GetStream();
                using StreamReader reader =
                    new StreamReader(stream, Encoding.UTF8);
                using StreamWriter writer =
                    new StreamWriter(stream, Encoding.UTF8)
                    {
                        AutoFlush = true
                    };

                string json = JsonSerializer.Serialize(command);

                writer.WriteLine(json);

                string? response = reader.ReadLine();

                Console.WriteLine("\nSERVER RESPONSE:");
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: " + ex.Message);
            }
        }
    }
}

