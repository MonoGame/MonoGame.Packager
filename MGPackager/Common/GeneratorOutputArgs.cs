// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace MGPackager
{
    class GeneratorOutputArgs : EventArgs
    {
        public string Text { get; private set; }

        public GeneratorOutputArgs(string text)
        {
            Text = text;
        }
    }
}

