namespace Run00.CodeCoverageAudit
{
	public interface IReportAnalyzer
	{
		ReportAnalysis Analyze(string reportPath);
	}
}
