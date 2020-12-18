using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Collections;

namespace lab1
{
    public class V4DataCollection : V4Data, IEnumerable<DataItem>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enum(Dict);
        }
        public IEnumerator<DataItem> GetEnumerator()
        {
            return new Enum(Dict);
        }
        public class Enum : IEnumerator<DataItem>
        {
            private int x = 0;
            private Dictionary<Vector2, Complex> dict;
            public Enum(Dictionary<Vector2, Complex> d)
            {
                dict = d;
            }
            object IEnumerator.Current => Current;
            public DataItem Current
            {
                get
                {
                    return new DataItem(dict.Keys.ElementAt(x), dict.Values.ElementAt(x));
                }
            }
            public void Dispose()
            {
                dict = null;
            }
            public void Reset()
            {
                x = 0;
            }
            public bool MoveNext()
            {
                x += 1;
                return x < dict.Count;
            }
        }
        public Dictionary<System.Numerics.Vector2, System.Numerics.Complex> Dict { set; get; }
        public V4DataCollection(string inf, double freq) : base(inf, freq)
        {
            this.Dict = new Dictionary<System.Numerics.Vector2, System.Numerics.Complex>();
        }
        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue,/*добавил сюда*/ Random rand)
        {
            //Random rand = new Random();
            double x;
            double y;
            double real;
            double comp;
            Vector2 Coordinate;
            Complex Value;
            for (int count = 0; count < nItems; count++)
            {
                real = rand.NextDouble() * (maxValue - minValue) + minValue;
                comp = rand.NextDouble() * (maxValue - minValue) + minValue;
                x = rand.NextDouble() * xmax;
                y = rand.NextDouble() * ymax;
                Coordinate = new Vector2((float)x, (float)y);
                Value = new Complex(real, comp);
                Dict.Add(Coordinate, Value);
            }
        }
        public override Complex[] NearMax(float eps)
        {

            double Max = 0;
            foreach (Complex comp in Dict.Values)
            {
                if (comp.Magnitude > Max)
                    Max = comp.Magnitude;
            }

            Complex[] newmass = new Complex[5];
            int massln = 5;
            int count = 0;
            foreach (Complex comp in Dict.Values)
            {
                if (Max - comp.Magnitude <= eps)
                {
                    if (count >= massln)
                    {
                        massln += 5;
                        Array.Resize(ref newmass, massln);
                    }
                    newmass[count++] = comp;
                }
            }
            if (count != massln)
                Array.Resize(ref newmass, count);
            return newmass;
        }
        public override string ToString()
        {
            return "\n\nV4DataCollection: \nInfo) " + Info.ToString() + "\nFrequency) " + Frequency.ToString() +
                "\nDictsize: " + Dict.Count.ToString() + "\n\n";
        }
        public override string ToLongString()
        {
            string line = this.ToString();
            foreach (KeyValuePair<Vector2, Complex> count in Dict)
            {
                line += " (" + count.Key.X.ToString() + "," + count.Key.Y.ToString() + ") - "
                    + count.Value.ToString() + " mag:   " + count.Value.Magnitude + "\n";
            }
            return line;
        }
        public override string ToLongString(string format)
        {
            string line = "\n\nV4DataCollection: \nInfo) " + Info.ToString() + "\nFrequency) " + Frequency.ToString(format) +
                "\nDictsize: " + Dict.Count.ToString() + "\n\n";
            foreach (KeyValuePair<Vector2, Complex> count in Dict)
            {
                line += " (" + count.Key.X.ToString(format) + "," + count.Key.Y.ToString(format) + ") - "
                    + count.Value.ToString()+" mag:   "+count.Value.Magnitude+ "\n";
            }
            return line;
        }
    }
}
