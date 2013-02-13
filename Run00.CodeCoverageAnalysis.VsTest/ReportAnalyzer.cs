using Microsoft.VisualStudio.Coverage.Analysis;
using System;
using System.Collections.Generic;

namespace Run00.CodeCoverageAnalysis.VsCoverage
{
	public class ReportAnalyzer : IReportAnalyzer
	{
		ReportAnalysis IReportAnalyzer.Analyze(string reportPath)
		{
			var result = new ReportAnalysis();

			using (var coverage = CoverageInfo.CreateFromFile(reportPath))
			{
				foreach (var module in coverage.Modules)
				{
					using (var reader = module.Symbols.CreateReader())
					{
						var moduleReport = Read(reader, module);
						result.LinesCovered += moduleReport.LinesCovered;
						result.LinesNotCovered += moduleReport.LinesNotCovered;
						result.LinesPartiallyCovered += moduleReport.LinesPartiallyCovered;
					}
				}
			}

			result.Lines = result.LinesCovered + result.LinesNotCovered + result.LinesPartiallyCovered;
			var calcPercent = (double)(result.LinesNotCovered + result.LinesPartiallyCovered) / (double)result.Lines;
			result.PercentageCovered = (uint)Math.Truncate(100 - (calcPercent * 100));

			return result;
		}

		private ReportAnalysis Read(ISymbolReader reader, ICoverageModule module)
		{
			var lines = new List<BlockLineRange>();
			var result = new ReportAnalysis();

			//These are unused
			uint id;
			string name;
			string uName;
			string cName;
			string aName;

			byte[] coverageBuffer = module.GetCoverageBuffer(null);
			while (reader.GetNextMethod(out id, out name, out uName, out cName, out aName, lines))
			{
				var stats = CoverageInfo.GetMethodStatistics(coverageBuffer, lines);
				result.LinesCovered += stats.LinesCovered;
				result.LinesPartiallyCovered += stats.LinesPartiallyCovered;
				result.LinesNotCovered += stats.LinesNotCovered;
				lines.Clear();
			}

			return result;
		}
	}
}
