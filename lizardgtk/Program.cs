using System;
using Gtk;
using System.IO;
using System.Diagnostics;

namespace lizardgtk
{
    class MainClass
    {
        public static void Main (string[] args) {
            var theme = "ambiance_radiance_flat_blue.gtkrc";
            Application.Init ();
            Rc.AddDefaultFile (theme);
            Rc.Parse(theme);
            var win = new MainWindow ();
            win.Show ();
            Application.Run ();
        }
    }
}
