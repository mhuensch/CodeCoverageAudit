using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Diagnostics.Contracts;

namespace Run00.CodeCoverageAnalysis.WindowsConsole
{
	public class Installer : IWindsorInstaller
	{
		/// <summary>
		/// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="store">The configuration store.</param>
		[ContractVerification(false)] // Fluent usage without contracts
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IFileSystem>().ImplementedBy<FileSystem>());
		}
	}
}
