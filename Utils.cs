using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    static class Utils
    {

        public static bool PrimeTest(long n, int k = 1) // к - ітерації, не чіпай - хата сгорить
        {
            if (n < 0) n = -n;
            if ((n == 2) || (n == 3) || (n == 5)) return true;
            if ((n % 2 == 0) || (n == 1)) return false;

            long x = 1;
            for (int i = 0; i < k; i++)
            {
                Random rand = new Random();
                x = rand.NextInt64(3, n / 2);
                if (GCD(x, n) > 1) return false;
                if (CheckStrongPrime(n, x)) return true;
            }

            return false;

        }

        public static BigInteger Mod(BigInteger a, BigInteger n)
        {
            if (a < 0)
            {
                a = -a * (n - 1);
            }

            BigInteger res = a % n;
            return res;
        }

        // Взято з лаби 1
        public static BigInteger Sqrt(BigInteger value)
        {
            if (value < 0)
                throw new Exception("Cannot compute square root of a negative number.");

            if (value == 0)
                return 0;

            BigInteger low = 0;
            BigInteger high = value;
            BigInteger mid;

            while (low < high)
            {
                mid = (low + high) / 2;
                BigInteger midSquared = mid * mid;

                if (midSquared == value)
                    return mid;
                else if (midSquared < value)
                    low = mid + 1;
                else
                    high = mid;
            }

            return (low - 1);
        }

        private static Int64 TrialDivisions(BigInteger n)
        {
            for (Int64 d = 2; d <= Sqrt(n); d++)
            {
                if (n % d == 0) return d;
            }
            return 1;
        }

        public static Dictionary<BigInteger, int> Factor(BigInteger n)
        {
            Dictionary<BigInteger, int> res = [];
            BigInteger d = n;
            while (true)
            {
                d = TrialDivisions(n);
                if (d == 1) break;
                if (!res.ContainsKey(d)) res.Add(d, 0);
                res[d]++;
                n /= d;
            }
            if (!res.ContainsKey(n)) res.Add(n, 1);
            else res[n]++;
            return res;

        }

        public static BigInteger ExtendedGCD(BigInteger x, BigInteger n, out BigInteger xReverse)
        {
            xReverse = 0;

            BigInteger rPrev = x;
            BigInteger r = n;
            BigInteger q = 0;

            BigInteger u1 = 1;
            BigInteger u2 = 0;
            BigInteger u3 = 0;

            BigInteger v1 = 0;
            BigInteger v2 = 1;
            BigInteger v3 = 0;

            while (true)
            {
                (q, BigInteger rNext) = BigInteger.DivRem(rPrev, r);

                u3 = u1 - (u2 * q);
                v3 = v1 - (v2 * q);

                if (rNext == 0) break;

                // Preparing for the next iteration
                rPrev = r;
                r = rNext;
                u1 = u2;
                u2 = u3;
                v1 = v2;
                v2 = v3;
            }

            if (r == 1) xReverse = Mod(u2, n);

            return r;
        }

        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (b == 0) return a;
            return GCD(b, Mod(a, b));
        }

        public static bool CheckStrongPrime(BigInteger n, BigInteger x)
        {
            if (x >= n || x <= 1) throw new Exception("Bad x: its must be in range");
            BigInteger s, t;
            s = 0;
            t = n - 1;

            while (t % 2 == 0)
            {
                t >>= 1;
                s++;
            }

            BigInteger y = BigInteger.ModPow(x, t, n);

            if ((y == 1) || (y == n - 1))
            {
                return true;
            }

            for (int i = 1; i < s; i++)
            {
                y = BigInteger.ModPow(y, 2, n);
                if (y == n - 1) return true;
            }

            return false;

        }

        public static BigInteger CongruencesSolver(List<BigInteger> ys, List<BigInteger> mods)
        {
            BigInteger N = 1;
            BigInteger X = 0;

            foreach (var n in mods)
            {
                N *= n;
            }

            for (int i = 0; i < ys.Count; i++)
            {
                BigInteger N_i = N / mods[i];
                BigInteger N_iReverse = 0;
                ExtendedGCD(N_i, mods[i], out N_iReverse);
                X += N_i * N_iReverse * ys[i];
            }

            return Mod(X, N);

        }
    
        public static BigInteger RandInt(BigInteger max)
        {

            Random rand = new Random();

            var byteArr = max.ToByteArray();
            rand.NextBytes(byteArr);
            
            BigInteger res = new BigInteger(byteArr.Append<byte>(0x00).ToArray());

            return res % max;

        }

        public static BigInteger BruteForceLog(BigInteger alpha, BigInteger beta, BigInteger p)
        {
            var res = alpha;
            for (BigInteger x = 1; x <= p - 2; x++)
            {
                res = BigInteger.ModPow(alpha, x, p);
                if (res == beta) return x;
            }

            return -1;
        }

    }
}
