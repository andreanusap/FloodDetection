using FloodDetection.Domains;
using FloodDetection.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FloodDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Flood Detection Program....");

            Console.WriteLine("Process Device Data....");
            var _deviceService = new DeviceService();
            string deviceFilePath = ConfigurationManager.AppSettings["DeviceFile"];
            var deviceResults = _deviceService.ReadDeviceFile(deviceFilePath);

            Console.WriteLine("Process Reading Data....");
            string data1FilePath = ConfigurationManager.AppSettings["Data1File"];
            string data2FilePath = ConfigurationManager.AppSettings["Data2File"];
            var readingResults = GetReadingData(data1FilePath, data2FilePath);

            Console.WriteLine("Flood Status: ");

            ProcessReadingData(deviceResults, readingResults);
        }

        private static List<Reading> GetReadingData(string location1, string location2)
        {
            var _readingService = new ReadingService();

            var result1 = _readingService.ReadDataFile(location1);
            var result2 = _readingService.ReadDataFile(location2);
            return result1.Concat(result2).ToList();
        }

        private static void ProcessReadingData(IEnumerable<Device> devices, IEnumerable<Reading> readings)
        {
            
            DateTime current = new DateTime(2020, 5, 6, 14, 0, 0); //14 o'clock based on the latest time
            DateTime start = current.AddHours(-4);
            foreach (var device in devices) ///process reading data based on device data
            {
                var read = readings.Where(r => r.DeviceId == device.DeviceId && (r.Time >= start && r.Time <= current));

                if (read.Any(r => r.Rainfall > 30)) ///if there is any reading above 30mm in last 4 hours
                {
                    Console.WriteLine(device.DeviceId + " " + device.DeviceName + " " + device.Location);

                    Console.WriteLine("  Status: Red");
                }
                else ///else count the average rainfall to get the status
                {

                    var sumRead = read.Sum(r => r.Rainfall);
                    var countRead = read.Count();
                    var averageRainfall = 0;
                    if (sumRead > 0 && countRead > 0)
                    {
                        averageRainfall = sumRead / countRead;
                    }

                    var status = "";
                    if (averageRainfall < 10) ///less than 10mm
                    {
                        status = "Green";
                    }
                    else if (averageRainfall >= 10 && averageRainfall < 15) ///less than 15mm and 10mm and above
                    {
                        status = "Amber";
                    }
                    else if (averageRainfall >= 15) ///more than 15mm
                    {
                        status = "Red";
                    }

                    Console.WriteLine(device.DeviceId + " " + device.DeviceName + " " + device.Location);

                    Console.WriteLine("  Status: " + status);
                }
            }
        }
    }
}
