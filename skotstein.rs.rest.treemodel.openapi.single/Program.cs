// MIT License
//Copyright (c) 2023 Sebastian Kotstein
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.Diagnostics;

namespace skotstein.rs.rest.treemodel.openapi.single
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Arguments - application requires input and output path");
                Console.ReadKey();
            }
            else
            {
                string inputPath = args[0];
                string outputPath = args[1];

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Restart();
                OpenApiConverter.Create().LoadFromFile(inputPath).Convert("Test", "0", "Test-Version", "0").ToJsonFile(outputPath);

                stopwatch.Stop();
                Console.WriteLine("Completed in: " + stopwatch.ElapsedMilliseconds + " [ms]");
                Console.ReadKey();

            }
        }
    }
}
