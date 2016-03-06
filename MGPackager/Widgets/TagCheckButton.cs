// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Gtk;

namespace MGPackager
{
    class TagCheckButton : CheckButton
    {
        public object Tag { get; set; }

        public TagCheckButton(string label) : base(label) 
        {
            
        }
    }
}

