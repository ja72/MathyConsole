using System;
using System.Collections.Generic;
using System.Linq;

namespace MathyConsole
{
    public class Vec : 
     ICollection<double>, 
     System.Collections.ICollection
    {
        readonly double[] data;

        public Vec(int size)
        {
            data = new double[size];
        }
        public Vec(params double[] values)
        {
            data = values;
        }
        public Vec(ReadOnlySpan<double> span)
        {
            data = span.ToArray();
        }
        public Vec(IEnumerable<double> values)
        {
            this.data = values.ToArray();
        }
        public static Vec Element(int size, int index, double value = 1)
        {
            var vec = new Vec(size);
            vec.data[index] = value;
            return vec;
        }
        public int Size => data.Length;
        public int Count { get => data.Length; }
        public ref double this[int index] => ref data[index];
        public Span<double> AsSpan() => new Span<double>(data);
        public Span<double> AsSpan(int start, int length) => new Span<double>(data, start, length);
        public Vec Copy() => new Vec(data.Clone() as double[]);

        #region Collections
        public void CopyTo(Array array, int index) => data.CopyTo(array, index);
        public void CopyTo(double[] array, int arrayIndex) => data.CopyTo(array, arrayIndex);
        object System.Collections.ICollection.SyncRoot { get => null; }
        bool System.Collections.ICollection.IsSynchronized { get => false; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
        public bool IsReadOnly { get => true; }
        public IEnumerator<double> GetEnumerator()
        {
            foreach (var item in data)
            {
                yield return item;
            }
        }
        public void Clear() => data.Initialize();

        void ICollection<double>.Add(double item)
            => throw new NotSupportedException();


        bool ICollection<double>.Contains(double item)
            => throw new NotSupportedException();

        void ICollection<double>.CopyTo(double[] array, int arrayIndex)
            => throw new NotSupportedException();

        bool ICollection<double>.Remove(double item)
            => throw new NotSupportedException();

        #endregion

        #region Linear Algebra
        public static Vec Negate(Vec a)
        {
            var data = new double[a.Size];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = -a.data[i];
            }
            return new Vec(data);
        }

        public static Vec Scale(double factor, Vec a)
        {
            var data = new double[a.Size];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = factor * a.data[i];
            }
            return new Vec(data);
        }

        public static Vec Add(Vec a, Vec b)
        {
            if (a.Size != b.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }

            var data = new double[a.Size];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = a.data[i] + b.data[i];
            }
            return new Vec(data);
        }

        public static Vec Subtract(Vec a, Vec b)
        {
            if (a.Size != b.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }

            var data = new double[a.Size];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = a.data[i] - b.data[i];
            }
            return new Vec(data);
        }
        public static double Dot(Vec a, Vec b)
        {
            if (a.Size != b.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }
            double sum = 0;
            for (int i = 0; i < a.data.Length; i++)
            {
                sum += a.data[i] * b.data[i];
            }
            return sum;
        }
        public static Vec Stack(Vec a, Vec b)
        {
            var data = new double[a.Size + b.Size];
            a.CopyTo(data, 0);
            b.CopyTo(data, a.Size);
            return new Vec(data);
        }
        public Vec Slice(int index, int length)
        {
            var result = new double[length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = data[index + i];
            }
            return new Vec(result);
        }

        public static Vec operator +(Vec a, Vec b) => Add(a, b);
        public static Vec operator -(Vec a) => Negate(a);
        public static Vec operator -(Vec a, Vec b) => Subtract(a, b);
        public static Vec operator *(double f, Vec a) => Scale(f, a);
        public static Vec operator *(Vec a, double f) => Scale(f, a);
        public static Vec operator /(Vec a, double d) => Scale(1 / d, a);
        #endregion

        public IEnumerable<string> ToStringArray(string formatting, IFormatProvider provider)
            => data.Select((x) => x.ToString(formatting, provider));

        public IEnumerable<T> Map<T>(Func<double, T> f) => data.Select((x) => f(x));
        public IEnumerable<T> Map<T>(Func<double, int, T> f) => data.Select((x, index) => f(x, index));
    }
 }
