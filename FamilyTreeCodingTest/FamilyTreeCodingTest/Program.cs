using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeCodingTest
{
    class Program
    {
        //static readonly string textFile = @"D:\Code\Learning Test Project\Logical Improve\FamilyTreeCodingTest\FamilyTreeCodingTest\inputFile.txt";
        static void Main(string[] args)
        {
            List<Input> listOfInputs = new List<Input>();

            Family family = new Family(GetMembersV2());
            //  Console.WriteLine("Enter path of input text file :");
            //string textFile = Console.ReadLine();
            string textFile = args[0];
            Console.Write(textFile);
            if (File.Exists(textFile))
            {
                string text = File.ReadAllText(textFile);
                string[] lines = File.ReadAllLines(textFile);
                foreach (string line in lines)
                {
                    List<string> numbers = line.Split(' ').ToList<string>();
                    Input input = new Input();
                    input.action = numbers[0];
                    input.name = numbers[1];
                    input.relationship = numbers[2];
                    listOfInputs.Add(input);
                }
                HandleInputs(listOfInputs,family);
            }
            else
            {
                Console.WriteLine("File doesn't not exists");
                Console.ReadLine();
            }


        }

        public static void HandleInputs(List<Input> inputList,Family family)
        {
            if(inputList.Count>0)
            {
                foreach(var input in inputList)
                {
                    switch(input.action)
                    {
                        case "GET_RELATIONSHIP":
                          List<Member> members =  family.GetRelativeNameByRelationship(input.name, input.relationship);
                            family.PrintMembers(members);
                            break;
                          
                            
                    }
                }
            }
            Console.ReadLine();
        }

        public class Family
        {
            static List<Member> listOfMember = new List<Member>();

            public Family(List<Member> members)
            {
                listOfMember = members;
            }
            public void GetInput()
            {
                Console.WriteLine("Looking for relatives names in Family Tree?");
                Console.WriteLine("Enter member name :");
                string memberName = Console.ReadLine();
                Console.WriteLine("Enter RelationShip :");
                string relationship = Console.ReadLine();
                PrintMembers(GetRelativeNameByRelationship(memberName, relationship));
            }
            
            public void PrintMembers(List<Member> membersToPrint)
            {
                if (membersToPrint.Count > 0)
                {
                    Console.Write("\n");
                    foreach (Member member in membersToPrint)
                    {
                        Console.Write(member.name+" ");
                    }
                  
                }
                else
                {
                    Console.WriteLine("NONE");
                }
               
            }

            public List<Member> GetListOfSons(string parentName)
            {
                List<Member> listSons = new List<Member>();
                foreach (Member member in listOfMember)
                {
                    if (member.motherName.Equals(parentName) || member.fatherName.Equals(parentName))
                    {
                        if (member.gender == "male")
                        {
                            listSons.Add(member);
                        }
                    }
                }
                return listSons;
            }

            public List<Member> GetListOfDaughter(string parentName)
            {
                List<Member> filteredList = new List<Member>();
                foreach (Member member in listOfMember)
                {
                    if (member.motherName.Equals(parentName) || member.fatherName.Equals(parentName))
                    {
                        if (member.gender == "female")
                        {
                            filteredList.Add(member);
                        }
                    }
                }
                return filteredList;
            }

            public List<Member> GetListOfSiblings(string name)
            {
                List<Member> listSiblings = new List<Member>();

                //Get name of mother 
                var member = listOfMember.Where(obj => obj.name.Equals(name)).FirstOrDefault();
                if (member != null)
                {
                    listSiblings = listOfMember.Where(obj => obj.motherName.Equals(member.motherName)).ToList();
                    listSiblings.Remove(member);

                }
                return listSiblings;

            }

            public List<Member> GetListOfSisInLows(string name)
            {
                List<Member> listSisInLaws = new List<Member>();

                //Get name of mother 
                var member = listOfMember.Where(obj => obj.name.Equals(name)).FirstOrDefault();
                if (member != null)
                {
                    var listSiblings = listOfMember.Where(obj => obj.motherName.Equals(member.motherName) && obj.spouseName != "" && obj.gender.Equals("male")).ToList();
                    listSiblings.Remove(member);

                    foreach (Member _member in listSiblings)
                    {
                        var spouse = listOfMember.Where(obj => obj.name.Equals(_member.spouseName)).FirstOrDefault();
                        if (spouse != null)
                        {
                            listSisInLaws.Add(spouse);
                        }
                    }

                }
                return listSisInLaws;
            }

            public List<Member> GetListOfBroInLows(string name)
            {
                List<Member> listSisInLaws = new List<Member>();

                //Get name of mother 
                var member = listOfMember.Where(obj => obj.name.Equals(name)).FirstOrDefault();
                if (member != null)
                {
                    var listSiblings = listOfMember.Where(obj => obj.motherName.Equals(member.motherName) && obj.spouseName != "" && obj.gender.Equals("female")).ToList();
                    listSiblings.Remove(member);

                    foreach (Member _member in listSiblings)
                    {
                        var spouse = listOfMember.Where(obj => obj.name.Equals(_member.spouseName)).FirstOrDefault();
                        if (spouse != null)
                        {
                            listSisInLaws.Add(spouse);
                        }
                    }

                }
                return listSisInLaws;
            }

            private List<Member> GetListOfPaternalUncles(string name)
            {
                List<Member> listOfFathersSiblings = new List<Member>();

                //Get name of mother 
                var member = listOfMember.Where(obj => obj.name.Equals(name)).FirstOrDefault();
                if (member != null)
                {
                    //Getting his fathers entry 
                    var father = listOfMember.Where(obj => obj.name.Equals(member.fatherName)).FirstOrDefault();
                    if (father != null)
                    {
                        listOfFathersSiblings = GetListOfSiblings(father.name);
                        if (listOfFathersSiblings.Count > 0)
                        {
                            listOfFathersSiblings = listOfFathersSiblings.Where(obj => obj.gender != "female").ToList();
                            listOfFathersSiblings.Remove(father);
                        }
                    }

                }
                return listOfFathersSiblings;
            }

            private List<Member> GetListOfMaternalUncles(string name)
            {
                List<Member> listOfMothersSiblings = new List<Member>();

                //Get name of mother 
                var member = listOfMember.Where(obj => obj.name.Equals(name)).FirstOrDefault();
                if (member != null)
                {
                    //Getting his mother's entry 
                    var mother = listOfMember.Where(obj => obj.name.Equals(member.motherName)).FirstOrDefault();
                    if (mother != null)
                    {
                        listOfMothersSiblings = GetListOfSiblings(mother.name);
                        if (listOfMothersSiblings.Count > 0)
                        {
                            listOfMothersSiblings = listOfMothersSiblings.Where(obj => obj.gender != "female").ToList();
                            listOfMothersSiblings.Remove(mother);
                        }
                    }

                }
                return listOfMothersSiblings;
            }

            private List<Member> GetListOfPaternalAunts(string name)
            {
                List<Member> listOfPaternalAunts = new List<Member>();

                //Get name of mother 
                var member = listOfMember.Where(obj => obj.name.Equals(name)).FirstOrDefault();
                if (member != null)
                {
                    //Getting his mother's entry 
                    var father = listOfMember.Where(obj => obj.name.Equals(member.fatherName)).FirstOrDefault();
                    if (father != null)
                    {
                        listOfPaternalAunts = GetListOfSiblings(father.name);
                        if (listOfPaternalAunts.Count > 0)
                        {
                            listOfPaternalAunts = listOfPaternalAunts.Where(obj => obj.gender != "male").ToList();
                            //listOfPaternalAunts.Remove(mother);
                        }
                    }

                }
                return listOfPaternalAunts;
            }

            private List<Member> GetListOfMaternalAunts(string name)
            {
                List<Member> listOfMothersSiblings = new List<Member>();

                //Get name of mother 
                var member = listOfMember.Where(obj => obj.name.Equals(name)).FirstOrDefault();
                if (member != null)
                {
                    //Getting his mother's entry 
                    var mother = listOfMember.Where(obj => obj.name.Equals(member.motherName)).FirstOrDefault();
                    if (mother != null)
                    {
                        listOfMothersSiblings = GetListOfSiblings(mother.name);
                        if (listOfMothersSiblings.Count > 0)
                        {
                            listOfMothersSiblings = listOfMothersSiblings.Where(obj => obj.gender != "male").ToList();
                            listOfMothersSiblings.Remove(mother);
                        }
                    }

                }
                return listOfMothersSiblings;
            }

            public void AddMember(string parentName, Member member)
            {
                //Check parent is there?
                var parent = listOfMember.Where(obj => obj.fatherName.Equals(parentName) || obj.motherName.Equals(parentName)).FirstOrDefault();
                if (parent != null)
                {
                    listOfMember.Add(member);
                }
            }

            public List<Member> GetRelativeNameByRelationship(string member, string relationship)
            {
                List<Member> filteredList = new List<Member>();
                switch (relationship)
                {
                    case "Sons":
                        filteredList = GetListOfSons(member);
                        break;
                    case "Daughters":
                        filteredList = GetListOfDaughter(member);
                        break;
                    case "Siblings":
                        filteredList = GetListOfSiblings(member);
                        break;
                    case "Sister-In-Laws":
                        filteredList = GetListOfSisInLows(member);
                        break;
                    case "Brother-In-Laws":
                        filteredList = GetListOfBroInLows(member);
                        break;
                    case "Paternal-Uncles":
                        filteredList = GetListOfPaternalUncles(member);
                        break;
                    case "Maternal-Uncles":
                        filteredList = GetListOfMaternalUncles(member);
                        break;
                    case "Paternal-Aunts":
                        filteredList = GetListOfPaternalAunts(member);
                        break;
                    case "Maternal-Aunts":
                        filteredList = GetListOfMaternalAunts(member);
                        break;


                }
                return filteredList;
            }
            
        }


        public static List<Member> GetMembersV2()
        {
            List<Member> members = new List<Member>();

            members.Add(new Member("King Shan", "male", "", "", "Queen Anga"));
            members.Add(new Member("Queen Anga", "female", "", "", "King Shan"));
            members.Add(new Member("Chit", "male", "King Shan", "Queen Anga", "Amba"));
            members.Add(new Member("Ish", "male", "King Shan", "Queen Anga", ""));
            members.Add(new Member("Vich", "male", "King Shan", "Queen Anga", "Lika"));
            members.Add(new Member("Aras", "male", "King Shan", "Queen Anga", "Chitra"));
            members.Add(new Member("Satya", "female", "King Shan", "Queen Anga", "Vyan"));

            //Looks like spouses are also part of it
            //Isn't it, Lets add them
            members.Add(new Member("Amba", "female", "", "", "Chit"));
            members.Add(new Member("Lika", "female", "", "", "Vich"));
            members.Add(new Member("Chitra", "female", "", "", "Aras"));
            members.Add(new Member("Vyan", "male", "", "", "Satya"));

            members.Add(new Member("Dritha", "female", "Chit", "Amba", "Jaya"));
            members.Add(new Member("Tritha", "female", "Chit", "Amba", ""));
            members.Add(new Member("Vritha", "male", "Chit", "Amba", ""));
            members.Add(new Member("Jaya", "male", "", "", "Dritha"));

            members.Add(new Member("Vila", "female", "Vich", "Lika", ""));
            members.Add(new Member("Chika", "female", "Vich", "Lika", ""));

            members.Add(new Member("Jnki", "female", "Aras", "Chitra", "Arit"));
            members.Add(new Member("Ahit", "male", "Aras", "Chitra", ""));
            members.Add(new Member("Arit", "male", "", "", "Jnki"));

            members.Add(new Member("Asva", "male", "Vyan", "Satya", "Satvy"));
            members.Add(new Member("Vyas", "male", "Vyan", "Satya", "Krpi"));
            members.Add(new Member("Atya", "female", "Vyan", "Satya", ""));
            members.Add(new Member("Satvy", "female", "", "", "Asva"));
            members.Add(new Member("Krpi", "female", "", "", "Krpi"));

            members.Add(new Member("Yodhan", "male", "Jaya", "Dritha", ""));

            members.Add(new Member("Laki", "male", "Arit", "Jnki", ""));
            members.Add(new Member("Lavnya", "female", "Arit", "Jnki", ""));

            members.Add(new Member("Vasa", "male", "Asva", "Satvy", ""));

            members.Add(new Member("Kriya", "male", "Vyas", "Krpi", ""));
            members.Add(new Member("Krithi", "female", "Vyas", "Krpi", ""));

            return members;
        }
        public class Member
        {
            public string name { get; set; }
            public string gender { get; set; }
            public string level { get; set; }
            public string motherName { get; set; }
            public string fatherName { get; set; }
            public string spouseName { get; set; }

            public Member(string name, string gender, string level)
            {
                this.name = name;
                this.gender = gender;
                this.level = level;
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

        public class Input {
            public string action { get; set; }
            public string name { get; set; }
            public string relationship { get; set; }
            public string gender { get; set; }
        }
        
        //public static LinkedList<Member> GetMembers()
        //{
        //    LinkedList<Member> memberList = new LinkedList<Member>();
        //    //List<Member> memberList = new List<Member>();
        //    //Hardcoding existing members
        //    memberList.AddFirst(new Member("King Shan", "male", "0.0"));

        //    LinkedListNode<Member> listNode1;
        //    listNode1 = new LinkedListNode<Member>(new Member("Queen Anga", "female", "0.1"));
        //    memberList.AddAfter(memberList.First, listNode1);

        //    LinkedListNode<Member> listNode2;
        //    listNode2 = new LinkedListNode<Member>(new Member("Chit", "female", "1.1"));
        //    memberList.AddAfter(listNode1, listNode2);



        //    //  memberList.AddLast(new Member("Queen Anga", "female", "0.1"));

        //    // memberList.AddLast(new Member("Chit", "male", "1.0"));

        //    //memberList.AddLast(new Member("Amba", "female", "1.1"));
        //    //memberList.AddLast(new Member("Ish", "male", "1.2"));
        //    //memberList.AddLast(new Member("Vich", "male", "1.3"));
        //    //memberList.AddLast(new Member("Lika", "female", "1.4"));
        //    //memberList.AddLast(new Member("Aras", "male", "1.5"));
        //    //memberList.AddLast(new Member("Chitra", "female", "1.6"));
        //    //memberList.AddLast(new Member("Satya", "female", "1.7"));
        //    //memberList.AddLast(new Member("Vyan", "male", "1.8"));

        //    //memberList.Add(new Member("Dritha", "female", "2.0"));
        //    //memberList.Add(new Member("Jaya", "male", "2.1"));
        //    //memberList.Add(new Member("Tritha", "female", "2.2"));
        //    //memberList.Add(new Member("Vritha", "male", "2.3"));
        //    //memberList.Add(new Member("Vila", "female", "2.4"));
        //    //memberList.Add(new Member("Chika", "female", "2.5"));
        //    //memberList.Add(new Member("Arit", "male", "2.6"));
        //    //memberList.Add(new Member("Jnki", "female", "2.7"));
        //    //memberList.Add(new Member("Satvy", "female", "2.8"));
        //    //memberList.Add(new Member("Asva", "male", "2.9"));
        //    //memberList.Add(new Member("Krpi", "female", "2.10"));
        //    //memberList.Add(new Member("Vyas", "male", "2.11"));
        //    //memberList.Add(new Member("Atya", "female", "2.12"));

        //    //memberList.Add(new Member("Jnki", "female", "3.0"));
        //    //memberList.Add(new Member("Satvy", "female", "3.1"));
        //    //memberList.Add(new Member("Asva", "male", "3.2"));
        //    //memberList.Add(new Member("Krpi", "female", "3.3"));
        //    //memberList.Add(new Member("Vyas", "male", "3.4"));
        //    //memberList.Add(new Member("Atya", "female", "3.5"));



        //    return memberList;
        //}
    }
}
