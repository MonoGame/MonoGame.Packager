// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace MGPackager
{
    class PkgInstaller : IGenerator
    {
        public string Name
        {
            get
            {
                return "Mac (.pkg)";
            }
        }

        public bool Generate(GeneratorData data, GeneratorOutputHandler output)
        {
            return true;
        }
    }
}

