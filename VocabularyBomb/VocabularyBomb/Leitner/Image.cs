using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vocabulary
{
    public class Image : GameComponent
    {
        #region Propriétés

        //Position Y de l'image
        public int PositionY
        {
            set;
            get;
        }

        //Position X de l'image
        public int PositionX
        {
            set;
            get;
        }

        //Largeur de l'image à afficher
        public int Width
        {
            set;
            get;
        }

        //Hauteur de l'image à afficher
        public int Height
        {
            set;
            get;
        }

        //Texture de l'image
        public Texture2D Texture
        {
            set;
            get;
        }

        //Rectangle où l'on va afficher l'image
        protected Rectangle Rectangle
        {
            set;
            get;
        }
        #endregion

        #region Constructeur(s)
        public Image(Game game, Texture2D texture, int positionX, int positionY, int width, int height) :base(game)
        {
            this.Texture = texture;
            this.Rectangle = new Rectangle(positionX, positionY, width, height);
            this.Height = height;
            this.Width = width;
            this.PositionX = positionX;
            this.PositionY = positionY;
        }
        #endregion

        #region Méthode Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(this.Texture, this.Rectangle, Color.White);
          
        }
        #endregion
    }
}
