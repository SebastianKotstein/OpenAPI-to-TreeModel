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

namespace skotstein.rs.rest.treemodel.openapi
{
    public class PayloadGenerator
    {
        private PayloadSchemaGenerator _payloadSchemaGenerator = new PayloadSchemaGenerator();
        private ExampleExtractor exampleExtractor = new ExampleExtractor();

        public PayloadNode Generate(string mediaType,OpenApiMediaType openApiMediaType)
        {
            PayloadNode payloadNode = new PayloadNode();

            if(String.IsNullOrWhiteSpace(mediaType) || openApiMediaType == null)
            {
                //abort
                payloadNode.Type = NodeType.Invalid;
                return payloadNode;
            }
            payloadNode.Key = mediaType;
            payloadNode.Value = mediaType;

            IDictionary<string, IList<IOpenApiPrimitive>> examples = this.extractExamples(openApiMediaType);

            payloadNode.AppendChildren(_payloadSchemaGenerator.Generate(openApiMediaType.Schema,examples));

            return payloadNode;
        }

        private IDictionary<string, IList<IOpenApiPrimitive>> extractExamples(OpenApiMediaType mediaType)
        {
            IDictionary<string, IList<IOpenApiPrimitive>> examples = new Dictionary<string, IList<IOpenApiPrimitive>>();
            if(mediaType.Example != null)
            {
                this.exampleExtractor.ExtractExamples(mediaType.Example, "$", examples);
            }
            if(mediaType.Examples != null)
            {
                foreach(OpenApiExample item in mediaType.Examples.Values)
                {
                    this.exampleExtractor.ExtractExamples(item.Value, "$", examples);
                }
            }

            return examples;
        }

        
    }
}
