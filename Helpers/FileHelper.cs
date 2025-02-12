/*=============================================================================================*
* Class: FileHelper
*
* Description:
*   Provides helper methods for file and directory operations.
*   This class includes methods for copying directories recursively.
*
*=============================================================================================*/

using System.IO;

namespace Morven_Compatch_NFR_Patcher.Helpers
{
    public static class FileHelper
    {
        public static void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Create the destination directory if it doesn't exist.
            Directory.CreateDirectory(destinationDir);

            // Copy all files in the current directory.
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                // Combine the destination directory with the current file name.
                string destFile = Path.Combine(destinationDir, Path.GetFileName(file));

                // Copy the file, overwriting any existing file at the destination.
                File.Copy(file, destFile, overwrite: true);
            }

            // Recursively copy all subdirectories.
            foreach (var subDir in Directory.GetDirectories(sourceDir))
            {
                // Combine the destination directory with the current subdirectory name.
                string destSubDir = Path.Combine(destinationDir, Path.GetFileName(subDir));

                // Recursively copy the subdirectory.
                CopyDirectory(subDir, destSubDir);
            }
        }
    }
}
