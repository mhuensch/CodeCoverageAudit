using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Run00.Command;
using System.Diagnostics.Contracts;

namespace Run00.CodeCoverageAudit.Commands.Audit
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
			container.Register(Component.For<ICommand>().ImplementedBy<Command>());
			container.Register(Component.For<IArgumentConverter<Command, Argument>>().ImplementedBy<ArgumentConverter>());
		}
	}
}
