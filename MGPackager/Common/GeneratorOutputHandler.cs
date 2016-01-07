// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace MGPackager
{
    public class GeneratorOutputHandler
    {
        public event EventHandler<GeneratorOutputArgs> OutputHandler;

        public void Write(string text)
        {
            if (OutputHandler != null)
                OutputHandler(this, new GeneratorOutputArgs(text));
        }

        public void WriteLine(string line)
        {
            if (OutputHandler != null)
                OutputHandler(this, new GeneratorOutputArgs(line + "\r\n"));
        }
    }
}

