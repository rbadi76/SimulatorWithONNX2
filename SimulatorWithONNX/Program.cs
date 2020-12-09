using System;
using System.Collections.Generic;

namespace SimulatorWithONNX
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            List<(int, int, int, int, double)> records1 = new List<(int, int, int, int, double)>();

            Simulator sim = new Simulator();

            sim.read_stock_prices(records1);
            //sim.printList(records1);

            List<(string, int, int, int)> records2 = new List<(string, int, int, int)>();
            sim.read_market_analysis(records2);
            //sim.printList(records2);

            List<(int, string)> records3 = new List<(int, string)>();
            sim.read_market_segments(records3);
            //sim.printList(records3);

            List<(int, int, int, int, int, int, int, double, double, double, int)> records4 = new List<(int, int, int, int, int, int, int, double, double, double, int)>();
            sim.read_info(records4);
            //sim.printList(records4);

            sim.simulate(2019, 3);

        }
    }
}
