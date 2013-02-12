using System;

namespace Run00.CodeCoverageAnalysis.WindowsConsole
{
	public class ProgramOptionException : ArgumentException
	{
		public ProgramOptionException(string message) : base(message) { }
	}
}
