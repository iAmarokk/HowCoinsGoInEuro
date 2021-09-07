using System;

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
