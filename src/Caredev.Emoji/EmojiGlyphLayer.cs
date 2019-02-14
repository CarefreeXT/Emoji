using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Caredev.Emoji
{
    internal struct EmojiGlyphLayer
    {
        internal ushort LayerIndex { get; set; }

        internal Brush Brush { get; set; }
    }
}
