using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.model
{
    public class ParameterNode : RestNode<string>
    {
        private string _name;
        private string _parameterType;
        private string _dataType;
        private string _format;
        private string _pattern;
        private string _description;
        private bool _required;

        public override NodeType DefaultType
        {
            get
            {
                return NodeType.Parameter;
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

        [JsonProperty("parameterType")]
        public string ParameterType
        {
            get
            {
                return _parameterType;
            }
            set
            {
                _parameterType = value;
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

        [JsonProperty("required")]
        public bool Required
        {
            get
            {
                return _required;
            }

            set
            {
                _required = value;
            }
        }
    }
}
