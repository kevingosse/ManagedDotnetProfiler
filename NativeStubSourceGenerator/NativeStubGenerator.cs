using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NativeStubSourceGenerator
{
    [Generator]
    public class NativeStubGenerator : ISourceGenerator
    {
        private static bool _start = true;

        public static void Log(string text)
        {
            //const string logFile = @"E:\log\log.txt";

            //if (_start)
            //{
            //    if (File.Exists(logFile))
            //    {
            //        File.Delete(logFile);
            //    }

            //    _start = false;
            //}

            //File.AppendAllText(logFile, text + Environment.NewLine);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
            {
                return;
            }

            foreach (var symbol in receiver.Interfaces)
            {
                EmitStubForInterface(context, symbol);
            }
        }

        public void EmitStubForInterface(GeneratorExecutionContext context, INamedTypeSymbol symbol)
        {
            Log("Emitting stub for interface " + symbol.Name);

            var sourceBuilder = new StringBuilder(@"
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NativeStubs
{
    internal unsafe class {typeName}
    {
        private List<object> _delegates = new();

{delegates}
        private {typeName}({interfaceName} implementation)
        {
            var obj = Marshal.AllocHGlobal(IntPtr.Size);

            var ptr = (IntPtr*)obj;

            const int delegateCount = {delegateCount};

            int vtablePartSize = delegateCount * IntPtr.Size;
            IntPtr* vtable = (IntPtr*)Marshal.AllocHGlobal(vtablePartSize);
            *(void**)obj = vtable;

{functionPointers}

            Object = obj;
        }

        public IntPtr Object { get; private set; }

        public static {typeName} Wrap({interfaceName} implementation) => new(implementation);

        public static implicit operator IntPtr({typeName} stub) => stub.Object;

        public static {interfaceName} Wrap(IntPtr obj) => new {invokerName}(obj);

        public void Dispose()
        {
            var target = (IntPtr*)Object;
            Marshal.FreeHGlobal(*(target));
            Marshal.FreeHGlobal(Object);
            Object = IntPtr.Zero;
        }

        private class {invokerName} : {interfaceName}
        {
            private readonly IntPtr _implementation;
            private readonly nint* _vtable;

            public {invokerName}(IntPtr implementation)
            {
                _implementation = implementation;
                _vtable = (nint*)*(nint*)implementation;
            }

{invokerFunctions}
 
        }
       
    }
}
");

            var interfaceName = symbol.ToString();
            var typeName = $"{symbol.Name}Stub";
            var invokerName = $"{symbol.Name}Invoker";
            int delegateCount = 0;
            var delegates = new StringBuilder();
            var functionPointers = new StringBuilder();
            var invokerFunctions = new StringBuilder();

            var interfaceList = symbol.AllInterfaces.ToList();
            interfaceList.Reverse();
            interfaceList.Add(symbol);

            foreach (var @interface in interfaceList)
            {
                foreach (var member in @interface.GetMembers())
                {
                    if (member is not IMethodSymbol method)
                    {
                        continue;
                    }

                    delegateCount++;

                    var parameterList = new StringBuilder();

                    parameterList.Append("IntPtr self");

                    foreach (var parameter in method.Parameters)
                    {
                        Log($"Parameter: {parameter.OriginalDefinition} {parameter.MetadataName}");

                        parameterList.Append($", {parameter.OriginalDefinition} a{parameter.Ordinal}");
                    }

                    delegates.AppendLine("        [UnmanagedFunctionPointer(CallingConvention.StdCall)]");
                    delegates.Append($"        private delegate {method.ReturnType} {method.Name}({parameterList});");
                    delegates.AppendLine();
                    delegates.AppendLine();

                    var sourceArgsList = new StringBuilder();
                    sourceArgsList.Append("IntPtr _");

                    for (int i = 0; i < method.Parameters.Length; i++)
                    {
                        sourceArgsList.Append($", {method.Parameters[i].OriginalDefinition} a{i}");
                    }

                    var destinationArgsList = new StringBuilder();

                    for (int i = 0; i < method.Parameters.Length; i++)
                    {
                        if (i > 0)
                        {
                            destinationArgsList.Append(", ");
                        }

                        var refKind = method.Parameters[i].RefKind;

                        switch (refKind)
                        {
                            case RefKind.In:
                                destinationArgsList.Append("in ");
                                break;
                            case RefKind.Out:
                                destinationArgsList.Append("out ");
                                break;
                            case RefKind.Ref:
                                destinationArgsList.Append("ref ");
                                break;
                        }

                        destinationArgsList.Append($"a{i}");
                    }

                    functionPointers.AppendLine($"            var d{delegateCount} = new {method.Name}(({sourceArgsList}) => implementation.{method.Name}({destinationArgsList}));");
                    functionPointers.AppendLine($"            _delegates.Add(d{delegateCount});");
                    functionPointers.AppendLine($"            *vtable++ = Marshal.GetFunctionPointerForDelegate(d{delegateCount});");

                    invokerFunctions.Append($"            public {method.ReturnType} {method.Name}(");

                    for (int i = 0; i < method.Parameters.Length; i++)
                    {
                        if (i > 0)
                        {
                            invokerFunctions.Append(", ");
                        }

                        invokerFunctions.Append($"{method.Parameters[i].OriginalDefinition} a{i}");
                    }

                    invokerFunctions.AppendLine(")");
                    invokerFunctions.AppendLine("            {");

                    invokerFunctions.Append("                var func = (delegate* unmanaged[Stdcall]<IntPtr");

                    for (int i = 0; i < method.Parameters.Length; i++)
                    {
                        invokerFunctions.Append(", ");

                        var refKind = method.Parameters[i].RefKind;

                        switch (refKind)
                        {
                            case RefKind.In:
                                invokerFunctions.Append("in ");
                                break;
                            case RefKind.Out:
                                invokerFunctions.Append("out ");
                                break;
                            case RefKind.Ref:
                                invokerFunctions.Append("ref ");
                                break;
                        }

                        invokerFunctions.Append(method.Parameters[i].Type);
                    }

                    invokerFunctions.AppendLine($", {method.ReturnType}>)*(_vtable + {delegateCount - 1});");

                    invokerFunctions.Append("                ");

                    if (method.ReturnType.SpecialType != SpecialType.System_Void)
                    {
                        invokerFunctions.Append("return ");
                    }

                    invokerFunctions.Append("func(_implementation");

                    for (int i = 0; i < method.Parameters.Length; i++)
                    {
                        invokerFunctions.Append($", ");

                        var refKind = method.Parameters[i].RefKind;

                        switch (refKind)
                        {
                            case RefKind.In:
                                invokerFunctions.Append("in ");
                                break;
                            case RefKind.Out:
                                invokerFunctions.Append("out ");
                                break;
                            case RefKind.Ref:
                                invokerFunctions.Append("ref ");
                                break;
                        }

                        invokerFunctions.Append($"a{i}");
                    }

                    invokerFunctions.AppendLine(");");

                    invokerFunctions.AppendLine("            }");
                }
            }

            sourceBuilder.Replace("{typeName}", typeName);
            sourceBuilder.Replace("{delegates}", delegates.ToString());
            sourceBuilder.Replace("{interfaceName}", interfaceName);
            sourceBuilder.Replace("{delegateCount}", delegateCount.ToString());
            sourceBuilder.Replace("{functionPointers}", functionPointers.ToString());
            sourceBuilder.Replace("{invokerFunctions}", invokerFunctions.ToString());
            sourceBuilder.Replace("{invokerName}", invokerName);

            context.AddSource($"{symbol.ContainingNamespace?.Name ?? "_"}.{symbol.Name}.g.cs", sourceBuilder.ToString());

            Log("Generated code: ");
            Log("");
            Log(sourceBuilder.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(EmitAttribute);

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private void EmitAttribute(GeneratorPostInitializationContext context)
        {
            context.AddSource("GenerateNativeStubAttribute.g.cs", @"
using System;

[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
internal class GenerateNativeStubAttribute : Attribute { }
");
        }
    }

    public class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<INamedTypeSymbol> Interfaces { get; } = new();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is InterfaceDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                NativeStubGenerator.Log("Class: " + classDeclarationSyntax);
                var symbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                NativeStubGenerator.Log($"Symbol: {symbol.GetType()}");
                NativeStubGenerator.Log(symbol.ToString());

                foreach (var attribute in symbol.GetAttributes())
                {
                    NativeStubGenerator.Log($"Attribute: {attribute}");
                }

                if (symbol.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == "GenerateNativeStubAttribute"))
                {
                    NativeStubGenerator.Log($"{symbol.GetType().Name} - {symbol.ToString()}");
                    Interfaces.Add(symbol);
                }
            }
        }
    }
}