
using System.Numerics;


namespace lab1
{
    public struct DataItem
    {

        public Vector2 CoordPoint { get; set; }
        public Complex ElectFieldCompl { get; set; }
        public DataItem(Vector2 Cp, Complex Efc)
        {
            CoordPoint = Cp;
            ElectFieldCompl = Efc;
        }
        public override string ToString()
        {
            return "CoordPoint: " + CoordPoint.ToString() + " "
                + "ElectFieldCompl: " + ElectFieldCompl.ToString();
        }
        public string ToString(string format)
        {
            return "CoordPoint: " + CoordPoint.ToString() + " "
                 + "ElectFieldCompl: " + ElectFieldCompl.ToString(format) + " "
                 + "Magnitude: " + ElectFieldCompl.Magnitude.ToString(format);
        }
    }
}
