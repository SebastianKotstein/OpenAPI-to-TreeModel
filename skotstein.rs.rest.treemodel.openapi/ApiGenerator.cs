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
    public class ApiGenerator
    {

        private PathGenerator _pathGenerator = new PathGenerator();
        private PathSegmentGenerator _pathSegmentGenerator = new PathSegmentGenerator();

        public ApiRoot Generate(OpenApiDocument openApiDocument, Diagnosis diagnosis, string apiName, string apiKey, string versionName, string versionKey, bool simplePath = false)
        {
            ApiRoot apiRoot = new ApiRoot();
            apiRoot.Key = apiName + "-" + versionName;
            apiRoot.Value = null;
            apiRoot.ApiKey = apiKey;
            apiRoot.ApiName = apiName;
            apiRoot.VersionKey = versionKey;
            apiRoot.VersionName = versionName;

            PathSegmentNode rootPathSegment = null;
            if (!simplePath)
            {
                rootPathSegment = new PathSegmentNode();
                rootPathSegment.Key = "/";
                apiRoot.AppendChildren(rootPathSegment);
            }
            if(openApiDocument.Paths != null)
            {
                foreach (KeyValuePair<string, OpenApiPathItem> openApiPathItem in openApiDocument.Paths)
                {
                    if (simplePath)
                    {
                        apiRoot.AppendChildren(_pathGenerator.Generate(openApiPathItem.Key, openApiPathItem.Value), omitInvalidNodes: true);
                    }
                    else
                    {
                        _pathSegmentGenerator.Generate(rootPathSegment, openApiPathItem.Key, openApiPathItem.Value);
                    }

                }
            }

            return apiRoot;
        }
    }
}
