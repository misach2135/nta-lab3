using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace lab3
{
    class IndexSolver
    {
        private readonly BigInteger alpha;
        private readonly BigInteger beta;
        private readonly BigInteger n;

        private readonly List<BigInteger> factorBase;

        public IndexSolver(BigInteger alpha, BigInteger beta, BigInteger n)
        {
            this.alpha = alpha;
            this.beta = beta;
            this.n = n;
            factorBase = [];

            formFactorBase();
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
        
        private void checkSmoothness()
        {

        }

        private void formEquation()
        {

        }

        public BigInteger Solve()
        {
            const int c = 40; // bigger-- more accurate, but slower
            
            int rowIndex = 0;
            BigInteger res = 0;
            ModMatrix matrix = new ModMatrix(factorBase.Count + c, factorBase.Count + 1, n - 1);
            SortedSet<BigInteger> usedK = [];
            while (rowIndex < factorBase.Count + c)
            {
                var k = Utils.RandInt(n);
                if (usedK.Contains(k)) continue;
                usedK.Add(k);
                var powAlpha = BigInteger.ModPow(alpha, k, n);
                var factorPowAlpha = Utils.Factor(powAlpha);
                if (factorPowAlpha.Count >= factorBase.Count) continue;

                bool flag = false;
                foreach (var (key, _) in factorPowAlpha)
                {
                    if (!factorBase.Exists(x => x == key))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) continue;
                Console.WriteLine(k);

                for (int j = 0; j < factorBase.Count; j++)
                {
                    int deg = 0;
                    if (factorPowAlpha.ContainsKey(factorBase[j])) deg = factorPowAlpha[factorBase[j]];
                    matrix[rowIndex, j] = deg;
                }
                matrix[rowIndex, factorBase.Count] = k;
                rowIndex++;
            }
            Console.WriteLine("Matrix: {0}", matrix);
            var solution = matrix.Solve();
            Console.WriteLine("Solution: {0}", string.Join(',', solution));

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
