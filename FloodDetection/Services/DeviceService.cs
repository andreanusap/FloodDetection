using FloodDetection.Domains;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FloodDetection.Services
{
    public class DeviceService
    {
        public List<Device> ReadDeviceFile(string location)
        {
            try
            {
                var result = File.ReadAllLines(location).Skip(1)
                 .Select(line => line.Split(','))
                 .Select(x => new Device
                 {
                     DeviceId = x[0],
                     DeviceName = x[1],
                     Location = x[2]
                 }).ToList();


                return result.Where(d => d.DeviceId != string.Empty).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
