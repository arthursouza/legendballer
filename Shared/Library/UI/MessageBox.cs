﻿using Baller.Library.Input;
using Baller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Baller.Library
{
    public class MessageBox
    {
        public enum MessageBoxButtonType
        {
            Ok,
            OkCancel
        }

        public delegate void CloseHandler();

        private MessageBoxButtonType buttonType;
        public event CloseHandler Close;

        public static Vector2 DefaultWindowSize = new Vector2(800, 400);
        public static Vector2 DefaultWindowPosition;

        public TextureButton OkButton;
        public TextureButton CancelButton;

        public string Text;

        public bool Visible;
        public Color TextColor;

        public MessageBox(string text, Color textColor, SpriteFont font, MessageBoxButtonType type)
        {
            var marginBottom = 32;
            
            this.Text = text;
            this.TextColor = textColor;
            this.font = font;
            this.buttonType = type;
            
            WrapText();
            Visible = true;

            var buttonWidth = UserInterface.ButtonBlue.Width;
            var horizSpace = DefaultWindowSize.X - (2 * buttonWidth);

            if (type == MessageBoxButtonType.Ok)
            {
                // Button origin is the middle
                OkButton = new TextureButton(UserInterface.ButtonBlue, "Ok",
                    new Vector2(
                        DefaultWindowPosition.X + DefaultWindowSize.X/2,
                        DefaultWindowPosition.Y + DefaultWindowSize.Y - (UserInterface.ButtonBlue.Height/2f + marginBottom)),
                    true);
            }
            else if(type == MessageBoxButtonType.OkCancel)
            {
                OkButton = new TextureButton(UserInterface.ButtonBlue, "Ok",
                    new Vector2(DefaultWindowPosition.X + DefaultWindowSize.X / 2 - (buttonWidth + (horizSpace / 4)),
                        DefaultWindowPosition.Y + DefaultWindowSize.Y - (UserInterface.ButtonBlue.Height/2f + marginBottom)));

                CancelButton = new TextureButton(
                    UserInterface.ButtonBlue,
                    "Cancel",
                    new Vector2(
                        DefaultWindowPosition.X + DefaultWindowSize.X / 2 + (horizSpace / 4),
                        DefaultWindowPosition.Y + DefaultWindowSize.Y - (UserInterface.ButtonBlue.Height/2f + marginBottom)));
            }
        }

        public MessageBox()
        {
            font = Fonts.Arial36;
            Visible = false;
            OkButton = new TextureButton(UserInterface.ButtonBlue, "Ok",
                new Vector2(
                    DefaultWindowPosition.X + DefaultWindowSize.X / 2,
                    DefaultWindowPosition.Y + DefaultWindowSize.Y - (UserInterface.ButtonBlue.Height/2 + 20)));
        }
        
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.Selected, BallerGame.WindowBounds, Color.Black);
            batch.Draw(UserInterface.MessageBox, new Rectangle((int)DefaultWindowPosition.X, (int)DefaultWindowPosition.Y, (int)DefaultWindowSize.X, (int)DefaultWindowSize.Y), Color.White);
            batch.DrawString(font, Text, new Vector2(DefaultWindowPosition.X + DefaultWindowSize.X / 2 - Fonts.Arial26.MeasureString(Text).X/2, DefaultWindowPosition.Y + 50), TextColor);

            OkButton.Draw(batch);

            CancelButton?.Draw(batch);
        }

        public void MainInput()
        {
            if (OkButton.Pressed())
            {
                if (Close != null)
                    Close.Invoke();
            }
        }

        public void WrapText()
        {
            if (Text.Contains(" "))
            {
                StringBuilder sb = new StringBuilder();
                string[] words = Text.Split(' ');
                float lineWidth = 0f;

                float spaceWidth = font.MeasureString(" ").X;

                foreach (string word in words)
                {
                    if (!word.StartsWith("\n"))
                    {
                        Vector2 size = font.MeasureString(word);

                        if (lineWidth + size.X < DefaultWindowSize.X - 36)
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
                        Vector2 size = font.MeasureString(word);
                        sb.Append(word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
                Text = sb.ToString().Trim();
            }
        }

        public SpriteFont font { get; set; }
    }
}
