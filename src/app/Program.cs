using System;
using System.Threading.Tasks;
using Library;

namespace TeleprompterConsole
{
    public class Program
    {
        private static Teleprompter objLib = new Teleprompter();

        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            RunTeleprompter().Wait();
        }

        /// <summary>
        /// Call Both Methods, Show Teleprompter and Get Input 
        /// </summary>
        /// <returns></returns>
        private static async Task RunTeleprompter()
        {
            var displayTask = ShowTeleprompter(objLib);

            var speedTask = GetInput(objLib);

            await Task.WhenAny(displayTask, speedTask);
        }

        /// <summary>
        /// Show Teleprompter
        /// </summary>
        /// <param name="objLib"></param>
        /// <returns></returns>
        private static async Task ShowTeleprompter(Teleprompter objLib)
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            
            var words = objLib.ReadFrom(String.Concat(currentDirectory, "/../text/quotes.txt"));

            foreach (var word in words)
            {
                Console.Write(word);

                if (!string.IsNullOrWhiteSpace(word))
                {
                    await Task.Delay(objLib.DelayInMilliseconds);
                }
            }
        }

        /// <summary>
        /// Get Input to speed up or down the pace of the show
        /// </summary>
        /// <param name="objLib"></param>
        /// <returns></returns>
        private static async Task GetInput(Teleprompter objLib)
        {
            Action work = () =>
            {
                do 
                {
                    var key = Console.ReadKey(true);

                    if (key.KeyChar == '>')
                    {
                        objLib.UpdateDelay(-10);
                    }
                    else if (key.KeyChar == '<')
                    {
                        objLib.UpdateDelay(10);
                    }

                } while (!objLib.Done);
            };

            await Task.Run(work);
        }

    }
}
