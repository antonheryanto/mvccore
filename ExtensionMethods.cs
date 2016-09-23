using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MvcCore 
{
    public static class ExtensionMethods
    {
        public static string Current(this ClaimsPrincipal user)
        {
            return user.Identity.Name;
        }

        public static IServiceCollection DapperDb(this IServiceCollection services)
        {
            var cs = "server=localhost;user=root;password=tante;database=ukm_ifolio;";
            var cn = new MySql.Data.MySqlClient.MySqlConnection(cs);
            cn.Open();
            return services.AddSingleton<Db>(p => Db.Init(cn, 30));
        }

        public static void MonoRazor(this IMvcBuilder builder)
        {
            if (Type.GetType("Mono.Runtime") == null) return;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Select(p => MetadataReference.CreateFromFile(p.Location)).ToList();
            var path = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            foreach (var lib in new [] {"Dapper.dll", "Dapper.Rainbow.MySql.dll"}) {
                assemblies.Add(MetadataReference.CreateFromFile($"{path}/{lib}"));
            }

            builder.AddRazorOptions(options =>
            {
                var previous = options.CompilationCallback;
                options.CompilationCallback = (context) => {
                    if (previous != null) previous(context);
                    context.Compilation = context.Compilation.AddReferences(assemblies);
                    foreach (var lib in context.Compilation.ReferencedAssemblyNames) Console.WriteLine($"Loaded {lib}");
                };
            });
        }

        public static string Encrypt(this string s)
        {
            var validationKey = "518A9D0E650ACE4CB22A35DA4563315098A96D0BB8E357531C7065D032099214A11D1CA074B6D66FF0836B35CEAAD0E7EEEFAED772754832E0A5F94EF8522222";
            //var decryptionKey = "DB5660C109E9EC70F044BA1FED99DE0C5922321C5125E84C23A1B5CA0E426909";
            var validationKeyBytes = new byte[validationKey.Length / 2];
            for (int i = 0; i < validationKeyBytes.Length; i++)
                validationKeyBytes[i] = Convert.ToByte(validationKey.Substring(i * 2, 2), 16);

            var hash = new System.Security.Cryptography.HMACSHA1(validationKeyBytes);
            var encodedPassword = Convert.ToBase64String(hash.ComputeHash(System.Text.Encoding.Unicode.GetBytes(s)));

            return encodedPassword;
        }
    }
}