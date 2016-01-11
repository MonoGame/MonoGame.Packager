// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using System.IO.Compression;

namespace MGPackager
{
    public class LinuxBundler : BundleGenerator
    {
        public override string Name
        {
            get
            {
                return "Linux";
            }
        }

        protected override void Bundle(GeneratorData data, GeneratorOutputHandler output, string tempFolder, string dataFolder, string kickName)
        {
            // Copy MonoKickstart
            output.WriteLine("Extracting Kickstart stuff");

            ZipFile.ExtractToDirectory(Path.Combine(dataFolder, "MonoKickstart.zip"), tempFolder);
            ZipFile.ExtractToDirectory(Path.Combine(dataFolder, this.Name + "Libs.zip"), tempFolder);

            // Rename Kickstart files
            var files = Directory.GetFiles(tempFolder);

            foreach(var f in files)
            {
                if(f.Contains("kick.bin."))
                {
                    Utilities.CallNativeMethod("chmod +x " + f);
                    File.Move(f, f.Replace("kick", kickName));
                }
            }

            // Write Kickstart launch script
            var lines = File.ReadAllLines(Path.Combine(dataFolder, "Kick"));

            for(int i = 0;i < lines.Length;i++)
                if(lines[i].Contains("kick.bin."))
                    lines[i] = lines[i].Replace("kick", kickName);

            var kickFile = Path.Combine(tempFolder, kickName);

            File.WriteAllLines(kickFile, lines);
            Utilities.CallNativeMethod("chmod +x " + kickFile);

            // Generate Zip File
            var zipfile = Path.Combine(data.OutputFolder, kickName) + "_" + this.Name + ".zip";
            output.WriteLine("Generating zip file: " + zipfile);
            output.Write(Utilities.CallNativeMethod("cd " + tempFolder + " && zip -r " + zipfile + " ./*"));
        }
    }
}

