using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace Caredev.Emoji
{
    internal class EmojiTextParagraphProperties : TextParagraphProperties
    {

        internal EmojiTextParagraphProperties(IEmojiTextBlock element)
        {
            _Element = element;
            DefaultTextRunProperties = new EmojiTextRunProperties(element);
        }

        private readonly IEmojiTextBlock _Element;

        public override FlowDirection FlowDirection => FlowDirection.LeftToRight;

        public override TextAlignment TextAlignment => _Element.TextAlignment;

        public override double LineHeight => _Element.LineHeight;

        public override TextWrapping TextWrapping => _Element.TextWrapping;

        public override TextRunProperties DefaultTextRunProperties { get; }

        public override TextMarkerProperties TextMarkerProperties => null;

        public override bool FirstLineInParagraph => false;

        public override double Indent => 0;
    }
}
