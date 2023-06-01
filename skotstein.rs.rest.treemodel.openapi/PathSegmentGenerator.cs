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
using Microsoft.OpenApi.Models;
using skotstein.rs.rest.treemodel.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.openapi
{
    public class PathSegmentGenerator
    {
        private MethodGenerator _methodGenerator = new MethodGenerator();

        public void Generate(PathSegmentNode rootSegment, string path, OpenApiPathItem openApipathItem)
        {
            //special case: URI path is '/' (root element)
            if (path.CompareTo("/") == 0)
            {
                AddOperationToPathSegment(openApipathItem, rootSegment);
                return;
            }

            //split path into segments
            string[] segments = path.Split('/');

            //start at root
            PathSegmentNode currentSegment = rootSegment;

            //iterate over all segments (skip whitespaces and empty segments, especially the first segment, which is always empty since URI paths start with a leading slash '/'
            for(int i = 0; i < segments.Length; i++)
            {
                string segment = segments[i];
                if (!String.IsNullOrWhiteSpace(segment))
                {
                    PathSegmentNode pathSegment = currentSegment.GetPathSegment(segment);

                    //if such a path segment exists, ...
                    if(pathSegment != null)
                    {
                        //set context to this child segment
                        currentSegment = pathSegment;
                    }
                    else
                    {
                        //if such a path segment does not exist yet, create a new path segment:
                        PathSegmentNode newPathSegment = new PathSegmentNode();
                        newPathSegment.Key = "/"+segment;

                        //TODO: add documented path parameter to segment
                        if(segment.Contains("{") || segment.Contains("}"))
                        {
                            newPathSegment.ContainsParameter = true;
                        }

                        currentSegment.AppendChildren(newPathSegment);
                        currentSegment = newPathSegment;
                    }
                }
            }
            AddOperationToPathSegment(openApipathItem, currentSegment);
        }

        private void AddOperationToPathSegment(OpenApiPathItem openApiPathItem, PathSegmentNode pathSegment)
        {
            foreach (KeyValuePair<OperationType, OpenApiOperation> openApiOperation in openApiPathItem.Operations)
            {
                pathSegment.AppendChildren(_methodGenerator.Generate(openApiOperation.Key, openApiOperation.Value), omitInvalidNodes: true);
            }
        }
    }
}
