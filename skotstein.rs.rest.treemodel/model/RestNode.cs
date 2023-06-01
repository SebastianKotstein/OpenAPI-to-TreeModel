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
using skotstein.rs.rest.treemodel.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using skotstein.rs.rest.treemodel.extensions;

namespace skotstein.rs.rest.treemodel.model
{

    public abstract class RestNode<T> : GenericNode<T>
    {

    }

    public enum NodeType : int
    {
        [StringEnum("undefined")]
        Undefined = 0,

        [StringEnum("invalid")]
        Invalid = 1,

        [StringEnum("method")]
        Method = 2,

        [StringEnum("path")]
        Path = 3,

        [StringEnum("api")]
        ApiRoot = 4,

        [StringEnum("response")]
        Response = 5,

        [StringEnum("payload")]
        Payload = 6,

        [StringEnum("property")]
        Property = 7,

        [StringEnum("pathSegment")]
        PathSegment = 8
    }
}
