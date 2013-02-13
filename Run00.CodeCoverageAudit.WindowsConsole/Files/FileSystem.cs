using System.IO;

namespace Run00.CodeCoverageAudit.WindowsConsole
{
	public class FileSystem : IFileSystem
	{
		string IFileSystem.Read(string fromPath)
		{
			return File.ReadAllText(fromPath);
		}
	}
}
