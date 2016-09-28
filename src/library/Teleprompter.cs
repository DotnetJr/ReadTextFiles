using System;
using System.Collections.Generic;
using static System.Math;
using System.IO;

namespace Library
{
    public class Teleprompter
    {
        public bool Done => done;
        public int DelayInMilliseconds { get; private set; } = 200;
        private bool done;
        private object lockHandle = new object();

        /// <summary>
        /// Read Lines From File
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IEnumerable<string> ReadFrom(string file)
        {
            string line;

            using (var reader = File.OpenText(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var words = line.Split(' ');

                    var lineLength = 0;

                    foreach (var word in words)
                    {
                        yield return word + " ";
                        
                        lineLength += word.Length + 1;
                        
                        if (lineLength > 70)
                        {
                            yield return Environment.NewLine;
                            lineLength = 0;
                        }
                    }

                    yield return Environment.NewLine;
                }
            }
        }

        /// <summary>
        /// Update Delay
        /// </summary>
        /// <param name="increment"></param>
        public void UpdateDelay(int increment)
        {
            var newDelay = Min(DelayInMilliseconds + increment, 1000);

            newDelay = Max(newDelay, 20);

            lock (lockHandle)
            {
                DelayInMilliseconds = newDelay;
            }
        }

        /// <summary>
        /// Set Task as Done 
        /// </summary>
        public void SetDone()
        {
            done = true;    
        }

    }
}
