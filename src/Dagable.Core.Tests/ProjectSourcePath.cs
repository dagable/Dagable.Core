using System.Runtime.CompilerServices;

namespace Dagable.Core.Tests
{
    public static class ProjectSourcePath
    {
        private const string relativePath = $"{nameof(ProjectSourcePath)}.cs";
        public static string Value => CalculatePath();

        public static string GetSourceFilePathName([CallerFilePath] string callerFilePath = null) => callerFilePath ?? "";

        private static string CalculatePath()
        {
            string pathName = GetSourceFilePathName();
            return pathName[..^relativePath.Length];
        }
    }
}
