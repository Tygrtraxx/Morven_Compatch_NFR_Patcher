/*=============================================================================================*
* Class: ModFilerUpdater
* 
* Description: ModFileUpdater is a helper class responsible for updating mod descriptor files.
* It modifies specific lines in the mod files to ensure compatibility with a given game version
* 
* This class performs the following operations:
* - Locates the mod descriptor files in the "Assets/ModFiles" directory.
* - Updates the 6th line in "morven_patch_NFR.mod" and "descriptor.mod" with the provided game version.
* - Ensures the target files exist before attempting modifications.
* - Provides error handling to prevent unexpected crashes.
* 
* Usage:
* Call ModFileUpdater.UpdateModFiles(gameVersion) to apply the version update.
*=============================================================================================*/

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

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Ensure the file has at least 6 lines before modifying
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
