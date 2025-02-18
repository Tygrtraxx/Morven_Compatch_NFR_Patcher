using System;
using System.Reflection;

namespace Morven_Compatch_NFR_Patcher.Helpers
{
    /// <summary>
    /// Provides a helper to retrieve the application version as defined by the
    /// AssemblyInformationalVersion attribute. This temporary version returns
    /// detailed debug output showing the values of all the variables.
    /// </summary>
    public static class VersionHelper
    {
        public static string AppVersion
        {
            get
            {
                // Get the assembly containing VersionHelper.
                var assembly = typeof(VersionHelper).Assembly;
                string assemblyName = assembly.FullName;

                // Retrieve the informational version from the assembly attribute.
                // This is typically set by GitVersion (or Git2SemVer).
                string rawInfoVersion = assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion;

                // Start building our debug output.
                string debugOutput = "";
                debugOutput += $"Assembly Name: {assemblyName}\n";
                debugOutput += $"Raw Informational Version: {rawInfoVersion}\n";

                // If the informational version is missing, return our debug output.
                if (string.IsNullOrEmpty(rawInfoVersion))
                {
                    debugOutput += "Informational Version is empty or null.\n";
                    return debugOutput;
                }

                // Look for the '+' character that separates the semantic version from build metadata.
                int plusIndex = rawInfoVersion.IndexOf('+');
                debugOutput += $"Index of '+': {plusIndex}\n";

                if (plusIndex >= 0 && plusIndex < rawInfoVersion.Length - 1)
                {
                    // Extract the semantic version portion (everything before the '+').
                    string semVer = rawInfoVersion.Substring(0, plusIndex);
                    // Extract the build metadata (everything after the '+').
                    string buildMetadata = rawInfoVersion.Substring(plusIndex + 1);

                    debugOutput += $"Semantic Version (before '+'): {semVer}\n";
                    debugOutput += $"Build Metadata (after '+'): {buildMetadata}\n";

                    // If the build metadata is longer than 7 characters, trim it.
                    if (buildMetadata.Length > 7)
                    {
                        string trimmedBuildMetadata = buildMetadata.Substring(0, 7);
                        debugOutput += $"Trimmed Build Metadata: {trimmedBuildMetadata}\n";
                        string finalVersion = $"{semVer}+{trimmedBuildMetadata}";
                        debugOutput += $"Final Version: {finalVersion}\n";
                        return debugOutput;
                    }
                    else
                    {
                        string finalVersion = $"{semVer}+{buildMetadata}";
                        debugOutput += $"Final Version: {finalVersion}\n";
                        return debugOutput;
                    }
                }
                else
                {
                    debugOutput += $"No '+' found. Final Version: {rawInfoVersion}\n";
                    return debugOutput;
                }
            }
        }
    }
}
