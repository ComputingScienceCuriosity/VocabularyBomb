using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Vocabulary
{
    class Inventory
    {
        #region Variables
        private SoundEffect soundInventoryFull;

        int indexVs = 0;
        public Gem[] items;
        public char[] toFinish;
        public string[] toDoTable;
        public int toFinishIndex, cheatsIndMax;
        private Texture2D container, container_quest;
        public readonly Color Color = Color.White;
        private Vector2 text_position = new Vector2(40,450);
        private Vector2 container_quest_position = new Vector2(20, 450);
        private Vector2 Objects_position = new Vector2(450, 450);

        // Global content.
        private SpriteFont hudFont,hudFont_small,comique_small;

        #endregion

        #region Constructors 
        public Inventory(ContentManager Content, char[] toFinish,string[] toDoObject, int toFinishIndex, bool isGameColor)
        {
            this.toFinishIndex = toFinishIndex;
            this.items = new Gem[toFinishIndex];
            this.toFinish = toFinish;
            this.toDoTable = toDoObject;
            LoadContent(Content, isGameColor);
        }
        public Inventory(ContentManager Content, char[] toFinish, int toFinishIndex, bool isGameColor)
        {
            this.toFinishIndex = toFinishIndex;
            this.items = new Gem[toFinishIndex];
            this.toFinish = toFinish;
           
            LoadContent(Content, isGameColor);
        }
        #endregion

        #region Methodes
        public void LoadContent(ContentManager Content, bool isGameColor)
        {
            // Load fonts
            soundInventoryFull = Content.Load<SoundEffect>("Sounds/Triggers/InventoryFull");

            hudFont = Content.Load<SpriteFont>("Fonts/ComiqueBig");
            hudFont_small = Content.Load<SpriteFont>("Fonts/Hud_small");
            comique_small = Content.Load<SpriteFont>("Fonts/Comique");
            container_quest = Content.Load<Texture2D>("Plateforme/Inventory/Inventory_container_quests");
            if(isGameColor)
                container = Content.Load<Texture2D>("Plateforme/Inventory/Inventory_container_Colors");
            else
                container = Content.Load<Texture2D>("Plateforme/Inventory/Inventory_container_Vocabulary");
        }


        public void OnInventoryFull() 
        {
            soundInventoryFull.Play();

        }


        public Gem getItem(int currentItem)
        {
            Gem gemtoReturn;
            gemtoReturn = this.items[currentItem];
            return gemtoReturn;
        }

        public bool gemCompare(Gem g)
        {
            for (int i = 0; i < this.toFinishIndex; i++)
            {
                if (g.type == toFinish[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void useItem(int currentItem)
        {
            Gem item = this.items[currentItem];
            if (item.Amount == 0) { return; }
            item.Amount--;
            if (item.Amount == 0)
            {
                this.items[currentItem] = null;
            }
        }

        public void addItem(Gem toAdd)
        {
            for (int i = 0; i < this.toFinishIndex; i++)
            {
                if (items[i] == null)
                {
                    if (toAdd.type == toFinish[i])
                    {
                        items[i] = toAdd;
                        return;
                    }
                }
            }
        }

        public int gemCompareInventoryList(Gem g)
        {
            for (int i = 0; i < this.toFinishIndex; i++)
            {
                if (g.type == toFinish[i])
                {
                    indexVs = i;
                    break;
                }
            }
            return indexVs;
        }


        public bool isInventoryFull()
        {
            uint indMax = 0;
            for (int i = 0; i < this.toFinishIndex; i++)
            {
                if (items[i] != null)
                {
                    indMax++;
                }
            }
            if (indMax == this.toFinishIndex || this.cheatsIndMax == this.toFinishIndex)
            {
                return true;
            }
            else
                return false;
        }

        private void DrawShadowedString(SpriteBatch spriteBatch,SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position, color);
        }

        public void DrawGoalText(GameTime gameTime, SpriteBatch spriteBatch, int i,int leitnerResult, bool error)
        {
            spriteBatch.Draw(container_quest, container_quest_position, Color.White);
            if (!error)
            {

                if (!this.isInventoryFull())
                {
                    DrawShadowedString(spriteBatch, hudFont, "Tu dois récupérer la couleur:", text_position, Color.White);
                    DrawShadowedString(spriteBatch, hudFont, toDoTable[i], Objects_position, Color.White);
                }

                if (this.isInventoryFull() && leitnerResult == 0)
                    DrawShadowedString(spriteBatch, comique_small, "Trouve puis désamorce la bombe.",
                    new Vector2(text_position.X+ 170,text_position.Y), Color.White);
               
                if(this.isInventoryFull() && leitnerResult == 2)
                    DrawShadowedString(spriteBatch, comique_small, "Bravo , maintenant sort vite par la porte.",
                    new Vector2(text_position.X+ 100,text_position.Y), Color.White);
           
            }else
                
                DrawShadowedString(spriteBatch, comique_small, "Attention : Mauvaise couleur.", text_position, Color.White);

          
        }


        public void DrawContainer(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            for (int i = 0; i < this.toFinishIndex; i++)
            {
             
                spriteBatch.Draw(container, new Vector2(10, 80 + i * container.Height), Color.White);
              
            }

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.toFinishIndex; i++)
            {
                if(items[i]!= null){
                    spriteBatch.Draw(container, new Vector2(10, 80 + i * items[i].texture.Height), Color.White);
                    spriteBatch.Draw(items[i].texture, new Vector2(10, 80 + i * items[i].texture.Height), new Rectangle(0, 0, items[i].texture.Width, items[i].texture.Height), Color.Azure);
                }
            }
        }

        public void DrawUnit(GameTime gameTime, SpriteBatch spriteBatch, int i)
        {
            if (items[i] != null)
            {
                spriteBatch.Draw(container, new Vector2(10, 80 + i * items[i].texture.Height), Color.White);
                spriteBatch.Draw(items[i].texture, new Vector2(10, 80 + i * items[i].texture.Height), new Rectangle(0, 0, items[i].texture.Width, items[i].texture.Height), Color.Azure);
            }
        }
        #endregion

    }
}
