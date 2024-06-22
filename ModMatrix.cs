using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    class ModMatrix
    {
        private readonly BigInteger[,] data;
        private BigInteger mod;

        public ModMatrix()
        {
            data = new BigInteger[0, 0];
            mod = 0;
        }
        public ModMatrix(int rows, int cols, BigInteger mod)
        {
            data = new BigInteger[rows, cols];
            this.mod = mod;
        }

        public ModMatrix(BigInteger[,] rawModMatrix, BigInteger mod)
        {
            data = (rawModMatrix.Clone() as BigInteger[,]) ?? new BigInteger[0, 0];
            this.mod = mod;
        }

        public ModMatrix(List<BigInteger[]> bools, BigInteger mod)
        {
            if (bools.Count == 0)
            {
                data = new BigInteger[0, 0];
                return;
            }
            data = new BigInteger[bools.Count, bools[0].Length];
            this.mod = mod;
            int t = 0;
            bools.ForEach(x =>
            {
                if (x.Length != bools[0].Length) throw new Exception("List of bools must contain equal sized bool arrays");

                for (int i = 0; i < data.GetLength(1); i++)
                {
                    data[t, i] = x[i];
                }

                t++;

            });
        }

        public BigInteger this[int i, int j]
        {
            get
            {
                return data[i, j];
            }
            set
            {
                data[i, j] = value;
            }
        }

        public int RowsCount
        {
            get => data.GetLength(0);
        }

        public int ColsCount
        {
            get => data.GetLength(1);
        }

        public void CombineCols(int col1, int col2)
        {
            for (int i = 0; i < RowsCount; i++)
            {
                data[i, col1] = Utils.Mod(data[i, col1] + data[i, col2], mod);
            }
        }

        public void CombineRows(int row1, int row2)
        {
            for (int i = 0; i < ColsCount; i++)
            {
                data[row1, i] = Utils.Mod(data[row1, i] + data[row2, i], mod);
            }
        }

        public void CombineRowsWithMult(int row1, int row2, BigInteger a)
        {
            for (int i = 0; i < ColsCount; i++)
            {
                data[row1, i] = Utils.Mod(data[row1, i] + data[row2, i] * a, mod);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColsCount; j++)
                {
                    data[i, j] = 0;
                }
            }
        }
        public void SwapCols(int col1, int col2)
        {
            for (int i = 0; i < RowsCount; i++)
            {
                (data[i, col2], data[i, col1]) = (data[i, col1], data[i, col2]);
            }
        }

        public void SwapRows(int row1, int row2)
        {
            for (int i = 0; i < ColsCount; i++)
            {
                (data[row2, i], data[row1, i]) = (data[row1, i], data[row2, i]);
            }
        }

        public BigInteger[] Solve()
        {
            List<int> processed = [];
            BigInteger[] res = new BigInteger[ColsCount - 1];

            for (int j = 0; j < ColsCount - 1; j++)
            {
                for (int i = 0; i < RowsCount; i++)
                {
                    if (processed.Exists(x => x == i)) continue;
                    //Console.WriteLine("j = {0}:\n {1}", j, ToString());
                    BigInteger xReversed = 0;
                    Utils.ExtendedGCD(data[i, j], mod, out xReversed);
                    if (xReversed == 0) continue;
                    processed.Add(i);
                    for (int t = 0; t < RowsCount; t++)
                    {
                        if (t == i) continue;
                        CombineRowsWithMult(t, i, -xReversed * data[t, j]);
                    }
                }
            }

            // Console.WriteLine("Result: {0}", ToString());

            for (int j = 0; j < ColsCount - 1; j++)
            {
                for (int i = 0; i < RowsCount; i++)
                {
                    if (data[i, j] == 0) continue;

                    var gcd = Utils.ExtendedGCD(data[i, j], mod, out BigInteger inversed);
                    if (gcd != 1)
                    {
                        continue;
                    }
                    res[j] = (Utils.Mod(inversed * data[i, ColsCount - 1], mod));
                }
            }

            return res;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            for (int i = 0; i < RowsCount; i++)
            {
                sb.Append('|');
                for (int j = 0; j < ColsCount; j++)
                {
                    sb.Append(data[i, j]);
                    if (j != ColsCount - 1) sb.Append(' ');
                }
                sb.Append("|\n");
            }

            return sb.ToString();
        }

    }

}
