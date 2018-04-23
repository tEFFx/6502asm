using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502assembler {
    class OpCodeBranch : OpCode {
        private byte m_Hex;

        public OpCodeBranch(string id, byte hex) : base(id) {
            modes = new Dictionary<Mode, byte> {
                { Mode.Branch, hex }
            };

            m_Hex = hex;
        }

        public override Result TryParse(string line, bool containsSymbol, out Instruction instruction) {
            instruction = new Instruction(this);

            if(!containsSymbol)
                return Result.UnsupportedMode;

            try {
                string label = line.Substring(id.Length + 1);
                instruction.data = label;
                instruction.symbol = true;
                instruction.mode = Mode.Branch;
            } catch {
                return Result.SyntaxError;
            }

            return Result.Success;
        }

        public override int GetLength(Mode mode) {
            if(mode != Mode.Branch)
                return -1;

            return 2;
        }

        public override bool TryGetMode(Mode mode, out byte hex) {
            if(mode != Mode.Branch) {
                hex = 0x00;
                return false;
            }

            hex = m_Hex;
            return true;
        }
    }
}
