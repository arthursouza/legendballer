using System.Collections.Generic;
using Baller.Library.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Baller.Library
{
    public class Scene
    {
        protected BallerGame Game;
        public List<IControl> Controls;

        protected int windowPadding = 15;
        
        public Scene(BallerGame game)
        {
            Controls = new List<IControl>();
            Game = game;
        }

        public virtual void Draw(SpriteBatch batch)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            bool mouseOver = false;

            foreach (IButton btn in Controls.FindAll(x => x is IButton))
            {
                if (btn.Pressed())
                {
                    mouseOver = true;
                }
            }

            Game.MouseCursor = mouseOver? MouseCursor.MouseOver : MouseCursor.Normal;
        }

        //public virtual void MouseDown(MouseButton button)
        //{
        //}

        //public virtual void MouseClick(MouseButton button)
        //{
        //}

        //public virtual void Touch(Vector2 position)
        //{
        //}

        public virtual void MainInput(Vector2 pos)
        {
        }
        
        public virtual void InputDown(Vector2 pos)
        {
        }
        
        public virtual void InputMoved(Vector2 position)
        {
        }
    }
}
