﻿/*=============================================================================================*
*   Class: VersionHelper
*
*   Description: This static class provides a helper property for retrieving the application version
*      as defined by the AssemblyInformationalVersion attribute of the executing assembly.
*      The AppVersion property processes the raw version string to extract a semantic version and a short
*      SHA commit identifier. The final version is returned in the format:
*         SemanticVersion + "+" + ShortSha
*      For example, if the raw version is "v.2.0.1-alpha.THEBEAST.141+hotfix.163c57f", it will be transformed to
*      "2.0.1-alpha+163c57f". This processing includes:
*         - Removing a leading "v." if present.
*         - Truncating the semantic version after the pre-release label if extra identifiers exist.
*         - Extracting a 7-character short SHA from the build metadata.
*=============================================================================================*/

using Avalonia.OpenGL;
using System;
using System.Reflection;

public static class VersionHelper
{
    public static string AppVersion
    {
        get
        {
            // Get the assembly that contains this helper class.
            var assembly = typeof(VersionHelper).Assembly;

            // Retrieve the informational version from the assembly attribute.
            string? rawInfoVersionNullable = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            // If the informational version is missing, return a default message.
            if (string.IsNullOrEmpty(rawInfoVersionNullable))
            {
                return "Version not available";
            }
            // Now that we know it's not null or empty, assign it to a non-nullable variable.
            string rawInfoVersion = rawInfoVersionNullable;

            // Find the '+' character that separates the semantic version from the build metadata.
            int plusIndex = rawInfoVersion.IndexOf('+');
            if (plusIndex < 0)
            {
                // If no '+' is found, return the raw version string.
                return rawInfoVersion;
            }

            // Extract the semantic version (everything before the '+').
            string semVer = rawInfoVersion.Substring(0, plusIndex);

            // Remove a leading "v." if present.
            if (semVer.StartsWith("v."))
            {
                semVer = semVer.Substring(2);
            }

            // If the semantic version contains extra identifiers after the pre-release label, trim it so only the pre-release label remains.
            // For example: "2.0.1-alpha.THEBEAST.141" becomes "2.0.1-alpha".
            int hyphenIndex = semVer.IndexOf('-');
            if (hyphenIndex >= 0)
            {
                int dotAfterHyphen = semVer.IndexOf('.', hyphenIndex);
                if (dotAfterHyphen > 0)
                {
                    semVer = semVer.Substring(0, dotAfterHyphen);
                }
            }

            string shortSha = "";

            // Check for the branch "main" because we have to get the version a bit differently since it's considered a release.
            if (rawInfoVersion.Contains("main"))
            {
                // Find the index of "main" in the string.
                int mainIndex = rawInfoVersion.IndexOf("main");

                // Find the period immediately after "main".
                int periodIndex = rawInfoVersion.IndexOf('.', mainIndex + "main".Length);

                // Take the 7 characters immediately after the dot as the ShortSha.
                shortSha = rawInfoVersion.Substring(periodIndex + 1, 7);
            }

            // For anything else that is not main.
            else
            {
                // Extract the build metadata (everything after the '+').
                string buildMetadata = rawInfoVersion.Substring(plusIndex + 1);

                // Look for a '.' in the build metadata which separates a prefix from the SHA.
                int dotIndex = buildMetadata.IndexOf('.');

                if (dotIndex >= 0 && buildMetadata.Length >= dotIndex + 1 + 7)
                {
                    // If found, take the 7 characters immediately after the dot as the ShortSha.
                    shortSha = buildMetadata.Substring(dotIndex + 1, 7);
                }
                else
                {
                    // Fallback: use the first 7 characters of buildMetadata.
                    shortSha = buildMetadata.Length > 7 ? buildMetadata.Substring(0, 7) : buildMetadata;
                }
            }
            // Construct and return the final version string in the format: SemanticVersion + "+" + ShortSha.
            return $"{semVer}+{shortSha}";
        }
    }
}
