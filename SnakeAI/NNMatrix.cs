using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAI
{
    public class NNMatrix
    {
        private double[,] m;

        public NNMatrix() : this(0, 0)
        {
        }

        public NNMatrix(NNMatrix other)
        {
            Func<double, double, double> op = (x, y) => x;
            m = applyOp(other, other, op).m;
        }

        public NNMatrix(int cols, int rows) {
            m = new double[cols, rows];
        }

        public NNMatrix(double[][] vecOfVecs)
        {
            if (vecOfVecs.Length == 0)
            {
                m = new double[0, 0];
            }
            else
            {
                int rows = vecOfVecs[0].Length;
                for (int v = 1; v < vecOfVecs.Length; v++)
                {
                    if (vecOfVecs[v].Length != rows) throw new Exception("Vectors must all be of the same length");
                }
                m = new double[vecOfVecs.Length, rows];
            }
        }

        public NNMatrix(double[] singleVec) : this(new double[1][] { singleVec })
        {
        }

        public double[] toVector()
        {
            if (colCount() != 1) throw new Exception("Conversion to a vector is only possible for matrices with exactly one column");
            return getCol(0);
        }

        public double this[int col, int row]
        {
            get => m[col, row];
            set => m[col, row] = value;
        }

        public static implicit operator NNMatrix (double[] singleVec)
        {
            return new NNMatrix(singleVec);
        }

        public static implicit operator double[] (NNMatrix m) {
            return m.toVector();
        }

        public int colCount()
        {
            return m.GetLength(0);
        }

        public int rowCount()
        {
            return m.GetLength(1);
        }

        public double[] getCol(int col)
        {
            double[] ret = new double[rowCount()];
            for (int r = 0; r < ret.Length; r++) ret[r] = this[col, r];
            return ret;
        }

        public double[] getRow(int row)
        {
            double[] ret = new double[colCount()];
            for (int c = 0; c < ret.Length; c++) ret[c] = this[c, row];
            return ret;
        }

        public static NNMatrix operator +(NNMatrix a, NNMatrix b)
        {
            Func<double, double, double> op = (x, y) => x + y;
            return applyOp(a, b, op);
        }

        public static NNMatrix operator -(NNMatrix a, NNMatrix b)
        {
            Func<double, double, double> op = (x, y) => x - y;
            return applyOp(a, b, op);
        }

        public static NNMatrix operator *(NNMatrix a, NNMatrix b)
        {
            Func<double, double, double> op = (x, y) => x * y;
            return applyOp(a, b, op);
        }

        public static NNMatrix outerProduct(double[] vec1, double[] vec2)
        {
            NNMatrix ret = new NNMatrix(vec2.Length, vec1.Length);
            for (int c = 0; c < vec2.Length; c++)
            {
                for (int r = 0; r < vec1.Length; r++)
                {
                    ret[c, r] = vec1[r] * vec2[c];
                }
            }
            return ret;
        }

        public static NNMatrix operator *(NNMatrix a, double f)
        {
            NNMatrix ret = new NNMatrix(a.rowCount(), a.colCount());
            for (int c = 0; c < a.colCount(); c++)
                for (int r = 0; r < a.rowCount(); r++)
                    ret[c, r] = a[c, r] * f;
            return ret;
        }

        public static NNMatrix operator -(NNMatrix a)
        {
            return a * (-1);
        }

        private static NNMatrix applyOp(NNMatrix a, NNMatrix b, Func<double, double, double> op)
        {
            if (a.rowCount() != b.rowCount() || a.colCount() != b.colCount())
            {
                throw new Exception("Matrices must have the same dimensions");
            }
            NNMatrix ret = new NNMatrix(a.rowCount(), a.colCount());
            for (int c = 0; c < a.colCount(); c++)
                for (int r = 0; r < a.rowCount(); r++)
                    ret[c, r] = op(a[c, r], b[c, r]);
            return ret;
        }
    }
}
