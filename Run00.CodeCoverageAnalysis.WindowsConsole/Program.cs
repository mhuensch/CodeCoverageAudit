using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NDesk.Options;
using System;
using System.IO;
using System.Text;

namespace Run00.CodeCoverageAnalysis.WindowsConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var percentage = default(int);
			var minimum = default(int);
			try
			{
				args = new string[] { @"-r=C:\TeamCity\buildAgent\work\498206cac5de9896\UnitTest.coverage", "-m=95" };
				Console.WriteLine("Starting: Code Coverage Analysis...");

				var processDir = Path.GetDirectoryName(typeof(Program).Assembly.Location);

				Console.WriteLine("Installing Processes From: " + processDir);
				var container = new WindsorContainer();
				container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));
				container.Install(FromAssembly.This());
				container.Install(FromAssembly.InDirectory(new AssemblyFilter(processDir, "*.dll")));

				Console.WriteLine("\tParsing: args");
				var options = ParseOptions(args);
				var analyzer = container.Resolve<IReportAnalyzer>();

				Console.WriteLine("\tAnalyzing: " + options.ReportPath);
				var result = analyzer.Analyze(options.ReportPath);

				Console.WriteLine("\tLinesCovered: " + result.LinesCovered);
				Console.WriteLine("\tLinesNotCovered: " + result.LinesNotCovered);
				Console.WriteLine("\tLinesPartiallyCovered: " + result.LinesPartiallyCovered);

				var totalLines = result.LinesCovered + result.LinesNotCovered + result.LinesPartiallyCovered;
				Console.WriteLine("\tTotal Lines: " + totalLines);

				var calcPercent = (double)(result.LinesNotCovered + result.LinesPartiallyCovered) / (double)totalLines;
				percentage = (int)Math.Truncate(100 - (calcPercent * 100));
				minimum = options.MinimumCoverage;
				Console.WriteLine("\tReported: " + percentage + "% Covered");
				Console.WriteLine("\tMinimum Required: " + options.MinimumCoverage + "%");

			}
			catch (ProgramOptionException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			if (percentage < minimum)
				throw new Exception("Coverage of %" + percentage + " is below required minimum of %" + minimum);

			Console.WriteLine("Exiting: Build Runner");
		}

		static ProgramOptions ParseOptions(string[] args)
		{
			var result = new ProgramOptions();
			var options = new OptionSet() {
				{"r=","The {PATH} of the code coverage report to run analysis on.", s => result.ReportPath = s },
				{"m=","The {MINIMUM} of the code covered or an exception will be thrown.", m => result.MinimumCoverage = int.Parse(m) },
			};
			options.Parse(args);

			var hasRequiredOptions = string.IsNullOrEmpty(result.ReportPath) == false;
			if (hasRequiredOptions == false)
			{
				var stringBuilder = new StringBuilder("\r\n");
				using (var writer = new StringWriter(stringBuilder))
				{
					options.WriteOptionDescriptions(writer);
				}
				throw new ProgramOptionException(stringBuilder.ToString());
			}

			return result;
		}
	}
}
