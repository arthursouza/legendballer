﻿using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Baller.Library.Menu
{
    public enum LobbyOptionFunction
    {
        NextGame,
        Training,
        Shop,
        MyStats,
        RecentNews,
        League,
        Business
    }

    public class LobbyOption : BaseButton
    {
        public LobbyOption(string text, Vector2 position, Vector2 buttonSize, LobbyOptionFunction option)
        {
            Position = position;
            Text = text;
            Option = option;
            this.BackgroundTexture = UserInterface.LobbyButton;

            Bounds = new Rectangle((int) Position.X,  (int) Position.Y,  (int) buttonSize.X, (int) buttonSize.Y);
        }

        public LobbyOptionFunction Option { get; set; }
        public new Rectangle Bounds { get; set; }

        public void UpdatePosition(Vector2 position)
        {
            Position = position;

            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Bounds.Width, Bounds.Height);
        }

        public string Text
        {
            get; set; 
        }
        
        internal void Draw(SpriteBatch batch, bool selected = false)
        {
            //var color = selected ? Color.LightBlue : Color.White;

            var sourceRect = new Rectangle(0, selected ? BackgroundTexture.Height / 2 : 0, BackgroundTexture.Width, BackgroundTexture.Height / 2);

            batch.Draw(UserInterface.LobbyButton, Bounds, sourceRect, Color.White);

            var font = Fonts.Arial26;
            var textSize = font.MeasureString(Text);

            batch.DrawString(
                font, 
                Text,
                Position + new Vector2(Bounds.Width / 2f - textSize.X / 2, Bounds.Height / 2f - textSize.Y / 2), 
                Color.White);
        }
    }
}
