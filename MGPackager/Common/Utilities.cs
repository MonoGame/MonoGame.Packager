// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Diagnostics;
using System.IO;

namespace MGPackager
{
    static class Utilities
    {
        public static void CopyDirectory(string sourceDirPath, string destDirPath)
        {
            var dir = new DirectoryInfo(sourceDirPath);

            if (!Directory.Exists(destDirPath))
                Directory.CreateDirectory(destDirPath);

            var files = dir.GetFiles();
            foreach (var f in files)
                f.CopyTo(Path.Combine(destDirPath, f.Name), true);

            var dirs = dir.GetDirectories();
            foreach (var d in dirs)
                CopyDirectory(d.FullName, Path.Combine(destDirPath, d.Name));
        }

        public static string CallNativeMethod(string command)
        {
            var ret = "";

            var proc = new Process ();
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \"" + command + "\"";
            proc.StartInfo.UseShellExecute = false; 
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start ();

            while (!proc.StandardOutput.EndOfStream)
                ret += proc.StandardOutput.ReadLine() + "\n";

            return ret;
        }
    }
}

