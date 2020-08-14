using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        StreamReader inputData;
        private bool isEndTask;
        //public IList<Country> Countries { get; private set; }
        public FileWorker()
        {
            isEndTask = false;
        }
        public void ReadAllFile()
        {
            try
            {
                inputData = new StreamReader("Test.txt");
            }
            catch
            {
                throw new Exception("No file");
            }

        }
        public List<Country> Read()
        {
            int n;
            string inputLine;
            List<Country> Countries = new List<Country>();
            inputLine = inputData.ReadLine();
            if (!int.TryParse(inputLine, out n))
            {
                throw new ArgumentException("Wrong input format");
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    Countries.Add(ReadCountry(inputData));
                }
            }
            Countries.Sort();
            return Countries;
        }

        private Country ReadCountry(TextReader country)
        {
            int[] coordOfCountry = new int[expectedInputLength - 1];
            Country ResultCountry = new Country();

            string line = country.ReadLine();

            var splitLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (splitLine.Length != expectedInputLength)
            {
                throw new ArgumentException("Wrong input format.");
            }

            for (int i = 0; i < coordOfCountry.Length; i++)
            {
                if (!int.TryParse(splitLine[i + 1], out coordOfCountry[i]))
                {
                    throw new ArgumentException("Wrong input format");
                }
            }

            ResultCountry.Name = splitLine[0];
            ResultCountry.Xl = coordOfCountry[0];
            ResultCountry.Yl = coordOfCountry[1];
            ResultCountry.Xh = coordOfCountry[2];
            ResultCountry.Yh = coordOfCountry[3];

            if (ResultCountry.Xl > maxX || ResultCountry.Xl < minX || ResultCountry.Xl > ResultCountry.Xh)
            {
                throw new ArgumentException(string.Format("Wrong format in country {0}. {1}<=xl<=xh<={2}", ResultCountry.Name, minX, maxX));
            }
            if (ResultCountry.Yl > maxY || ResultCountry.Yl < minY || ResultCountry.Yl > ResultCountry.Yh)
            {
                throw new ArgumentException(string.Format("Wrong format in country {0}. {1}<=yl<=yh<={2}", ResultCountry.Name, minY, maxY));
            }
            if (ResultCountry.Name.Count() > maxCountryNameString)
            {
                throw new ArgumentException(string.Format("Too long name of country {0} ", ResultCountry.Name));
            }

            return ResultCountry;
        }
    }
}
