﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttc_wtc
{
    class Item
    {
        public string Name { get; set; }

        public Item (string name)
        {
            Name = name;
        }
        public static Item[] Items = new Item[] { };
    }
}
