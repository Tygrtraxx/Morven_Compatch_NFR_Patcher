
using System;
using System.IO;
using System.Linq;

namespace Morven_Compatch_NFR_Patcher.Helpers
{
    public static class ModFileUpdater
    {
        public static void UpdateModFiles(string gameVersion)
        {
            try
            {
                // Define file paths for the mod files in the program
                string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "ModFiles");
                string modFilePath = Path.Combine(basePath, "morven_patch_NFR.mod");
                string descriptorFilePath = Path.Combine(basePath, "morven_patch_NFR", "descriptor.mod");

                // Update both files by modifying their game version
                UpdateGameVersion(modFilePath, gameVersion);
                UpdateGameVersion(descriptorFilePath, gameVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating mod files: {ex.Message}");
            }
        }

        private static void UpdateGameVersion(string filePath, string newLineContent)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length < 6)
            {
                Console.WriteLine($"File {filePath} does not contain enough lines.");
                return;
            }

            // Replace the 6th line (index 5)
            lines[5] = newLineContent;

            // Write back to file
            File.WriteAllLines(filePath, lines);
        }
    }
}
