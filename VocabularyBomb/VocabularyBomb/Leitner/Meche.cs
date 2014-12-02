using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vocabulary
{
    public class Meche : GameComponent
    {

        #region Propriété

        //Texture de l'image à afficher.
        public Texture2D Texture
        {
            set;
            get;
        }

        //Position Y de l'image
        public float PositionY
        {
            set;
            get;
        }

        //Position X de l'image
        public float PositionX
        {
            set;
            get;
        }

        //Hauteur actuelle de la hauteur de l'image
        public float Height
        {
            set;
            get;
        }

        //Largeur de l'image de la méche
        public float Width
        {
            set;
            get;
        }

        //Valeur initial de la meche.
        public float StartValue
        {
            set;
            get;
        }

        //temps passé dans le jeu.
        public TimeSpan Time
        {
            set;
            get;
        }

        //Vrai si la méche à commencé à ce consumer
        //sinon faux.
        public bool Started
        {
            get;
            set;
        }

        //Vrai si la meche est en pause sino faux.
        public bool Paused
        {
            get;
            set;
        }

        //Vrai si la méche est terminé sinon faux.
        public bool Finished
        {
            get;
            set;
        }
        
        #endregion

        #region Constructeur(s)
        public Meche(Game game, Texture2D texture, float positionX, float positionY, float width, float height, float startValue)
            : base(game)
        {
            this.Texture = texture;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.Width = width;
            this.Height = height;
            this.StartValue = startValue;
            this.Time = TimeSpan.FromSeconds(0);
            this.Finished = false;
            this.Paused = false;
            this.Started = true;
        }
        #endregion

        #region Méthode Update
        public override void Update(GameTime gameTime)
        {
            if (this.Started)
            {
                if (!this.Paused)
                {
                    this.Time += gameTime.ElapsedGameTime;
                    if (this.Time.TotalMilliseconds >= 1000)
                    {
                        if (0 <= this.Height - 33)
                        {
                            this.Height -= (this.Texture.Height - 33) / this.StartValue;
                            this.PositionY += (this.Texture.Height - 33) / this.StartValue;
                        }
                        else
                        {
                            this.Finished = true;
                        }
                        this.Time = TimeSpan.FromSeconds(0);
                    }
                }
            }
        }
        #endregion

        #region Méthode Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture,new Vector2(this.PositionX,this.PositionY), new Rectangle(0, 0, (int)this.Width, (int)this.Height), Color.White);
        }
        #endregion
    }
}
