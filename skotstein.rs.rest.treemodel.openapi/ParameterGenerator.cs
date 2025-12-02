using Microsoft.OpenApi.Models;
using skotstein.rs.rest.treemodel.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.rs.rest.treemodel.openapi
{
    public class ParameterGenerator
    {
        public ParameterNode Generate(OpenApiParameter openApiParameter)
        {
            ParameterNode parameterNode = new ParameterNode();
            String type = ParameterTypeToString(openApiParameter);
            if(type == null)
            {
                //abort
                parameterNode.Type = NodeType.Invalid;
                return parameterNode;
            }
            parameterNode.Key = openApiParameter.Name + "<" + type + ">";
            parameterNode.Name = openApiParameter.Name;
            parameterNode.ParameterType = type;
            parameterNode.Description = openApiParameter.Description;
            if(openApiParameter.Schema != null)
            {
                parameterNode.Format = openApiParameter.Schema.Format;
                parameterNode.Pattern = openApiParameter.Schema.Pattern;
                parameterNode.DataType = openApiParameter.Schema.Type;
            }
            else
            {
                parameterNode.Format = null;
                parameterNode.Pattern = null;
                parameterNode.DataType = "unknown";
            }
            parameterNode.Required = openApiParameter.Required;
            return parameterNode;
        }

        public ParameterNode Generate(string name, OpenApiHeader openApiHeader)
        {
            ParameterNode parameterNode = new ParameterNode();
            String type = "header";
            parameterNode.Key = name + "<" + type + ">";
            parameterNode.Name = name;
            parameterNode.ParameterType = type;
            parameterNode.Description = openApiHeader.Description;
            if (openApiHeader.Schema != null)
            {
                parameterNode.Format = openApiHeader.Schema.Format;
                parameterNode.Pattern = openApiHeader.Schema.Pattern;
                parameterNode.DataType = openApiHeader.Schema.Type;
            }
            else
            {
                parameterNode.Format = null;
                parameterNode.Pattern = null;
                parameterNode.DataType = "unknown";
            }
            parameterNode.Required = openApiHeader.Required;
            return parameterNode;
        }

        private string ParameterTypeToString(OpenApiParameter openApiParameter)
        {   
            if(openApiParameter.In == null)
            {
                return null;
            }

            switch (openApiParameter.In)
            {
                case ParameterLocation.Path:
                    return "path";
                case ParameterLocation.Query:
                    return "query";
                case ParameterLocation.Header:
                    return "header";
                case ParameterLocation.Cookie:
                    return "cookies";
                default:
                    return null;
            }
        }
    }
}
