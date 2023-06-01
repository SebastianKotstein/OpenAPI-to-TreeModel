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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.model
{
    public class PropertyNode<T> : RestNode<T>
    {
        private string _name;
        private string _dataType;
        private string _xpath;
        private string _format;
        private string _pattern;
        private string _description;

        public override NodeType DefaultType
        {
            get
            {
                return NodeType.Property;
            }
        }

        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        [JsonProperty("dataType")]
        public string DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        [JsonProperty("xpath")]
        public string Xpath
        {
            get
            {
                return _xpath;
            }

            set
            {
                _xpath = value;
            }
        }

        [JsonProperty("format")]
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        [JsonProperty("pattern")]
        public string Pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
    }
}
