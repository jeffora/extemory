using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Extemory.Disassembly.Bea;

namespace Extemory.Disassembly
{
    public class Disassembler
    {
         public static IEnumerable<DisassemblyInstruction> Disassemble(IntPtr pAddr, int maxInstructionCount = 0)
         {
             if (pAddr == IntPtr.Zero)
                 throw new ArgumentNullException("pAddr");

             const int maxInstructionSize = 15;
             const int maxBufferSize = 4096;
             
             // default buffer of 100 x maxInstructionSize bytes (1500 bytes)
             // unless maxInstructionCount is specified
             var bufferSize = (maxInstructionCount == 0 ? 100 : maxInstructionCount)*maxInstructionSize;
             // ensure we don't allocate too large a buffer if a huge instruction count is specified
             bufferSize = Math.Min(bufferSize, maxBufferSize);

             // allocate an unmanaged buffer (instead of reading into managed byte array)
             var pBuffer = IntPtr.Zero;
             try
             {
                 pBuffer = Process.GetCurrentProcess().Allocate(IntPtr.Zero, (uint) bufferSize);

                 // TODO: this is probably horribly inefficient
                 Marshal.Copy(pAddr.ReadArray<byte>(bufferSize), 0, pBuffer, bufferSize);

                 var pDisasmLoc = pBuffer;
                 var virtualAddr = (uint) pAddr; // TODO: currently doesnt support x64
                 var disasm = new Disasm {EIP = pDisasmLoc, VirtualAddr = virtualAddr};

                 int length;
                 var instructionsRead = 0;
                 var bufferOffset = 0;
                 while ((length = BeaEngine.Disasm(disasm)) != (int) BeaConstants.SpecialInfo.UNKNOWN_OPCODE)
                 {
                     instructionsRead++;

                     var disasmInstr = new DisassemblyInstruction(disasm, length,
                                                                  virtualAddr.ReadArray<byte>(length));
                     yield return disasmInstr;

                     pDisasmLoc += length;
                     virtualAddr += (uint) length;
                     bufferOffset += length;

                     if (maxInstructionCount > 0 && instructionsRead >= maxInstructionCount)
                         break;

                     // if we don't have an instruction limit and we're less than maxInstructionSize away
                     // from the end of the buffer, reread buffer data from current location
                     if ((bufferSize - bufferOffset) < maxInstructionSize)
                     {
                         // Copy new bytes to buffer from current location
                         Marshal.Copy(virtualAddr.ReadArray<byte>(bufferSize), 0, pBuffer, bufferSize);
                         // reset pointers etc
                         pDisasmLoc = pBuffer;
                         bufferOffset = 0;
                     }

                     disasm.EIP = pDisasmLoc;
                     disasm.VirtualAddr = virtualAddr;
                 }
             }
             finally
             {
                 if (pBuffer != IntPtr.Zero)
                     Process.GetCurrentProcess().Free(pBuffer);
             }
         }
    }

    public class DisassemblyInstruction
    {
        public DisassemblyInstruction(Disasm disasm, int length, byte[] instructionData)
        {
            Length = length;
            InstructionData = instructionData;
            CompleteInstruction = disasm.CompleteInstr;
            Architecture = disasm.Archi;
            Options = disasm.Options;
            Instruction = disasm.Instruction;
            Argument1 = disasm.Argument1;
            Argument2 = disasm.Argument2;
            Argument3 = disasm.Argument3;
            Prefix = disasm.Prefix;
        }

        // Bea Disasm property duplicates
        public string CompleteInstruction { get; private set; }
        public UInt32 Architecture { get; private set; }
        public UInt64 Options { get; private set; }
        public InstructionType Instruction { get; private set; }
        public ArgumentType Argument1 { get; private set; }
        public ArgumentType Argument2 { get; private set; }
        public ArgumentType Argument3 { get; private set; }
        public PrefixInfo Prefix { get; private set; }

        // Extra information
        public int Length { get; private set; }
        public byte[] InstructionData { get; private set; }
    }
}