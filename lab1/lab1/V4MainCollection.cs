﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Numerics;

namespace lab1
{   
    
    public class V4MainCollection/**/ : IEnumerable<V4Data>//
    {
        public event DataChangedEventHandler DataChanged;

        public void OnDataChanged(object source, DataChangedEventArgs args)
        {
            DataChanged?.Invoke(source, args);
        }
        public void PropertyC(object sender, PropertyChangedEventArgs args)
        {
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, num));
        }
        public double AverAll
        {
            get
            {
                return list.Select(x => Math.Abs(x.Frequency)).Average();
            }
        }
        public IEnumerable<DataItem> NearZero(float R)
        {
            
            
            IEnumerable<V4DataCollection> fs1 = from V4Data x in list where x is V4DataCollection select (V4DataCollection)x;  
            IEnumerable<V4DataOnGrid> fs2 = from V4Data x in list where x is V4DataOnGrid select (V4DataOnGrid)x;
            
            var f1 = fs1.SelectMany(x => x).Where(y => y.CoordPoint.Length() < R);
            var f2 = fs2.SelectMany(x => x).Where(y => y.CoordPoint.Length() < R);

            var res = f1.Concat(f2);
            return res;

        }
        public V4Data this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                value.PropertyChanged += PropertyC;
                list[index] = value;
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace, num));
            }
        }
        
        public IEnumerable<Vector2> Ones
        {
            get
            {
                IEnumerable<V4DataCollection> first = from V4Data x in list where x is V4DataCollection select (V4DataCollection)x;
                var second = first.SelectMany(x => x.Dict);
                var third = from x in second group x by x.Key;
                //var temp = from x in third select x.Count();
                var fourth = from x in third where (x.Count()==1) select x.Key;
                
                return fourth;
                
            }
        }
        private List<V4Data> list;
        public V4MainCollection()
        {
            list = new List<V4Data>();
            num = 0;
        }
        public int num;
        public int count
        {
            set { }
            get { return list.Count(); }

        }/*
        public interface IEnumerable<V4Data>
        {
            IEnumerator GetEnumerator();
        }*/
        public IEnumerator<V4Data> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        /**/
        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
        
        public void Add(V4Data item)
        {
            item.PropertyChanged += PropertyC;
            list.Add(item);
            num++;

            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, num));
        }
        public bool Remove(string id, double w)
        {
            
            //V4Data tmp = list[ list.FindIndex(item => (item.Frequency == w) && (item.Info == id))];
            //tmp.PropertyChanged -= PropertyC;
            List<V4Data> tmp = list.FindAll(item => (item.Frequency == w) && (item.Info == id));
            int count = tmp.Count;
            if (count != 0)
            {
                tmp[0].PropertyChanged -= PropertyC;
                num -= count;
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove,num ));
            }
                
            return count > 0;
        }
        public void AddDefaults()
        {
            Random rand = new Random();
            int nItems = 4;
            double minValue = 0.0;
            double maxValue = 100.0;
            double fr;
            Grid2D def0;
            V4DataOnGrid def1;
            V4DataCollection def2;
            int a = -1;
            for (int i = 0; i < 2; i++)
            {
                //rand = new Random();
                
                def0 = new Grid2D((float)rand.NextDouble() * 10, (float)rand.NextDouble() * 10, 4, 4);
                fr = (float)rand.NextDouble() * 10 * a; a *= -1;
                def1 = new V4DataOnGrid("Grid", fr, def0);
                fr = (float)rand.NextDouble() * 10 * a; a *= -1;
                def2 = new V4DataCollection("Coll", fr);

                def1.InitRandom(minValue, maxValue, /*добавил сюда*/ rand);
                def2.InitRandom(nItems, (float)rand.NextDouble() * 10, (float)rand.NextDouble() * 10, minValue, maxValue, rand);
                list.Add(def1);
                list.Add(def2);

            }
        }
        public override string ToString()
        {
            string line = "V4MainCollection: " + "\n";
            foreach (V4Data item in list)
            {
                line += item.ToString();
            }
            return line;
        }
        public string ToLongString(string format)
        {
            string line = "V4MainCollection: "+"\n";
            foreach (V4Data item in list)
            {
                line += item.ToLongString(format);
            }
            return line;
        }


    }
}
