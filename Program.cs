using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _6502assembler {
    class Program {
        static void Main(string[] args) {
            string[] lines = File.ReadAllLines("input.asm");

            BinaryWriter writer = new BinaryWriter(File.Open("output.prg", FileMode.Create));
            writer.Write((short)0x1000);

            int lineNumber = 1;
            foreach(string line in lines) {
                string opId;
                if(line.Contains(" "))
                    opId = line.Remove(line.IndexOf(" "));
                else
                    opId = line;

                OpCode opCode = OpCodes.Find(opId);

                byte[] bytes;
                OpCode.Result result = opCode.TryParse(line, out bytes);

                if(result != OpCode.Result.Success) {
                    Console.WriteLine(lineNumber + ": " + result);
                    break;
                }

                writer.Write(bytes);

                foreach(byte b in bytes) {
                    Console.Write(b.ToString("X") + " ");
                }
                Console.WriteLine("");
            }

            Console.ReadKey();
        }
    }
}
