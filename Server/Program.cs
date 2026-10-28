using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Text.Json;
using Shared;

namespace Server
{
    class Program
    {
        static void Main()
        {
            TcpListener listener =
                new TcpListener(IPAddress.Loopback, 5000);

            CarList service = new CarList();

            listener.Start();

            Console.WriteLine("CAR MANAGEMENT SERVER");
            Console.WriteLine("Waiting for client Commands...");

            while (true)
            {
                using TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Client connected!");

                using NetworkStream stream = client.GetStream();
                using StreamReader reader =
                    new StreamReader(stream, Encoding.UTF8);
                using StreamWriter writer =
                    new StreamWriter(stream, Encoding.UTF8)
                    {
                        AutoFlush = true
                    };

                string? message = reader.ReadLine();

                if (message == null)
                    continue;

                Command? Command =
                    JsonSerializer.Deserialize<Command>(message);

                if (Command == null)
                {
                    writer.WriteLine("Invalid Command.");
                    continue;
                }

                string response = "";

                switch (Command.Method)
                {
                    case HttpMethods.GET:
                        if (Command.Car.Id == 0)
                        {
                            response = JsonSerializer.Serialize(
                                service.GetAll());
                        }
                        else
                        {
                            Car? car = service.GetById(Command.Car.Id);

                            response = car == null
                                ? "Car not found."
                                : JsonSerializer.Serialize(car);
                        }
                        break;

                    case HttpMethods.POST:
                        response = Command.Car != null &&
                            service.Add(Command.Car)
                            ? "Car added successfully."
                            : "Could not add car.";
                        break;

                    case HttpMethods.PUT:
                        response = Command.Car != null &&
                            service.Update(Command.Car)
                            ? "Car updated successfully."
                            : "Car not found or update failed.";
                        break;

                    case HttpMethods.DELETE:
                        response = service.Delete(Command.Car.Id)
                            ? "Car deleted successfully."
                            : "Car not found.";
                        break;

                    default:
                        response = "Unknown Command.";
                        break;
                }

                Console.WriteLine("Command: " + Command.Method);

                writer.WriteLine(response);

                Console.WriteLine("Response sent!");
            }
        }
    }
}

