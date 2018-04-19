using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502assembler {
    static class OpCodes {
        public static Dictionary<string, OpCode> instances = new Dictionary<string, OpCode>();

        public static OpCode Find(string opCode) {
            if(instances.ContainsKey(opCode))
                return instances[opCode];

            return null;
        }

        static OpCode ADC = new OpCode("adc") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0x69 },
                { OpCode.Mode.ZeroPage, 0x65 },
                { OpCode.Mode.ZeroPageX, 0x75 },
                { OpCode.Mode.Absolute, 0x6D },
                { OpCode.Mode.AbsoluteX, 0x7D },
                { OpCode.Mode.AbsoluteY, 0x79 },
                { OpCode.Mode.IndirectX, 0x61 },
                { OpCode.Mode.IndirectY, 0x71 }
            }
        };

        static OpCode AND = new OpCode("and") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0x29 },
                { OpCode.Mode.ZeroPage, 0x25 },
                { OpCode.Mode.ZeroPageX, 0x35 },
                { OpCode.Mode.Absolute, 0x2D },
                { OpCode.Mode.AbsoluteX, 0x3D },
                { OpCode.Mode.AbsoluteY, 0x39 },
                { OpCode.Mode.IndirectX, 0x21 },
                { OpCode.Mode.IndirectY, 0x31 }
            }
        };

        static OpCode ASL = new OpCode("asl") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x0A },
                { OpCode.Mode.ZeroPage, 0x06 },
                { OpCode.Mode.ZeroPageX, 0x16 },
                { OpCode.Mode.Absolute, 0x0E },
                { OpCode.Mode.AbsoluteX, 0x1E },
            }
        };

        static OpCode BIT = new OpCode("bit") {
        modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.ZeroPage, 0x24 },
                { OpCode.Mode.Absolute, 0x2C },
            }
        };

        //Add support for branch instructions and more importantly labels!

        static OpCode BRK = new OpCode("brk") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x00 },
            }
        };

        static OpCode CMP = new OpCode("cmp") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0xC9 },
                { OpCode.Mode.ZeroPage, 0xC5 },
                { OpCode.Mode.ZeroPageX, 0xD5 },
                { OpCode.Mode.Absolute, 0xCD },
                { OpCode.Mode.AbsoluteX, 0xDD },
                { OpCode.Mode.AbsoluteY, 0xD9 },
                { OpCode.Mode.IndirectX, 0xC1 },
                { OpCode.Mode.IndirectY, 0xD1 }
            }
        };

        static OpCode CPX = new OpCode("cpx") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0xE0 },
                { OpCode.Mode.ZeroPage, 0xE4 },
                { OpCode.Mode.Absolute, 0xEC }
            }
        };

        static OpCode CPY = new OpCode("cpy") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0xC0 },
                { OpCode.Mode.ZeroPage, 0xC4 },
                { OpCode.Mode.Absolute, 0xCC }
            }
        };

        static OpCode DEC = new OpCode("dec") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.ZeroPage, 0xC6 },
                { OpCode.Mode.ZeroPageX, 0xD6 },
                { OpCode.Mode.Absolute, 0xCE },
                { OpCode.Mode.AbsoluteX, 0xDE }
            }
        };

        static OpCode EOR = new OpCode("eor") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0x49 },
                { OpCode.Mode.ZeroPage, 0x45 },
                { OpCode.Mode.ZeroPageX, 0x55 },
                { OpCode.Mode.Absolute, 0x4D },
                { OpCode.Mode.AbsoluteX, 0x5D },
                { OpCode.Mode.AbsoluteY, 0x59 },
                { OpCode.Mode.IndirectX, 0x41 },
                { OpCode.Mode.IndirectY, 0x51 }
            }
        };

        static OpCode CLC = new OpCode("clc") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x18 },
            }
        };

        static OpCode SEC = new OpCode("sec") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x38 },
            }
        };

        static OpCode CLI = new OpCode("cli") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x58 },
            }
        };

        static OpCode SEI = new OpCode("sei") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x78 },
            }
        };

        static OpCode CLV = new OpCode("clv") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xB8 },
            }
        };

        static OpCode CLD = new OpCode("cld") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xD8 },
            }
        };

        static OpCode SED = new OpCode("sed") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xF8 },
            }
        };

        static OpCode INC = new OpCode("inc") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.ZeroPage, 0xE6 },
                { OpCode.Mode.ZeroPageX, 0xF6 },
                { OpCode.Mode.Absolute, 0xEE },
                { OpCode.Mode.AbsoluteX, 0xFE }
            }
        };

        static OpCode JMP = new OpCode("jmp") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Absolute, 0x4C },
                { OpCode.Mode.Indirect, 0x6C }
            }
        };

        static OpCode JSR = new OpCode("jsr") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Absolute, 0x20 }
            }
        };

        static OpCode LDA = new OpCode("lda") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0xA9 },
                { OpCode.Mode.ZeroPage, 0xA5 },
                { OpCode.Mode.ZeroPageX, 0xB5 },
                { OpCode.Mode.Absolute, 0xAD },
                { OpCode.Mode.AbsoluteX, 0xBD },
                { OpCode.Mode.AbsoluteY, 0xB9 },
                { OpCode.Mode.IndirectX, 0xA1 },
                { OpCode.Mode.IndirectY, 0xB1 }
            }
        };

        static OpCode LDX = new OpCode("ldx") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0xA2 },
                { OpCode.Mode.ZeroPage, 0xA6 },
                { OpCode.Mode.ZeroPageY, 0xB6 },
                { OpCode.Mode.Absolute, 0xAE },
                { OpCode.Mode.AbsoluteY, 0xBE }
            }
        };

        static OpCode LDY = new OpCode("ldy") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0xA0 },
                { OpCode.Mode.ZeroPage, 0xA4 },
                { OpCode.Mode.ZeroPageX, 0xB4 },
                { OpCode.Mode.Absolute, 0xAC },
                { OpCode.Mode.AbsoluteX, 0xBC }
            }
        };

        static OpCode LSR = new OpCode("lsr") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0x4A },
                { OpCode.Mode.ZeroPage, 0x46 },
                { OpCode.Mode.ZeroPageX, 0x56 },
                { OpCode.Mode.Absolute, 0x4E },
                { OpCode.Mode.AbsoluteX, 0x5E }
            }
        };

        static OpCode NOP = new OpCode("nop") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xEA }
            }
        };

        static OpCode ORA = new OpCode("ora") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0x09 },
                { OpCode.Mode.ZeroPage, 0x05 },
                { OpCode.Mode.ZeroPageX, 0x15 },
                { OpCode.Mode.Absolute, 0x0D },
                { OpCode.Mode.AbsoluteX, 0x1D },
                { OpCode.Mode.AbsoluteY, 0x19 },
                { OpCode.Mode.IndirectX, 0x01 },
                { OpCode.Mode.IndirectY, 0x11 }
            }
        };

        static OpCode TAX = new OpCode("tax") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xAA }
            }
        };

        static OpCode TXA = new OpCode("txa") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x8A }
            }
        };

        static OpCode DEX = new OpCode("dex") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xCA }
            }
        };

        static OpCode INX = new OpCode("inx") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xEA }
            }
        };

        static OpCode TAY = new OpCode("tay") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xA8 }
            }
        };

        static OpCode TYA = new OpCode("tya") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x98 }
            }
        };

        static OpCode DEY = new OpCode("dey") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x88 }
            }
        };

        static OpCode INY = new OpCode("iny") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xC8 }
            }
        };

        static OpCode ROL = new OpCode("rol") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x2A },
                { OpCode.Mode.ZeroPage, 0x26 },
                { OpCode.Mode.ZeroPageX, 0x36 },
                { OpCode.Mode.Absolute, 0x2E },
                { OpCode.Mode.AbsoluteX, 0x3E }
            }
        };

        static OpCode ROR = new OpCode("ror") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x6A },
                { OpCode.Mode.ZeroPage, 0x66 },
                { OpCode.Mode.ZeroPageX, 0x76 },
                { OpCode.Mode.Absolute, 0x6E },
                { OpCode.Mode.AbsoluteX, 0x7E }
            }
        };

        static OpCode RTI = new OpCode("rti") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x40 }
            }
        };

        static OpCode RTS = new OpCode("rts") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x60 }
            }
        };

        static OpCode SBC = new OpCode("sbc") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Immediate, 0xE9 },
                { OpCode.Mode.ZeroPage, 0xE5 },
                { OpCode.Mode.ZeroPageX, 0xF5 },
                { OpCode.Mode.Absolute, 0xED },
                { OpCode.Mode.AbsoluteX, 0xFD },
                { OpCode.Mode.AbsoluteY, 0xF9 },
                { OpCode.Mode.IndirectX, 0xE1 },
                { OpCode.Mode.IndirectY, 0xF1 }
            }
        };

        static OpCode STA = new OpCode("sta") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.ZeroPage, 0x85 },
                { OpCode.Mode.ZeroPageX, 0x95 },
                { OpCode.Mode.Absolute, 0x8D },
                { OpCode.Mode.AbsoluteX, 0x9D },
                { OpCode.Mode.AbsoluteY, 0x99 },
                { OpCode.Mode.IndirectX, 0x81 },
                { OpCode.Mode.IndirectY, 0x91 }
            }
        };

        static OpCode TXS = new OpCode("txs") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x9A }
            }
        };

        static OpCode TSX = new OpCode("tsx") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0xBA }
            }
        };

        static OpCode PHA = new OpCode("pha") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x48 }
            }
        };

        static OpCode PLA = new OpCode("pla") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x68 }
            }
        };

        static OpCode PHP = new OpCode("php") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x08 }
            }
        };

        static OpCode PLP = new OpCode("plp") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.Implied, 0x28 }
            }
        };

        static OpCode STX = new OpCode("stx") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.ZeroPage, 0x86 },
                { OpCode.Mode.ZeroPageY, 0x96 },
                { OpCode.Mode.Absolute, 0x8E }
            }
        };

        static OpCode STY = new OpCode("sty") {
            modes = new Dictionary<OpCode.Mode, byte>() {
                { OpCode.Mode.ZeroPage, 0x84 },
                { OpCode.Mode.ZeroPageY, 0x94 },
                { OpCode.Mode.Absolute, 0x8C }
            }
        };
    }
}
