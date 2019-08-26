using System.Collections.Generic;
using baller.windows.Library.Input;
using baller.windows.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace baller.windows.Library.Scenes
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
                if (btn.MouseOver())
                {
                    mouseOver = true;
                }
            }

            Game.MouseCursor = mouseOver? MouseCursor.MouseOver : MouseCursor.Normal;
        }

        public virtual void MouseDown(MouseButton button)
        {
        }

        public virtual void MouseClick(MouseButton button)
        {
        }
    }
}
