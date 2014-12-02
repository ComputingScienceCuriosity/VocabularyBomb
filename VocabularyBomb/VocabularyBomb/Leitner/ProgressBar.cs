using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;


namespace Vocabulary
{
   public class ProgressBar
   {
        #region Varibles
       int m_value = 0;
       #endregion

        #region Properties
       public Color BarColor
        {
            set;
            get;
        }
        public Vector2 Position
        {
            set;
            get;
        }
        public Texture2D LifeBar
        {
            set;
            get;
        }
        public Texture2D Container2
        {
            set;
            get;
        }
        public Texture2D Container
        {
            set;
            get;
        }
        public int CurrentHealth
        {
            get;
            set;
        }
        public int Value
        {
            get { return m_value; }
            set
            {
                if (Minimum <= value && value <= Maximum)
                {
                    m_value = value;
                }
            }
        }
        public int Minimum
        {
            get;
            set;
        }
        public int Maximum
        {
            get;
            set;
        }
        public bool IsDecreasing
        {
            get;
            set;
        }
        public bool IsIncreasing
        {
            get;
            set;
        }
        #endregion

        #region Constructeur(s)
        public ProgressBar(ContentManager Content, Vector2 Position, int minimum, int maximum, int currentValue) 
        {
            this.Maximum = maximum;
            this.Minimum = minimum;
            this.Position = Position;
            this.Container = Content.Load<Texture2D>("Leitner/ProgressBar/lifeBar");
            this.Container2 = Content.Load<Texture2D>("Leitner/ProgressBar/lifeBar2");
            this.LifeBar = Content.Load<Texture2D>("Leitner/ProgressBar/healthGauge");
            this.Value = currentValue;
            this.IsDecreasing = false;
            this.IsIncreasing = false;
        }
        #endregion

        #region Méthode Increase
        public void Increase(int amount)
        {
            this.IsIncreasing = true;

            if (amount <= (Maximum - Value))
            {
                Value += amount;
            }
            else
            {
                Value = Maximum;
            }
            this.LevelBarColor();
            this.IsIncreasing = false;
        }
        #endregion

        #region Méthode Decrease
        public void Decrease(int amount)
        {
            this.IsDecreasing = true;

            if (amount <= Value)
            {
                Value -= amount;
            }
            else
            {
                Value = Minimum;
            }
            this.LevelBarColor();
            this.IsDecreasing = false;
        }
        #endregion

        #region Méthode LevelBarColor
        public void LevelBarColor()
        {
            if (Value >= this.Maximum * 0.666)
                this.BarColor = Color.MediumSeaGreen;
            else if (Value >= this.Maximum * 0.333)
                this.BarColor = Color.LightGoldenrodYellow;
            else
                this.BarColor = Color.Firebrick;
        }
        #endregion

        #region Méthode Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Container2, this.Position, new Rectangle(0, 0, this.Maximum, this.Container2.Height) , Color.White);
            spriteBatch.Draw(this.LifeBar, this.Position, new Rectangle(0, 0, this.Value, this.LifeBar.Height), this.BarColor);
            spriteBatch.Draw(this.Container, this.Position, new Rectangle(0, 0, this.Maximum, this.Container.Height), Color.White);
        }
        #endregion
    }
}
