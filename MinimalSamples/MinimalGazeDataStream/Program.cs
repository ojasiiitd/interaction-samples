//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using EyeXFramework;
using System;
using Tobii.EyeX.Framework;

namespace MinimalGazeDataStream
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../../coordata.csv" , false))
            {
                file.WriteLine("X , Y , UTC");
            }

            using (var eyeXHost = new EyeXHost())
            {
                // Create a data stream: lightly filtered gaze point data.
                // Other choices of data streams include EyePositionDataStream and FixationDataStream.
                using (var lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered))
                {
                    // Start the EyeX host.
                    eyeXHost.Start();

                    // Write the data to the console.
                    lightlyFilteredGazeDataStream.Next += (s , e) => 
                    {
                        try
                        {
                            using(System.IO.StreamWriter file = new System.IO.StreamWriter(@"../../../coordata.csv" , true))
                            {
                                file.WriteLine($"{e.X:0.00} , {e.Y:0.00} , {e.Timestamp:0.00}");
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error while writing data: " , ex);
                        }

                        //Console.WriteLine($"Gaze point at ({e.X:0.00}, {e.Y:0.00}) @{e.Timestamp:0.00}");
                    };

                    // Let it run until a key is pressed.
                    Console.WriteLine("Listening for gaze data, press any key to exit...");
                    Console.In.Read();
                }
            }
        }
    }
}
