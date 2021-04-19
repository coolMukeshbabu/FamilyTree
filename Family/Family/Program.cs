using Family.Models;
using Family.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Family
{
    class Program
    {
        static void Main(string[] args)
        {
            FamilyHelper familyHelper = new FamilyHelper();

            Family family = new Family(familyHelper.GetMembers());

            //Reading command line params to read path of file
            string filePath = args[0];

            //Getting list of intruction from the file
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("FilePath", filePath);
            List<string> inputInstructions = GetInputInstrutions("File", data);

            List<InputIntruction> parsedInputIntructions = ParseInputIntructions(inputInstructions);

            ExecuteInputInstructions(parsedInputIntructions, family);

           // ParseInputInstructionAndExecute(inputInstructions, family);
        }

        private static List<string> GetInputInstrutions(string source, Dictionary<string,string> data)
        {
            List<string> inputInstrutions = new List<string>();

            switch (source)
            {
                case "File":
                    string filePath = data["FilePath"];
                    if (File.Exists(filePath))
                    {
                        string text = File.ReadAllText(filePath);
                        string[] lines = File.ReadAllLines(filePath);
                        foreach (string line in lines)
                        {
                            inputInstrutions.Add(line);
                        }
                    }
                    break;
            }

            return inputInstrutions;
        }
        
        public static void ExecuteInputInstructions(List<InputIntruction> inputIntrucations, Family family)
        {
            if (inputIntrucations.Count > 0)
            {
                foreach (var input in inputIntrucations)
                {
                    switch (input.action)
                    {
                        case "GET_RELATIONSHIP":
                            string memberName = input.data["name"];
                            string relationship = input.data["relationship"];
                            List<Member> members = family.GetRelativeNameByRelationship(memberName, relationship);
                            family.PrintMembers(members);
                            break;


                        case "ADD_CHILD":
                            string motherName = input.data["motherName"];
                            string childName = input.data["childName"];
                            string gender = input.data["gender"];
                            Member member = new Member(childName, gender, "", motherName, "");
                            string status = family.AddMember(motherName, member);
                            Console.Write(status);
                            break;
                    }
                }
            }
            Console.ReadLine();
        }
        
        private static List<InputIntruction> ParseInputIntructions(List<string> inputIntrucations)
        {
            List<InputIntruction> parsedInputInstructions = new List<InputIntruction>();
            if(inputIntrucations.Count>0)
            {
                foreach(var input in inputIntrucations)
                {
                    InputIntruction inputIntruction = new InputIntruction();

                    List<string> inputLine = input.Split(' ').ToList<string>();
                    string action = inputLine[0];
                    inputIntruction.action = "GET_RELATIONSHIP";
                    
                    switch (action)
                    {
                        case "GET_RELATIONSHIP":
                            Dictionary<string, string> data = new Dictionary<string, string>();
                            data.Add("name", inputLine[1]);
                            data.Add("relationship", inputLine[2]);
                            inputIntruction.data = data;
                            parsedInputInstructions.Add(inputIntruction);
                            break;

                        case "ADD_CHILD":
                            inputIntruction.action = "ADD_CHILD";
                            Dictionary<string, string> _data = new Dictionary<string, string>();
                            _data.Add("motherName", inputLine[1]);
                            _data.Add("childName", inputLine[2]);
                            _data.Add("gender", inputLine[3]);
                            inputIntruction.data = _data;
                            parsedInputInstructions.Add(inputIntruction);
                            break;
                    }
                }
            }
            return parsedInputInstructions;
        }
    }
}
