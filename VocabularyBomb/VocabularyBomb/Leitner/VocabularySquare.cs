using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Vocabulary
{
    public class VocabularySquare : GameComponent
    {
        #region Constructor(s)
        public VocabularySquare(Game game, int x, int y, int size, VocabularyLeitner VocabularyLeitner) : base(game)
        {
            this.Square = new Rectangle(x, y, size, size);
            this.VocabularyLeitner = VocabularyLeitner;
            this.CurrentMouseState = Mouse.GetState();
            this.LastMouseState = this.CurrentMouseState;
            this.Size = this.Square.X;
            VocabularySquare.DisplayCorrectImage = 0;
        }
        #endregion

        #region Properties

        public static int DisplayCorrectImage
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public VocabularyLeitner VocabularyLeitner
        {
            get;
            set;
        }

        private Rectangle Square
        {
            get;
            set;
        }

        public MouseState CurrentMouseState
        {
            get;
            set;
        }

        public MouseState LastMouseState
        {
            get;
            set;
        }
        #endregion

        #region Méthode isMatch
        public bool isMatch()
        {
            bool isMatch = false;
            this.LastMouseState = this.CurrentMouseState;
            this.CurrentMouseState = Mouse.GetState();
            
            if (this.CurrentMouseState.LeftButton == ButtonState.Released 
                && this.LastMouseState.LeftButton == ButtonState.Pressed
                && this.Square.Contains(this.CurrentMouseState.X, this.CurrentMouseState.Y))
            {
                isMatch = true;
            }
            return isMatch;
        }
        #endregion

        #region Méthode isPressed
        public bool isPressed()
        {
            bool isPressed = false;
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed
                && this.Square.Contains(mouseState.X, mouseState.Y))
            {
                isPressed = true;
            }
            return isPressed;
        }
        #endregion

        #region Méthode Update
        public override void Update(GameTime gameTime)
        {
            if (this.isMatch())
            {
                VocabularySquare.DisplayCorrectImage = 2;
                if (Leitner.Instance.SelectedVocabulary.Equals(this.VocabularyLeitner))
                {
                    LeitnerGame.Right = true;
                }
                else
                {
                    LeitnerGame.Wrong = true;
                    VocabularySquare.DisplayCorrectImage = 1;
                }
            }
            base.Update(gameTime);
        }
        #endregion

        #region Méthode Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.isPressed()) 
            {
                spriteBatch.Draw(this.VocabularyLeitner.ImagePressed, this.Square, Color.White);
            }
            else
            {
                spriteBatch.Draw(this.VocabularyLeitner.Image, this.Square, Color.White);
            }
        }
        #endregion
    }
}
