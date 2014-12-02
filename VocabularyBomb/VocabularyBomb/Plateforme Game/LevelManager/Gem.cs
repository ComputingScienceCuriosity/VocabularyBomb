#region File Description
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using VocabularyBomb;

namespace Vocabulary
{
    /// <summary>
    /// A valuable item the player can collect.
    /// </summary>
    class Gem
    {

        public Texture2D texture,content_texture,content_texture_2;
        public Vector2 origin;
        private SoundEffect collectedSound;

       
        public static int pointValue = 50; // points given to the player for collecting a icon
         
        public int Amount 
        {
            get { return amount; }
            set { amount = value; }
        
        }int amount;

        public readonly Color Color = Color.White; // colour of the sprite

        // The gem is animated from a base position along the Y axis.
        private Vector2 basePosition;
        // Type of gems 'A..Z'
        public char type;
        public string name;
        public float bounce; // the icon will bounce

        public Level Level
        {
            get { return level; }
        }
        Level level;

        /// <summary>
        /// Gets the current position of this gem in world space.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return basePosition + new Vector2(0.0f, bounce);
            }
        }

        public Vector2 Position2
        {
            get
            {
                return basePosition + new Vector2(0.0f, bounce* 1 / 2);
            }
        }
        public Vector2 Position3
        {
            get
            {
                return basePosition + new Vector2(0.0f, -(bounce * 1 / 2));
            }
        }

        /// <summary>
        /// Gets a circle which bounds this gem in world space.
        /// </summary>
        public Circle BoundingCircle
        {
            get
            {
                return new Circle(Position, Tile.Width / 3.0f);
            }
        }

        /// <summary>
        /// Constructs a new gem.
        /// </summary>
        public Gem(Level level, Vector2 position, char type , string name, bool isColorGame)
        {
            //this.type = type;
            //this.pointValue = 50;
            this.level = level;
            this.basePosition = position;
            this.type = type;
            this.name = name;

            LoadContent(this.type,isColorGame);
        }
        /// <summary>
        /// Constructs a new gem.
        /// </summary>
        public Gem(Level level, Vector2 position, char type,  bool isColorGame)
        {
            //this.type = type;
            //this.pointValue = 50;
            this.level = level;
            this.basePosition = position;
            this.type = type;
            

            LoadContent(this.type, isColorGame);
        }

        /// <summary>
        /// Loads the gem texture and collected sound.
        /// </summary>
        public void LoadContent(char type, bool isColorGame)
        {
            content_texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Etoile");
            content_texture_2 = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Etoile_2");
            if (isColorGame)
            {
                switch (type)
                {
                    //case '0': texture = Level.Content.Load<Texture2D>("Help/Main");    break;
                    case 'B': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Black"); break;//Black
                    case 'W': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/White"); break;//White
                    case 'R': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Red"); break;//REd
                    case 'G': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Green"); break;//Green
                    case 'L': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Blue"); break;//Blue
                    case 'Y': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Yellow"); break;//Yellow
                    case 'C': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Cyan"); break;//Cyan
                    case 'M': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Magenta"); break;//Magenta
                    case 'O': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Orange"); break;//Orange
                    case 'P': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Pink"); break;//Pink
                    case 'g': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Gray"); break;//Gray
                    case 'b': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Brown"); break;//Brown
                    case 'V': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Violet"); break;//Violet
                    case 'i': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Beige"); break;//Beige
                    case 'T': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Turquoise"); break;//Turquoise
                    case 'E': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Emerald"); break;//Emerald
                    case 'U': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Burgundy"); break;//Burgundy
                    case 'N': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Navy"); break;//Navy
                    case 'u': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Mauve"); break;//Mauve
                    case 'H': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Khaki"); break;//Khaki
                    case 'r': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Ruby"); break;//Ruby
                    case 'n': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Salmon"); break;//Salmon
                    case 'o': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Gold"); break;//Gold
                    case 'S': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Silver"); break;//SILVER
                    case 'Z': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/Colors/Bronze"); break;//BRONZE

                }
            }
            else 
            {
                switch (type)
                {
                    //case '0': texture = Level.Content.Load<Texture2D>("Help/Main");           break;
                    case 'A': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam A"); break;
                    case 'B': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam B"); break;
                    case 'C': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam C"); break;
                    case 'D': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam D"); break;
                    case 'E': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam E"); break;
                    case 'F': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam F"); break;
                    case 'G': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam G"); break;
                    case 'H': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam H"); break;
                    case 'I': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam I"); break;
                    case 'J': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam J"); break;
                    case 'K': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam K"); break;
                    case 'L': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam L"); break;
                    case 'M': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam M"); break;
                    case 'N': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam N"); break;
                    case 'O': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam O"); break;
                    case 'P': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam P"); break;
                    case 'Q': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam Q"); break;
                    case 'R': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam R"); break;
                    case 'S': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam S"); break;
                    case 'T': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam T"); break;
                    case 'U': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam U"); break;
                    case 'V': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam V"); break;
                    case 'W': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam W"); break;
                    case 'X': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam X"); break;
                    case 'Y': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam Y"); break;
                    case 'Z': texture = Level.Content.Load<Texture2D>("Plateforme/Sprites/Gem/steam Z"); break;
                }
            }
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            collectedSound = Level.Content.Load<SoundEffect>("Sounds/Triggers/gemCollected"); // change of sound effects
        }

        /// <summary>
        /// Bounces up and down in the air to entice players to collect them.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Bounce control constants
            const float BounceHeight = 0.12f; // how high it bounces
            const float BounceRate = 3.0f;    // how often it bounces
            const float BounceSync = -0.75f;  // in sync with other icons

            // Bounce along a sine curve over time.
            // Include the X coordinate so that neighboring gems bounce in a nice wave pattern.            
            double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceHeight * texture.Height;
        }

        /// <summary>
        /// Called when this gem has been collected by a player and removed from the level.
        /// </summary>
        /// <param name="collectedBy">
        /// The player who collected this gem. Although currently not used, this parameter would be
        /// useful for creating special powerup gems. For example, a gem could make the player invincible.
        /// </param>
        /// <param name="collectedGemIndex">
        /// </param>
        public void OnCollected(Player collectedBy)
        {
            // collectedSound.Play();
            // WE PLAY THE SOUND WITH FMOD 
            // the sound changes with the current index of the collected Gem(ITEM)
        }

        /// <summary>
        /// Draws a gem in the appropriate color.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(content_texture, Position2, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(content_texture_2, Position3, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texture, Position, null, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
           
        }
    }
}
