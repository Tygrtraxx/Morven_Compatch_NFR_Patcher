/*=============================================================================================*
*   Class: ModFileUpdater
* 
*   Description: This static class provides helper methods for updating mod descriptor files
*      for Crusader Kings III. It locates the mod files within the designated mod folder
*      (for example, "Paradox Interactive\Crusader Kings III\mod\") and updates specific lines
*      in these files to ensure compatibility with the provided game version.
*
*   This class performs the following operations:
*       - Locates the mod descriptor files based on a given mod folder path.
*       - Updates the 6th line in files such as "morven_patch_NFR.mod" and "descriptor.mod" with the supplied game version.
*       - Verifies that the target files exist before attempting modifications.
*       - Provides error handling to prevent unexpected crashes during file updates.
*
*   Usage:
*       Call ModFileUpdater.UpdateModFiles(modFolderPath, gameVersion) where modFolderPath
*       points to your CK3 mod directory (e.g., "Paradox Interactive\Crusader Kings III\mod\") and
*       gameVersion is the version string to apply.
*=============================================================================================*/

using System;
using System.IO;
using System.Linq;

namespace Morven_Compatch_NFR_Patcher.Helpers
{
    public static class ModFileUpdater
    {
        public static void UpdateModFiles(string modFolderPath, string gameVersion)
        {
            try
            {
                // Define the file paths for the .mod file and the descriptor file.
                string modFilePath = Path.Combine(modFolderPath, "morven_patch_NFR.mod");
                string descriptorFilePath = Path.Combine(modFolderPath, "morven_patch_NFR", "descriptor.mod");

                // Update both files by modifying their game version.
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
