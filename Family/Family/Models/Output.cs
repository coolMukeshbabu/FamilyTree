using System;
using System.Collections.Generic;
using System.Text;

namespace Family.Models
{
    class Output
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Member> data { get; set; }
    }
}
