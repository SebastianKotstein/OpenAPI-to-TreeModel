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
    public class ResponseGenerator
    {
        private PayloadGenerator _payloadGenerator = new PayloadGenerator();

        public ResponseNode Generate(string responseCode, OpenApiResponse openApiResponse)
        {
            ResponseNode responseNode = new ResponseNode();

            int responseCodeInt = 0;
            if(!Int32.TryParse(responseCode,out responseCodeInt) || responseNode == null)
            {
                //abort
                responseNode.Type = NodeType.Invalid;
                return responseNode;
            }

            responseNode.Key = responseCode;
            responseNode.Value = responseCodeInt;
            responseNode.Description = openApiResponse.Description;
            
            if(openApiResponse.Content != null)
            {
                foreach(KeyValuePair<string,OpenApiMediaType> openApiMediaTypeItem in openApiResponse.Content)
                {
                    responseNode.AppendChildren(_payloadGenerator.Generate(openApiMediaTypeItem.Key, openApiMediaTypeItem.Value),omitInvalidNodes:true);
                }
            }
            return responseNode;
        }
    }
}
