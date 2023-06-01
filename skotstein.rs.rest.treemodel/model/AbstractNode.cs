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
using skotstein.rs.rest.treemodel.extensions;

namespace skotstein.rs.rest.treemodel.model
{
    public abstract class AbstractNode
    {
        private string _key;
        private NodeType _type = NodeType.Undefined;

        private AbstractNode _parent;
        private IList<AbstractNode> _childern = new List<AbstractNode>();

        /// <summary>
        /// Gets or sets the key (part of the unique identifier of the node)
        /// Note that dots ('.') will be substituted with '!', since a dot represents an edge in an <see cref="Identifier"/>
        /// </summary>
        [JsonProperty("key")]
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value.Replace(".","!").ToLower();
            }
        }

        [JsonProperty("id")]
        public string Identifier
        {
            get
            {
                string id = Key;
                if(Parent == null)
                {
                    return id;
                }
                else
                {
                    return Parent.Identifier + "." + id;
                }
            }
        }

        [JsonIgnore]
        public NodeType Type
        {
            get
            {
                if(_type == NodeType.Undefined)
                {
                    return DefaultType;
                }
                else
                {
                    return _type;
                }
            }
            set
            {
                _type = value;
            }
        }

        [JsonIgnore]
        public abstract NodeType DefaultType
        {
            get;
        }

        [JsonIgnore]
        public AbstractNode Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        [JsonProperty("elements")]
        public IList<AbstractNode> Children
        {
            get
            {
                return _childern;
            }
        }

        [JsonIgnore]
        public bool IsRoot
        {
            get
            {
                return _parent == null;
            } 
        }

        [JsonProperty("type")]
        public string TypeAsString
        {
            get
            {
                return Type.GetString();
            }
        }

        public IList<AbstractNode> GetChildrens(NodeType type)
        {
            IList<AbstractNode> filteredChildren = new List<AbstractNode>();
            foreach(AbstractNode node in Children)
            {
                if(node.Type == type)
                {
                    filteredChildren.Add(node);
                }
            }
            return filteredChildren;
        }

        public AbstractNode GetChildren(string key)
        {
            foreach(AbstractNode node in Children)
            {
                if (key.CompareTo(node.Key) == 0)
                {
                    return node;
                }
            }
            return null;
        }

        public AbstractNode GetChildren(string key, NodeType type)
        {
            foreach(AbstractNode node in Children)
            {
                if(key.CompareTo(node.Key)==0 && node.Type == type)
                {
                    return node;
                }
            }
            return null;
        }

        public void AppendChildren(AbstractNode node, bool omitInvalidNodes= true)
        {
            if ((omitInvalidNodes && node.Type != NodeType.Invalid) || !omitInvalidNodes)
            {
                node.Parent = this;
                this.Children.Add(node);
            }
        }

    }
}
