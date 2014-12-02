#region File Description
//-----------------------------------------------------------------------------
// VocabularyBomb.cs
//--------------------
// IMAGINA TEAM
// 2013-2014
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using Voca.Plateforme_Game.LevelManager;
using VocabularyBomb.Plateforme_Game.Sound_Player;
using Microsoft.Xna.Framework.Audio;
using VocabularyBomb;
using FMOD;
namespace Vocabulary
{

    public class VocabularyBomb : Microsoft.Xna.Framework.Game
    {
        #region Variables 
        private SoundEffect fallSound;
        private SoundEffect gameOverSound;

        public FMOD.EventParameter Position { set; get; }FMOD.EventParameter position = null;
        public FMOD.Event GemEvent { set; get; } FMOD.Event gemEvent = null;
        #region FMOD
        public FmodFactory fmodtest;
        #endregion 


        // Primary Ressources 
        Gui        gui;
        Pause      pause;
        HealthBar  health;
        Player     player;
        Credits    credits;

        // Ressource to fix the Key.DOWN infinite case problem
        bool isDownD, isDownL = true; 
        // Used to Control when the pause is lunched
        private bool canUsePause = false;
        // Used to Change the type of game
        private bool isColorGame = true;
        // Used to Lunch our leitner gui
        private bool setMainMenuGameState = false; 


        // Ressources for screen loading
        LoadingScreen screenLoad;
        // Timer used for Game Over Overlay
        public TimeSpan timerGO = TimeSpan.FromSeconds(0) ;
        // Timer used to fix the latences between the gui and the game
        public TimeSpan timerSwitchGame = TimeSpan.FromSeconds(0);
        // Number of lifes
        private uint lifeMax = 5;
        private uint life;

        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Global content.
        private SpriteFont hudFont;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;
        private Texture2D diedOverlay;
        private Texture2D gameoverOverlay;

        private bool LoadingFinished
        {
            get;
            set;
        }bool loadingFinished = false;

        private static bool PlaySoundOnce
        {
            get;
            set;
        }bool playSoundOnce = false;
        bool playSoundOnce2 = false;
        // Meta-level game state.
        private int levelIndex = -1;
        private Level level;
        private bool wasContinuePressed;

        // When the time remaining is less than the warning time, it blinks on the hud
        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        private AccelerometerState accelerometerState;
        
        // The number of levels in the Levels directory of our content.
        private const int numberOfLevels = 4;
        #endregion

        #region Properties
        private bool LeitnerFinished
        {
            get;
            set;
        }bool leitnerFinished = false;
      
        #endregion

        #region Constructors
        public VocabularyBomb()
        {    
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;
            
            Content.RootDirectory = "Content";

            Accelerometer.Initialize();
        }
        #endregion

        #region Methodes
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
           
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
          
       
            // Load fonts
            hudFont = Content.Load<SpriteFont>("Fonts/Hud");

            // Load overlay textures
            winOverlay = Content.Load<Texture2D>("Plateforme/Overlays/you_win");
            loseOverlay = Content.Load<Texture2D>("Plateforme/Overlays/you_lose");
            diedOverlay = Content.Load<Texture2D>("Plateforme/Overlays/you_died");
            gameoverOverlay = Content.Load<Texture2D>("Plateforme/Overlays/game_over");

         
            gui =     new Gui(Content, graphics);
            pause = new Pause(Content, graphics);
            screenLoad = new LoadingScreen(Content, graphics, numberOfLevels);
            health = new HealthBar(Content);
            player = new Player();
            credits = new Credits(Content, graphics);

            gameOverSound = Content.Load<SoundEffect>("Sounds/GameOver/game-over");
            fallSound = Content.Load<SoundEffect>("Sounds/Player/PlayerFall");
            LoadNextLevel(this);

            life = lifeMax;

        }


        public void LeitnerFmodSound() 
        {
            switch (this.level.LeitnerGame.NbVocabulary)
            {
                case 4:
                    #region FMOD
                    level.LeitnerGame.BombWickFmodSound(TimeSpan.FromSeconds(60),ref level.LeitnerGame.bombWickEvent, ref  level.LeitnerGame.bombWick_fmod_param);
                    #endregion
                    break;
                case 9:
                    #region FMOD
                    level.LeitnerGame.BombWickFmodSound(TimeSpan.FromSeconds(90),ref level.LeitnerGame.bombWickEvent_2, ref level.LeitnerGame.bombWick_fmod_param_2);
                    #endregion
                    break;
                case 16:
                    #region FMOD
                    level.LeitnerGame.BombWickFmodSound(TimeSpan.FromSeconds(120), ref level.LeitnerGame.bombWickEvent_3, ref level.LeitnerGame.bombWick_fmod_param_3);
                    #endregion
                    break;
                case 25:
                    #region FMOD
                    level.LeitnerGame.BombWickFmodSound(TimeSpan.FromSeconds(160), ref level.LeitnerGame.bombWickEvent_4, ref level.LeitnerGame.bombWick_fmod_param_4);
                    #endregion
                    break;
            }
        
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (level.LeitnerIsOn)
                level.ChangeLevelsVol(0.2f);
            else
                level.ChangeLevelsVol(1.0f);

            // Allows the default game to exit on Xbox 360 and Windows
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (!gui.IsPlaying)
            {
                gui.Update(setMainMenuGameState);
            }
            else
            {
                
                if (!pause.IsPaused && levelIndex < numberOfLevels)
                {
                    #region FMOD levels Songs
                    level.playFmodSong(this.levelIndex);
                    #endregion
                    #region FMOD Door SOUND
                    if (level.inventory.isInventoryFull() && level.LeitnerGame.Result == 2)
                    {
                        level.playerPositionTowardDoor(this.levelIndex);
                    }
                    #endregion
          
                    if (!level.LeitnerIsOn && LoadingFinished)
                    {
                        #region Plateforme Update
                        // Handle polling for our input and handling high-level input
                        HandleInput();
                        // Allow to switch to the next level easilly
                        CheatsUpdate();
                        // update our level, passing down the GameTime along with all of our input states
                        level.Update(gameTime, keyboardState, gamePadState, touchState,
                                     accelerometerState, Window.CurrentOrientation);
                        UpdateSnow(3);
                        pause.Update();
                        #region FMOD Timer
                        level.OnTimerAlert(); 
                        #endregion
                        if (level.inventory.isInventoryFull() && level.LeitnerGame.Result == 2)
                        {
                            level.ChangeLevelsVol(0.5f);
                        }
                        #endregion
                    }
                    else
                    if (level.LeitnerIsOn)
                    {
                       
                        // Here We update our Leitner
                        CheatsLeitnerUpdate();
                        // Update MouseGame
                        #region Update MouseGame
                        level.LeitnerGame.Update(gameTime);
                        if (level.LeitnerGame.Result != 0)
                        {
                            level.LeitnerIsOn = false;
                            level.LeitnerGame.bombWickEvent.stop();
                            level.LeitnerGame.bombWickEvent_2.stop();
                            level.LeitnerGame.bombWickEvent_3.stop();
                            level.LeitnerGame.bombWickEvent_4.stop();
                        }
                        #endregion
                        
                    }

                    timerSwitchGame += TimeSpan.FromSeconds(1);
                }
                else pause.Update();

                if (levelIndex == numberOfLevels)
                {
                    #region FMOD Door SOUND
                    if (level.inventory.isInventoryFull() && level.LeitnerGame.Result == 2)
                    {
                        level.playerPositionTowardDoor(this.levelIndex);
                    }
                    #endregion
                    level.ChangeLevelsVol(0.0f);
                    credits.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Allows Acceleration of the snow weather.
        /// </summary>
        private void UpdateSnow(int ratio) 
        {
            if (levelIndex == 0)
            {
                if (level.TimeRemaining >= TimeSpan.FromSeconds(40))
                    this.level.SnowWeather.SnowAccelerate = ratio;
                else
                if (level.TimeRemaining <= TimeSpan.FromSeconds(30) && level.TimeRemaining >= TimeSpan.FromSeconds(20))
                    this.level.SnowWeather.SnowAccelerate = ratio + 2;
                else
                if (level.TimeRemaining <= TimeSpan.FromSeconds(20))
                {
                    this.level.SnowWeather.SnowDirection = true;

                    this.level.SnowWeather.SnowAccelerate = ratio + 4;
                }
            }
         }

        private void CreditsUpdate() 
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Enter))
                credits.IsCreditsExited = true;
        }

        /// <summary>
        /// Allows Cheat to Load the next Level.
        /// </summary>
        private void CheatsUpdate()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.O) && key.IsKeyDown(Keys.I) && key.IsKeyDown(Keys.P) && !isDownD)
            {
                LoadNextLevel(this);
                isDownD = true;
            }
            if (key.IsKeyUp(Keys.O) && key.IsKeyUp(Keys.I) && key.IsKeyUp(Keys.P) && isDownD) isDownD = false;

            if (key.IsKeyDown(Keys.U) && key.IsKeyDown(Keys.I) && key.IsKeyDown(Keys.L) && !isDownL)
            {
                this.level.inventory.cheatsIndMax = this.level.inventory.toFinishIndex;
                this.level.LeitnerGame.Result = 2;
                isDownL = true;
            }
            if (key.IsKeyUp(Keys.U) && key.IsKeyUp(Keys.I) && key.IsKeyUp(Keys.L) && isDownL) isDownL = false;

            if (key.IsKeyDown(Keys.F10)  && !isDownL)
            {
                this.level.TimeRemaining = TimeSpan.FromSeconds(30.0);
                isDownL = true;
            }
            if (key.IsKeyUp(Keys.F10)  && isDownL) isDownL = false;
        }
        private void CheatsLeitnerUpdate()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.F11) && !isDownL)
            {
                this.level.LeitnerGame.GameTimer.Time = TimeSpan.FromSeconds(0.0);
                this.level.LeitnerGame.Time = TimeSpan.FromSeconds(0.0);
                this.level.LeitnerGame.GameTimer.Finished = true;
                this.level.LeitnerGame.MecheBomb.Finished = true;
                isDownL = true;
            }
            if (key.IsKeyUp(Keys.F11) && isDownL) isDownL = false;

        }
        /// <summary>
        /// Handles the Keyboard and GamePad inputs
        /// </summary>
        private void HandleInput()
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
            accelerometerState = Accelerometer.GetState();

            // Exit the game when back is pressed.
            if (gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

          
            bool continuePressed =
                keyboardState.IsKeyDown(Keys.Space) ||
                gamePadState.IsButtonDown(Buttons.A) ||
                touchState.AnyTouch();

            // Perform the appropriate action to advance the game and
            // to get the player back to playing.
            if (!wasContinuePressed && continuePressed)
            {
                if (!level.Player.IsAlive)
                {
                    this.life--;
                    if (this.life >= 1)
                    {
                        level.StartNewLife();
                        level.HealthPlayer = 150;
                    }
                    else
                        pause.ExitClicked = true;                 
                }
                else if (level.TimeRemaining == TimeSpan.Zero)
                {
                    if (level.ReachedExit)
                    {
                         
                        LoadNextLevel(this);
                        timerSwitchGame = TimeSpan.FromSeconds(0);
                    }
                    else if (life > 1)
                        ReloadCurrentLevel();
                    else if (life == 1)
                        Exit();
                }
            }

            wasContinuePressed = continuePressed;
        }
        /// <summary>
        /// Load the next level.
        /// </summary>
        private void LoadNextLevel(Game game)
        {
            LoadingFinished = false;
            isDownD = false;
            isDownL = false;
            // move to the next level
            levelIndex = (levelIndex + 1);//% numberOfLevels;
            // Unloads the content for the current level before loading the next one.
            if (level != null)
            {
                level.Dispose();
            }
            // Load the level.
            if (levelIndex < numberOfLevels)
            {
                if (isColorGame)
                {
                    string levelPath = string.Format("Content/Plateforme/Levels_colors/{0}.txt", levelIndex);
                    using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                        level = new Level(game, graphics, Services, fileStream, levelIndex, isColorGame);
                }
                else
                {
                    string levelPath = string.Format("Content/Plateforme/Levels_vocabulary/{0}.txt", levelIndex);
                    using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                        level = new Level(game, graphics, Services, fileStream, levelIndex, isColorGame);
                }
                level.HealthPlayer = 150;
            }
           
        }
        /// <summary>
        /// Reload's the Current Level
        /// </summary>
        private void ReloadCurrentLevel()
        {
          
            --levelIndex;
            health.resetHealth();
            LoadNextLevel(this);
            this.life--;
        }

        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!gui.IsPlaying)
            {
                gui.Draw(spriteBatch, graphics);
            }
            else
            if (gui.IsPlaying)
            {
                Loading();
                if (timerSwitchGame > TimeSpan.FromMinutes(2) || levelIndex == numberOfLevels)
                {
                    screenLoad.Draw(spriteBatch, graphics, true, levelIndex, numberOfLevels);
                    if (timerSwitchGame > TimeSpan.FromMinutes(6) || levelIndex == numberOfLevels)
                    {
                        LoadingFinished = true;
                        if (levelIndex == numberOfLevels)
                        {
                            credits.Draw(spriteBatch, graphics);
                            if (credits.IsCreditsExited)                           
                                pause.ExitClicked = true;
                        }
                        else
                        if (!level.LeitnerIsOn && levelIndex < numberOfLevels)
                        {
                            level.Draw(gameTime, spriteBatch);
                            DrawHud();
                            #region WeatherDraw
                            if (levelIndex == 0)
                                level.SnowWeather.Draw(spriteBatch, gameTime);
                            #endregion
                            canUsePause = true;
                            pause.Draw(spriteBatch);
                        }
                        else if (level.LeitnerIsOn && levelIndex < numberOfLevels)
                        {
                            level.LeitnerGame.Draw(spriteBatch);
                        }
                       
                        if (timerGO > TimeSpan.FromMinutes(2.4) || pause.ExitClicked)
                        {
                            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
                            canUsePause = false;
                            playSoundOnce = false;
                            playSoundOnce2 = false;
                           
                            life = lifeMax;
                            levelIndex = -1;
                            base.Initialize();
                            gui.Draw(spriteBatch, graphics);
                            timerSwitchGame = TimeSpan.FromSeconds(0);
                            timerGO = TimeSpan.FromSeconds(0);
                            LoadingFinished = false;
                        }
                        
                    }
                }
                
                
            }
            base.Draw(gameTime);
        }
        private void Loading() 
        {
            // Here we call our Loading routine
            #region LoadingScreen
            screenLoad.Draw(spriteBatch, graphics, false, 0);
            if (timerSwitchGame > TimeSpan.FromMinutes(0.5))
                screenLoad.Draw(spriteBatch, graphics, false, 1);
            if (timerSwitchGame > TimeSpan.FromMinutes(1.0))
                screenLoad.Draw(spriteBatch, graphics, false, 2);
            if (timerSwitchGame > TimeSpan.FromMinutes(1.5))
                screenLoad.Draw(spriteBatch, graphics, false, 3);
            #endregion
        }
        private void DrawHud()
        {
            spriteBatch.Begin();

            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.Height -140 , titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string timeString = "Temps: " + level.TimeRemaining.Minutes.ToString("00") + ":" + level.TimeRemaining.Seconds.ToString("00");
            Color timeColor;
            if (level.TimeRemaining > WarningTime ||
                level.ReachedExit ||
                (int)level.TimeRemaining.TotalSeconds % 2 == 0)
            {
                timeColor = Color.Khaki; 
            }
            else
            {
                timeColor = Color.Red;
            }
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

            // Draw score
            float timeHeight = hudFont.MeasureString(timeString).Y;
            DrawShadowedString(hudFont, "SCORE: " + level.Score.ToString(), hudLocation + new Vector2(300.0f,  1.2f), Color.PaleGreen);
            DrawShadowedString(hudFont, "Vies: x " + this.life.ToString(), new Vector2(10.0f, timeHeight * 1.2f + 10), Color.MidnightBlue);

            Texture2D status = null;
            
            // Determine the status overlay message to show.
            if (level.TimeRemaining == TimeSpan.Zero)
            {
                if (level.ReachedExit)
                {
                    status = winOverlay;
                }
                else
                {
                    if (life > 1)
                    {
                        level.ChangeLevelsVol(0.2f);
                        status = loseOverlay;
                    }
                    else
                    {
                        level.ChangeLevelsVol(0.2f);
                        status = gameoverOverlay;
                        timerGO += TimeSpan.FromSeconds(1);
                    }
                }
            }
            else if (!level.Player.IsAlive)
            {
                level.ChangeLevelsVol(0.2f);
                status = diedOverlay;
            }
            if ((life == 1 && !level.Player.IsAlive))
            {
                status = gameoverOverlay;
                timerGO += TimeSpan.FromSeconds(1);

            }

            #region SOUNDEFFECT Game over & Fall Sound
            if (status == gameoverOverlay)
            {
                if (!playSoundOnce)
                {
                    gameOverSound.Play();
                    playSoundOnce = true;
                }
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteBatch.Draw(status, center - statusSize / 2, Color.White);
            }
          
            if (status == loseOverlay)
            {
                if (!playSoundOnce2)
                {
                    this.fallSound.Play();
                    //if (level.Lvl4_SongEvent != null)
                    
                    playSoundOnce2 = true;
                }
            }
            #endregion

            if (status != null && status != gameoverOverlay)
            {

                // Draw status message.
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteBatch.Draw(status, center - statusSize / 2, Color.White);

            }
            spriteBatch.End();
            
        }


        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {    
            spriteBatch.DrawString(font, value, position, color);
        }
        #endregion
    }
}
