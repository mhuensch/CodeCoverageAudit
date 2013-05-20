using Run00.Command;
using System;

namespace Run00.CodeCoverageAudit.Commands.Audit
{
	public class Command : CommandBase<Command, Argument>
	{
		public override string Name { get { return "Audit Code Coverage Report"; } }
		public override string Description { get { return "examines the code coverage report for the percentage supplied."; } }

		public Command(IArgumentConverterFactory converter, ICommandLogger logger, IReportAnalyzer reportAuditer)	
			: base(converter, logger)
		{
			_reportAuditer = reportAuditer;
			_logger = logger;
		}

		protected override object Execute(Argument arg)
		{
			_logger.Information("Analyzing: " + arg.ReportPath);
			var result = _reportAuditer.Analyze(arg.ReportPath);

			_logger.Information("LinesCovered: " + result.LinesCovered);
			_logger.Information("LinesNotCovered: " + result.LinesNotCovered);
			_logger.Information("LinesPartiallyCovered: " + result.LinesPartiallyCovered);
			_logger.Information("Total Lines: " + result.Lines);
			_logger.Information("Reported: " + result.PercentageCovered + "% Covered");

			if (result.PercentageCovered < arg.MinimumCoverage)
				throw new Exception("Coverage of %" + result.PercentageCovered + " is below required minimum of %" + arg.MinimumCoverage);

			return result;
		}

		private IReportAnalyzer _reportAuditer;
		private ICommandLogger _logger;


	}
}
