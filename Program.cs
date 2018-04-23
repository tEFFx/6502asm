using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _6502assembler {
    class Program {
        static readonly ushort START_ADDRESS = 0x1000;
        static void Main(string[] args) {
            string[] lines = File.ReadAllLines("input.asm");

            //Add all labels as symbols
            List<string> symbols = new List<string>();
            symbols.AddRange(lines.Where(x => x.Contains(":")).Select(x => x.Substring(0, x.IndexOf(":"))));
            Console.WriteLine("Found " + symbols.Count + " symbols");
            foreach(string symbol in symbols) {
                Console.WriteLine(symbol);
            }

            Console.WriteLine("");

            ushort programCounter = START_ADDRESS;
            List<Label> labels = new List<Label>();
            Label currentLabel = new Label(string.Empty, START_ADDRESS);
            labels.Add(currentLabel);

            int lineNumber = 1;
            foreach(string line in lines) {
                string l = line.Replace("\t", string.Empty);

                //Does this line start with a label?
                if(l.Contains(":")) {
                    string labelName = l.Remove(l.IndexOf(":"));
                    currentLabel = new Label(labelName, programCounter);
                    labels.Add(currentLabel);

                    l = l.Substring(l.IndexOf(":") + 1);
                    char start = l.First(x => x != ' ' && x != '\t');
                    l = l.Substring(l.IndexOf(start));
                }

                bool containsSymbol = symbols.Any(x => l.Contains(x));

                string opId;
                if(l.Contains(" "))
                    opId = l.Remove(l.IndexOf(" "));
                else
                    opId = l;

                OpCode opCode = OpCodes.Find(opId);

                Instruction instruction;
                OpCode.Result result = opCode.TryParse(l, containsSymbol, out instruction);

                if(result != OpCode.Result.Success) {
                    Console.WriteLine(result + " on line  " + lineNumber + "\"" + l + "\"");
                    break;
                }

                lineNumber++;
                programCounter += (ushort)instruction.GetLength();
                currentLabel.instructions.Add(instruction);
            }

            BinaryWriter writer = new BinaryWriter(File.Open("output.prg", FileMode.Create));
            writer.Write(START_ADDRESS);

            programCounter = START_ADDRESS;
            foreach(Label label in labels) {
                if(label.name != string.Empty)
                    Console.Write(label.name + ": \t");
                else if(label.instructions.Count > 0)
                    Console.Write("\t");

                bool firstInstruction = true;
                foreach(Instruction instruction in label.instructions) {
                    if(firstInstruction)
                        firstInstruction = false;
                    else
                        Console.Write("\n\t");

                    ushort value = 0x0;

                    if(instruction.symbol) {
                        //TODO: More sophisticated way of handling labels
                        Label l = labels.First(x => x.name == instruction.data);
                        if(instruction.mode != OpCode.Mode.Branch) {
                            instruction.value = l.address;
                        } else {
                            sbyte target = (sbyte)(l.address - programCounter - 2);
                            instruction.value = (ushort)target;
                        }
                    } 

                    byte[] bytes = instruction.GetBytes();
                    foreach(byte b in bytes) {
                        Console.Write(b.ToString("X") + " ");
                    }

                    writer.Write(bytes);
                    programCounter += (ushort)bytes.Length;
                }

                if(!firstInstruction)
                    Console.Write("\n");
            }


            writer.Close();
            writer.Dispose();

            Console.ReadKey();
        }
    }
}
