using System;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Caredev.Emoji
{
    internal class EmojiTextSource : TextSource
    {
        internal EmojiTextSource(IEmojiTextBlock element)
        {
            _Element = element;
            DefaultTextRunProperties = new EmojiTextRunProperties(element);
        }

        private readonly IEmojiTextBlock _Element;

        internal double RenderWidth { get; set; }

        internal string Text => _Element.Text;

        internal int Length => Text.Length;

        internal TextRunProperties DefaultTextRunProperties { get; }
        
        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
        {
            var cbr = new CharacterBufferRange(Text, 0, textSourceCharacterIndexLimit);
            return new TextSpan<CultureSpecificCharacterBufferRange>(
             textSourceCharacterIndexLimit,
             new CultureSpecificCharacterBufferRange(System.Globalization.CultureInfo.CurrentUICulture, cbr)
             );
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new NotImplementedException();
        }

        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
                throw new ArgumentOutOfRangeException("textSourceCharacterIndex", "Value must be greater than 0.");
            // Create TextCharacters using the current font rendering properties.
            if (textSourceCharacterIndex < Text.Length)
            {
                var start = EmojiCharacters.CheckEmoji(_Element.Text, textSourceCharacterIndex);
                if (start == textSourceCharacterIndex)
                {
                    return new EmojiCharacters(this, Text, textSourceCharacterIndex, DefaultTextRunProperties);
                }
                else if (start > textSourceCharacterIndex)
                {
                    return new TextCharacters(Text, textSourceCharacterIndex, start - textSourceCharacterIndex, DefaultTextRunProperties);
                }
                return new TextCharacters(Text, textSourceCharacterIndex, Text.Length - textSourceCharacterIndex, DefaultTextRunProperties);
            }
            // Return an end-of-paragraph if no more text source.
            return new TextEndOfParagraph(1);
        }
    }
}
