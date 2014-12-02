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
using VocabularyBomb;


namespace Vocabulary
{
    /// <summary>
    /// Component Credits Screen
    /// </summary>
    public class Credits 
    {
        public bool IsCreditsExited
        {
            set;
            get;    
        }

        #region FMOD 
        public FMOD.Event Credits_SongEvent { set; get; } FMOD.Event credits_SongEvent = null;
        #endregion

        private Texture2D[] credits_screen;

        private int screen_number = 3;
        private int screen_iterator = 0;
        private bool isEnterDown = false;

        public Credits(ContentManager Content, GraphicsDeviceManager graphics)  
        {
            LoadContent(Content, graphics);      
        }

        /// <summary>
        /// Content LOADING 
        /// </summary>
        public void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            credits_screen = new Texture2D[3];
            for (int i = 0; i < screen_number; i++ )
                credits_screen[i] = Content.Load<Texture2D>("Plateforme/Gui/credits_"+(i+1));

            #region FMOD Song
            FmodFactory.Instance.load("Credits_THEME_song", ref credits_SongEvent);
            #endregion
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">.</param>
        public void Update(GameTime gameTime)
        {
            #region FMOD
            FmodFactory.Instance.start(ref credits_SongEvent);
            #endregion

            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Enter) && !isEnterDown)
            {
                screen_iterator++;
                isEnterDown = true;
            }

            if (key.IsKeyUp(Keys.Enter) &&    isEnterDown)
                isEnterDown = false;

            if(screen_iterator == screen_number)
                IsCreditsExited = true;
        }
        /// <summary>
        /// Draw Credits Screen 
        /// </summary>
        /// <param name="gameTime">.</param>
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            if (screen_iterator < screen_number)
            spriteBatch.Draw(credits_screen[this.screen_iterator], new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();     
        }
    }
}
