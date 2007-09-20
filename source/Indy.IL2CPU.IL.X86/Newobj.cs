using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;
using Asm = Indy.IL2CPU.Assembler;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(Code.Newobj, false)]
	public class Newobj: Op {
		public string CtorName;
		public uint ObjectSize = 0;
		public int CtorArgumentCount;
		public Newobj()
			: base(null, null, null) {
		}

		public Newobj(Mono.Cecil.Cil.Instruction aInstruction, MethodInformation aMethodInfo)
			: base(aInstruction, aMethodInfo, null) {
			MethodReference xCtor;
			xCtor = (MethodReference)aInstruction.Operand;
			CtorName = new Asm.Label((MethodReference)aInstruction.Operand).Name;
			DoQueueMethodRef(xCtor);
			DoQueueMethodRef(RuntimeEngineRefs.Heap_AllocNewObjectRef);
			CtorArgumentCount = xCtor.Parameters.Count;
			ObjectSize = ObjectUtilities.GetObjectStorageSize(Engine.GetDefinitionFromTypeReference(xCtor.DeclaringType));
		}

		public override void DoAssemble() {
			Pushd("0" + ObjectSize.ToString("X").ToUpper() + "h");
			Call(new CPU.Label(RuntimeEngineRefs.Heap_AllocNewObjectRef).Name);
			Pushd("eax");
//			Move(Assembler, "ecx", "eax");
			Pushd("eax");
			for (int i = 0; i < CtorArgumentCount; i++) {
				Pushd("[ebp - 08h]");
			}
			//Pushd("ecx");
			Call(CtorName);
			//Assembler.Add(new CPUx86.Add("esp", objSize.ToString()));
		}
	}
}