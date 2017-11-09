﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers.Interface;
using Microsoft.OpenApi.Readers.ParseNodes;

namespace Microsoft.OpenApi.Readers.V3
{
    /// <summary>
    /// Class containing logic to deserialize Open API V3 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class OpenApiV3Deserializer
    {
        public static OpenApiSecurityRequirement LoadSecurityRequirement(ParseNode node)
        {
            var mapNode = node.CheckMapNode("security");

            var obj = new OpenApiSecurityRequirement();

            foreach (var property in mapNode)
            {
                var scheme = LoadSecuritySchemeByReference(
                    mapNode.Context,
                    mapNode.Diagnostic,
                    property.Name);

                obj.Schemes.Add(scheme, property.Value.CreateSimpleList(n2 => n2.GetScalarValue()));
            }

            return obj;
        }

        private static OpenApiSecurityScheme LoadSecuritySchemeByReference(
            ParsingContext context,
            OpenApiDiagnostic diagnostic,
            string schemeName)
        {
            var securitySchemeObject = (OpenApiSecurityScheme)context.GetReferencedObject(
                diagnostic,
                new OpenApiReference(ReferenceType.SecurityScheme, schemeName));

            return securitySchemeObject;
        }
    }
}