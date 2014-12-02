using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Vocabulary
{
    public class Boom : Image
    {

        private int WindowWidth
        {
            set;
            get;
        }

        private int WindowHeight
        {
            set;
            get;
        }

        public bool Finished
        {
            set;
            get;
        }

        public Boom(Game game, Texture2D texture, int positionX, int positionY, int width, int height)
            : base(game, texture, positionX, positionY, width, height)
        {
            this.WindowWidth = game.GraphicsDevice.Viewport.Width;
            this.WindowHeight = game.GraphicsDevice.Viewport.Height;
            this.Finished = false;
        }

        public void Increase(int amount)
        {
            if (this.Texture.Width > this.Width)
            {
                this.Width += amount;
                this.Height += amount;
                this.PositionX = (this.WindowWidth / 2) - (this.Width / 2);
                this.PositionY = (this.WindowHeight / 2) - (this.Height / 2);
                this.Rectangle = new Rectangle(this.PositionX, this.PositionY, this.Width, this.Height);
            }
            else
            {
                this.Finished = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
