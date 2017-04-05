using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TexTastic
{
    class Program
    {
        static void Main(string[] args)
        {
            PoopyGameServer game = new PoopyGameServer();
            
            Console.WriteLine("Server started.");

            float limitFrameTime = 1000f / 60f;
            do
            {
                //string line = Console.ReadLine();
                Stopwatch FPSTimer = Stopwatch.StartNew();

                //if(!string.IsNullOrEmpty(line))
                //{
                //    game.SendMessage(line);
                //}

                while (true)
                {
                    //Start of Tick
                    Stopwatch SW = Stopwatch.StartNew();

                    //The Actual Tick
                    game.UpdateGame((float)(limitFrameTime - SW.Elapsed.TotalMilliseconds) * .001f);

                    //End of Tick
                    SW.Stop();
                    if (SW.Elapsed.TotalMilliseconds < limitFrameTime)
                    {
                        Thread.Sleep(Convert.ToInt32(limitFrameTime - SW.Elapsed.TotalMilliseconds));
                    }
                    else
                    {
                        Thread.Yield();
                    }
                }
            } while (true);
        }
    }
}
