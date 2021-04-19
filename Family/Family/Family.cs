using Family.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Family
{
    public class Family
    {
        private List<Member> members = new List<Member>();

        public Family(List<Member> members)
        {
            this.members = members;
        }

        public void PrintMembers(List<Member> membersToPrint)
        {
            if (membersToPrint.Count > 0)
            {
                foreach (Member member in membersToPrint)
                {
                    Console.Write(member.name + " ");
                }
                Console.Write("\n");

            }
            else
            {
                Console.Write("\n");
                Console.Write("NONE");
            }

        }

        public List<Member> GetSons(string parentName)
        {
            List<Member> listSons = new List<Member>();
            foreach (Member member in members)
            {
                if (member.motherName.Equals(parentName) || member.fatherName.Equals(parentName))
                {
                    if (member.gender == "Male")
                    {
                        listSons.Add(member);
                    }
                }
            }
            return listSons;
        }

        public List<Member> GetDaughters(string parentName)
        {
            List<Member> filteredList = new List<Member>();
            foreach (Member member in members)
            {
                if (member.motherName.Equals(parentName) || member.fatherName.Equals(parentName))
                {
                    if (member.gender == "Female")
                    {
                        filteredList.Add(member);
                    }
                }
            }
            return filteredList;
        }

        public List<Member> GetSiblings(string name)
        {
            List<Member> listSiblings = new List<Member>();

            //Get name of mother 
            var member = members.Where(obj => obj.name.Equals(name)).FirstOrDefault();
            if (member != null)
            {
                listSiblings = members.Where(obj => obj.motherName.Equals(member.motherName) && member.motherName!="").ToList();
                listSiblings.Remove(member);

            }
            return listSiblings;

        }

        public List<Member> GetSisterInLaws(string name)
        {
            List<Member> listSisInLaws = new List<Member>();

            //Get name of mother 
            var member = members.Where(obj => obj.name.Equals(name)).FirstOrDefault();
            if (member != null)
            {
                //Spounse's siblings
                var spouse = members.Where(obj => obj.name.Equals(member.spouseName)).FirstOrDefault();
                if(spouse!=null)
                {
                    var listSiblings = GetSiblings(spouse.name);
                    if(listSiblings.Count>0)
                    {
                        listSisInLaws = listSiblings.Where(obj => obj.gender == "Female").ToList();
                       
                    }
                }

                //Get list of siblings's wives
                var listSibling = GetSiblings(member.name);
                if(listSibling.Count>0)
                {
                    foreach(var _member in listSibling )
                    {
                        var _spouse = members.Where(obj => obj.name.Equals(_member.spouseName)).FirstOrDefault();
                        if (_spouse != null)
                        {
                            listSisInLaws.Add(_spouse);
                        }
                    }
                }
                
            }


            return listSisInLaws;
        }

        public List<Member> GetBrotherInLows(string name)
        {
            List<Member> listSisInLaws = new List<Member>();

            //Get name of mother 
            var member = members.Where(obj => obj.name.Equals(name)).FirstOrDefault();
            if (member != null)
            {
                var listSiblings = members.Where(obj => obj.motherName.Equals(member.motherName) && obj.spouseName != "" && obj.gender.Equals("Female")).ToList();
                listSiblings.Remove(member);

                foreach (Member _member in listSiblings)
                {
                    var spouse = members.Where(obj => obj.name.Equals(_member.spouseName)).FirstOrDefault();
                    if (spouse != null)
                    {
                        listSisInLaws.Add(spouse);
                    }
                }

            }
            return listSisInLaws;
        }

        private List<Member> GetPaternalUncles(string name)
        {
            List<Member> listOfFathersSiblings = new List<Member>();

            //Get name of mother 
            var member = members.Where(obj => obj.name.Equals(name)).FirstOrDefault();
            if (member != null)
            {
                //Getting his fathers entry 
                var father = members.Where(obj => obj.name.Equals(member.fatherName)).FirstOrDefault();
                if (father != null)
                {
                    listOfFathersSiblings = GetSiblings(father.name);
                    if (listOfFathersSiblings.Count > 0)
                    {
                        listOfFathersSiblings = listOfFathersSiblings.Where(obj => obj.gender != "Female").ToList();
                        listOfFathersSiblings.Remove(father);
                    }
                }

            }
            return listOfFathersSiblings;
        }

        private List<Member> GetMaternalUncles(string name)
        {
            List<Member> listOfMothersSiblings = new List<Member>();

            //Get name of mother 
            var member = members.Where(obj => obj.name.Equals(name)).FirstOrDefault();
            if (member != null)
            {
                //Getting his mother's entry 
                var mother = members.Where(obj => obj.name.Equals(member.motherName)).FirstOrDefault();
                if (mother != null)
                {
                    listOfMothersSiblings = GetSiblings(mother.name);
                    if (listOfMothersSiblings.Count > 0)
                    {
                        listOfMothersSiblings = listOfMothersSiblings.Where(obj => obj.gender != "Female").ToList();
                        listOfMothersSiblings.Remove(mother);
                    }
                }

            }
            return listOfMothersSiblings;
        }

        private List<Member> GetPaternalAunts(string name)
        {
            List<Member> listPaternalAunts = new List<Member>();

            var member = members.Where(obj => obj.name.Equals(name)).FirstOrDefault();
            if (member != null)
            {
                var father = members.Where(obj => obj.name.Equals(member.fatherName)).FirstOrDefault();
                if (father != null)
                {
                    var fathersSiblings = GetSiblings(father.name);
                    if (fathersSiblings.Count > 0)
                    {
                        listPaternalAunts = fathersSiblings.Where(obj => obj.gender == "Female").ToList();
                    }
                }

            }
            return listPaternalAunts;
        }

        private List<Member> GetMaternalAunts(string name)
        {
            List<Member> listOfMothersSiblings = new List<Member>();

            //Get name of mother 
            var member = members.Where(obj => obj.name.Equals(name)).FirstOrDefault();
            if (member != null)
            {
                //Getting his mother's entry 
                var mother = members.Where(obj => obj.name.Equals(member.motherName)).FirstOrDefault();
                if (mother != null)
                {
                    listOfMothersSiblings = GetSiblings(mother.name);
                    if (listOfMothersSiblings.Count > 0)
                    {
                        listOfMothersSiblings = listOfMothersSiblings.Where(obj => obj.gender != "Male").ToList();
                        listOfMothersSiblings.Remove(mother);
                    }
                }

            }
            return listOfMothersSiblings;
        }

        public string AddMember(string motherName, Member member)
        {
            string returnMessage = "";
            var mother = members.Where(obj => obj.name.Equals(motherName) && obj.gender.Equals("Female")).FirstOrDefault();
            if (mother != null)
            {
                member.fatherName = mother.spouseName;
                members.Add(member);
                returnMessage = "CHILD_ADDITION_SUCCEEDED";
            }
            else
            {
                returnMessage = "CHILD_ADDITION_FAILED";
            }
            return returnMessage;
        }

        public List<Member> GetRelativeNameByRelationship(string member, string relationship)
        {
            List<Member> filteredList = new List<Member>();
            switch (relationship)
            {
                case "Son":
                    filteredList = GetSons(member);
                    break;
                case "Daughter":
                    filteredList = GetDaughters(member);
                    break;
                case "Siblings":
                    filteredList = GetSiblings(member);
                    break;
                case "Sister-In-Law":
                    filteredList = GetSisterInLaws(member);
                    break;
                case "Brother-In-Law":
                    filteredList = GetBrotherInLows(member);
                    break;
                case "Paternal-Uncle":
                    filteredList = GetPaternalUncles(member);
                    break;
                case "Maternal-Uncle":
                    filteredList = GetMaternalUncles(member);
                    break;
                case "Paternal-Aunt":
                    filteredList = GetPaternalAunts(member);
                    break;
                case "Maternal-Aunt":
                    filteredList = GetMaternalAunts(member);
                    break;

            }
            return filteredList;
        }

        public bool IsMemberExists(string memberName)
        {
            bool isExist = false;
            var _member = members.Where(obj => obj.name.Equals(memberName)).FirstOrDefault();
            if(_member!=null)
            {
                isExist = true;
            }
            return isExist;
        }
       
    }
}
