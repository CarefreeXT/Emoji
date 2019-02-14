using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Caredev.Emoji
{
    internal class EmojiTextRunProperties : TextRunProperties
    {
        internal EmojiTextRunProperties(IEmojiTextElement element)
        {
            _Element = element;
            this.Typeface = new Typeface(element.FontFamily,
                element.FontStyle, element.FontWeight, element.FontStretch);
        }

        private readonly IEmojiTextElement _Element;

        public override Typeface Typeface { get; }

        public override double FontRenderingEmSize => _Element.FontSize;

        public override double FontHintingEmSize => _Element.FontSize;

        public override TextDecorationCollection TextDecorations => null;

        public override Brush ForegroundBrush => _Element.Foreground;

        public override Brush BackgroundBrush => _Element.Background;

        public override CultureInfo CultureInfo => CultureInfo.CurrentUICulture;

        public override TextEffectCollection TextEffects => _Element.TextEffects;
    }
}
