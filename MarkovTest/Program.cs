using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarkovTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MarkovDictionary md = new MarkovDictionary();

            string testString = "This is a test. What is this? A test! This is a very important test because it is for testing my ";
            testString += "Markov chain generator. This sort of text generator is often used by online review bots to generate human ";
            testString += "looking walls of text that look legitimate at first glance. The problem with Markov chain text is that while ";
            testString += "frequency and some word patterns are preserved, actual content is completely lost. Reading a paragraph by ";
            testString += "a Markov chain generator is enough to make you think you are having a stroke. You may struggle for a while, ";
            testString += "trying to find meaning in the words, and become very disoriented. ";
            testString += " ";

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    testString = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            md.BuildDictionary(testString);

            Random RNG = new Random();
            while (true)
            {
                Console.WriteLine("\n==============GENERATING===============");
                Console.WriteLine("\n" + md.GenerateSentence(RNG.Next()));
                //Console.ReadLine();
                Thread.Sleep(5000);
            }
        }
    }
}