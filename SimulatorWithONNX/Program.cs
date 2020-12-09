using System;
using System.Collections.Generic;

namespace SimulatorWithONNX
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            List<(int, int, int, int, double)> records = new List<(int, int, int, int, double)>();

            Simulator sim = new Simulator();
            sim.read_stock_prices(records);

            foreach(var rec in records)
            {
                Console.WriteLine(rec);
            }
        }
    }
}
