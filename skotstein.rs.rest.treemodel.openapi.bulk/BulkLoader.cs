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
using skotstein.research.rest.apiguru.loader.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.openapi.bulk
{
    public class BulkLoader
    {
        /// <summary>
        /// Loads all OpenAPI documentations from the passed directory (args[0]), analyzes them and writes the output (one API per file) into an "out" sub-directory of the passed directory. 
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void execute(string[] args)
        {
            string projectPath = null;
            bool preferredVersionOnly = false;
            string outputPath = null;
            string logFile = null;
            IList<string> excludedApiKeys = new List<string>();
            string startDate = DateTime.Now.ToString(String.Format("yyyy-MM-dd_HH.mm.ss.FFF"));

            IDictionary<String, int> exceptionCounter = new Dictionary<String, int>();


            //read input path (at least this Argument is required)
            if (args.Length < 1)
            {
                Console.WriteLine("Invalid Arguments - application requires at least one argument containing the project's input path");
                Console.ReadKey();
                return;
            }
            else
            {
                projectPath = args[0];
            }

            //read optional output path
            if (args.Length < 2)
            {
                outputPath = projectPath + @"\" + startDate.Replace(".", "-");
            }
            else
            {
                outputPath = args[1];
            }
            logFile = outputPath+@"\" + startDate.Replace(".", "-")+".log";

            //read optional preferred version flag
            if (args.Length < 3)
            {
                preferredVersionOnly = false;
            }
            else
            {
                preferredVersionOnly = args[2].CompareTo("preferred") == 0 ? true : false;
            }

            //read optional excluded list
            if (args.Length >= 4)
            {
                foreach (string key in args[3].Split(","))
                {
                    excludedApiKeys.Add(key);
                }
            }


            int counter = 0;
            int errorCounter = 0;


            Directory.CreateDirectory(outputPath);

            //read all apis' meta files
            IList<Alias> apis = Alias.ReadList(File.ReadAllText(projectPath + @"\alias.json"));

            Stopwatch stopwatch = new Stopwatch();

            OpenApiConverter openApiConverter = OpenApiConverter.Create();

            foreach (Alias api in apis)
            {
                //load API info
                ApiGuruApi apiInfo = GetApiInfo(api.Key, projectPath);
                if (apiInfo == null)
                {
                    Log(logFile, "API Info for '" + api.Value + "' (" + api.Key + ") does not exist", ConsoleColor.Red);
                    //Console.WriteLine("API Info for '" + api.Value + "' (" + api.Key + ") does not exist", ConsoleColor.Red);
                    
                    continue;
                }

                //check whether API key is on exclusion list
                if (excludedApiKeys.Contains(api.Key))
                {
                    Log(logFile, "API '" + api.Value + "' (" + api.Key + ") skipped", ConsoleColor.Blue);
                    //Console.WriteLine("API '" + api.Value + "' (" + api.Key + ") skipped", ConsoleColor.Blue);
                    continue;
                }

                //load versions
                IList<Alias> versions = GetVersionAlias(api.Key, projectPath);
                if (versions == null)
                {
                    Log(logFile, "API Versions for '" + api.Value + "' (" + api.Key + ") does not exist", ConsoleColor.Red);
                    //Console.WriteLine("API Versions for '" + api.Value + "' (" + api.Key + ") does not exist", ConsoleColor.Red);
                    continue;
                }
                foreach (Alias version in versions)
                {
                    if (!preferredVersionOnly || (preferredVersionOnly && apiInfo.Preferred.CompareTo(version.Value) == 0))
                    {
                        //load version Info
                        ApiGuruApiVersion versionInfo = apiInfo.Versions.Versions[version.Value];

                        stopwatch.Restart();

                        //determine documentation path
                        string rawContent = LoadOpenApiFile(api.Key, version.Key, projectPath);
                        string path = outputPath + @"\" + api.Key + "_" + version.Key + ".json";
                        try
                        {
                            openApiConverter.Load(rawContent).Convert(api.Value,api.Key,version.Value,version.Key).ToJsonFile(path);
                            //Console.ResetColor();
                            Log(logFile, "Tree file created for '" + api.Value + "' version '" + version.Value + "' (" + api.Key + "." + version.Key + "), in " + stopwatch.Elapsed.TotalSeconds + " s",ConsoleColor.White);
                            //Console.WriteLine("Tree file created for '" + api.Value + "' version '" + version.Value + "' (" + api.Key + "." + version.Key + "), in " + stopwatch.Elapsed.TotalSeconds + " s");
                            counter++;
                        }
                        catch (Exception e)
                        {
                            Log(logFile, "Tree file creation for '" + api.Value + "' version '" + version.Value + "' (" + api.Key + "." + version.Key + ") failed due to exception: " + e.Message, ConsoleColor.Red);
                            if (exceptionCounter.ContainsKey(e.Message))
                            {
                                exceptionCounter[e.Message]++;
                            }
                            else
                            {
                                exceptionCounter.Add(e.Message, 1);
                            }
                            errorCounter++;
                        }
                    }
                }
            }
            Log(logFile, "Completed for " + counter + " APIs", ConsoleColor.White);
            Log(logFile, "Failed APIs: " + errorCounter+", with the following expections: ", ConsoleColor.Red);
            foreach(KeyValuePair<string, int> exception in exceptionCounter)
            {
                Log(logFile,exception.Key+" ("+exception.Value+")", ConsoleColor.Red);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Returns the list of <see cref="Alias"/> stored in the API folder.
        /// </summary>
        private static IList<Alias> GetVersionAlias(string apiKey, string projectFolderPath)
        {
            try
            {
                return Alias.ReadList(File.ReadAllText(projectFolderPath + @"\" + apiKey + @"\alias.json"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open " + projectFolderPath + @"\" + apiKey + @"\alias.json, Exception: " + e.Message, ConsoleColor.Red);
                return null;
            }
        }

        private static ApiGuruApi GetApiInfo(string apiKey, string projectFolderPath)
        {
            try
            {
                return ApiGuruApi.LoadFromDisk(projectFolderPath + @"\" + apiKey + @"\info.json");
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open " + projectFolderPath + @"\" + apiKey + @"\info.json, Exception: " + e.Message, ConsoleColor.Red);
                return null;
            }
        }

        /// <summary>
        /// Loads the OpenAPI file (.json is preferred)
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="versionKey"></param>
        /// <returns></returns>
        private static string LoadOpenApiFile(string apiKey, string versionKey, string projectFolderPath)
        {
            if (File.Exists(projectFolderPath + @"\" + apiKey + @"\" + versionKey + @"\swagger.json"))
            {
                return File.ReadAllText(projectFolderPath + @"\" + apiKey + @"\" + versionKey + @"\swagger.json");
            }
            else if (File.Exists(projectFolderPath + @"\" + apiKey + @"\" + versionKey + @"\swagger.yaml"))
            {
                return File.ReadAllText(projectFolderPath + @"\" + apiKey + @"\" + versionKey + @"\swagger.yaml");
            }
            else
            {
                Console.WriteLine("Cannot open OpenAPI Documentation in " + projectFolderPath + @"\" + apiKey + @"\" + versionKey, ConsoleColor.Red);
                return null;
            }
        }

        private static void Log(string path, string msg, ConsoleColor color)
        {
            Console.WriteLine(msg, color);
            File.AppendAllText(path, msg+Environment.NewLine);
        }
    }
}
