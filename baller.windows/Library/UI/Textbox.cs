using System;
using baller.windows.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace baller.windows.Library.UI
{
    public class Textbox
    {
        public Texture2D Background;
        public string Text;
        public int MaxSize;
        public Vector2 Position;
        public bool HasFocus;
        SpriteFont font;
        bool cursorShow = false;
        private float cursorInterval = 300f;
        private float cursorTimer;

        private float repeatDelay = 500f;
        private float startRepeating = 1000f;
        private float repeatTimer;
        private bool repeating;

        public Textbox(Vector2 position)
        {
            MaxSize = 20;
            font = Fonts.Arial36;
            Position = position;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Graphics.Selected, new Rectangle((int)Position.X, (int)Position.Y, 500, font.LineSpacing), Color.White * .5f);
            batch.DrawString(font, Text, new Vector2(5 + Position.X, Position.Y), Color.White);
            if (cursorShow)
                batch.Draw(Graphics.Selected, new Rectangle((int)(5 + Position.X + (font.MeasureString(Text).X)), (int)Position.Y, 2, font.LineSpacing), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (HasFocus)
            {
                cursorTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (cursorTimer > cursorInterval)
                {
                    cursorTimer = 0f;
                    cursorShow = !cursorShow;
                }

                UpdateKeyboardInput(gameTime);
            }
            else
            {
                cursorShow = false;
            }
        }

        private void UpdateKeyboardInput(GameTime gameTime)
        {
            bool input = false;

            if (InputInfo.KeyPress(Microsoft.Xna.Framework.Input.Keys.Back))
            {
                input = true;
                Backspace();
            }
            else if (InputInfo.KeyPress(Keys.Space))
            {
                if (Text.Length < 21)
                {
                    input = true;
                    Text = Text + " ";
                }
            }
            else if (InputInfo.KeyboardState.IsKeyDown(Keys.Back) && InputInfo.LastKeyboardState.IsKeyDown(Keys.Back))
            {
                input = true;
                repeatTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (repeating && repeatTimer > repeatDelay ||
                    !repeating && repeatTimer > startRepeating)
                {
                    Backspace();
                }
            }
            else
            {
                if (Text.Length < 21)
                {
                    for (int i = 65; i < 91; i++)
                    {
                        if (InputInfo.KeyboardState.IsKeyDown((Keys)i) &&
                            InputInfo.LastKeyboardState.IsKeyDown((Keys)i))
                        {
                            input = true;
                            repeatTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (repeating && repeatTimer > repeatDelay ||
                                !repeating && repeatTimer > startRepeating)
                            {
                                repeatTimer = 0f;
                                InsertKey(i);
                            }
                        }
                        else if (InputInfo.KeyPress((Keys)i))
                        {
                            input = true;
                            InsertKey(i);
                        }
                    }
                }
            }

            if (!input)
            {
                repeatTimer = 0f;
                repeating = false;
            }
        }

        private void InsertKey(int i)
        {
            string letter = string.Empty;

            if (InputInfo.KeyboardState.IsKeyDown(Keys.LeftShift) || InputInfo.KeyboardState.IsKeyDown(Keys.RightShift))
            {
                letter = Convert.ToChar(i).ToString();
            }
            else
            {
                letter = Convert.ToChar(i + 32).ToString();
            }

            Text += letter;
        }
        private void Backspace()
        {
            if (Text.Length > 0)
                Text = Text.Remove(Text.Length - 1);
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, 500, font.LineSpacing); }
        }

        public bool MouseOver
        {
            get { return Bounds.Contains((int)InputInfo.MousePosition.X, (int)InputInfo.MousePosition.Y); }
        }
    }
}
