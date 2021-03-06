﻿// ReSharper disable UnusedMember.Global

namespace OpenRasta.CodeDom.Compiler
{
    #region Using Directives

    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.CSharp;

    using OpenRasta.Contracts.CodeDom.Compiler;
    using OpenRasta.Contracts.DI;
    using OpenRasta.DI;
    using OpenRasta.Web.Markup.Rendering;

    #endregion

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
                            snippetModifiers = DependencyManager.GetService<IDependencyResolver>()
                                                                .ResolveAll<ICodeSnippetModifier>();
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