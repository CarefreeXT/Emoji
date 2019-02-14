using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Caredev.Emoji
{
    internal partial class EmojiCharacters
    {
        private const string EmojiFontName = "Segoe UI Emoji";
        private readonly static Dictionary<int, EmojiCharacter> _Emojis;
        private readonly static ushort _UnitsPerEm;
        private readonly static GlyphTypeface _Typeface;

        static EmojiCharacters()
        {
            var typeface = new System.Windows.Media.Typeface(EmojiFontName);
            _Emojis = new Dictionary<int, EmojiCharacter>();
            if (typeface.TryGetGlyphTypeface(out _Typeface))
            {
                using (var s = _Typeface.GetFontStream())
                {
                    var r = new Typography.OpenFont.OpenFontReader();
                    var m_openfont = r.Read(s, Typography.OpenFont.ReadFlags.Full);

                    _UnitsPerEm = m_openfont.UnitsPerEm;

                    var colrTable = m_openfont.COLRTable;
                    int palette = 0; // FIXME: support multiple palettes?
                    var brushs = new Dictionary<Color, Brush>();
                    byte R, G, B, A;
                    foreach (var kv in _Typeface.CharacterToGlyphMap)
                    {
                        var glyphIndex = kv.Value;
                        if (colrTable.LayerIndices.TryGetValue(glyphIndex, out var layer_index))
                        {
                            int endIndex = layer_index + colrTable.LayerCounts[glyphIndex];
                            var layers = new EmojiGlyphLayer[endIndex - layer_index];
                            for (int i = layer_index; i < endIndex; ++i)
                            {
                                ushort sub_gid = colrTable.GlyphLayers[i];
                                int cid = m_openfont.CPALTable.Palettes[palette] + colrTable.GlyphPalettes[i];
                                m_openfont.CPALTable.GetColor(cid, out R, out G, out B, out A);
                                var color = Color.FromArgb(A, R, G, B);
                                if (!brushs.TryGetValue(color, out Brush brush))
                                {
                                    brush = new SolidColorBrush(color);
                                    brushs.Add(color, brush);
                                }
                                layers[i - layer_index] = new EmojiGlyphLayer()
                                {
                                    Brush = brush,
                                    //Color = Color.FromArgb(A, R, G, B),
                                    LayerIndex = sub_gid
                                };
                            }

                            var glyph = m_openfont.Lookup(kv.Key);
                            var width = glyph.OriginalAdvanceWidth;
                            if (!glyph.HasOriginalAdvancedWidth)
                            {
                                width = m_openfont.GetHAdvanceWidthFromGlyphIndex(glyphIndex);
                            }
                            _Emojis.Add(kv.Key, new EmojiCharacter(layers, width,
                                Char.ConvertFromUtf32(kv.Key).Length));
                        }
                    }
                }
            }
        }

        internal static int CheckEmoji(string source, int startIndex)
        {
            for (int i = startIndex; i < source.Length; i++)
            {
                if (IsEmoji(source, i))
                {
                    return i;
                }
            }
            return -1;
        }

        private double CalculateScale(double targetPixelSize)
        {
            return targetPixelSize / _UnitsPerEm;
        }

        private static EmojiCharacter GetEmojiChar(string source, int index)
        {
            if (!Char.IsSurrogate(source[index]))
            {
                if (_Emojis.TryGetValue(source[index], out EmojiCharacter val))
                {
                    return val;
                }
            }
            else if (index + 1 < source.Length)
            {
                var code = Char.ConvertToUtf32(source[index], source[index + 1]);
                if (_Emojis.TryGetValue(code, out EmojiCharacter val))
                {
                    return val;
                }
            }
            return null;
        }

        private static bool IsEmoji(string source, int index)
        {
            if (!char.IsSurrogate(source[index]))
            {
                return _Emojis.ContainsKey(source[index]);
            }
            else
            {
                if (source.Length > index + 1)
                {
                    return _Emojis.ContainsKey(char.ConvertToUtf32(source[index], source[index + 1]));
                }
                return false;
            }
        }
    }
    internal partial class EmojiCharacters : TextEmbeddedObject
    {
        private readonly double _Scale;
        private readonly EmojiTextSource _Source;
        private readonly EmojiCharacter[] _Chars;

        internal EmojiCharacters(EmojiTextSource source, string characterString, int offsetToFirstChar, TextRunProperties textRunProperties)
        {
            _Source = source;
            this.Properties = textRunProperties;
            _Scale = CalculateScale(this.Properties.FontRenderingEmSize);
            var list = new List<EmojiCharacter>();
            var index = offsetToFirstChar;
            EmojiCharacter emoji;
            double width = 0;
            while (index < characterString.Length &&
                (emoji = GetEmojiChar(characterString, index)) != null)
            {
                var temp = emoji.AdvanceWidth * _Scale;
                if (width + temp > source.RenderWidth)
                {
                    break;
                }
                width += temp;
                list.Add(emoji);
                index += emoji.Length;
            }
            _Chars = list.ToArray();
        }

        public override LineBreakCondition BreakBefore => LineBreakCondition.BreakAlways;

        public override LineBreakCondition BreakAfter => LineBreakCondition.BreakAlways;

        public override bool HasFixedSize => true;

        public override CharacterBufferReference CharacterBufferReference { get; }

        public override TextRunProperties Properties { get; }

        public override int Length => _Chars.Sum(a => a.Length);

        public override Rect ComputeBoundingBox(bool rightToLeft, bool sideways)
        {
            var fontsize = this.Properties.FontRenderingEmSize;
            return new Rect(0, 0, _Chars.Sum(a => a.AdvanceWidth * _Scale), fontsize * _Typeface.Height);
        }

        public override TextEmbeddedObjectMetrics Format(double remainingParagraphWidth)
        {
            var fontsize = this.Properties.FontRenderingEmSize;
            var width = _Chars.Sum(a => a.AdvanceWidth * _Scale);
            return new TextEmbeddedObjectMetrics(width, fontsize * _Typeface.Height, fontsize * _Typeface.Baseline);
        }

        public override void Draw(DrawingContext drawingContext, Point origin, bool rightToLeft, bool sideways)
        {
            foreach (var item in _Chars)
            {
                item.Draw(drawingContext, _Typeface, origin, this.Properties);
                origin.X += item.AdvanceWidth * _Scale;
            }
        }
    }
}
