// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

namespace MGPackager
{
    class BundleGenerator : IGenerator
    {
        public virtual string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Generate(GeneratorData data, GeneratorOutputHandler output)
        {
            var ret = false;

            var tempFolder = Path.GetTempFileName();
            var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            var kickName = Path.GetFileNameWithoutExtension(data.ExeFile);

            output.WritePassage("Starting to bundle");

            try
            {
                output.WriteLine("Generating temporary folder");

                File.Delete(tempFolder);
                Directory.CreateDirectory(tempFolder);

                // Copy Game
                output.WriteLine("Copying game");
                Utilities.CopyDirectory(data.Folder, tempFolder);

                Bundle(data, output, tempFolder, dataFolder, kickName);

                ret = true;
            }
            catch (Exception ex)
            {
                output.WriteLine(ex.Message);
            }

            try
            {
                output.WriteLine("Cleanup");
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);
            }
            catch (Exception ex)
            {
                output.WriteLine(ex.Message);
            }

            output.WriteLine("Done bundling, result: " + ((ret) ? "SUCCESS" : "FAILURE"));

            return ret;
        }

        protected virtual void Bundle(GeneratorData data, GeneratorOutputHandler output, string tempFolder, string dataFolder, string kickName)
        {
            
        }
    }
}

