// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Windows.Forms;
using Gtk;

namespace MGPackager
{
    class FileDialog : IFileDialog
    {
        public string FileName { get; private set; }

        CommonDialog _dialog;

        public FileDialog(MainWindow window, FileChooserAction action)
        {
            if (action == FileChooserAction.Open)
                _dialog = new OpenFileDialog ();
            else
                _dialog = new FolderBrowserDialog ();
        }

        public void AddFilter(string name, string pattern)
        {
            var dialog = (OpenFileDialog)_dialog;
            dialog.Filter = (dialog.Filter + "|" + name + "|" + pattern).TrimStart (new [] { '|' });
        }

        public ResponseType Run()
        {
            var result = _dialog.ShowDialog ();

            if (result == DialogResult.OK)
            {
                if (_dialog is OpenFileDialog)
                    FileName = ((OpenFileDialog)_dialog).FileName;
                else
                    FileName = ((FolderBrowserDialog)_dialog).SelectedPath;

                return ResponseType.Ok;
            }

            return ResponseType.Cancel;
        }
    }
}

