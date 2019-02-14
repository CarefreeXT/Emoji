using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Caredev.Emoji
{

    internal interface IEmojiTextBlock : IEmojiTextElement
    {
        double LineHeight { get; set; }
        TextWrapping TextWrapping { get; set; }
        TextTrimming TextTrimming { get; set; }
        TextAlignment TextAlignment { get; set; }
    }
}
