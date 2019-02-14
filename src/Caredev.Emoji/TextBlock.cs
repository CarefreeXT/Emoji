using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Caredev.Emoji
{
    public partial class TextBlock
    {
        private readonly EmojiTextParagraphProperties _TextParagraph;
        private readonly EmojiTextSource _TextSource;

        public TextBlock()
        {
            _TextParagraph = new EmojiTextParagraphProperties(this);
            _TextSource = new EmojiTextSource(this);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
#if DEBUG
            var watch = System.Diagnostics.Stopwatch.StartNew(); 
#endif
            var size = base.MeasureOverride(availableSize);
            var length = _TextSource.Length;
            using (var formatter = TextFormatter.Create())
            {
                if (length > 0)
                {
                    int textStorePosition = 0;
                    var maxWidth = double.IsInfinity(availableSize.Width) ?
                        10000 : availableSize.Width;
                    var position = new Point(0, 0);
                    _TextSource.RenderWidth = maxWidth;
                    while (textStorePosition < length && size.Height < availableSize.Height)
                    {
                        using (var line = formatter.FormatLine(_TextSource, textStorePosition,
                            maxWidth, _TextParagraph, null))
                        {
                            size.Width = Math.Max(size.Width, line.Width);
                            size.Height += line.Height;
                            textStorePosition += line.Length;
                        }
                    }
                }
                else
                {
                    using (var line = formatter.FormatLine(_TextSource, 0, 0, _TextParagraph, null))
                    {
                        size.Height = line.Height;
                    }
                }
            }
#if DEBUG
            watch.Stop();
            Debug.WriteLine($"MeasureOverride ElapsedTicks:{watch.ElapsedTicks},ElapsedMilliseconds:{watch.ElapsedMilliseconds}");
#endif
            return size;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
#if DEBUG
            var watch = System.Diagnostics.Stopwatch.StartNew(); 
#endif
            base.OnRender(drawingContext);
            if (!string.IsNullOrEmpty(Text))
            {
                int textStorePosition = 0;
                var position = new Point(0, 0);
                var length = _TextSource.Length;
                var maxWidth = this.ActualWidth;
                var maxHeight = this.ActualHeight;
                _TextSource.RenderWidth = maxWidth;
                using (var formatter = TextFormatter.Create())
                {
                    while (textStorePosition < length && position.Y < maxHeight)
                    {
                        using (var line = formatter.FormatLine(_TextSource,
                            textStorePosition, maxWidth, _TextParagraph, null))
                        {
                            line.Draw(drawingContext, position, InvertAxes.None);
                            position.Y += line.Height;
                            textStorePosition += line.Length;
                        }
                    }
                }
            }
#if DEBUG
            watch.Stop();
            System.Diagnostics.Debug.WriteLine($"OnRender ElapsedTicks:{watch.ElapsedTicks},ElapsedMilliseconds:{watch.ElapsedMilliseconds}"); 
#endif
        }
    }
    public partial class TextBlock : FrameworkElement, IEmojiTextBlock
    {
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty BackgroundProperty;
        public static readonly DependencyProperty ForegroundProperty;
        public static readonly DependencyProperty FontSizeProperty;
        public static readonly DependencyProperty FontFamilyProperty;
        public static readonly DependencyProperty FontWeightProperty;
        public static readonly DependencyProperty FontStyleProperty;
        public static readonly DependencyProperty FontStretchProperty;
        public static readonly DependencyProperty LineHeightProperty;
        public static readonly DependencyProperty TextDecorationsProperty;
        public static readonly DependencyProperty TextEffectsProperty;
        public static readonly DependencyProperty TextAlignmentProperty;
        public static readonly DependencyProperty TextWrappingProperty;
        public static readonly DependencyProperty TextTrimmingProperty;

        static TextBlock()
        {
            TextProperty = DependencyProperty.Register(
                "Text", 
                typeof(string), 
                typeof(TextBlock),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

            FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(TextBlock));
            FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(TextBlock));
            FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(TextBlock));
            FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(TextBlock));
            BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(TextBlock));
            ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(TextBlock));
            FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(TextBlock));
            TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(typeof(TextBlock));

            LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(TextBlock));
            TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextBlock));
            TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(TextBlock));

            TextTrimmingProperty = DependencyProperty.Register(
                "TextTrimming",
                typeof(TextTrimming),
                typeof(TextBlock),
                new FrameworkPropertyMetadata(
                    TextTrimming.None,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
            TextWrappingProperty = DependencyProperty.Register(
                "TextWrapping",
                typeof(TextWrapping),
                typeof(TextBlock),
                new FrameworkPropertyMetadata(
                    TextWrapping.NoWrap,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }
        public double LineHeight
        {
            get { return (double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }
        public TextEffectCollection TextEffects
        {
            get { return (TextEffectCollection)GetValue(TextEffectsProperty); }
            set { SetValue(TextEffectsProperty, value); }
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }
    }

}
