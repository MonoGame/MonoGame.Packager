// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

using Gtk;

namespace MGPackager
{
    public partial class MainWindow
    {
        VBox vbox1, vbox2, vbox3;
        Notebook notebook1;
        HBox hbox1, hbox2;
        Table table1, table2, table3;
        Button btnCancel, btnPrev, btnNext, btnIcon, btnBrowse, btnBrowse2;
        Label label1, label2, label3, label4, label5, label6, label7, label8, label9, label10;
        ScrolledWindow scroll1, scroll2;
        TreeView treeview1;
        Entry entryGameDir, entryTitle, entryVersion, entryCompany, entryOutputDir;
        Image imageIcon;
        TextView textView1;
        List<TagCheckButton> checkBundlers = new List<TagCheckButton>();
        List<TagCheckButton> checkInstallers = new List<TagCheckButton>();

        private void Build()
        {
            this.Title = "MonoGame Packager";
            this.DefaultWidth = this.WidthRequest = 640;
            this.DefaultHeight = this.HeightRequest = 480;
            this.BorderWidth = 4;

#if GTK3
            var geom = new Gdk.Geometry();
            geom.MinWidth = geom.MaxWidth = this.DefaultWidth;
            geom.MinHeight = geom.MaxHeight = this.DefaultHeight;
            this.SetGeometryHints(this, geom, Gdk.WindowHints.MinSize | Gdk.WindowHints.MaxSize);
#else
            this.Resizable = false;
#endif

            vbox1 = new VBox();
            vbox1.Spacing = 4;

            notebook1 = new Notebook();
            notebook1.ShowBorder = false;
            notebook1.ShowTabs = false;

            // Wizard Page 0

            vbox2 = new VBox();
            vbox2.Spacing = 10;

            label1 = new Label();
            label1.Wrap = true;
            label1.LineWrapMode = Pango.WrapMode.Word;
            label1.Text = "Welcome to MonoGame Packager\n" +
                "\n" +
                "This tool will help you pack you desktop game for redistribution. It offers 2 options, installer and bundle of binaries. The difference between bundling the game into an archive with this tool and doing it by hand is the fact that this tool will help by adding per platform dependencies.";
            vbox2.PackStart(label1, true, true, 0);

            label2 = new Label("Do note that installer generation is usually only supported for the OS this tool is run from.\n");
            vbox2.PackStart(label2, false, true, 1);

            notebook1.Add(vbox2);

            // Wizaed Page 1

            table1 = new Table(5, 3, false);

            table1.Attach(new Label(), 0, 3, 0, 1);

            label3 = new Label(" Select game folder: ");
            label3.SetAlignment(0f, 0.5f);
            table1.Attach(label3, 0, 1, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 4, 4);

            entryGameDir = new Entry();
            entryGameDir.Sensitive = false;
            table1.Attach(entryGameDir, 1, 2, 1, 2, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 4, 4);

            btnBrowse = new Button("Browse...");
            btnBrowse.Clicked += BtnBrowse_Clicked;
            table1.Attach(btnBrowse, 2, 3, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 4, 4);

            label4 = new Label(" Select game .exe file:");
            label4.SetAlignment(0f, 0.5f);
            table1.Attach(label4, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 4, 4);

            scroll1 = new ScrolledWindow();
            scroll1.HeightRequest = 200;

            treeview1 = new TreeView();
            treeview1.HeightRequest = scroll1.HeightRequest;
            treeview1.HeadersVisible = false;
            treeview1.Reorderable = false;
            treeview1.CursorChanged += Treeview1_CursorChanged;

            scroll1.Add(treeview1);

            table1.Attach(scroll1, 0, 3, 3, 4, AttachOptions.Fill, AttachOptions.Shrink, 0, 0);

            table1.Attach(new Label(), 0, 3, 4, 5);

            notebook1.Add(table1);

            // Wizard Page 2

            table2 = new Table(10, 3, false);

            table2.Attach(new Label(), 0, 3, 0, 1);

            imageIcon = new Image();

            btnIcon = new Button(imageIcon);
            btnIcon.WidthRequest = btnIcon.HeightRequest = 64;
            btnIcon.Clicked += BtnIcon_Clicked;
            table2.Attach(btnIcon, 0, 1, 1, 3, AttachOptions.Fill, AttachOptions.Fill, 4, 4);

            label5 = new Label("Title:");
            label5.SetAlignment(0f, 0.5f);
            table2.Attach(label5, 1, 2, 1, 2, AttachOptions.Fill, AttachOptions.Shrink, 4, 4);

            entryTitle = new Entry();
            table2.Attach(entryTitle, 2, 3, 1, 2, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 4, 4);

            label6 = new Label("Version:");
            label6.SetAlignment(0f, 0.5f);
            table2.Attach(label6, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 4, 4);

            entryVersion = new Entry();
            table2.Attach(entryVersion, 2, 3, 2, 3, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 4, 4);

            label7 = new Label("Creator:");
            label7.SetAlignment(0f, 0.5f);
            table2.Attach(label7, 1, 2, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 4, 4);

            entryCompany = new Entry();
            table2.Attach(entryCompany, 2, 3, 3, 4, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 4, 4);

            table2.Attach(new HSeparator(), 0, 3, 4, 5, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 4, 4);

            table3 = new Table(3, 5, false);

            label8 = new Label("Generate bundle of binaries:");
            label8.SetAlignment(0f, 0.5f);
            table3.Attach(label8, 0, 1, 0, 1, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 4, 4);

            var bundlers = Generators.GetBundlerList();
            for (int i = 0; i < bundlers.Count; i++)
            {
                var checkButton = new TagCheckButton(bundlers[i].Name);
                checkButton.SetAlignment(0f, 0.5f);
                checkButton.Tag = bundlers[i];
                checkButton.Toggled += (sender, e) => btnNext.Sensitive = Page3NextSensitive();

                table3.Attach(checkButton, 0, 1, 1 + (uint)i, 2 + (uint)i, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 20, 2);
                checkBundlers.Add(checkButton);
            }

            label9 = new Label("Generate installer:");
            label9.SetAlignment(0f, 0.5f);
            table3.Attach(label9, 2, 3, 0, 1, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 4, 4);

            var installers = Generators.GetInstallerList();
            for (int i = 0; i < installers.Count; i++)
            {
                var checkButton = new TagCheckButton(installers[i].Name);
                checkButton.SetAlignment(0f, 0.5f);
                checkButton.Tag = installers[i];
                checkButton.Toggled += (sender, e) => btnNext.Sensitive = Page3NextSensitive();

                table3.Attach(checkButton, 2, 3, 1 + (uint)i, 2 + (uint)i, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Shrink, 20, 2);
                checkInstallers.Add(checkButton);
            }

            table2.Attach(table3, 0, 3, 5, 6, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 40, 4);

            table2.Attach(new Label(), 0, 3, 6, 7);

            notebook1.Add(table2);

            // Wizard Page 3

            vbox3 = new VBox();
            vbox3.BorderWidth = 4;
            vbox3.Spacing = 4;

            label10 = new Label("Select output folder:");
            label10.SetAlignment(0f, 1f);
            vbox3.PackStart(label10, true, true, 0);

            hbox2 = new HBox();
            hbox2.Spacing = 4;

            entryOutputDir = new Entry();
            entryOutputDir.Sensitive = false;
            hbox2.PackStart(entryOutputDir, true, true, 0);

            btnBrowse2 = new Button("Browse...");
            btnBrowse2.Clicked += BtnBrowse2_Clicked;
            hbox2.PackStart(btnBrowse2, false, true, 1);

            vbox3.PackStart(hbox2, false, false, 1);

            vbox3.PackStart(new Label(), true, true, 2);

            notebook1.Add(vbox3);

            // Wizard Page 4

            scroll2 = new ScrolledWindow();

            textView1 = new TextView();
            scroll2.Add(textView1);

            notebook1.Add(scroll2);

            // Control Buttons

            vbox1.PackStart(notebook1, true, true, 0);

            hbox1 = new HBox();

            btnCancel = new Button("Cancel");
            btnCancel.Clicked += (sender, e) => Application.Quit();
            hbox1.PackStart(btnCancel, false, true, 0);

            hbox1.PackStart(new Label(), true, true, 1);

            btnPrev = new Button("Previous");
            btnPrev.Clicked += BtnPrev_Clicked;
            btnPrev.Sensitive = false;
            hbox1.PackStart(btnPrev, false, true, 2);

            btnNext = new Button("Next");
            btnNext.Clicked += BtnNext_Clicked;
            hbox1.PackStart(btnNext, false, true, 3);

            vbox1.PackStart(hbox1, false, true, 1);

            this.Add(vbox1);
            this.ShowAll();

            this.DeleteEvent += OnDeleteEvent;
        }
    }
}

