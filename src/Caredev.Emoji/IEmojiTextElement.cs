using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Caredev.Emoji
{
    internal interface IEmojiTextElement
    {
        Brush Background { get; set; }
        FontFamily FontFamily { get; set; }
        double FontSize { get; set; }
        FontStretch FontStretch { get; set; }
        FontStyle FontStyle { get; set; }
        FontWeight FontWeight { get; set; }
        Brush Foreground { get; set; }
        TextEffectCollection TextEffects { get; set; }
        string Text { get; set; }
    }
}
