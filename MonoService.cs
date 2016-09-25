using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MvcCore
{
    public static class MonoService
    {
        public static void MonoRazor(this IMvcBuilder builder)
        {
            if (Type.GetType("Mono.Runtime") == null) return;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Select(p => MetadataReference.CreateFromFile(p.Location)).ToList();
            var path = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            /*
            foreach (var lib in new [] {"Dapper.dll", "Dapper.Rainbow.MySql.dll"}) {
                assemblies.Add(MetadataReference.CreateFromFile($"{path}/{lib}"));
            }
            */

            builder.AddRazorOptions(options =>
            {
                var previous = options.CompilationCallback;
                options.CompilationCallback = (context) => {
                    if (previous != null) previous(context);
                    context.Compilation = context.Compilation.AddReferences(assemblies);
                    // foreach (var lib in context.Compilation.ReferencedAssemblyNames) Console.WriteLine($"Loaded {lib}");
                };
            });
        }

    }
    
}