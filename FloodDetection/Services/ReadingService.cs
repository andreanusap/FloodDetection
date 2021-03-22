using FloodDetection.Domains;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FloodDetection.Services
{
    public class ReadingService
    {
        public List<Reading> ReadDataFile(string location)
        {
            try
            {
                var result = File.ReadAllLines(location).Skip(1)
                 .Select(line => line.Split(','))
                 .Select(x => new Reading
                 {
                     DeviceId = x[0],
                     Time = !string.IsNullOrEmpty(x[1]) ? DateTime.Parse(x[1]) : DateTime.MinValue,
                     Rainfall = !string.IsNullOrEmpty(x[2]) ? Int32.Parse(x[2]) : 0
                 }).ToList();

                return result.Where(r => r.DeviceId != string.Empty).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
