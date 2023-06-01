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
using Microsoft.OpenApi.Readers;
using Newtonsoft.Json;
using skotstein.rs.rest.treemodel.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.openapi
{
    public class OpenApiConverter
    {
        private OpenApiDocument _openApiDocument = null;
        private OpenApiDiagnostic _openApiDiagnostic = null;
        private Diagnosis _diagnosis = null;

        private ApiRoot _apiRoot;

        public static OpenApiConverter Create()
        {
            return new OpenApiConverter();
        }

        public OpenApiConverter Load(string openApiContent)
        {
            _openApiDiagnostic = new OpenApiDiagnostic();
            _openApiDocument = new OpenApiStringReader().Read(openApiContent, out _openApiDiagnostic);
            return this;
        }

        public OpenApiConverter LoadFromFile(string path)
        {
            _openApiDiagnostic = new OpenApiDiagnostic();
            _openApiDocument = new OpenApiStringReader().Read(File.ReadAllText(path), out _openApiDiagnostic);
            return this;
        }

        public OpenApiConverter Convert(string apiName, string apiKey, string versionName, string versionKey)
        {
            if(_openApiDocument == null)
            {
                throw new Exception("Precondition not satisfied. OpenAPI document must be loaded first.");
            }

            _diagnosis = new Diagnosis();

            ApiGenerator apiGenerator = new ApiGenerator();
            _apiRoot = apiGenerator.Generate(_openApiDocument, _diagnosis, apiName, apiKey, versionName, versionKey);
            return this;
        }

        public string ToJson()
        {
            if(_apiRoot == null)
            {
                throw new Exception("Precondition not satisfied. OpenAPI document must be converted first.");
            }
            using(StringWriter stringWriter = new StringWriter())
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(stringWriter, _apiRoot);
                return stringWriter.ToString();
            }
        }

        public void ToJsonFile(string path)
        {
            if (_apiRoot == null)
            {
                throw new Exception("Precondition not satisfied. OpenAPI document must be converted first.");
            }
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _apiRoot);
            }
        }

        public Diagnosis Diagnosis
        {
            get
            {
                return _diagnosis;
            }
        }

        public OpenApiDiagnostic Diagnostic
        {
            get
            {
                return _openApiDiagnostic;
            }
        }
    }
}
