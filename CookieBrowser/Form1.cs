using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookieBrowser
{
    public partial class Form1 : Form
    {
        public CefSharp.WinForms.ChromiumWebBrowser chromebrowser;
        static string lib, browser, locales, res;

        public Form1()
        {
            // Assigning file paths to varialbles
            lib = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\libcef.dll");
            browser = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\CefSharp.BrowserSubprocess.exe");
            locales = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\locales\");
            res = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\");

            var libraryLoader = new CefLibraryHandle(lib);

            InitializeComponent();
            GetWebPage();

            libraryLoader.Dispose();
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void GetWebPage()
        {
            if (Cef.IsInitialized)
            {

            }
            else
            {
                try
                {
                    CefSettings settings = new CefSettings();
                    settings.BrowserSubprocessPath = browser;
                    settings.LocalesDirPath = locales;
                    settings.ResourcesDirPath = res;
                    settings.CefCommandLineArgs.Add(settings.LogFile, @"bin\debug.log"); //Disable GPU vsync
                    Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

                    chromebrowser = new CefSharp.WinForms.ChromiumWebBrowser("https://ixware.biz/")
                    { Dock = DockStyle.Fill };
                    panel1.Controls.Add(chromebrowser);
                    chromebrowser.Dock = DockStyle.Fill;
                }
                catch (Exception){ }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            chromebrowser.Load(textBox1.Text);

            var mngr = Cef.GetGlobalCookieManager();
            Cookie Ac = new Cookie();
            Ac.Name = textBox2.Text;
            Ac.Value = textBox3.Text;
            mngr.SetCookieAsync(textBox2.Text, Ac);
        }
    }
}
