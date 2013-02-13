namespace Run00.CodeCoverageAnalysis
{
	public class ReportAnalysis
	{
		public uint Lines { get; set; }
		public uint LinesCovered { get; set; }
		public uint LinesPartiallyCovered { get; set; }
		public uint LinesNotCovered { get; set; }
		public uint PercentageCovered { get; set; }
	}
}
