using System;
using System.Collections.Generic;
using System.Text;

namespace FloodDetection.Domains
{
    public class Reading
    {
        public string DeviceId { get; set; }
        public DateTime Time { get; set; }
        public int Rainfall { get; set; }
    }
}
