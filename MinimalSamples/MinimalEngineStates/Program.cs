//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using Tobii.EyeX.Client.Interop;
using Tobii.EyeX.Framework;
using System;
using EyeXFramework;
using Tobii.EyeX.Client;

namespace MinimalEngineStates
{

    public static class Program
    {
        public static void Main(string[] args)
        {
            // Adding file to save eye coordinates
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../coordata.csv" , false))
            {
                file.WriteLine("X , Y , UTC");
            }

            // checking software availability
            switch (EyeXHost.EyeXAvailability)
            {
                case EyeXAvailability.NotAvailable:
                    Console.WriteLine("This sample requires the EyeX Engine, but it isn't available.");
                    Console.WriteLine("Please install the EyeX Engine and try again.");
                    return;

                case EyeXAvailability.NotRunning:
                    Console.WriteLine("This sample requires the EyeX Engine, but it isn't rnning.");
                    Console.WriteLine("Please make sure that the EyeX Engine is started.");
                    break;
            }

            using (var eyeXHost = new EyeXHost())
            {
                // starting the EyeX host.
                eyeXHost.Start();

                // calibrating options
                /*Console.WriteLine("EYEX CONFIGURATION TOOLS");
                Console.WriteLine("========================");
                Console.WriteLine();
                Console.WriteLine("Choose an option :");
                Console.WriteLine("T) Test calibration");
                Console.WriteLine("G) Guest calibration");
                Console.WriteLine("R) Recalibrate");
                Console.WriteLine("D) Display Setup");
                Console.WriteLine("P) Create Profile");

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.T:
                        eyeXHost.LaunchCalibrationTesting();
                        break;
                    case ConsoleKey.G:
                        eyeXHost.LaunchGuestCalibration();
                        break;
                    case ConsoleKey.R:
                        eyeXHost.LaunchRecalibration();
                        break;
                    case ConsoleKey.D:
                        eyeXHost.LaunchDisplaySetup();
                        break;
                    case ConsoleKey.P:
                        eyeXHost.LaunchProfileCreation();
                        break;
                    default:
                        Console.WriteLine("Please enter a valid input.");
                        break;
                }*/

                // listen to state-changed events.
                eyeXHost.ScreenBoundsChanged += (s, e) => Console.WriteLine($"Screen Bounds in pixels (state-changed event): {e}");
                eyeXHost.DisplaySizeChanged += (s, e) => Console.WriteLine($"Display Size in millimeters (state-changed event): {e}");
                eyeXHost.EyeTrackingDeviceStatusChanged += (s, e) => Console.WriteLine($"Eye tracking device status (state-changed event): {e}");
                eyeXHost.UserPresenceChanged += (s, e) => Console.WriteLine($"User presence (state-changed event): {e}");
                eyeXHost.UserProfileNameChanged += (s, e) => Console.WriteLine($"Active profile name (state-changed event): {e}");
                eyeXHost.ConfigurationStatusChanged += (s, e) => Console.WriteLine($"Configuration status is : {e}");

                // this state-changed event required EyeX Engine 1.4.
                eyeXHost.UserProfilesChanged += (s, e) => Console.WriteLine($"User profile names (state-changed event): {e}");
                eyeXHost.GazeTrackingChanged += (s, e) => Console.WriteLine($"Gaze tracking (state-changed event): {e}");

                eyeXHost.WaitUntilConnected(TimeSpan.FromSeconds(5));

                // first, let's display the current engine state. Because we're still in the 
                // process of connecting to the engine, we might not get any valid state 
                // information at this point.
                Console.WriteLine($"Screen Bounds in pixels (initial value): {eyeXHost.ScreenBounds}");
                Console.WriteLine($"Display Size in millimeters (initial value): {eyeXHost.DisplaySize}");
                Console.WriteLine($"Eye tracking device status (initial value): {eyeXHost.EyeTrackingDeviceStatus}");
                Console.WriteLine($"User presence (initial value): {eyeXHost.UserPresence}");
                Console.WriteLine($"Active profile name (initial value): {eyeXHost.UserProfileName}");
                Console.WriteLine($"Configuration status (initial value): {eyeXHost.ConfigurationStatus}");

                // this state requires EyeX Engine 1.4.
                Console.WriteLine($"User profile names (initial value): {eyeXHost.UserProfiles}");
                Console.WriteLine($"Gaze tracking (initial value): {eyeXHost.GazeTracking}");

                // tracking and saving the coordinates
                using (var lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered))
                {
                    lightlyFilteredGazeDataStream.Next += (s , e) => 
                    {
                        try
                        {
                            using(System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../coordata.csv" , true))
                            {
                                // data in csv format
                                file.WriteLine($"{e.X:0.00} , {e.Y:0.00} , {e.Timestamp:0.00}");
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error while writing data !! " , ex);
                        }

                        // Console.WriteLine($"Gaze point at ({e.X:0.00}, {e.Y:0.00}) @{e.Timestamp:0.00}");
                    };

                    // let it run until a key is pressed.
                    Console.WriteLine("Listening for gaze data, press any key to exit...");
                    Console.In.Read();
                }

                // wait for the user to exit the application.
                Console.WriteLine("Listening for state changes, press any key to exit...");
                Console.ReadKey(true);
            }
        }
    }
}
