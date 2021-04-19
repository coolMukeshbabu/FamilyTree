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

            //Parsing input instruction to better and readable format
            List<InputIntruction> parsedInputIntructions = ParseInputIntructions(inputInstructions);

            //Executing the intruction and printing the result
            List<Output> outputs = ExecuteInputInstructions(parsedInputIntructions, family);
            if(outputs.Count>0)
            {
                foreach (var output in outputs)
                {
                    if (output.status == "SUCCESS")
                    {
                        if (output.data != null)
                        {
                            if (output.data.Count>0)
                            {
                                family.PrintMembers(output.data);
                            }
                            else
                            {
                                Console.WriteLine(output.message);

                            }
                        }
                        else
                        {
                            Console.WriteLine(output.message);
                        }
                    }
                    else
                    {
                        Console.WriteLine(output.message);
                    }
                }
                Console.ReadLine();
            }

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
       
        public static List<Output> ExecuteInputInstructions(List<InputIntruction> inputIntrucations, Family family)
        {
            List<Output> listOutputs = new List<Output>();
            if (inputIntrucations.Count > 0)
            {
                foreach (var input in inputIntrucations)
                {
                    Output output = new Output();

                    switch (input.action)
                    {
                        case "GET_RELATIONSHIP":
                            string memberName = input.data["name"];
                            string relationship = input.data["relationship"];
                            //Check whether given member is exists
                            bool isMemberExists = family.IsMemberExists(memberName);
                            if (isMemberExists)
                            {
                                List<Member> members = family.GetRelativeNameByRelationship(memberName, relationship);
                                output.message = "SUCCESS";
                                output.status = "SUCCESS";
                                if (members.Count > 0)
                                {
                                    output.data = members;
                                }
                                else
                                {
                                    output.message = "NONE";
                                }
                            }
                            else
                            {
                                output.message = "PERSON_NOT_FOUND";
                                output.status = "FAILURE";
                            }
                            listOutputs.Add(output);
                            break;


                        case "ADD_CHILD":
                            string motherName = input.data["motherName"];
                            string childName = input.data["childName"];
                            string gender = input.data["gender"];
                            bool _isMemberExists = family.IsMemberExists(motherName);
                            if (_isMemberExists)
                            {
                                Member member = new Member(childName, gender, "", motherName, "");
                                string message = family.AddMember(motherName, member);
                                output.status = "SUCCESS";
                                output.message = message;
                            }
                            else
                            {
                                output.status = "FAILURE";
                                output.message = "PERSON_NOT_FOUND";
                                output.data = null;
                            }
                            listOutputs.Add(output);
                            break;
                    }
                }
            }
            return listOutputs;
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
                    inputIntruction.action = action;

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
