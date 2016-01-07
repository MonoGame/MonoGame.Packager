// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Reflection;
using System.Threading;

using Gtk;

namespace MGPackager
{
    public partial class MainWindow : Window
    {
        GeneratorData generatorData;
        GeneratorOutputHandler generatorOutput;

        TreeStore treestore1;
        TreeIter nulliter = new TreeIter();
        Thread scanningThread, buildingThread;

        public MainWindow() : base(WindowType.Toplevel)
        {
            Build();

            generatorData = new GeneratorData();

            generatorOutput = new GeneratorOutputHandler();
            generatorOutput.OutputHandler += GeneratorOutput_OutputHandler;

            var textCell = new CellRendererText();

            var column = new TreeViewColumn ();
            column.PackStart (textCell, false);
            column.AddAttribute (textCell, "text", 0);
            treeview1.AppendColumn (column);

            treestore1 = new TreeStore(typeof(string));
            treeview1.Model = treestore1;
        }

        protected void OnDeleteEvent (object o, DeleteEventArgs args)
        {
            if (scanningThread != null && scanningThread.IsAlive)
                scanningThread.Abort();
            
            Application.Quit();
        }

        // Wizard Page 1

        protected void BtnBrowse_Clicked (object sender, EventArgs e)
        {
            var dialog = new FileDialog(this, FileChooserAction.SelectFolder);
            var result = dialog.Run();

            if (result == ResponseType.Ok)
            {
                entryGameDir.Text = dialog.FileName;
                entryGameDir.Position = entryGameDir.Text.Length;

                treestore1.Clear();
                treestore1.AppendValues("Scanning directory...");

                if (scanningThread != null && scanningThread.IsAlive)
                    scanningThread.Abort();

                scanningThread = new Thread(new ThreadStart(this.ScanningThread));
                scanningThread.Start();
            }
        }

        protected void Treeview1_CursorChanged (object sender, EventArgs e)
        {
            if (notebook1.CurrentPage == 1)
            {
                TreeIter iter;
                btnNext.Sensitive = treeview1.Selection.GetSelected(out iter) && !treestore1.IterHasChild(iter);
            }
        }

        private void ScanningThread()
        {
            treestore1 = new TreeStore(typeof(string));
            ScanFolder(nulliter, entryGameDir.Text);

            Gtk.Application.Invoke(delegate
                {
                    treeview1.Model = treestore1;
                });
        }

        private bool ScanFolder(TreeIter iter, string folder)
        {
            bool ret = false;

            try
            {
                var directories = System.IO.Directory.GetDirectories(folder);
                var files = System.IO.Directory.GetFiles(folder);

                foreach (var d in directories)
                {
                    var dirname = System.IO.Path.GetFileName(d);
                    var i = (iter.Equals(nulliter)) ? treestore1.AppendValues(dirname) : treestore1.AppendValues(iter, dirname);

                    if (!ScanFolder(i, d))
                        treestore1.Remove(ref i);
                    else
                        ret = true;
                }

                foreach (var f in files)
                {
                    if (f.EndsWith(".exe"))
                    {
                        var fname = System.IO.Path.GetFileName(f);

                        if (iter.Equals(nulliter))
                            treestore1.AppendValues(fname);
                        else
                            treestore1.AppendValues(iter, fname);

                        ret = true;
                    }
                }
            }
            catch
            {
                // Possible in case user doesn't have access to read the directory
            }

            return ret;
        }

        // Wizard Page 2

        protected void BtnIcon_Clicked (object sender, EventArgs e)
        {
            var dialog = new FileDialog(this, FileChooserAction.Open);
            dialog.AddFilter("Icon files (*.ico)", "*.ico");
            dialog.AddFilter("All Files (*.*)", "*.*");

            if (dialog.Run() == ResponseType.Ok)
            {
                try
                {
                    generatorData.Icon = new System.Drawing.Icon(dialog.FileName);
                    imageIcon.Pixbuf = (new Gdk.Pixbuf(dialog.FileName)).ScaleSimple(32, 32, Gdk.InterpType.Bilinear);
                }
                catch
                {
                }
            }
        }

        private string GetAttribute(Assembly assembly, Type type)
        {
            object[] attributes = assembly.GetCustomAttributes(type, false);

            if (attributes.Length == 0)
                return "";

            return type.GetProperties()[0].GetValue(attributes[0], null).ToString();
        }

        private bool Page3NextSensitive()
        {
            foreach (var c in checkBundlers)
                if (c.Active)
                    return true;

            foreach (var c in checkInstallers)
                if (c.Active)
                    return true;

            return false;
        }

        // Wizard Page 3

        protected void BtnBrowse2_Clicked (object sender, EventArgs e)
        {
            var dialog = new FileDialog(this, FileChooserAction.SelectFolder);
            var result = dialog.Run();

            if (result == ResponseType.Ok)
            {
                entryOutputDir.Text = dialog.FileName;
                entryOutputDir.Position = entryOutputDir.Text.Length;

                btnNext.Sensitive = true;
            }
        }

        // Wizard Page 4

        private void BuildThread()
        {
            foreach (var c in checkBundlers)
                ((IGenerator)c.Tag).Generate(generatorData, generatorOutput);

            foreach (var c in checkInstallers)
                ((IGenerator)c.Tag).Generate(generatorData, generatorOutput);

            Application.Invoke(delegate
                {
                    textView1.Buffer.Text += "\r\n\r\nDONE";

                    var btn = new Button("Close");
                    btn.Clicked += (sender, e) => Application.Quit();
                    vbox1.PackStart(btn, false, true, 2);
                    vbox1.ShowAll();
                });
        }

        protected void GeneratorOutput_OutputHandler (object sender, GeneratorOutputArgs e)
        {
            Application.Invoke(delegate
                {
                    textView1.Buffer.Text += e.Text;
                });
        }

        // Button Conrols

        protected void BtnNext_Clicked (object sender, EventArgs e)
        {
            if (notebook1.Page == 0)
            {
                // Load Page 1

                btnNext.Sensitive = treeview1.Selection.CountSelectedRows() != 0;
            }
            else if (notebook1.Page == 1)
            {
                // Save Page 1

                string path = "";

                TreeIter iter;
                treeview1.Selection.GetSelected(out iter);

                do
                {
                    path = treestore1.GetValue(iter, 0) + "/" + path;
                }
                while(treestore1.IterParent(out iter, iter));

                path = path.TrimEnd('/');

                generatorData.ExeFile = path;
                generatorData.Folder = entryGameDir.Text;

                // Load Page 2

                path = System.IO.Path.Combine(entryGameDir.Text, path);

                try
                {
                    var assembly = Assembly.LoadFile(path);

                    var ms = new System.IO.MemoryStream();
                    generatorData.Icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
                    generatorData.Icon.Save(ms);
                    imageIcon.Pixbuf = (new Gdk.Pixbuf(ms.GetBuffer())).ScaleSimple(32, 32, Gdk.InterpType.Bilinear);
                    ms.Dispose();

                    entryTitle.Text = GetAttribute(assembly, typeof(AssemblyTitleAttribute));
                    entryVersion.Text = assembly.GetName().Version.ToString();
                    entryCompany.Text = GetAttribute(assembly, typeof(AssemblyCompanyAttribute));
                }
                catch
                { 
                    imageIcon.Pixbuf = null;

                    entryTitle.Text = "";
                    entryVersion.Text = "";
                    entryCompany.Text = "";
                }

                btnNext.Sensitive = Page3NextSensitive();
            }
            else if (notebook1.Page == 2)
            {
                // Save Page 2

                //Icon gets saved on loading because when displaying it we need to resize it
                generatorData.Title = entryTitle.Text;
                generatorData.Version = entryVersion.Text;
                generatorData.Creator = entryCompany.Text;

                // Load Page 3

                btnNext.Label = "Generate";
                btnNext.Sensitive = !string.IsNullOrEmpty(entryOutputDir.Text);
            }
            else if (notebook1.Page == 3)
            {
                // Save Page 3

                generatorData.OutputFolder = entryOutputDir.Text;

                // Load Page 4

                vbox1.Remove(hbox1);

                buildingThread = new Thread(new ThreadStart(this.BuildThread));
                buildingThread.Start();
            }

            notebook1.Page++;
            btnPrev.Sensitive = true;
        }

        protected void BtnPrev_Clicked (object sender, EventArgs e)
        {
            notebook1.Page--;

            btnNext.Label = "Next";
            btnNext.Sensitive = true;

            if (notebook1.Page == 0)
                btnPrev.Sensitive = false;
        }
    }
}

