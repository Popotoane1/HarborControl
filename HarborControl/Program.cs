using HarborControl.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HarborControl
{
    class Program
    {        
        private static Queue<Boat> boats = new Queue<Boat>();
        private static Wind currentWind = new Wind();

        static void Main(string[] args)
        {
            WeatherService ws = new WeatherService();            
            currentWind = ws.GetCurrentWeather().wind;            

            Console.WriteLine("Harbor Control Console System");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Current wind speed is : {currentWind.speed}");

            //2 speedboats, 1 sailboat, 1 cargo 
            generateInitialBoats();

            //Get boat into the perimeter     GetAllowedBoat();
            var allowBoat = GetAllowedBoat(); 
            while (!allowBoat)
            {
                allowBoat = GetAllowedBoat();
            }
            getAllowedBoat();
            var boatWithinPerimeter = boats.Dequeue();
            BoatInPerimeter(boatWithinPerimeter);
            var timeToDock = GetTime(boatWithinPerimeter.speed);
            Task.Delay(timeToDock).Wait();
            displayBoatDocked(boatWithinPerimeter);
            ScheduleBoats();

            // TODO: Random boat generator method to be called. Could not be completed due to time constraint
        }
        
        private static bool getAllowedBoat()
        {            
            bool isAllowed = true;
            float tooSlowWindSpeed = 10;
            float tooFastWindSpeed = 30;
            var firstBoatIntheQueue = boats.Peek();
            if (firstBoatIntheQueue.type == "Sailboat" &&
                (currentWind.speed < tooSlowWindSpeed || currentWind.speed > tooFastWindSpeed))
            {
                // move boat to the last on the queue and check other boats
                var boat = boats.Dequeue();
                boats.Enqueue(boat);
                Console.WriteLine($"Sailboat not allowed in the perimeter due to current wind speed, speed = {currentWind.speed}");

                isAllowed = false;
            }
            return isAllowed;
        }

        private static void generateInitialBoats()
        {
            var cargo = new Boat() { id = 1, type = "Cargo", speed = 5 };
            boats.Enqueue(cargo);
            var speedBoat1 = new Boat() { id = 2, type = "Speedboat", speed = 30 };
            boats.Enqueue(speedBoat1);
            var speedBoat2 = new Boat() { id = 3, type = "Speedboat", speed = 30 };
            boats.Enqueue(speedBoat2);
            var sailBoat = new Boat() { id = 4, type = "Sailboat", speed = 15 };
            boats.Enqueue(sailBoat);
        }

        private static void displayBoatDocked(Boat boat)
        {            
            Console.WriteLine($"The {boat.type} has docked");            
        }
        private static void BoatInPerimeter(Boat boat)
        {
            Console.WriteLine($"The {boat.type} is in the perimeter");
        }

        private static void ScheduleBoats()
        {
            var allowBoat = GetAllowedBoat();
            while (!allowBoat)
            {
                allowBoat = GetAllowedBoat();
            }
            if (boats.Count != 0)
            {
                var allowedBoat = getAllowedBoat();
                if (allowedBoat)
                {
                    var boatInPerimeter = boats.Dequeue();
                    BoatInPerimeter(boatInPerimeter);
                    var timeToDock = GetTime(boatInPerimeter.speed);
                    Task.Delay(timeToDock).Wait();
                    displayBoatDocked(boatInPerimeter);
                }

                ScheduleBoats();
            }
        }

        private static bool GetAllowedBoat()
        {
            Console.WriteLine("\nPress 'Enter' to allow boat to enter the harbor perimeter and wait for the boat to dock before allowing another one:");
            return (Console.ReadKey().Key == ConsoleKey.Enter);
        }

        private static int GetTime(int boatSpeed)
        {
            double distance = 10;
            double timeToTake = distance / (double)boatSpeed * 3600;            
            return Convert.ToInt32(timeToTake);
        }
    }
}
