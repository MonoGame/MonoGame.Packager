// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace MGPackager
{
    public class LinuxBundler : IGenerator
    {
        public string Name
        {
            get
            {
                return "Linux";
            }
        }

        public bool Generate(GeneratorData data, GeneratorOutputHandler output)
        {
            return false;
        }
    }
}

