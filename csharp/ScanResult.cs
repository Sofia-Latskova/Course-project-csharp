using System.Collections.Generic;

namespace Firewatch
{
    public class ScanResult
    {
        public class Reading
        {
            public string Serial { get; set; } = "";
            public string Kind { get; set; } = "";
            public double Value { get; set; }
            public bool Reliable { get; set; }
        }

        public List<Reading> Readings { get; } = new();
        public bool HasReliableData { get; set; }
    }
}