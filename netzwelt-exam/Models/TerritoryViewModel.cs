﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netzwelt_exam.Models
{
    public class TerritoryViewModel
    {
        public string Parent { get; set; }

        public IEnumerable<string> Children { get; set; }
    }
}
