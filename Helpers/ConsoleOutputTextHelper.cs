/*=============================================================================================*
*   Class: ConsoleOutputTextHelper
*
*   Description: This static class provides helper methods for appending colored text to a TextBlock.
*      It simplifies the process of adding text with varying colors to a TextBlock's inlines.
*      The class includes a method to append generic colored text and a method (ShowStatusText)
*      to display a status message. The ShowStatusText method accepts a message and a Boolean flag
*      indicating whether the message is an error. Based on the flag, it displays either an "OK"
*      or an "ERROR" status (using different colored text) followed by the custom message. The 
*      custom message is parsed to highlight specific keywords ("mod" and "steam") in varying colors. 
*      After displaying the status, the method pauses for 300 milliseconds.
*=============================================================================================*/


using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Morven_Compatch_NFR_Patcher.Helpers
{
    public static class ConsoleOutputTextHelper
    {
        public static async Task ShowStatusText(TextBlock target, string message, int statusTitle)
        {
            // Ensure the Inlines collection is initialized.
            target.Inlines ??= (InlineCollection)new AvaloniaList<Inline>();

            // Create a new variable to hold the status title text
            string statusTitleText;

            // Display the "OK" status with a message.
            if (statusTitle == 0 || statusTitle == 5)
            {
                if(statusTitle == 0)
                {
                    // Set the status tile text.
                    statusTitleText = "[  OK  ]  ";

                    // Check to make sure the message doesn't go over the character limit.
                    CheckMessageCharacterLimit(target, statusTitleText, message);
                }

                else
                {
                    // Set the status tile text.
                    statusTitleText = "[  PATCHED SUCCESSFULLY  ]  ";

                    // Check to make sure the message doesn't go over the character limit.
                    CheckMessageCharacterLimit(target, statusTitleText, message);
                }

            }

            // Display the "ERROR" with a message
            else if (statusTitle == 1 || statusTitle == 6)
            {
                if(statusTitle == 1)
                {
                    // Set the status tile text.
                    statusTitleText = "[  ERROR  ]  ";

                    // Check to make sure the message doesn't go over the character limit.
                    CheckMessageCharacterLimit(target, statusTitleText, message);
                }

                else
                {
                    // Set the status tile text.
                    statusTitleText = "[  CRITICAL ERROR  ]  ";

                    // Check to make sure the message doesn't go over the character limit.
                    CheckMessageCharacterLimit(target, statusTitleText, message);
                }
            }

            // Display the "TIP" with a message
            else if (statusTitle == 2)
            {
                // Set the status tile text.
                statusTitleText = "[  TIP  ]  ";

                // Check to make sure the message doesn't go over the character limit.
                CheckMessageCharacterLimit(target, statusTitleText, message);
            }

            // Display the "PATCH" with a message
            else if(statusTitle == 3)
            {
                // Set the status tile text.
                statusTitleText = "[  PATCH  ]  ";

                // Check to make sure the message doesn't go over the character limit.
                CheckMessageCharacterLimit(target, statusTitleText, message);
            }

            else
            {
                // Set the status tile text.
                statusTitleText = "[  DEBUG  ]  ";

                // Check to make sure the message doesn't go over the character limit.
                CheckMessageCharacterLimit(target, statusTitleText, message);
            }

            // Wait 300 milliseconds
            await Task.Delay(300);

            // Insert a line break.
            target.Inlines.Add(new LineBreak());
        }

        // Checks to make sure we don't go over the character limit
        public static void CheckMessageCharacterLimit(TextBlock target, string statusTitleText, string message)
        {
            // Figure out the total length of the two strings together.
            string totalMessageLength = statusTitleText + message;

            if (totalMessageLength.Length <= 75)
            {
                // Output the normal message to console, since we're below 75 characters total.
                AppendColoredTextWithKeywords(target, statusTitleText + message);
            }

            else
            {
                // Set the default break index at 75.
                int breakIndex = 75;

                // Try to find the last space within the first 75 characters.
                int lastSpace = totalMessageLength.LastIndexOf(' ', 75);
                if (lastSpace > 0)
                {
                    breakIndex = lastSpace;
                }

                // Extract the first part (up to the break point) and trim any trailing spaces.
                string part1 = totalMessageLength.Substring(0, breakIndex).TrimEnd();

                // Extract the remainder of the totalMessageLength and trim any leading spaces.
                string part2 = totalMessageLength.Substring(breakIndex).TrimStart();

                // Output the broken up messages to the console.
                AppendColoredTextWithKeywords(target, part1 + "\n" + part2);
            }

        }

        // Allows changing the color of the text without huge blocks of text in other parts of the program.
        public static void AppendColoredText(TextBlock target, string text, IBrush foreground)
        {
            // Ensure the target's Inlines collection is initialized.
            target.Inlines ??= (InlineCollection)new AvaloniaList<Inline>();

            // Create a new Run with the specified text and foreground color, then add it.
            target.Inlines.Add(new Run
            {
                Text = text,
                Foreground = foreground
            });
        }

        // Display certain keywords in different colors
        private static void AppendColoredTextWithKeywords(TextBlock target, string message)
        {
            // Pattern to match keywords (case-insensitive).
            string pattern = @"\b(mod|steam|steamapps|Crusader|Kings|III|Paradox|Interactive|line 6|SUCCESSFULLY|OK|ERROR|CRITICAL|PATCH|PATCHED|TIP|DEBUG|You must also disable/uninstall his mod now|avoid conflicts)\b";
            Regex regex = new(pattern, RegexOptions.IgnoreCase);
            int lastIndex = 0;

            // Process each match in the message.
            foreach (Match match in regex.Matches(message))
            {
                // Append any text before the matched keyword in white.
                if (match.Index > lastIndex)
                {
                    AppendColoredText(target, message[lastIndex..match.Index], Brushes.White);
                }

                // Determine the color for the matched keyword.
                string keyword = match.Value;

                // Default color is cyan for keywords ('steam', 'mod' and 'steamapps')
                IBrush keywordBrush = Brushes.Cyan;

                // Change certain keywords to lime green
                if (keyword.Equals("SUCCESSFULLY", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("OK", System.StringComparison.OrdinalIgnoreCase))
                {
                    keywordBrush = Brushes.Lime;
                }

                // Change certain keywords to red
                if (keyword.Equals("ERROR", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("CRITICAL", System.StringComparison.OrdinalIgnoreCase))
                {
                    keywordBrush = Brushes.Red;
                }

                // Change the keyword "PATCH" or "PATCHED" to a special blue
                if (keyword.Equals("PATCH", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("PATCHED", System.StringComparison.OrdinalIgnoreCase))
                {
                    keywordBrush = new SolidColorBrush(Color.Parse("#7c86f4"));
                }

                // Change certain keywords to yellow
                if (
                        keyword.Equals("Crusader", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("Kings", System.StringComparison.OrdinalIgnoreCase) ||
                        keyword.Equals("III", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("Paradox", System.StringComparison.OrdinalIgnoreCase) || 
                        keyword.Equals("Interactive", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("line 6", System.StringComparison.OrdinalIgnoreCase
                    ))
                {
                    keywordBrush = Brushes.Yellow;
                }

                // Change the keyword "DEBUG" to Orange
                if (keyword.Equals("DEBUG", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("You must also disable/uninstall his mod now", System.StringComparison.OrdinalIgnoreCase) || keyword.Equals("avoid conflicts", System.StringComparison.OrdinalIgnoreCase))
                {
                    keywordBrush = Brushes.Orange;
                }

                // Change the keyword "TIP" to HotPink
                if (keyword.Equals("TIP", System.StringComparison.OrdinalIgnoreCase))
                {
                    keywordBrush = Brushes.HotPink;
                }

                // Append the keyword with its assigned color.
                AppendColoredText(target, keyword, keywordBrush);
                lastIndex = match.Index + match.Length;
            }

            // Append any remaining text after the last match in white.
            if (lastIndex < message.Length)
            {
                AppendColoredText(target, message[lastIndex..], Brushes.White);
            }
        }
    }

}
