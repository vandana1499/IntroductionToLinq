using System;
using System.Collections.Generic;
using System.Text;

namespace IntroductionToLinq
{
    public class Car
    {
        public int Year { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public double Displacement { get; set; }
        public int Cylinders { get; set; }
        public int City { get; set; }
        public int Highway { get; set; }
        public int Combined { get; set; }

        internal static Car ParseFromCsv(string line)
        {
            var col = line.Split(",");
            return new Car
            {
                Year = int.Parse(col[0]),
                Manufacturer = col[1],
                Name = col[2],
                Displacement = double.Parse(col[3]),
                Cylinders = int.Parse(col[4]),
                City = int.Parse(col[5]),
                Highway = int.Parse(col[6]),
                Combined = int.Parse(col[7]),


            };
        }
    }
}
        