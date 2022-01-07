using System;
using System.Collections.Generic;
using System.Text;

namespace JsonSerializePerf
{
    public class SomeModel
    {
        public SomeModel(Guid id, string des, int count)
        {
            ID = id;
            Description = des;
            Count = count;
        }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
    }
}
