using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vocabulary
{
    public class LoadingScreen
    {
        #region Variables
        private Texture2D[] loading;
        private Texture2D[] tutorialScreen;
        public GameState CurrentGameState;
        private int numberOfLevels;
        public TimeSpan timerLoading = TimeSpan.FromSeconds(0);
        #endregion 

        #region Properties
        public enum GameState { 
        
            Loading,
            Tutorial,
        
        }
        #endregion

        #region Constructors
        public LoadingScreen(ContentManager Content, GraphicsDeviceManager graphics, int numberOfLevels)
        {
            LoadContent(Content,graphics,numberOfLevels);
        }
        #endregion

        #region Methodes
        public void LoadContent(ContentManager Content, GraphicsDeviceManager graphics, int numberOfLevels)
        {
            this.numberOfLevels = numberOfLevels;
            loading = new Texture2D[numberOfLevels];
            tutorialScreen = new Texture2D[numberOfLevels];
            
            for (int i = 0; i < numberOfLevels; i++)
            {
                loading[i] = Content.Load<Texture2D>("Plateforme/Loading/Chargement" + i);
                tutorialScreen[i] = Content.Load<Texture2D>("Plateforme/Tutorials/level" + i);       
            }
        }
        public void Update()
        { 
            
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, bool isTutorial, int levelIndex)
        {
          
                if (!isTutorial)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(loading[levelIndex], new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                }
                else if (isTutorial)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(tutorialScreen[levelIndex], new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                }
            
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics,bool isTutorial, int levelIndex,int numberOfLevels)
        {
            if (levelIndex < numberOfLevels)
            {
                if (!isTutorial)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(loading[levelIndex], new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                }
                else if (isTutorial)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(tutorialScreen[levelIndex], new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.End();
                }
            }
        }
        #endregion  
    }
}
