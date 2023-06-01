﻿// MIT License
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
using Microsoft.OpenApi.Models;
using skotstein.rs.rest.treemodel.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.openapi
{
    public class MethodGenerator
    {
        private ResponseGenerator _responseGenerator = new ResponseGenerator();

        public MethodNode Generate(OperationType method, OpenApiOperation openApiOperation)
        {
            MethodNode methodNode = new MethodNode();
            methodNode.Key = OperationTypeToString(method);
            if(methodNode.Key == null)
            {
                //abort
                methodNode.Type = NodeType.Invalid;
                return methodNode;
            }
            methodNode.Value = null;
            methodNode.Summary = openApiOperation.Summary;
            methodNode.Description = openApiOperation.Description;

            if(openApiOperation.Responses != null)
            {
                foreach (KeyValuePair<string, OpenApiResponse> openApiResponsesItem in openApiOperation.Responses)
                {
                    methodNode.AppendChildren(_responseGenerator.Generate(openApiResponsesItem.Key, openApiResponsesItem.Value), omitInvalidNodes: true);
                }
            }
            return methodNode;
        }

        private string OperationTypeToString(OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Get:
                    return "get";
                case OperationType.Head:
                    return "head";
                case OperationType.Options:
                    return "options";
                case OperationType.Patch:
                    return "patch";
                case OperationType.Post:
                    return "post";
                case OperationType.Put:
                    return "put";
                case OperationType.Delete:
                    return "delete";
                case OperationType.Trace:
                    return "trace";
                default:
                    return null;
            }
        }
    }
}