using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NDesk.Options;
using System;
using System.IO;
using System.Text;

namespace Run00.CodeCoverageAudit.WindowsConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
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
				Console.WriteLine("\tTotal Lines: " + result.Lines);
				Console.WriteLine("\tReported: " + result.PercentageCovered + "% Covered");
				Console.WriteLine("\tMinimum Required: " + options.MinimumCoverage + "%");

				if (result.PercentageCovered < options.MinimumCoverage)
					throw new Exception("Coverage of %" + result.PercentageCovered + " is below required minimum of %" + options.MinimumCoverage);
			}
			catch (ProgramOptionException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

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
