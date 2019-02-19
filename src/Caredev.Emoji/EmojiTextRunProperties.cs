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
            RefreshTypeface();
        }

        internal void RefreshTypeface()
        {
            _Typeface = new Typeface(_Element.FontFamily,
               _Element.FontStyle, _Element.FontWeight, _Element.FontStretch);
        }

        private readonly IEmojiTextElement _Element;

        public override Typeface Typeface => _Typeface;
        private Typeface _Typeface;

        public override double FontRenderingEmSize => _Element.FontSize;

        public override double FontHintingEmSize => _Element.FontSize;

        public override TextDecorationCollection TextDecorations => null;

        public override Brush ForegroundBrush => _Element.Foreground;

        public override Brush BackgroundBrush => _Element.Background;

        public override CultureInfo CultureInfo => CultureInfo.CurrentUICulture;

        public override TextEffectCollection TextEffects => _Element.TextEffects;
    }
}
