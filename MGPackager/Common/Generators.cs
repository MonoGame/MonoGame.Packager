// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace MGPackager
{
    static class Generators
    {
        internal static List<IGenerator> GetBundlerList()
        {
            return new List<IGenerator>()
            {
                new WindowsBundler(),
#if LINUX || MAC
                new MacBundler(),
                new LinuxBundler(),
#endif
            };
        }

        internal static List<IGenerator> GetInstallerList()
        {
            var ret = new List<IGenerator>();

#if WINDOWS
            ret.Add(new ExeInstaller());
#elif MAC
            ret.Add(new PkgInstaller());
#elif LINUX
            ret.Add(new DebInstaller());
            ret.Add(new RpmInstaller());
            ret.Add(new RunInstaller());
#endif

            return ret;
        }
    }
}

