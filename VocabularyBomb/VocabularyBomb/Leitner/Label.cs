using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vocabulary
{
   
    public class Label  : GameComponent
    {
        #region Constructors
        public Label(Game game, SpriteFont font, string text, Vector2 position, Color color) : base(game)
        {
            this.Font = font;
            this.Text = text;
            this.Width = (int)(this.Font.MeasureString(this.Text).X);
            this.Height = (int)(this.Font.MeasureString(this.Text).Y);
            this.Position = position;
            this.Color = color;
        }

        public Label(Game game, SpriteFont font, string text, Color color) : base(game)
        {
            this.Font = font;
            this.Text = text;
            this.Width = (int)(this.Font.MeasureString(this.Text).X);
            this.Height = (int)(this.Font.MeasureString(this.Text).Y);
            this.Position = new Vector2(game.Window.ClientBounds.Width - this.Width - 60, game.Window.ClientBounds.Height - this.Height);
            this.Color = color;
        }
        public Label(Game game, SpriteFont font, string text, Vector2 position, Color color, int width, int height)
            : base(game)
        {
            this.Font = font;
            this.Text = text;
            this.Width = width;
            this.Height = height;
            this.Position = position;
            this.Color = color;
        }


        #endregion

        #region Properties

        public SpriteFont Font
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.DrawString(this.Font, this.Text, this.Position, this.Color);
        }
        #endregion
    }
}
