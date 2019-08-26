using System.Collections.Generic;
using LegendBaller.Library.Input;
using LegendBaller.Library.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendBaller.Library.Scenes
{
    public class Scene
    {
        protected BrazucaGame Game;
        public List<IControl> Controls;

        protected int windowPadding = 15;
        
        public Scene(BrazucaGame game)
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
