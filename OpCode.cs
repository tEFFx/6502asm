using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502assembler {
    class OpCode {
        public enum Mode { Invalid, Implied, Immediate, ZeroPage, ZeroPageX, ZeroPageY, Absolute, AbsoluteX, AbsoluteY, Indirect, IndirectX, IndirectY, Branch }
        public enum Result { Success, UnsupportedMode, InvalidData, SyntaxError }
        public string id { get; private set; }
        public Dictionary<Mode, byte> modes { get; set; }

        public OpCode(string id) {
            this.id = id.ToLower();
            OpCodes.instances.Add(id, this);
        }

        public virtual Result TryParse(string line, bool containsSymbol, out Instruction instruction) {
            instruction = new Instruction(this);
            instruction.mode = Mode.Invalid;
            line = line.ToLower();

            if(line == id) {
                instruction.mode = Mode.Implied;
                return Result.Success;
            }

            if(line.Length <= id.Length)
                return Result.SyntaxError;

            //Remove OpCode from string
            line = line.Substring(id.Length + 1);

            if(line.StartsWith("#")) {
                instruction.mode = Mode.Immediate;
                line = line.Substring(1);
            } else if(line.StartsWith("(")) {
                if(line.EndsWith(",x)"))
                    instruction.mode = Mode.IndirectX;
                else if(line.EndsWith("),y"))
                    instruction.mode = Mode.IndirectY;
                else
                    instruction.mode = Mode.Indirect;

                line = line.Substring(1);
                line = line.Remove(instruction.mode != Mode.Indirect ? line.Length - 3 : line.Length - 1);
            }

            string data;
            if(line.Contains(",")) {
                int commaIndex = line.IndexOf(",");
                data = line.Remove(commaIndex);
                line = line.Substring(commaIndex);
            } else {
                data = line;
                line = string.Empty;
            }

            ushort value = 0;
            if(!containsSymbol) {
                try {
                    data = data.Replace(" ", string.Empty);
                    if(data.StartsWith("$")) {
                        value = Convert.ToUInt16(data.Substring(1), 16);
                    } else if(data.StartsWith("%")) {
                        value = Convert.ToUInt16(data.Substring(1), 2);
                    } else {
                        value = Convert.ToUInt16(data);
                    }
                } catch {
                    return Result.SyntaxError;
                }

                instruction.value = value;
            }

            if(instruction.mode == Mode.Invalid) {
                if(!containsSymbol && value <= 255) {
                    if(line == string.Empty)
                        instruction.mode = Mode.ZeroPage;
                    else if(line == ",x")
                        instruction.mode = Mode.ZeroPageX;
                    else if(line == ",y")
                        instruction.mode = Mode.ZeroPageY;
                } else {
                    if(line == string.Empty)
                        instruction.mode = Mode.Absolute;
                    else if(line == ",x")
                        instruction.mode = Mode.AbsoluteX;
                    else if(line == ",y")
                        instruction.mode = Mode.AbsoluteY;
                }
            } else if(value > 255) {
                //Immediate or indirect modes can only be 1 byte!
                return Result.InvalidData;
            }

            instruction.data = data;
            instruction.symbol = containsSymbol;
            return Result.Success;
        }

        public virtual bool TryGetMode(Mode mode, out byte hex) {
            if(!modes.ContainsKey(mode)) {
                hex = 0x00;
                return false;
            }

            hex = modes[mode];
            return true;
        }

        public virtual int GetLength(Mode mode) {
            if(!modes.ContainsKey(mode))
                return -1;

            byte hex = modes[mode];

            switch(mode) {
                case Mode.Invalid:
                    return -1;

                case Mode.Implied:
                    return 1;

                case Mode.Immediate:
                case Mode.ZeroPage:
                case Mode.ZeroPageX:
                case Mode.ZeroPageY:
                case Mode.IndirectX:
                case Mode.IndirectY:
                    return 2;

                case Mode.Absolute:
                case Mode.AbsoluteX:
                case Mode.AbsoluteY:
                case Mode.Indirect:
                    return 3;
            }

            return -1;
        } 
    }
}
