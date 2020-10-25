using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace lab1
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Grid2D def0 = new Grid2D((float)1.0, (float)1.0, 5, 5);
            V4DataOnGrid def1 = new V4DataOnGrid("first", 1.0, def0);
            Random rand = new Random();
            def1.InitRandom(0, 100, rand);
            Console.WriteLine(def1.ToLongString());

            V4DataCollection param =(V4DataCollection) def1; //преобразование типа 
            Console.WriteLine(param.ToLongString());
            
            V4MainCollection def3 = new V4MainCollection();
            def3.AddDefaults();
            Console.WriteLine(def3.ToString());
            Complex[] obj;
            foreach (var item in def3)
                {
                    Console.WriteLine("\n");
                    obj = item.NearMax((float)10);
                    for (int i = 0; i < obj.Length; i++)
                    {
                        Console.WriteLine(obj[i].ToString());
                    }
                }

            
        }
        

    }
}
