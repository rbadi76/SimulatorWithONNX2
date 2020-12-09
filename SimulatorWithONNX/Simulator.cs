using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

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

        public int find<V, T>(List<V> data, List<int> keyList, T key)
        {
            for(int i=0; i < data.Count(); i++)
            {
                int num_correct = 0;
                for(int j=0; j< keyList.Count(); j++)
                {
                    var t1 = ((ITuple)data[i])[keyList[j]];
                    var t2 = ((ITuple)key)[j];
                    if (t1.Equals(t2))
                    {
                        num_correct++;
                    }
                }
                if (num_correct == keyList.Count())
                {
                    return i;
                }
            }
            throw new ArgumentException("Value not found.");
        }

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

            List<int> key_market_analysis = new List<int>();
            key_market_analysis.Add(1);
            key_market_analysis.Add(2);
            key_market_analysis.Add(0);
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

            //this.printList(info_daily);


            HashSet<int> companies = new HashSet<int>();
            HashSet<string> sectors = new HashSet<string>();

            foreach((int, string) record in market_segments)
            {
                companies.Add(record.Item1);
                sectors.Add(record.Item2);
            }
            /*foreach(var comp in companies)
            {
                Console.WriteLine(comp);
            }
            foreach (var sec in sectors)
            {
                Console.WriteLine(sec);
            }*/
            int num_companies = companies.Count();

            //Call predictor
            Predictor pred = new Predictor();

            // Add here reference to Predictor class

            double total = 0;
            double correct = 0;
            List<double> previous_stock_prices = new List<double>();

            for (int i = 0; i < info_daily.Count(); i = i + num_companies)
            {
                List<(int, int, int, int, int, int, int, double, double, double, int)> daily = info_daily.GetRange(i, num_companies);
                
                List<double> current_stock_prices = new List<double>();

                for (int j = 0; j < num_companies; j++)
                {
                    current_stock_prices.Add(stock_prices[i + j].Item5);
                }
                if (daily[0].Item2 >= start_year && daily[0].Item3 >= start_day)
                {
                    List<(string, int, int, int)> quarterly = new List<(string, int, int, int)>();
                    foreach(var s in sectors)
                    {
                        (int, int, string) key = (daily[0].Item2, daily[0].Item4, s);
                        int idx = this.find(market_analysis, key_market_analysis, key);
                        quarterly.Add(market_analysis[idx]);
                    }
                    //this.printList(quarterly);

                    if(previous_stock_prices.Count() > 0)
                    {
                        List<bool> increased = new List<bool>();
                        for(int c = 0; c<num_companies;c++)
                        {
                            increased.Add(current_stock_prices[c] > previous_stock_prices[c]);
                        }

                        List<bool> y_list= pred.inference(daily);
                        (bool, bool, bool) y = (y_list[0], y_list[1], y_list[2]);

                        string s = String.Format("Predictions (year, day): {0} {1} {2} Target: ({3}, {4}, {5})", daily[0].Item2, daily[0].Item3, y, increased[0], increased[1], increased[2]);
                        Console.WriteLine(s);

                        for(int c = 0; c < num_companies; c++)
                        {
                            if((bool)((ITuple)y)[c] == increased[c])
                            {
                                correct++;
                            }
                            total++;
                        }
                    }
                }
                previous_stock_prices = current_stock_prices.ToList();
            }
            if(total != 0)
                Console.WriteLine(String.Format("Accuracy(%) = {0}", 100 * correct / total));          
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
