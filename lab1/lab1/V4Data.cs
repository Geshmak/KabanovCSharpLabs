using System.Numerics;


namespace lab1
{
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
        public abstract string ToLongString(string format);
    }
}
