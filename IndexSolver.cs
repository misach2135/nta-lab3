using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Diagnostics;

namespace lab3
{
    class IndexSolver
    {
        private readonly BigInteger alpha;
        private readonly BigInteger beta;
        private readonly BigInteger n;

        private readonly List<BigInteger> factorBase;
        private readonly ModMatrix matrix;

        public IndexSolver(BigInteger alpha, BigInteger beta, BigInteger n)
        {
            const int c = 50;
            this.alpha = alpha;
            this.beta = beta;
            this.n = n;
            factorBase = [];
            formFactorBase();
            matrix = new ModMatrix(factorBase.Count + c, factorBase.Count + 1, n - 1);
            Console.WriteLine("Factor base: {0}", string.Join(',', factorBase));

        }

        private void formFactorBase(double c = 3.38)
        {
            factorBase.Clear();

            double logN = BigInteger.Log(n);
            double loglogN = BigInteger.Log(new BigInteger(logN));
            double B = c * Math.Exp(0.5 * Math.Sqrt(logN * loglogN));

            for (long i = 1; i < B; i++)
            {
                if (Utils.PrimeTest(i))
                {
                    factorBase.Add(i);
                }
            }
        }
        
        private bool checkSmoothness(Dictionary<BigInteger, int> factorization)
        {
            if (factorization.Count >= factorBase.Count) return false;
            bool flag = false;
            foreach (var (key, _) in factorization)
            {
                if (!factorBase.Exists(x => x == key))
                {
                    flag = true;
                    break;
                }
            }
            if (flag) return false;
            return true;
        }

        private void formEquations(int fromRow, int count)
        {
            int counter = fromRow;
            while (counter < fromRow + count)
            {
                var k = Utils.RandInt(n);
                //if (usedK.Contains(k)) continue;
                //usedK.Add(k);
                var powAlpha = BigInteger.ModPow(alpha, k, n);
                var factorPowAlpha = Utils.Factor(powAlpha);

                if (!checkSmoothness(factorPowAlpha)) continue;
                // Console.WriteLine(k);
                Debug.Assert(counter < matrix.RowsCount);

                for (int j = 0; j < factorBase.Count; j++)
                {
                    int deg = 0;
                    if (factorPowAlpha.ContainsKey(factorBase[j])) deg = factorPowAlpha[factorBase[j]];
                    lock (matrix)
                    {
                        matrix[counter, j] = deg;
                    }
                }
                lock (matrix) matrix[counter, factorBase.Count] = k;
                counter++;
            }
        }

        public BigInteger SolveMultiThread()
        {
            matrix.Clear();
            BigInteger res = 0;

            int start = 0;
            int step = (int)matrix.RowsCount / 4;

            Thread thread1 = new Thread(() => formEquations(start, step));
            thread1.Start();
            Thread thread2 = new Thread(() => formEquations(start + step * 1, step));
            thread2.Start();
            Thread thread3 = new Thread(() => formEquations(start + step * 2, step));
            thread3.Start();
            Thread thread4 = new Thread(() => formEquations(start + step * 3, step));
            thread4.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            //Console.WriteLine("Matrix: {0}", matrix);
            var solution = matrix.Solve();


            while (true)
            {
                BigInteger l = Utils.RandInt(n - 1);
                var powL = Utils.Mod(beta * BigInteger.ModPow(alpha, l, n), n);
                var factorPowL = Utils.Factor(powL);

                bool flag = false;
                foreach (var (key, _) in factorPowL)
                {
                    if (!factorBase.Exists(x => x == key))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) continue;

                for (int i = 0; i < factorBase.Count; i++)
                {
                    int deg = 0;
                    if (factorPowL.ContainsKey(factorBase[i])) deg = factorPowL[factorBase[i]];
                    res += solution[i] * deg;
                    res = Utils.Mod(res, n - 1);
                }
                res = Utils.Mod(res - l, n - 1);
                break;
            }

            return res;
        }

        public BigInteger SolveOneThread()
        {   
            int rowIndex = 0;
            BigInteger res = 0;
            SortedSet<BigInteger> usedK = [];
            matrix.Clear();
            while (rowIndex < matrix.RowsCount)
            {
                var k = Utils.RandInt(n);
                //if (k > n) Console.WriteLine("Bigger!");
                //if (usedK.Contains(k)) continue;
                //usedK.Add(k);
                var powAlpha = BigInteger.ModPow(alpha, k, n);
                var factorPowAlpha = Utils.Factor(powAlpha);

                if (!checkSmoothness(factorPowAlpha)) continue;
                //Console.WriteLine(k);

                for (int j = 0; j < factorBase.Count; j++)
                {
                    int deg = 0;
                    if (factorPowAlpha.ContainsKey(factorBase[j])) deg = factorPowAlpha[factorBase[j]];
                    matrix[rowIndex, j] = deg;
                }
                matrix[rowIndex, factorBase.Count] = k;
                rowIndex++;
            }
            //Console.WriteLine("Matrix: {0}", matrix);
            var solution = matrix.Solve();
            //Console.WriteLine("Solution: {0}", string.Join(',', solution));

            while (true)
            {
                BigInteger l = Utils.RandInt(n - 1);
                var powL = Utils.Mod(beta * BigInteger.ModPow(alpha, l, n), n);
                var factorPowL = Utils.Factor(powL);

                bool flag = false;
                foreach (var (key, _) in factorPowL)
                {
                    if (!factorBase.Exists(x => x == key))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) continue;

                for (int i = 0; i < factorBase.Count; i++)
                {
                    int deg = 0;
                    if (factorPowL.ContainsKey(factorBase[i])) deg = factorPowL[factorBase[i]];
                    res += solution[i] * deg;
                    res = Utils.Mod(res, n - 1);
                }
                res = Utils.Mod(res - l, n - 1);
                break;
            }

            return res;
        }


    }
}
