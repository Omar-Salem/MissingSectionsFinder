using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Area
    {
        public string Name { get; set; }
        public IEnumerable<Page> Pages { get; set; }
    }
}