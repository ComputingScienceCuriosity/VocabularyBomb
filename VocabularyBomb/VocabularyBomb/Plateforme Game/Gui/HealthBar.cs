using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;


namespace Vocabulary
{
   public class HealthBar
    {
        private Texture2D container, container2, lifeBar;
        private Vector2 position;

        private SoundEffect healthImpactSound; 

        int m_value = 0;
        public int fullHealth;
        private Color barColor;

        Player player;

        public bool isHealthImpactSound = false;

        public int CurrentHealth 
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }
        int currentHealth;

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

        public HealthBar(ContentManager Content)
        {
            position = new Vector2(10, 10);
            LoadContent(Content);
            fullHealth = 150;
            currentHealth = fullHealth;
        }

        public HealthBar(ContentManager Content, Vector2 Position) 
        {
            this.IsDecreasing = false;
            this.IsIncreasing = false;
            this.Maximum = 150;
            this.Minimum = 0;
            this.position = Position;
            LoadContent(Content);
            fullHealth = 150;
            currentHealth = fullHealth;


        }

        
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

            this.IsIncreasing = false;
        }
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

            this.IsDecreasing = false;
        }
        public void HealthColor(int h)
        {
            if (h >= lifeBar.Width * 0.666) 
                barColor = Color.MediumSeaGreen;           
           
            else if (h >= lifeBar.Width * 0.333)
                barColor = Color.LightGoldenrodYellow;
            
            else if(h > 0)
                barColor = Color.Firebrick;

        }

        public void OnHealthImpact() 
        {
                healthImpactSound.Play();            
        }

        public void HealthColor()
        {
            if (Value >= lifeBar.Width * 0.666)
                barColor = Color.MediumSeaGreen;

            else if (Value >= lifeBar.Width * 0.333)
            
                barColor = Color.LightGoldenrodYellow;          
            else
                barColor = Color.Firebrick;
        }

        public void resetHealth() 
        {
            currentHealth = fullHealth;
        }

        public void LoadContent(ContentManager Content) 
        {
            healthImpactSound = Content.Load<SoundEffect>("Sounds/Triggers/healthImpact");

            container = Content.Load<Texture2D>("Plateforme/Health/lifeBar");
            container2 = Content.Load<Texture2D>("Plateforme/Health/lifeBar2");
            lifeBar = Content.Load<Texture2D>("Plateforme/Health/healthGauge");

            player = new Player();
        }

        public void Update( )
        {
            HealthColor();
        }
        public void Update(int h)
        {
            HealthColor(h);
        }

        public void Draw(SpriteBatch spriteBatch,int C)
        {
            spriteBatch.Draw(container2, this.position, Color.White);
            spriteBatch.Draw(lifeBar, position, new Rectangle(0, 0, C , lifeBar.Height), barColor);
            spriteBatch.Draw(container, position, Color.White);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(container2, this.position, Color.White);
            spriteBatch.Draw(lifeBar, position, new Rectangle(0, 0, Value, lifeBar.Height), barColor);
            spriteBatch.Draw(container, position, Color.White);

        }
    }
}
