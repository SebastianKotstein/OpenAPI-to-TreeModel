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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace skotstein.rs.rest.treemodel.model
{
    public class ApiRoot : RestNode<String>
    {

        private String _apiName;
        private String _apiKey;
        private String _versionName;
        private String _versionKey;

        public override NodeType DefaultType
        {
            get
            {
                return NodeType.ApiRoot;
            }
        }

        [JsonProperty("apiKey")]
        public string ApiKey
        {
            get
            {
                return _apiKey;
            }

            set
            {
                _apiKey = value;
            }
        }


        [JsonProperty("apiName")]
        public string ApiName
        {
            get
            {
                return _apiName;
            }

            set
            {
                _apiName = value;
            }
        }

        [JsonProperty("versionKey")]
        public string VersionKey
        {
            get
            {
                return _versionKey;
            }

            set
            {
                _versionKey = value;
            }
        }

        [JsonProperty("versionName")]
        public string VersionName
        {
            get
            {
                return _versionName;
            }

            set
            {
                _versionName = value;
            }
        }

       
    }
}
