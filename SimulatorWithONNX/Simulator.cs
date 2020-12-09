using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SimulatorWithONNX
{
    class Simulator
    {
        public void read_stock_prices(List<(int, int, int, int, double)> records)
        {
            foreach (string line in File.ReadLines("data/stock_prices.txt", Encoding.UTF8))
            {
                // process the line
                string[] fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                (int, int, int, int, double) record = (Convert.ToInt32(fields[0]), Convert.ToInt32(fields[1]), Convert.ToInt32(fields[2]), Convert.ToInt32(fields[3]), Convert.ToDouble(fields[4], CultureInfo.InvariantCulture));
                
                records.Add(record);
            }
        }

    }
}
