using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Vocabulary;

namespace Vocabulary
{
    class Pause
    {
        #region Variables
        Texture2D pausedTexture;
        Rectangle pausedRectangle;
        private bool isDownD, isDownU, isDownE = false;
        int positionX, positionY;
        Button btPlay, btExit, btHand;
        #endregion

        #region Properties
        public bool ExitClicked
        {
            set;
            get;
        }
        public bool IsPaused
        {
            set;
            get;
        }
        #endregion 

        #region Constructors
        public Pause(ContentManager Content, GraphicsDeviceManager graphics) 
        {
            LoadContent(Content, graphics);
          
        }
        #endregion

        #region Methodes
        public void LoadContent(ContentManager Content, GraphicsDeviceManager graphics) 
        {

            pausedTexture = Content.Load<Texture2D>("Plateforme/Pause/Paused");

            pausedRectangle = new Rectangle(0, 0, pausedTexture.Width, pausedTexture.Height);

            btPlay = new Button(Content.Load<Texture2D>("Plateforme/Pause/Play"), 350, 275);

            btExit = new Button(Content.Load<Texture2D>("Plateforme/Pause/Quit"), 350, 275);

            btHand = new Button(Content.Load<Texture2D>("Plateforme/Gui/Main"), 40, 40);

            positionX = (265);
            positionY = (230);


            btPlay.setPosition(new Vector2(350, 225));
            btExit.setPosition(new Vector2(350, 275));
            btHand.setPosition(new Vector2(positionX, positionY));
          
        }
       
        public void Update()
        {
            KeyboardState key = Keyboard.GetState();
            if (!IsPaused)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    IsPaused = true;
                    btPlay.isClicked = false;
                    isDownE = true;
                }
            }
            else if (IsPaused)
            {
                if (key.IsKeyDown(Keys.Enter) && isDownE )
                {
                   
                    btPlay.isSelected = true;
                    isDownE = false;
                }
                if (key.IsKeyDown(Keys.Down) && !isDownD && btHand.position.X == positionX && btHand.position.Y == positionY )
                {
                    btHand.setPosition(new Vector2(positionX + 20, positionY + 80));
                   
                    
                    btExit.isSelected = true;
                    btPlay.isSelected = false;
                    isDownD = true;  
                }

                else if (key.IsKeyDown(Keys.Up) && !isDownU && btHand.position.X == positionX + 20 && btHand.position.Y == positionY + 80)
                {
                    btHand.setPosition(new Vector2(positionX, positionY));
                   
                    btPlay.isSelected = true;
                    btExit.isSelected = false;
                    isDownU = true;
                }
                else if (key.IsKeyDown(Keys.Down) && !isDownD && btHand.position.X == positionX -20 && btHand.position.Y == positionY -80)
                {
                    btHand.setPosition(new Vector2(positionX, positionY + 80));


                    btExit.isSelected = true;
                    btPlay.isSelected = false;
                    isDownD = true;
                }
                if (key.IsKeyUp(Keys.Down) && isDownD) isDownD = false;
                if (key.IsKeyUp(Keys.Up)   && isDownU) isDownU = false;

                if (key.IsKeyUp(Keys.Up) && isDownU) isDownU = false;
                btHand.Update(key);

                if (btPlay.isSelected == true && key.IsKeyDown(Keys.Enter))
                    IsPaused = false;

                if (btExit.isSelected == true && key.IsKeyDown(Keys.Enter))
                    ExitClicked = true;
            } 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (IsPaused)
            {
                spriteBatch.Draw(pausedTexture, pausedRectangle, Color.White);
                btPlay.Draw(spriteBatch);
                btExit.Draw(spriteBatch);
                btHand.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        #endregion
    }
}
