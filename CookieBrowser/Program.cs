using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookieBrowser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AssemblyResolver.Hook("bin");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public static class AssemblyResolver
    {
        internal static void Hook(params string[] folders)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                if (loadedAssembly != null)
                    return loadedAssembly;

                var n = new AssemblyName(args.Name);

                if (n.Name.EndsWith(".xmlserializers", StringComparison.OrdinalIgnoreCase))
                    return null;

                if (n.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                    return null;

                string assy = null;
                var rootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";

                foreach (var dir in folders)
                {
                    assy = new[] { "*.dll", "*.exe" }.SelectMany(g => Directory.EnumerateFiles(Path.Combine(rootFolder, dir), g)).FirstOrDefault(f =>
                    {
                        try
                        {
                            return n.Name.Equals(AssemblyName.GetAssemblyName(f).Name,
                                StringComparison.OrdinalIgnoreCase);
                        }
                        catch (BadImageFormatException)
                        {
                            return false;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    });

                    if (assy != null)
                        return Assembly.LoadFrom(assy);
                }
                return null;
            };
        }
    }
}
