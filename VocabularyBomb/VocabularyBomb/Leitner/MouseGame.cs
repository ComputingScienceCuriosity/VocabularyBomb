using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vocabulary
{
    public class MouseGame : GameComponent
    {

        #region Constructeur(s)
        public MouseGame(Game game, Texture2D texture, int positionX, int positionY) : base(game)
        {
            this.X = positionX;
            this.Y = positionY;
            this.Texture = texture;
            this.Height = this.Texture.Height;
            this.Width = this.Texture.Width;
            this.Position = new Vector2(this.X, this.Y);
        }
        #endregion

        #region Properties

        public int Width
        {
            set;
            get;
        }

        public int Height
        {
            set;
            get;
        }

        private Vector2 Position
        {
            set;
            get;
        }

        public Texture2D Texture
        {
            set;
            get;
        }

        public int X
        {
            set;
            get;
        }

        public int Y
        {
            set;
            get;
        }
        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            this.Position = new Vector2(mouseState.X, mouseState.Y);
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }
        #endregion
    }
}
