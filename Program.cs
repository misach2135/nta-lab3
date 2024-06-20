using System;
using System.Collections.Generic;
using System.Numerics;

namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Utils.PrimeTest(4));
            var indexSolver = new IndexSolver(2586203, 2115834, 6707689);
            List<BigInteger> res = [];

            for (int i = 0; i < 20; i++)
            {
                res.Add(indexSolver.Solve());
            }

            Console.WriteLine("res: {0}", string.Join(',', res));

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