using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502assembler {
    class Label {
        public string name { get; private set; }
        public ushort address { get; private set; }
        public List<Instruction> instructions = new List<Instruction>();

        public Label(string name, ushort address) {
            this.name = name;
            this.address = address;
        }
    }
}
