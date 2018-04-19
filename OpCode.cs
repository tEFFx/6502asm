using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502assembler {
    class OpCode {
        public enum Mode { Invalid, Implied, Immediate, ZeroPage, ZeroPageX, ZeroPageY, Absolute, AbsoluteX, AbsoluteY, Indirect, IndirectX, IndirectY }
        public enum Result { Success, UnsupportedMode, InvalidData, SyntaxError }
        public string id { get; private set; }
        public Dictionary<Mode, byte> modes { get; set; }

        public OpCode(string id) {
            this.id = id.ToLower();
            OpCodes.instances.Add(id, this);
        }

        public Result TryParse(string line, out byte[] bytes) {
            Mode mode = Mode.Invalid;

            bytes = new byte[0];
            line = line.ToLower();

            if(line == id) {
                mode = Mode.Implied;
                if(TryGetBytes(mode, 0, ref bytes))
                    return Result.Success;

                return Result.UnsupportedMode;
            }

            if(line.Length <= id.Length)
                return Result.SyntaxError;

            //Remove OpCode from string
            line = line.Substring(id.Length + 1);

            if(line.StartsWith("#")) {
                mode = Mode.Immediate;
                line = line.Substring(1);
            } else if(line.StartsWith("(")) {
                if(line.EndsWith(",x)"))
                    mode = Mode.IndirectX;
                else if(line.EndsWith("),y"))
                    mode = Mode.IndirectY;
                else
                    mode = Mode.Indirect;

                line = line.Substring(1);
                line = line.Remove(mode != Mode.Indirect ? line.Length - 3 : line.Length - 1);
            }

            string dataString;
            if(line.Contains(",")) {
                int commaIndex = line.IndexOf(",");
                dataString = line.Remove(commaIndex);
                line = line.Substring(commaIndex);
            } else {
                dataString = line;
                line = string.Empty;
            }

            ushort data;
            try {
                if(dataString.StartsWith("$")) {
                    data = Convert.ToUInt16(dataString.Substring(1), 16);
                } else if(dataString.StartsWith("%")) {
                    data = Convert.ToUInt16(dataString.Substring(1), 2);
                } else {
                    data = Convert.ToUInt16(dataString);
                }
            } catch {
                return Result.SyntaxError;
            }

            if(mode == Mode.Invalid) {
                if(data <= 255) {
                    if(line == string.Empty)
                        mode = Mode.ZeroPage;
                    else if(line == ",x")
                        mode = Mode.ZeroPageX;
                    else if(line == ",y")
                        mode = Mode.ZeroPageY;
                } else {
                    if(line == string.Empty)
                        mode = Mode.Absolute;
                    else if(line == ",x")
                        mode = Mode.AbsoluteX;
                    else if(line == ",y")
                        mode = Mode.AbsoluteY;
                }
            } else if(data > 255) {
                //Immediate or indirect modes can only be 1 byte!
                return Result.InvalidData;
            }

            if(TryGetBytes(mode, data, ref bytes))
                return Result.Success;

            return Result.UnsupportedMode;
        }

        private bool TryGetBytes(Mode mode, ushort data, ref byte[] bytes) {
            if(!modes.ContainsKey(mode))
                return false;

            byte hex = modes[mode];

            switch(mode) {
                case Mode.Invalid:
                    return false;

                case Mode.Implied:
                    bytes = new byte[1] { hex };
                    return true;

                case Mode.Immediate:
                case Mode.ZeroPage:
                case Mode.ZeroPageX:
                case Mode.ZeroPageY:
                case Mode.IndirectX:
                case Mode.IndirectY:
                    bytes = new byte[2] { hex, (byte)data };
                    return true;

                case Mode.Absolute:
                case Mode.AbsoluteX:
                case Mode.AbsoluteY:
                case Mode.Indirect:
                    byte[] dataBytes = BitConverter.GetBytes(data);
                    if(BitConverter.IsLittleEndian)
                        bytes = new byte[3] { hex, dataBytes[0], dataBytes[1] };
                    else
                        bytes = new byte[3] { hex, dataBytes[1], dataBytes[0] };
                    return true;
            }

            return false;
        } 
    }
}
