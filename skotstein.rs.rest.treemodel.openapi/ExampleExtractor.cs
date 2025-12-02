using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.openapi
{
    public class ExampleExtractor
    {
        public void ExtractExamples(IOpenApiAny anyElement, string xPath, IDictionary<string, IList<IOpenApiPrimitive>> extractedExamples)
        {
            if (anyElement == null)
            {
                return;
            }
            if (anyElement is OpenApiString
                || anyElement is OpenApiBinary
                || anyElement is OpenApiBoolean
                || anyElement is OpenApiByte
                || anyElement is OpenApiDate
                || anyElement is OpenApiDateTime
                || anyElement is OpenApiDouble
                || anyElement is OpenApiFloat
                || anyElement is OpenApiInteger
                || anyElement is OpenApiLong)
            {
                if (!extractedExamples.ContainsKey(xPath))
                {
                    extractedExamples.Add(xPath, new List<IOpenApiPrimitive>());
                }
                extractedExamples[xPath].Add((IOpenApiPrimitive)anyElement);
            }
            else if (anyElement is OpenApiObject)
            {
                OpenApiObject obj = (OpenApiObject)anyElement;
                foreach (KeyValuePair<string, IOpenApiAny> prop in obj)
                {
                    this.ExtractExamples(prop.Value, xPath + "." + prop.Key, extractedExamples);
                }
            }
            else if (anyElement is OpenApiArray)
            {
                OpenApiArray array = (OpenApiArray)anyElement;
                if (!xPath.EndsWith("[*]"))
                {
                    xPath = xPath + "[*]";
                }
                foreach (IOpenApiAny item in array)
                {
                    this.ExtractExamples(item, xPath, extractedExamples);
                }
            }
        }

        public IList<string> GenerateExamples(string xpath, IDictionary<string, IList<IOpenApiPrimitive>> extractedExamples)
        {
            ISet<string> examples = new HashSet<string>();
            if (extractedExamples.ContainsKey(xpath))
            {
                foreach (IOpenApiPrimitive item in extractedExamples[xpath])
                {
                    if (item is OpenApiString)
                    {
                        string value = ((OpenApiString)item).Value;
                        if (!this.CompareTo(value.ToLower(),new string[]{"string","str","int","integer","boolean","bool","date","float","double","long","num","numeric","uuid","id"})){
                            examples.Add(value);
                        }
                    }
                    if (item is OpenApiBinary)
                    {
                        examples.Add("" + ((OpenApiBinary)item).Value);
                    }
                    if (item is OpenApiBoolean)
                    {
                        examples.Add("" + ((OpenApiBoolean)item).Value);
                    }
                    if (item is OpenApiByte)
                    {
                        examples.Add("" + ((OpenApiByte)item).Value);
                    }
                    if (item is OpenApiDate)
                    {
                        examples.Add("" + ((OpenApiDate)item).Value);
                    }
                    if (item is OpenApiDateTime)
                    {
                        examples.Add("" + ((OpenApiDateTime)item).Value);
                    }
                    if (item is OpenApiDouble)
                    {
                        examples.Add("" + ((OpenApiDouble)item).Value);
                    }
                    if (item is OpenApiFloat)
                    {
                        examples.Add("" + ((OpenApiFloat)item).Value);
                    }
                    if (item is OpenApiInteger)
                    {
                        examples.Add("" + ((OpenApiInteger)item).Value);
                    }
                    if (item is OpenApiLong)
                    {
                        examples.Add("" + ((OpenApiLong)item).Value);
                    }
                }

                IList<String> examplesAsList = examples.ToList();
                string line = null;
                foreach(string item in examplesAsList)
                {
                    if(line == null)
                    {
                        line = item;
                    }
                    else
                    {
                        line = line +", " + item;
                    }
                }
                if(examplesAsList.Count > 0)
                {
                    return examplesAsList;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method returns true, if the passed string s matches at least one string of the passed array 'values'.
        /// The method uses <see cref="string.CompareTo(string)"/> for comparison.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool CompareTo(string s, string[] values)
        {
            foreach (string v in values)
            {
                if (s.CompareTo(v) == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
