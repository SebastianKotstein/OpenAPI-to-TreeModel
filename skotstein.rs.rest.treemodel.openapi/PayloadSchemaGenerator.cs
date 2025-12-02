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
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using skotstein.rs.rest.treemodel.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static skotstein.rs.rest.treemodel.extensions.StringCompare;

namespace skotstein.rs.rest.treemodel.openapi
{
    public class PayloadSchemaGenerator
    {
        private ExampleExtractor exampleExtractor = new ExampleExtractor();

        public PropertyNode<object> Generate(OpenApiSchema openApiSchema,IDictionary<string, IList<IOpenApiPrimitive>> examples)
        {
            PropertyNode<object> propertyNode = new PropertyNode<object>();
            if (openApiSchema == null)
            {
                //abort
                propertyNode.Type = NodeType.Invalid;
                return propertyNode;
            }
            propertyNode.Key = "$";
            propertyNode.Name = "$";
            propertyNode.Xpath = "$";
            propertyNode.DataType = null;
            propertyNode.Description = openApiSchema.Description;
            propertyNode.Format = null;
            propertyNode.Pattern = null;
            
            
            

            AnalyzeSchema(openApiSchema, propertyNode, propertyNode.Xpath, new HashSet<OpenApiSchema>(), examples);
            return propertyNode;
        }

        private void AnalyzeSchema(OpenApiSchema schema, AbstractNode parent, string xPath, ISet<OpenApiSchema> alreadyAnalyzedSchemas, IDictionary<string, IList<IOpenApiPrimitive>> examples)
        {
            //Create a copy of the Set of already analyzed schemas. Thus, only current path within the structure is memorized:
            ISet<OpenApiSchema> alreadyAnalyzedSchemasCopy = new HashSet<OpenApiSchema>(alreadyAnalyzedSchemas);
            if (alreadyAnalyzedSchemasCopy.Contains(schema) || schema == null)
            {
                return;
            }
            alreadyAnalyzedSchemasCopy.Add(schema);

            if (schema.Example != null)
            {
                this.exampleExtractor.ExtractExamples(schema.Example, xPath, examples);
            }

            //analyze items
            if (schema.Items != null)
            {
                AnalyzeSchema(schema.Items, parent, xPath, alreadyAnalyzedSchemasCopy,examples);
            }

            //analyze linked schemas type of 'AllOf'
            if (schema.AllOf != null)
            {
                foreach (OpenApiSchema linkedSchema in schema.AllOf)
                {
                    AnalyzeSchema(linkedSchema, parent, xPath, alreadyAnalyzedSchemasCopy, examples);
                }
            }

            //analyze linked schemas type of 'AnyOf'
            if (schema.AnyOf != null)
            {
                foreach (OpenApiSchema linkedSchema in schema.AnyOf)
                {
                    AnalyzeSchema(linkedSchema, parent, xPath, alreadyAnalyzedSchemasCopy, examples);
                }
            }

            //analyze linked schemas type of 'OneOf'
            if (schema.OneOf != null)
            {
                foreach (OpenApiSchema linkedSchema in schema.OneOf)
                {
                    AnalyzeSchema(linkedSchema, parent, xPath, alreadyAnalyzedSchemasCopy, examples);
                }
            }

            if (schema.Properties != null)
            {
                foreach (KeyValuePair<string, OpenApiSchema> prop in schema.Properties)
                {
                    OpenApiSchema property = prop.Value;
                    
                 
                    if (property != null)
                    {
                        if (!String.IsNullOrWhiteSpace(property.Type) && property.Type.CompareTo("string") == 0)
                        {
                            PropertyNode<string> stringNode = new PropertyNode<string>();
                            stringNode.Key = prop.Key + "<str>";
                            stringNode.Value = null;
                            stringNode.Name = prop.Key;
                            stringNode.DataType = "string";
                            stringNode.Description = property.Description;
                            stringNode.Format = property.Format;
                            stringNode.Pattern = property.Pattern;
                            stringNode.Xpath = xPath + "." + prop.Key;
                            

                            if (property.Example != null)
                            {
                                this.exampleExtractor.ExtractExamples(property.Example, stringNode.Xpath, examples);
                            }
                            stringNode.Examples = this.exampleExtractor.GenerateExamples(stringNode.Xpath, examples);

                            parent.AppendChildren(stringNode, omitInvalidNodes: true);
                        }
                        else if (!String.IsNullOrWhiteSpace(property.Type) && property.Type.CompareTo("number") == 0)
                        {
                            PropertyNode<double> numberNode = new PropertyNode<double>();
                            numberNode.Key = prop.Key + "<num>";
                            numberNode.Value = 0.0;
                            numberNode.Name = prop.Key;
                            numberNode.DataType = "number";
                            numberNode.Description = property.Description;
                            numberNode.Format = property.Format;
                            numberNode.Pattern = property.Pattern;
                            numberNode.Xpath = xPath + "." + prop.Key;

                            if (property.Example != null)
                            {
                                this.exampleExtractor.ExtractExamples(property.Example, numberNode.Xpath, examples);
                            }
                            numberNode.Examples = this.exampleExtractor.GenerateExamples(numberNode.Xpath, examples);

                            parent.AppendChildren(numberNode, omitInvalidNodes: true);
                        }
                        else if (!String.IsNullOrWhiteSpace(property.Type) && property.Type.CompareTo("integer") == 0)
                        {
                            PropertyNode<long> integerNode = new PropertyNode<long>();
                            integerNode.Key = prop.Key + "<int>";
                            integerNode.Value = 0l;
                            integerNode.Name = prop.Key;
                            integerNode.DataType = "integer";
                            integerNode.Description = property.Description;
                            integerNode.Format = property.Format;
                            integerNode.Pattern = property.Pattern;
                            integerNode.Xpath = xPath + "." + prop.Key;

                            if (property.Example != null)
                            {
                                this.exampleExtractor.ExtractExamples(property.Example, integerNode.Xpath, examples);
                            }
                            integerNode.Examples = this.exampleExtractor.GenerateExamples(integerNode.Xpath, examples);

                            parent.AppendChildren(integerNode, omitInvalidNodes: true);
                        }
                        else if (!String.IsNullOrWhiteSpace(property.Type) && property.Type.CompareTo("boolean") == 0)
                        {
                            PropertyNode<bool> booleanNode = new PropertyNode<bool>();
                            booleanNode.Key = prop.Key + "<bool>";
                            booleanNode.Value = false;
                            booleanNode.Name = prop.Key;
                            booleanNode.DataType = "boolean";
                            booleanNode.Description = property.Description;
                            booleanNode.Format = property.Format;
                            booleanNode.Pattern = property.Pattern;
                            booleanNode.Xpath = xPath + "." + prop.Key;

                            if (property.Example != null)
                            {
                                this.exampleExtractor.ExtractExamples(property.Example, booleanNode.Xpath, examples);
                            }
                            booleanNode.Examples = this.exampleExtractor.GenerateExamples(booleanNode.Xpath, examples);

                            parent.AppendChildren(booleanNode, omitInvalidNodes: true);
                        }
                        else if(!String.IsNullOrWhiteSpace(property.Type) && property.Type.CompareTo("array")==0)
                        {
                            PropertyNode<IList<object>> arrayNode = new PropertyNode<IList<object>>();
                            arrayNode.Key = prop.Key + "[*]";
                            arrayNode.Value = null;
                            arrayNode.Name = prop.Key;
                            arrayNode.DataType = "array";
                            arrayNode.Xpath = xPath + "." + prop.Key + "[*]";
                            arrayNode.Description = property.Description;
                            arrayNode.Format = property.Format;
                            arrayNode.Pattern = property.Pattern;

                            AnalyzeSchema(property, arrayNode, arrayNode.Xpath, alreadyAnalyzedSchemasCopy, examples);

                            parent.AppendChildren(arrayNode, omitInvalidNodes: true);
                        }
                        else if (!String.IsNullOrWhiteSpace(property.Type) && property.Type.CompareTo("object") == 0)
                        {
                            PropertyNode<object> objectNode = new PropertyNode<object>();
                            objectNode.Key = prop.Key+"{_}";
                            objectNode.Value = null;
                            objectNode.Name = prop.Key;
                            objectNode.DataType = "object";
                            objectNode.Xpath = xPath + "." + prop.Key;
                            objectNode.Description = property.Description;
                            objectNode.Format = property.Format;
                            objectNode.Pattern = property.Pattern;

                            AnalyzeSchema(property, objectNode, objectNode.Xpath, alreadyAnalyzedSchemasCopy, examples);

                            parent.AppendChildren(objectNode, omitInvalidNodes: true);
                        }
                        else
                        {
                            PropertyNode<string> unknownNode = new PropertyNode<string>();
                            unknownNode.Key = prop.Key + "<?>";
                            unknownNode.Value = null;
                            unknownNode.Name = prop.Key;
                            unknownNode.DataType = "unknown";
                            unknownNode.Xpath = xPath + "." + prop.Key;
                            unknownNode.Description = property.Description;
                            unknownNode.Format = property.Format;
                            unknownNode.Pattern = property.Pattern;

                            AnalyzeSchema(property, unknownNode, unknownNode.Xpath, alreadyAnalyzedSchemasCopy, examples);

                            parent.AppendChildren(unknownNode, omitInvalidNodes: true);
                        }  
                    }
                }
            }
        }
    }
}
