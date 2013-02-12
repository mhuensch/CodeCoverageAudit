namespace Run00.CodeCoverageAnalysis
{
	public interface IReportAnalyzer
	{
		ReportAnalysis Analyze(string reportPath);
	}
}
