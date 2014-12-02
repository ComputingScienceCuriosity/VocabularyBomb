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
using Vocabulary;
using VocabularyBomb;

namespace Vocabulary
{
    /// <summary>
    /// This is the main gui of the game
    /// </summary>
    public class Gui
    {

        private int help_it = 0;
        private int help_max = 3;
        #region Variables
        public Texture2D main_background, help_background_1, help_background_2, help_background_3;
        private Texture2D scenario, titre, titre2;
        private bool isDownD,isDownU,isDownE = false;
        private bool isDownR, isDownL= false;

        public GameState CurrentGameState;
        int positionX, positionX2, positionY, positionY2;

        // Sound 
        #region FMOD 
        public FMOD.Event Main_Theme_SongEvent { set; get; } FMOD.Event main_Theme_SongEvent = null;
        #endregion 


        private SoundEffect buttonSound;
        Button btstart, btjouer, btaide, btquitter, bthand, bthand2, btNext, btBack;
        #endregion                                         
        #region Properties
        public bool IsPlaying
        {
            set;
            get;
        }
        public  bool isDrawing = false;

        public enum GameState { 
            Intro,
            StartMenu,
            MainMenu,
            Playing,
            Aide,
        }
        #endregion

        #region Constructors
        public Gui(ContentManager Content, GraphicsDeviceManager graphics)
        {
            LoadContent(Content,graphics);
            CurrentGameState = GameState.Intro;
        }
        #endregion
        #region methodes 

        public void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            btstart = new Button(Content.Load<Texture2D>("Plateforme/Gui/start"), 770, 110);
            btjouer = new Button(Content.Load<Texture2D>("Plateforme/Gui/jouer"), 235, 40);
            btaide = new Button(Content.Load<Texture2D>("Plateforme/Gui/aide"), 180, 40);
            btquitter = new Button(Content.Load<Texture2D>("Plateforme/Gui/Quitter"), 320, 40);
            bthand = new Button(Content.Load<Texture2D>("Plateforme/Gui/Main"), 40, 40);
            bthand2 = new Button(Content.Load<Texture2D>("Plateforme/Gui/Main"), 40, 40);
            btNext = new Button(Content.Load<Texture2D>("Plateforme/Gui/suivant"), 100, 60);
            btBack = new Button(Content.Load<Texture2D>("Plateforme/Gui/retour"), 100, 60);

            scenario = Content.Load<Texture2D>("Plateforme/Gui/intro");
            titre = Content.Load<Texture2D>("Plateforme/Gui/titre");
            titre2 = Content.Load<Texture2D>("Plateforme/Gui/titre2");
            main_background = Content.Load<Texture2D>("Plateforme/Gui/background");

            help_background_1 = Content.Load<Texture2D>("Plateforme/Gui/background aide_1");
            help_background_2 = Content.Load<Texture2D>("Plateforme/Gui/background aide_2");
            help_background_3 = Content.Load<Texture2D>("Plateforme/Gui/background aide_3");

            positionX = ((graphics.PreferredBackBufferWidth / 2) - 110);
            positionY = ((graphics.PreferredBackBufferHeight / 2) - 65);

            positionX2 = 20;
            positionY2 = 435;
            btstart.setPosition(new Vector2(graphics.PreferredBackBufferWidth / 5, (graphics.PreferredBackBufferHeight / 2) - 50));
            btjouer.setPosition(new Vector2((graphics.PreferredBackBufferWidth / 2) - 120, (graphics.PreferredBackBufferHeight / 2) - 140));
            btaide.setPosition(new Vector2((graphics.PreferredBackBufferWidth / 2) - 80, (graphics.PreferredBackBufferHeight / 2) - 55));
            btquitter.setPosition(new Vector2((graphics.PreferredBackBufferWidth / 2) - 150, (graphics.PreferredBackBufferHeight / 2) + 30));
            bthand.setPosition(new Vector2(positionX, positionY));
            bthand2.setPosition(new Vector2(positionX2, positionY2));
            btNext.setPosition(new Vector2(750, 450));
            btBack.setPosition(new Vector2(20, 450));

            IsPlaying = false;
            // SOUND LOADING 
            buttonSound = Content.Load<SoundEffect>("Sounds/Gui/Buttonsound_16");

            #region FMOD Song 
            FmodFactory.Instance.load("Main_THEME_song", ref main_Theme_SongEvent);
            #endregion
        }

        public void Update(bool setMainMenuGameState)
        {

            FmodFactory.Instance.start(ref main_Theme_SongEvent);

            if (setMainMenuGameState)
                CurrentGameState = GameState.MainMenu;

            KeyboardState key = Keyboard.GetState();

            switch (CurrentGameState) {


                case GameState.Intro :

                    btstart.Update(key);

                    if (btstart.isClicked == true && key.IsKeyDown(Keys.Enter))
                    {
                        isDownE = false;
                        CurrentGameState = GameState.StartMenu;
                    }

                break;

                case GameState.StartMenu:

                    btstart.Update(key);

                    if (btstart.isClicked == true && key.IsKeyDown(Keys.Enter) && isDownE)
                    {
                        isDownE = false;
                        CurrentGameState = GameState.MainMenu;  
                    }

                    if (key.IsKeyUp(Keys.Enter))
                        isDownE = true;
                    
                break;

                case GameState.MainMenu:

                btBack.isSelected = false;
                // Here we fixe UI bug's 
         

                if (key.IsKeyDown(Keys.Enter) && isDownE )
                {
                    bthand.setPosition(new Vector2(positionX, positionY));
                    btjouer.isSelected = true;
                    isDownE = false;
                    buttonSound.Play();
                }

                if (key.IsKeyUp(Keys.Enter))
                    isDownE = true;

                // TESTING FOR EVERY SELECTION COMBINAISON
                if (key.IsKeyDown(Keys.Down) && !isDownD && bthand.position.X == positionX && bthand.position.Y == positionY)
                {
                    bthand.setPosition(new Vector2(positionX + 25, positionY + 45));
                          
                    btjouer.isSelected = false; 
                    btaide.isSelected = true;
                    btquitter.isSelected = false;

                    isDownD = true;
                    buttonSound.Play();
                }

                else if (key.IsKeyDown(Keys.Down) && !isDownD )
                {                     
                    bthand.setPosition(new Vector2(positionX -15, positionY + 95));
                    btjouer.isSelected = false; 
                    btaide.isSelected = false;
                    btquitter.isSelected = true;

                    isDownD = true;
                    buttonSound.Play();
                }
                if (key.IsKeyUp(Keys.Down) && isDownD) isDownD = false;

                if (key.IsKeyDown(Keys.Up) && !isDownU && bthand.position.X == positionX - 15 && bthand.position.Y == positionY + 95)
                {
                        
                    bthand.setPosition(new Vector2(positionX + 25, positionY + 45));

                    btjouer.isSelected = false; 
                    btaide.isSelected = true;
                    btquitter.isSelected = false;

                    isDownU = true;
                    buttonSound.Play();
                }

                else if (key.IsKeyDown(Keys.Up) && !isDownU && bthand.position.X == (positionX + 25) && bthand.position.Y == (positionY + 45))
                {
                        
                    bthand.setPosition(new Vector2(positionX, positionY));

                    btjouer.isSelected = true;
                    btaide.isSelected = false;
                    btquitter.isSelected = false;

                    isDownU = true;
                    buttonSound.Play();
                }

                if (key.IsKeyUp(Keys.Up) && isDownU) isDownU = false;
                    bthand.Update(key);
               
                if (btjouer.isSelected == true && key.IsKeyDown(Keys.Enter)) 
                    CurrentGameState = GameState.Playing;

                if (btaide.isSelected == true && key.IsKeyDown(Keys.Enter) )
                    CurrentGameState = GameState.Aide;

                if (btquitter.isSelected == true && key.IsKeyDown(Keys.Enter))
                    Environment.Exit(0);

                break;

                case GameState.Playing:
                #region FMOD
                this.IsPlaying = true;
                    FmodFactory.Instance.stop(ref main_Theme_SongEvent);
                #endregion
                break;

                case GameState.Aide: 

                    btjouer.isSelected = false;
                    btaide.isSelected =   false;
                    btquitter.isSelected = false;

                    if (key.IsKeyDown(Keys.Enter) && isDownE && bthand2.position.X == positionX2 && bthand2.position.Y == positionY2 && help_it == 0)
                    {
                        CurrentGameState = GameState.MainMenu;
                       // btBack.isSelected = true;
                        isDownE = false;
                        buttonSound.Play();
                    }
                    if (key.IsKeyUp(Keys.Enter) && isDownE) isDownE = true;


                    if (key.IsKeyDown(Keys.Right) && !isDownR && bthand2.position.X == positionX2 && bthand2.position.Y == positionY2)
                    {
                     
                        bthand2.setPosition(new Vector2(positionX2 + 550, positionY2 ));
       
                        btNext.isSelected = true;
                        btBack.isSelected = false;
                        isDownR = true;
                        buttonSound.Play();
                    }
                    if (key.IsKeyUp(Keys.Down) && isDownR) isDownR = false;

                    if (key.IsKeyDown(Keys.Left) && !isDownL && bthand2.position.X == positionX2 + 550 && bthand2.position.Y == positionY2)
                    {
                        bthand2.setPosition(new Vector2(positionX2, positionY2));

                        btNext.isSelected = false;
                        btBack.isSelected = true;
                        isDownL = true;
                        buttonSound.Play();
                    }
                    if (key.IsKeyUp(Keys.Down) && isDownL) isDownL = false;


                    if (btNext.isSelected == true && key.IsKeyDown(Keys.Enter) && isDownE && help_it < help_max-1)
                    {
                        help_it++;
                        isDownE = false;
                        buttonSound.Play();
                    }

                    if (key.IsKeyUp(Keys.Enter))
                        isDownE = true;

                    if (btBack.isSelected == true && key.IsKeyDown(Keys.Enter) && isDownE && help_it >= 0)
                    {
                        help_it--;
                        isDownE = false;
                        buttonSound.Play();
                    }

                    if (key.IsKeyUp(Keys.Enter))
                        isDownE = true;

                    if (btBack.isSelected == true && key.IsKeyDown(Keys.Enter) && help_it < 0)
                    {
                        CurrentGameState = GameState.MainMenu;
                        help_it = 0;
                    }
                    
                             bthand2.Update(key);
               break;


            }
        }
        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
          
            spriteBatch.Begin();   
            switch (CurrentGameState)
            {
                case GameState.Intro :
                    spriteBatch.Draw(scenario, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                break;
                case GameState.StartMenu:   
                    spriteBatch.Draw(titre, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White); 
                break;

                case GameState.MainMenu:
                    // We Draw The Main Menu GUI Components
                    spriteBatch.Draw(titre2, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btjouer.Draw(spriteBatch);
                    btaide.Draw(spriteBatch);
                    btquitter.Draw(spriteBatch);
                    bthand.Draw(spriteBatch);

                break;
                case GameState.Aide:
                    if(help_it ==0)
                    spriteBatch.Draw(help_background_1, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    if(help_it ==1)
                    spriteBatch.Draw(help_background_2, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    if (help_it == 2)
                    spriteBatch.Draw(help_background_3, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
           
                    btNext.Draw(spriteBatch);
                    btBack.Draw(spriteBatch);
                    bthand2.Draw(spriteBatch);
                break;
            }
            spriteBatch.End();

        }
        #endregion
    }
}
