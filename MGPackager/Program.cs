// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Gtk;

namespace MGPackager
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();

            var window = new MainWindow();
            window.Show();

            Application.Run();
        }
    }
}
