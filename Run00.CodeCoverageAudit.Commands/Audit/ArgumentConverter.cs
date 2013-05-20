using Run00.Command;

namespace Run00.CodeCoverageAudit.Commands.Audit
{
	public class ArgumentConverter : ArgumentConverterBase<Command, Argument>
	{
		public ArgumentConverter()
		{
			SetConverter("r", "is the path to the report to audit", (a, v) => a.ReportPath = v);
			SetConverter("p", "is the minimum percentage of coverage for the audit to pass", (a, v) => a.MinimumCoverage = int.Parse(v));
		}
	}
}
