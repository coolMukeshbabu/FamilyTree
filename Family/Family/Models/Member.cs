using System;
using System.Collections.Generic;
using System.Text;

namespace Family.Models
{
    public class Member
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string motherName { get; set; }
        public string fatherName { get; set; }
        public string spouseName { get; set; }

        public Member(string name, string gender)
        {
            this.name = name;
            this.gender = gender;
        }

        public Member(string name, string gender, string fatherName, string motherName, string spouseName)
        {
            this.name = name;
            this.gender = gender;
            this.motherName = motherName;
            this.fatherName = fatherName;
            this.spouseName = spouseName;
        }


    }
}
