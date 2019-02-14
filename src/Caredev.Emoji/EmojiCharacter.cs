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

        private readonly EmojiGlyphLayer[] _Layers;
        private static readonly double[] _EmptyArray = new double[] { 0 };
        internal void Draw(DrawingContext context, GlyphTypeface m_gtf, Point origin, TextRunProperties properties)
        {
            foreach (var layer in _Layers)
            {
                var run = new GlyphRun(m_gtf, 0, false, properties.FontRenderingEmSize,
                    new ushort[] { layer.LayerIndex },
                    origin, _EmptyArray,
                    null, null, null,
                    null, null, null);
                context.DrawGlyphRun(layer.Brush, run);
            }
        }
    }
}
