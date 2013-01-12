using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Result
    {
        public string PageName { get; set; }

        public IEnumerable<string> Sections { get; set; }
    }
}
