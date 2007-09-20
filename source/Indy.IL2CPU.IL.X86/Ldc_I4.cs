using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(Code.Ldc_I4)]
	public class Ldc_I4: Op {
		private int mValue;
		protected void SetValue(int aValue) {
			mValue = aValue;
		}

		protected void SetValue(string aValue) {
			SetValue(Int32.Parse(aValue));
		}

		public Ldc_I4(Mono.Cecil.Cil.Instruction aInstruction, MethodInformation aMethodInfo)
			: base(aInstruction, aMethodInfo, null) {
			if (aInstruction.Operand != null) {
				SetValue(aInstruction.Operand.ToString());
			}
		}

		public int Value {
			get {
				return mValue;
			}
		}
		public override sealed void DoAssemble() {
			Pushd("0" + mValue.ToString("X") + "h");
		}
	}
}