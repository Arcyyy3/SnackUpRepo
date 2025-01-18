using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using Microsoft.Maui.LifecycleEvents;
using System.Diagnostics;
#if ANDROID
using Android.OS;
using Android.Views;
#endif

namespace SnackUpClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Starting MauiApp Builder");

                var builder = MauiApp.CreateBuilder();

                // Configura l'app principale
                builder
                    .UseMauiApp<App>()
                    .UseMauiCommunityToolkit() // Toolkit di utilità per MAUI
                    .ConfigureSyncfusionCore() // Supporto per Syncfusion
                    .UseSkiaSharp() // SkiaSharp per disegni personalizzati
                    .ConfigureFonts(fonts =>
                    {
                        // Aggiunta dei font personalizzati
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                        fonts.AddFont("Roboto-Black.ttf", "Roboto-Black");
                        fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                        fonts.AddFont("Rotondo.ttf", "Rotondo");
                    });

                // Eventi specifici per piattaforma
                builder.ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    events.AddAndroid(android =>
                        android.OnCreate((activity, bundle) =>
                        {
                              System.Diagnostics.Debug.WriteLine("Android OnCreate Called");

                            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                            {
                                activity.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                                activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                                activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                            }
                            activity.Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
                        }));
#endif
                });

#if DEBUG
                // Logging abilitato solo in debug
                builder.Logging.AddDebug();
                System.Diagnostics.Debug.WriteLine("Debug logging enabled");
#endif

                var app = builder.Build();

                System.Diagnostics.Debug.WriteLine("MauiApp built successfully");

                return app;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during MauiApp creation: {ex}");
                throw;
            }
        }
    }
}