using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.Linq;

namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Utils.PrimeTest(4));
            
            string input = "";

            Stopwatch sw = new Stopwatch();

            Console.WriteLine("Enter params, separeted with space: ");
            input = Console.ReadLine();
            string[] param = input.Split(' ');

            var alpha = BigInteger.Parse(param[0]);
            var beta = BigInteger.Parse(param[1]);
            var n = BigInteger.Parse(param[2]);

            var indexSolver = new IndexSolver(alpha, beta, n);

            input = "";
            while(input != "exit")
            {
                
                Console.WriteLine("Entry mode, in which you want to solve DLP(one, multi, exit): ");
                input = Console.ReadLine();
            
                sw.Reset();
                if(input == "one")
                {
                    sw.Start();
                    var res1 = indexSolver.SolveOneThread();
                    sw.Stop();
                    Console.WriteLine("(One-threaded)   Execution ended for {0} ms. with result {1}", sw.ElapsedMilliseconds, res1);
                }
                if (input == "multi")
                {
                    sw.Start();
                    var res2 = indexSolver.SolveMultiThread();
                    sw.Stop();
                    Console.WriteLine("(Multi-threaded) Execution ended for {0} ms. with result {1}", sw.ElapsedMilliseconds, res2);

                }

            }
            


            

            //ModMatrix modMatrix = new ModMatrix(new BigInteger[,] {
            //    { 1, 0, 1, 0, 1},
            //    { 1, 1, 0, 0, 2},
            //    { 2, 0, 0, 1, 6},
            //    { 0, 2, 1, 0, 7},

            //}, 46);

            //Console.WriteLine(modMatrix);

            //var res = modMatrix.Solve();
            //Console.WriteLine(string.Join(',', res));

            //Console.WriteLine(modMatrix);


        }
    }
}

            //int[] counts = new int[10];
            //for (int i = 0; i < 20; i++)
            //{
            //    var res = Utils.RandInt(10);
            //    counts[((int)res)]++;
            //    //Console.Write("{0} ,", res);
            //}

            //Console.WriteLine();
            //Console.WriteLine();

            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine("{0}:  {1}", i, counts[i]);
            //}