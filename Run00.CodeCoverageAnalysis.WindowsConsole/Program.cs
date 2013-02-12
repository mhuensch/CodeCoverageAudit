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
			try
			{
				//args = new string[] { @"-r=C:\TeamCity\buildAgent\work\498206cac5de9896\UnitTest.coverage" };
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

				Console.WriteLine(result);
			}
			catch (ProgramOptionException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			Console.WriteLine("Exiting: Build Runner");
		}

		static ProgramOptions ParseOptions(string[] args)
		{
			var result = new ProgramOptions();
			var options = new OptionSet() {
				{"r=","The {PATH} of the code coverage report to run analysis on. If [NULL], build will fail.", s => result.ReportPath = s },
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
