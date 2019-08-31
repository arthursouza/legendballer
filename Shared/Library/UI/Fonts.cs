using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library
{
    public class Fonts
    {
        public static SpriteFont Arial18 { get; set; }
        public static SpriteFont Arial12 { get; set; }
        public static SpriteFont Arial20 { get; set; }
        
        public static SpriteFont DefaultFont { get; set; }

        public static SpriteFont Arial26 { get; set; }
        public static SpriteFont Arial36 { get; set; }
        public static SpriteFont Arial42 { get; set; }
        public static SpriteFont Arial54 { get; set; }

        public static SpriteFont TimesNewRoman26 { get; set; }
        //public static SpriteFont Impact26 { get; set; }

        
        /// <summary>
        ///     Faz a quebra de linha de um texto para que caiba numa área especifica.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Retorna a string formatada</returns>
        public static string WrapText(string text, int width, SpriteFont spriteFont)
        {
            var spaceWidth = spriteFont.MeasureString(" ").X;
            var fullSize = spriteFont.MeasureString(text);

            if (text.Contains(" ") && fullSize.X > width)
            {
                var sb = new StringBuilder();
                var words = text.Split(' ');
                var lineWidth = 0f;
                
                foreach (var word in words)
                {
                    if (!word.StartsWith("\n"))
                    {
                        var size = spriteFont.MeasureString(word);

                        if (lineWidth + size.X < width)
                        {
                            sb.Append(word + " ");
                            lineWidth += size.X + spaceWidth;
                        }
                        else
                        {
                            sb.Append("\n" + word + " ");
                            lineWidth = size.X + spaceWidth;
                        }
                    }
                    else
                    {
                        var size = spriteFont.MeasureString(word);
                        sb.Append(word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
                return sb.ToString();
            }
            return text;
        }
    }
}
