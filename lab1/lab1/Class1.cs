using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace lab1
{
    public struct Grid2D
    {
        public float StepX { get; set; }
        public float StepY { get; set; }
        public int NumberX { get; set; }
        public int NumberY { get; set; }
        public Grid2D(float sx, float sy, int nx, int ny)
        {
            this.StepX = sx;
            this.StepY = sy;
            this.NumberX = nx;
            this.NumberY = ny;
        }
        public override string ToString()
        {
            return "size: " + NumberX.ToString() + " " + NumberY.ToString() +
                "step x,y: " + StepX.ToString() + "," + StepY.ToString();
        }

    }

    public abstract class V4Data
    {
        public string Info { get; set; }
        public double Frequency { get; set; }
        public V4Data(string inf, double freq)
        {
            this.Info = inf;
            this.Frequency = freq;
        }
        public abstract Complex[] NearMax(float eps);
        public abstract string ToLongString();
        public abstract override string ToString();
    }

    public class V4DataOnGrid : V4Data
    {
        public Grid2D Grid { get; set; }
        public Complex[,] Massiv { get; set; }
        public V4DataOnGrid(string inf, double freq, Grid2D grd) : base(inf, freq)
        {
            this.Grid = grd;
            this.Massiv = new Complex[Grid.NumberX, Grid.NumberY];
        }
        public void InitRandom(double minValue, double maxValue, Random rand)
        {
            //Random rand = new Random();
            int row = Massiv.GetLength(0);
            int coll = Massiv.GetLength(1);
            double real;
            double comp;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < coll; j++)
                {
                    real = rand.NextDouble() * (maxValue - minValue) + minValue;
                    comp = rand.NextDouble() * (maxValue - minValue) + minValue;
                    Massiv[i, j] = new Complex(real, comp);
                }
            }
        }
        public override string ToString()
        {
            return "V4DataOnGrid:\n Info) " + Info.ToString() + "\n Frequency) " + Frequency.ToString() +
                "\n Grid: size) " + Grid.NumberX.ToString() + " " + Grid.NumberY.ToString() +
                "\n step x,y) " + Grid.StepX.ToString() + "," + Grid.StepY.ToString();
        }
        public override string ToLongString()
        {
            string line = this.ToString();
            int row = Massiv.GetLength(0);
            int coll = Massiv.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < coll; j++)
                {
                    line += "\n(" + i.ToString() + "," + j.ToString() + ") - " + Massiv[i, j].ToString();
                }
                line += "\n";
            }
            return line;
        }
        public override Complex[] NearMax(float eps)
        {
            double Max = Massiv[0, 0].Magnitude;
            int row = Massiv.GetLength(0);
            int coll = Massiv.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < coll; j++)
                {
                    if (Max < Massiv[i, j].Magnitude)
                        Max = Massiv[i, j].Magnitude;
                }
            }
            Complex[] newmass = new Complex[5];
            int massln = 5;
            int count = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < coll; j++)
                {
                    if (Max - Massiv[i, j].Magnitude <= eps)
                    {
                        if (count >= massln)
                        {
                            massln += 5;
                            Array.Resize(ref newmass, massln);
                        }
                        newmass[count++] = Massiv[i, j];
                    }

                }
            }
            if (count != massln)
                Array.Resize(ref newmass, count);
            return newmass;
        }
        public static explicit operator V4DataCollection(V4DataOnGrid var)
        {
            V4DataCollection temp = new V4DataCollection(var.Info, var.Frequency);
            temp.Dict = new Dictionary<System.Numerics.Vector2, System.Numerics.Complex>();

            int row = var.Massiv.GetLength(0);
            int coll = var.Massiv.GetLength(1);
            Vector2 Coordinate;
            Complex Value;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < coll; j++)
                {
                    Coordinate = new Vector2(i * var.Grid.StepX, j * var.Grid.StepY);
                    Value = var.Massiv[i, j];
                    temp.Dict.Add(Coordinate, Value);
                }
            }
            return temp;
        }
    }
    public class V4DataCollection : V4Data
    {
        public Dictionary<System.Numerics.Vector2, System.Numerics.Complex> Dict { set; get; }
        public V4DataCollection(string inf, double freq) : base(inf, freq)
        {
            this.Dict = new Dictionary<System.Numerics.Vector2, System.Numerics.Complex>();
        }
        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue, Random rand)
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
                    + count.Value.ToString();
            }
            return line;
        }
    }
    public class V4MainCollection
    {
        private List<V4Data> list;
        public V4MainCollection()
        {
            this.list = new List<V4Data>();
        }
        public int count
        {
            set { }
            get { return list.Count(); }

        }
        public interface IEnumerable
        {
            IEnumerator GetEnumerator();
        }
        public IEnumerator<V4Data> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public void Add(V4Data item)
        {
            list.Add(item);
        }
        public bool Remove(string id, double w)
        {
            int count = list.RemoveAll(item => (item.Frequency == w) && (item.Info == id));
            return count > 0;
        }
        public void AddDefaults()
        {
            Random rand = new Random();
            int nItems = 10;
            double minValue = 0.0;
            double maxValue = 100.0;
            double fr;
            Grid2D def0;
            V4DataOnGrid def1;
            V4DataCollection def2;
            for (int i = 0; i < 2; i++)
            {
                //rand = new Random();
                fr = (float)rand.NextDouble() * 7;
                //Console.Write(fr);
                def0 = new Grid2D((float)rand.NextDouble() * 5, (float)rand.NextDouble() * 5, 10, 10);
                def1 = new V4DataOnGrid((i * 2).ToString(), fr, def0);
                def2 = new V4DataCollection((i * 2 + 1).ToString(), fr);

                def1.InitRandom(minValue, maxValue, rand);
                def2.InitRandom(nItems, (float)rand.NextDouble(), (float)rand.NextDouble(), minValue, maxValue, rand);
                list.Add(def1);
                list.Add(def2);

            }
        }
        public override string ToString()
        {
            string line = "line: ";
            foreach (V4Data item in list)
            {
                line += item.ToString();
            }
            return line;
        }
    }
}
