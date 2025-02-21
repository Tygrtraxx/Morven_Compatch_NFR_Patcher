/*=============================================================================================*
*   Class: Program
*
*   Description: This class serves as the entry point for the Avalonia desktop application.
*      It contains the Main method which initializes the Avalonia framework and starts the
*      application using a classic desktop lifetime. The BuildAvaloniaApp method configures the
*      Avalonia AppBuilder with platform detection, a custom font (Inter), and logging support.
*
*   Note: Avoid using Avalonia, third-party APIs, or any SynchronizationContext-reliant code 
*         before the application is fully initialized. All such initialization should occur 
*         after AppMain is called.
*=============================================================================================*/

using System;
using Avalonia;

namespace Morven_Compatch_NFR_Patcher.Desktop
{
    class Program
    {
        // Entry point of the application.
        // This method sets up and starts the Avalonia application with a classic desktop lifetime.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Configures and returns an Avalonia AppBuilder instance.
        // This method sets up platform detection, custom font integration, and logging.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
