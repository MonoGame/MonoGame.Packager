// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using System.IO.Compression;

namespace MGPackager
{
    public class WindowsBundler : BundleGenerator
    {
        public override string Name
        {
            get
            {
                return "Windows";
            }
        }

        protected override void Bundle(GeneratorData data, GeneratorOutputHandler output, string tempFolder, string dataFolder, string kickName)
        {
            // TODO Setup WindowsDX bundler

            // Extract Windows Libraries
            output.WriteLine("Extracting Kickstart stuff");
            ZipFile.ExtractToDirectory(Path.Combine(dataFolder, this.Name + "Libs.zip"), tempFolder);

            // Generate Zip File
            var zipfile = Path.Combine(data.OutputFolder, kickName) + "_" + this.Name + ".zip";
            output.WriteLine("Generating zip file: " + zipfile);
            ZipFile.CreateFromDirectory(tempFolder, zipfile);
        }
    }
}

