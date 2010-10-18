#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

// ReSharper disable UnusedMember.Global
namespace OpenRasta.CodeDom.Compiler
{
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.CSharp;

    using OpenRasta.DI;
    using OpenRasta.Web.Markup;
    using OpenRasta.Web.Markup.Rendering;

    public class ExtensibleCSharpCodeProvider : CSharpCodeProvider
    {
        private static readonly IDictionary<string, string> DefaultCompilerParameters = new Dictionary<string, string>
        {
            { "WarnAsError", "false" }, 
            { "CompilerVersion", "3.5" }
        };

        private static readonly IList<ICodeSnippetModifier> EmptySnippetModifiers;
        private static readonly object SyncRoot = new object();
        private static IEnumerable<ICodeSnippetModifier> snippetModifiers;

        static ExtensibleCSharpCodeProvider()
        {
            EmptySnippetModifiers = new List<ICodeSnippetModifier>().AsReadOnly();
        }

        public ExtensibleCSharpCodeProvider() : base(DefaultCompilerParameters)
        {
        }

        public ExtensibleCSharpCodeProvider(IDictionary<string, string> providerOptions) : base(providerOptions)
        {
        }

        public static IEnumerable<ICodeSnippetModifier> SnippetModifiers
        {
            get
            {
                if (snippetModifiers == null && DependencyManager.IsAvailable)
                {
                    lock (SyncRoot)
                    {
                        if (snippetModifiers == null)
                        {
                            snippetModifiers =
                                DependencyManager.GetService<IDependencyResolver>().ResolveAll<ICodeSnippetModifier>();
                        }
                    }
                }

                return snippetModifiers ?? EmptySnippetModifiers;
            }
        }

        public static string PreProcessObject(object source, object value)
        {
            return
                value == null
                    ? null
                    : SnippetModifiers
                          .Where(m => m.CanProcessObject(source, value))
                          .Select(m => m.ProcessObject(source, value))
                          .DefaultIfEmpty(XhtmlTextWriter.HtmlEncode(value.ToString())).First();
        }

        public override void GenerateCodeFromStatement(
            CodeStatement statement, 
            TextWriter writer, 
            CodeGeneratorOptions options)
        {
            var codeExpressionStatement = statement as CodeExpressionStatement;
            
            if (codeExpressionStatement != null)
            {
                var methodInvokeExpression = codeExpressionStatement.Expression as CodeMethodInvokeExpression;
                
                if (methodInvokeExpression != null)
                {
                    if (methodInvokeExpression.Method.MethodName == "Write"
                        && methodInvokeExpression.Parameters.Count == 1)
                    {
                        var parameter = methodInvokeExpression.Parameters[0] as CodeSnippetExpression;
                
                        if ((parameter != null) && (!string.IsNullOrEmpty(parameter.Value)))
                        {
                            // Appears to be a candidate for rewriting
                            string originalValue = parameter.Value;
                            var processor = SnippetModifiers.OfType<ICodeSnippetTextModifier>()
                                                            .FirstOrDefault(m => m.CanProcessString(originalValue));

                            if (processor != null)
                            {
                                originalValue = processor.ProcessString(originalValue);
                            }

                            parameter.Value = "global::" + GetType().FullName + ".PreProcessObject(this, " + originalValue + ")";
                        }
                    }
                }
            }

            base.GenerateCodeFromStatement(statement, writer, options);
        }
    }
}

// ReSharper restore UnusedMember.Global
#region Full license
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion