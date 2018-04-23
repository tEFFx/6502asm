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

            //Add user-defined symbols
            symbols.AddRange(lines.Where(x => x.Contains(":")).Select(x => x.Substring(0, x.IndexOf(":"))));

            Console.WriteLine("Found " + symbols.Count + " symbols");
            foreach(string symbol in symbols) {
                Console.WriteLine(symbol);
            }
            Console.WriteLine("");

            //Add predefined symbols
            symbols.Add("*");

            ushort programCounter = START_ADDRESS;
            List<Label> labels = new List<Label>();
            Label currentLabel = new Label(string.Empty, START_ADDRESS);
            labels.Add(currentLabel);

            int lineNumber = 1;
            foreach(string currentLine in lines) {
                string line = currentLine;

                //Remove any comments
                if(line.Contains("//"))
                    line = line.Remove(line.IndexOf("//"));

                //Is this an empty line?
                if(!line.Any(x => x != ' ' && x != '\t'))
                    continue;

                line = line.Replace("\t", string.Empty);

                //Does this line start with a label?
                if(line.Contains(":")) {
                    string labelName = line.Remove(line.IndexOf(":"));
                    currentLabel = new Label(labelName, programCounter);
                    labels.Add(currentLabel);

                    line = line.Substring(line.IndexOf(":") + 1);

                    //Is this label empty?
                    if(!line.Any(x => x != ' ' && x != '\t'))
                        continue;
                }

                char start = line.First(x => x != ' ' && x != '\t');
                line = line.Substring(line.IndexOf(start));

                bool containsSymbol = symbols.Any(x => line.Contains(x));

                string opId;
                if(line.Contains(" "))
                    opId = line.Remove(line.IndexOf(" "));
                else
                    opId = line;

                OpCode opCode = OpCodes.Find(opId);

                Instruction instruction;
                OpCode.Result result = opCode.TryParse(line, containsSymbol, out instruction);

                if(result != OpCode.Result.Success) {
                    Console.WriteLine(result + " on line " + lineNumber + " \"" + line + "\"");
                    Console.ReadKey();
                    return;
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
                        switch(instruction.data) {
                            case "*":
                                instruction.value = programCounter;
                                break;

                            default:
                                bool highByte = instruction.data.StartsWith(">");
                                bool lowByte = instruction.data.StartsWith("<");
                                if(highByte || lowByte)
                                    instruction.data = instruction.data.Substring(1);

                                //TODO: More sophisticated way of handling labels
                                Label l = labels.First(x => x.name == instruction.data);
                                if(instruction.mode != OpCode.Mode.Branch) {
                                    if(!highByte && !lowByte)
                                        instruction.value = l.address;
                                    else if(highByte)
                                        instruction.value = (ushort)(l.address >> 8);
                                    else
                                        instruction.value = (ushort)(l.address & 0x00FF);
                                } else {
                                    sbyte target = (sbyte)(l.address - programCounter - 2);
                                    instruction.value = (ushort)target;
                                }
                                break;
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
