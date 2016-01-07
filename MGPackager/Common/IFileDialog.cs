// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Gtk;

namespace MGPackager
{
    interface IFileDialog
    {
        string FileName { get; }

        ResponseType Run();

        void AddFilter(string name, string pattern);
    }
}

