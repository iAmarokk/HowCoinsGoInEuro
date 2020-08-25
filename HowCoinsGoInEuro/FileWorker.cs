using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EuroDiffusion
{
    class FileWorker
    {
        const int expectedInputLength = 5;
        const int minY = 1;
        const int maxY = 10;
        const int minX = 1;
        const int maxX = 10;
        const int maxCountryNameString = 25;
        const int numberCountries = 20;
        const string like = "Correct like - France 1 4 4 6";
        StreamReader inputData;
        private bool isEndTask;

        public FileWorker()
        {
			this.isEndTask = false;
        }

        /// <summary>
        /// read a file and run solutions
        /// </summary>
        public void ReadAllFile()
        {
            int n;
            string inputLine;
            ///check file is exists
            try
            {
				this.inputData = new StreamReader("Test.txt");
            }
            catch
            {
                throw new Exception("No file");
            }
            ///read file
            int i = 0;
            while (!this.isEndTask)
            {
                inputLine = this.inputData.ReadLine();
                if (!int.TryParse(inputLine, out n))
                {
                    throw new Exception("Wrong input format... Need number countries");
                }

                if (n == 0)
                {
					this.isEndTask = true;
                    Console.WriteLine("Done");
                    return;
                }

                if (n >= numberCountries || n < 1)
                {
                    throw new Exception("Wrong input format... Need number 1 <= countries <= 20");
                }
                ///run solutions
                var Countries = this.Read(n);
                i++;
                ModelDiffusion ModelDiffusion = new ModelDiffusion(Countries);
                PrintNumberSolution(i);
                ModelDiffusion.Solve();
            }
        }

        private static void PrintNumberSolution(int i)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Case Number {0}", i);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public List<Country> Read(int n)
        {
            List<Country> Countries = new List<Country>();
            for (int i = 0; i < n; i++)
            {
                Countries.Add(this.ReadCountry(this.inputData));
            }

            Countries.Sort();
            return Countries;
        }

        /// <summary>
        /// read one string of Country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        private Country ReadCountry(TextReader country)
        {
            int[] coordOfCountry = new int[expectedInputLength - 1];
            Country ResultCountry = new Country();

            string line = country.ReadLine();

            var splitLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (splitLine.Length != expectedInputLength)
            {
                throw new Exception("Wrong input format..." + like);
            }

            for (int i = 0; i < coordOfCountry.Length; i++)
            {
                if (!int.TryParse(splitLine[i + 1], out coordOfCountry[i]))
                {
                    throw new Exception("Wrong input format..." + like);
                }
            }

            ResultCountry.Name = splitLine[0];
            ResultCountry.Xl = coordOfCountry[0];
            ResultCountry.Yl = coordOfCountry[1];
            ResultCountry.Xh = coordOfCountry[2];
            ResultCountry.Yh = coordOfCountry[3];

            if (ResultCountry.Xl > maxX || ResultCountry.Xl < minX || ResultCountry.Xl > ResultCountry.Xh)
            {
                throw new Exception(string.Format("Wrong format in country {0}. {1}<=xl<=xh<={2}", ResultCountry.Name, minX, maxX));
            }
            if (ResultCountry.Yl > maxY || ResultCountry.Yl < minY || ResultCountry.Yl > ResultCountry.Yh)
            {
                throw new Exception(string.Format("Wrong format in country {0}. {1}<=yl<=yh<={2}", ResultCountry.Name, minY, maxY));
            }
            if (ResultCountry.Name.Count() > maxCountryNameString)
            {
                throw new Exception(string.Format("Too long name of country {0} ", ResultCountry.Name));
            }

            return ResultCountry;
        }
    }
}
