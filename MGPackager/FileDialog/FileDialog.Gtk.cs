// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Gtk;

namespace MGPackager
{
    class FileDialog : IFileDialog
    {
        public string FileName { get; private set; }

        FileChooserDialog dialog;

        public FileDialog(Window window, FileChooserAction action)
        {
            dialog = new FileChooserDialog("Select Folder", window, action,
                "Cancel", ResponseType.Cancel,
                "Select", ResponseType.Ok);
        }

        public void AddFilter(string name, string pattern)
        {
            var filter = new FileFilter();
            filter.Name = name;
            filter.AddPattern(pattern);

            dialog.AddFilter(filter);
        }

        public ResponseType Run()
        {
            var ret = dialog.Run();
            FileName = dialog.Filename;

            dialog.Destroy();

            return (ResponseType)ret;
        }
    }
}

