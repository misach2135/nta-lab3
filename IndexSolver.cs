using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    class IndexSolver
    {
        BigInteger alpha;
        BigInteger beta;
        BigInteger n;

        public IndexSolver(BigInteger alpha, BigInteger beta, BigInteger n)
        {
            this.alpha = alpha;
            this.beta = beta;
            this.n = n;
        }

        private List<BigInteger> formFactorBase(double c = 3.38)
        {
            List<BigInteger> factorBase = [];

            double logN = BigInteger.Log(n);
            double loglogN = BigInteger.Log(new BigInteger(logN));
            double B = c * Math.Exp(0.5 * Math.Sqrt(logN * loglogN));

            for (int i = 1; i < B; i++)
            {

            }


            return [];
        }

        public BigInteger Solve()
        {

            return 0;
        }


    }
}
