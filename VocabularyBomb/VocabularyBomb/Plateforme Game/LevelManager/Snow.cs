using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Voca.Plateforme_Game.LevelManager
{
    /// <summary>
    /// Composant du jeu qui implémente IUpdateable.
    /// </summary>
    public class Snow : GameComponent
    {
        #region Variables
        private Point[] snow = new Point[225];  //You can increase/decrease this number depending on the density you want
        private bool isSnowing = false;           //I only set this to true (so far) in the main menu
        private Texture2D snowTexture;           //Our texture for each snow particle. I'll attach mine, though it's very simple
        private Point quitPoint;                       //The point at which we need to recycle the snow particle
        private Point startPoint;                     //The point where the snow particle gets recycled to
        private Random random;                    //Will be used for generating new random numbers
        #endregion

        #region Properties
        private int wid, hei;
        private int SnowRatio = 2;

        public int SnowAccelerate
        {
            set { this.SnowRatio = value; }
            
        }

        public bool SnowDirection
        {
            set;
            get;
        }bool snowDirection = false;

        public int Width 
        {   
            get { return this.wid; } 
        }

        public int Height 
        { 
            get { return this.hei; } 
        }

        #endregion 
        #region Constructors
        public Snow(Game game)
            : base(game)
        {
            
        }
        #endregion  


        #region Methodes
        public void LoadContent(ContentManager Content)
        {
            //Load our texture. I placed it in a folder called "Textures," if you don't put it in any folder, don't use "Textures\\"
            //or if you use a different folder, replaces Textures with its name.
            this.snowTexture = Content.Load<Texture2D>("Plateforme/Weather/Snow");

            //Set the snow's quit point. It's the bottom of the screen plus the texture's height so it looks like the snow goes completely off screen
            this.quitPoint = new Point(0,
                this.Height + this.snowTexture.Height);

            //Set the snow's start point. It's the top of the screen minus the texture's height so it looks like it comes from somewhere, rather than appearing
            this.startPoint = new Point(0,
                0 - this.snowTexture.Height);
        }
        /// <summary>
        /// Permet au composant de jeu d’effectuer une initialisation si nécessaire avant l’exécution.
        /// Il peut alors demander les services nécessaires et charger du contenu.
        /// </summary>
        /// 
        public void initialize()
        {
            // TODO : ajouter le code d’initialisation ici
            this.wid = 800;
            this.hei = 480;
            
            //There's more code in here, just not needed for this tutorial
   
            base.Initialize();
        }

        /// <summary>
        /// Permet au composant de jeu de se mettre à jour.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        /// 
        /*public override void Update(GameTime gameTime,int ratio)
        {
            this.SnowRatio = 

            base.Update(gameTime);
        }*/

        public void DrawSnow(SpriteBatch spriteBatch)
        {
            //If it's not supposed to be snowing, exit
            //if (!isSnowing)
            //    return;

            //This will be used as the index within our snow array
            int i;

            //NOTE: The following conditional is not exactly the best "initializer."
            //If snow has not been initialized
            if (this.snow[0] == new Point(0, 0))
            {
                //Make the random a new random
                this.random = new Random();

                //For every snow particle within our snow array,
                for (i = 0; i < this.snow.Length; i++)
                    //Give it a new, random x and y. This will give the illusion that it was already snowing
                    //and won't cluster the particles
                    this.snow[i] = new Point(
                        (random.Next(0, (this.Width - this.snowTexture.Width))),
                        (random.Next(0, (this.Height))));
            }

            //Make the random a new random (again, if just starting)
            this.random = new Random();

            //Go back to the beginning of the snow array
            i = 0;

            //Begin displaying the snow
            foreach (Point snowPnt in this.snow)
            {
                //Get the exact rectangle for the snow particle
                Rectangle snowParticle = new Rectangle(
                    snowPnt.X, snowPnt.Y, this.snowTexture.Width, this.snowTexture.Height);
               
                //Draw the snow particle (change white if you want any kind of tinting)
                spriteBatch.Draw(this.snowTexture, snowParticle, Color.White);
              
                //Make the current particle go down, but randomize it for a staggering snow
                this.snow[i].Y += random.Next(0, this.SnowRatio);

                if (this.SnowDirection)
                    this.snow[i].X += random.Next(-1, 0);
               
                //Make sure the point's location is not below quit point's
                if (this.snow[i].Y >= this.quitPoint.Y)
                    //If it is, give it a random X value, and the starting point variable's Y value
                    this.snow[i] = new Point(
                        (random.Next(0, (this.Width - this.snowTexture.Width))),
                        this.startPoint.Y);

                //Increment our position in the array ("go to the next snow particle")
                i++;
            }
        }
        public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            spriteBatch.Begin();
            DrawSnow(spriteBatch);
            spriteBatch.End();
        } 
        #endregion
    }
}
