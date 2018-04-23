using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502assembler {
    class Instruction {
        public OpCode opCode { get; private set; }
        public OpCode.Mode mode { get; set; }
        public string data { get; set; }
        public bool symbol { get; set; }
        public ushort value { get; set; }

        public Instruction(OpCode opCode) {
            this.opCode = opCode;
        }

        public int GetLength() {
            return opCode.GetLength(mode);
        }

        public byte[] GetBytes() {
            byte hex;
            if(!opCode.TryGetMode(mode, out hex))
                throw new Exception("Invalid instruction!");

            int len = GetLength();

            switch(len) {
                case 1:
                    return new byte[1] { hex };

                case 2:
                    return new byte[2] { hex, (byte)value };

                case 3:
                    byte[] bytes = BitConverter.GetBytes(value);
                    if(BitConverter.IsLittleEndian)
                        return new byte[3] { hex, bytes[0], bytes[1] };
                    else
                        return new byte[3] { hex, bytes[1], bytes[0] };
            }

            throw new Exception("Instruction too large!");
        }
    }
}
