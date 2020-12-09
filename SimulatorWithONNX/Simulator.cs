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

        public void read_market_analysis(List<(string, int, int, int)> records)
        {
            foreach(string line in File.ReadLines("data/market_analysis.txt", Encoding.UTF8))
            {
                // process the line
                string[] fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                (string, int, int, int) record = (fields[0], Convert.ToInt32(fields[1]), Convert.ToInt32(fields[2]), Convert.ToInt32(fields[3]));

                records.Add(record);
            }
        }

        public void read_market_segments(List<(int, string)> records)
        {
            foreach(string line in File.ReadLines("data/market_segments.txt", Encoding.UTF8))
            {
                // process the line
                string[] fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                (int, string) record = (Convert.ToInt32(fields[0]), fields[1]);

                records.Add(record);
            }
        }

        public void read_info(List<(int, int, int, int, int, int, int, double, double, double, int)> records)
        {
            foreach (string line in File.ReadLines("data/info.txt", Encoding.UTF8))
            {
                // process the line
                string[] fields = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                (int, int, int, int, int, int, int, double, double, double, int) record 
                    = (Convert.ToInt32(fields[0]), Convert.ToInt32(fields[1]), Convert.ToInt32(fields[2]), Convert.ToInt32(fields[3]),
                    Convert.ToInt32(fields[4]), Convert.ToInt32(fields[5]), Convert.ToInt32(fields[6]), Convert.ToDouble(fields[7], CultureInfo.InvariantCulture),
                     Convert.ToDouble(fields[8], CultureInfo.InvariantCulture), Convert.ToDouble(fields[9], CultureInfo.InvariantCulture),
                     Convert.ToInt32(fields[10]));

                records.Add(record);
            }
        }

       /* public int find(List<(string, int, int, int)> data, List<(int, int, string)> keyList, (int, int, string) key)
        {
            for(int i=0; i < data.Count(); i++)
            {
                if(keyList[data[i]] == key)
                {
                    return i;
                }
            }
            throw new ArgumentException("Value not found.");
        }*/

        public void simulate(int start_year, int start_day)
        {
            // Stock prices
            List<(int, int, int, int, double)> stock_prices = new List<(int, int, int, int, double)>();
            this.read_stock_prices(stock_prices);

            // Sort the list
            stock_prices.Sort(
                (t1, t2) =>
                {
                    int result = t1.Item2.CompareTo(t2.Item2);  
                    if(result == 0)
                    {
                        result = t1.Item3.CompareTo(t2.Item3);
                        if(result == 0)
                        {
                            result = t1.Item1.CompareTo(t2.Item1);
                        }
                    }
                    return result;
                });

            //List<(int, int, int, int, double)> stock_prices2 = stock_prices.OrderBy(a => a.Item2);
            //this.printList(stock_prices);

            // Market analysis
            List<(string, int, int, int)> market_analysis = new List<(string, int, int, int)>();
            this.read_market_analysis(market_analysis);

            // Sort the list
            market_analysis.Sort(
                (t1, t2) =>
                {
                    int result = t1.Item2.CompareTo(t2.Item2);
                    if (result == 0)
                    {
                        result = t1.Item3.CompareTo(t2.Item3);
                        if (result == 0)
                        {
                            result = t1.Item1.CompareTo(t2.Item1);
                        }
                    }
                    return result;
                });

            //this.printList(market_analysis);

            // Market segments
            List<(int, string)> market_segments = new List<(int, string)>();
            this.read_market_segments(market_segments);

            // Sort the list
            market_segments.Sort(
                (t1, t2) =>
                {
                    return t1.Item1.CompareTo(t2.Item1);
                });

            //this.printList(market_segments);

            List<(int, int, int, int, int, int, int, double, double, double, int)> info_daily = new List<(int, int, int, int, int, int, int, double, double, double, int)>();
            this.read_info(info_daily);

            // Sort the list
            info_daily.Sort(
                (t1, t2) =>
                {
                    int result = t1.Item2.CompareTo(t2.Item2);
                    if (result == 0)
                    {
                        result = t1.Item3.CompareTo(t2.Item3);
                        if (result == 0)
                        {
                            result = t1.Item1.CompareTo(t2.Item1);
                        }
                    }
                    return result;
                });

            this.printList(info_daily);


        }

        public void printList<T>(IList<T> list)
        {
            foreach (var rec in list)
            {
                Console.WriteLine(rec);
            }
        }

    }

}
