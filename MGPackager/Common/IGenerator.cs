// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MGPackager
{
    interface IGenerator
    {
        string Name { get; }

        bool Generate(GeneratorData data, GeneratorOutputHandler output);
    }
}

