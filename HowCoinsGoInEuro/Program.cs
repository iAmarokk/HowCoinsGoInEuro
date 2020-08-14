using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroDiffusion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Program");
            FileWorker fileWorker = new FileWorker();

            Console.WriteLine("Read File Test.txt");
            fileWorker.ReadAllFile();

            Console.ReadKey();
        }
    }
}
