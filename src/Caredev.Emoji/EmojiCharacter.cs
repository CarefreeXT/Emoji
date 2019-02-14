using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Caredev.Emoji
{
    internal class EmojiCharacter
    {
        internal EmojiCharacter(EmojiGlyphLayer[] layers, double advanceWidth, int length)
        {
            _Layers = layers;
            AdvanceWidth = advanceWidth;
            Length = length;
        }

        internal int Length { get; }

        internal double AdvanceWidth { get; }

#if NET462
        internal static DpiScale DpiScale { get; set; } = new DpiScale(1, 1);
#endif

        private readonly EmojiGlyphLayer[] _Layers;
        private static readonly double[] _EmptyArray = new double[] { 0 };
        internal void Draw(DrawingContext context, GlyphTypeface m_gtf, Point origin, TextRunProperties properties)
        {
            foreach (var layer in _Layers)
            {
#if NET462
                var run = new GlyphRun(m_gtf, 0, false, properties.FontRenderingEmSize,
                            (float)DpiScale.PixelsPerDip, new ushort[] { layer.LayerIndex },
                            origin, _EmptyArray,
                            null, null, null,
                            null, null, null);
#else
                var run = new GlyphRun(m_gtf, 0, false, properties.FontRenderingEmSize,
                            new ushort[] { layer.LayerIndex },
                            origin, _EmptyArray,
                            null, null, null,
                            null, null, null);
#endif
                context.DrawGlyphRun(layer.Brush, run);
            }
        }
    }
}
