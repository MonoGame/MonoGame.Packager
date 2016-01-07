// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Drawing;

namespace MGPackager
{
    public class GeneratorData
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Creator { get; set; }

        public string Folder { get; set; }
        public string ExeFile { get; set; }
        public Icon Icon { get; set; }

        public string OutputFolder { get; set; }
    }
}

