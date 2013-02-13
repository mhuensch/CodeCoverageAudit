using System;

namespace Run00.CodeCoverageAudit.WindowsConsole
{
	public class ProgramOptionException : ArgumentException
	{
		public ProgramOptionException(string message) : base(message) { }
	}
}
