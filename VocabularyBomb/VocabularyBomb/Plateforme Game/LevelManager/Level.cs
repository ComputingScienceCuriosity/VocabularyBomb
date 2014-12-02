#region File Description
#endregion

using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Voca.Plateforme_Game.LevelManager;
using Microsoft.Xna.Framework.Media;
using VocabularyBomb;

namespace Vocabulary
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    class Level : IDisposable
    {
        Vector2 pos = new Vector2(0, 0);
        bool enemyIsAlive = false;
        #region Variables
        #region FMOD
        // Events
        public FMOD.EventParameter Position_fmod_evt { set; get; }FMOD.EventParameter position_fmod_evt = null;
        public FMOD.Event GemEvent { set; get; } FMOD.Event gemEvent = null;
        #endregion FMOD
        public FMOD.EventParameter Time_fmod_evt { set; get; }FMOD.EventParameter time_fmod_evt = null;
        public FMOD.Event TimeEvent { set; get; } FMOD.Event timeEvent = null;

        public FMOD.EventParameter Enemy_distance_evt { set; get; }FMOD.EventParameter enemy_distance_evt = null;
        public FMOD.Event EnemyDangerEvent { set; get; } FMOD.Event enemyDangerEvent = null;
        // Levels_dynamic_musics
        public FMOD.EventParameter Door_distance_evt_1 { set; get; }FMOD.EventParameter door_distance_evt_1 = null;
        public FMOD.Event Lvl1_DynEvent { set; get; } FMOD.Event lvl1_DynEvent = null;

        public FMOD.EventParameter Door_distance_evt_2 { set; get; }FMOD.EventParameter door_distance_evt_2 = null;
        public FMOD.Event Lvl2_DynEvent { set; get; } FMOD.Event lvl2_DynEvent = null;

        public FMOD.EventParameter Door_distance_evt_3 { set; get; }FMOD.EventParameter door_distance_evt_3 = null;
        public FMOD.Event Lvl3_DynEvent { set; get; } FMOD.Event lvl3_DynEvent = null;

        public FMOD.EventParameter Door_distance_evt_4 { set; get; }FMOD.EventParameter door_distance_evt_4 = null;
        public FMOD.Event Lvl4_DynEvent { set; get; } FMOD.Event lvl4_DynEvent = null;
        // Theme SONGS
        public FMOD.Event Lvl1_SongEvent { set; get; } FMOD.Event lvl1_SongEvent = null;
        public FMOD.Event Lvl2_SongEvent { set; get; } FMOD.Event lvl2_SongEvent = null;
        public FMOD.Event Lvl3_SongEvent { set; get; } FMOD.Event lvl3_SongEvent = null;
        public FMOD.Event Lvl4_SongEvent { set; get; } FMOD.Event lvl4_SongEvent = null;
        #endregion 
        public Game Game
        {
            set;
            get;
        }
        public Song level1;
        private SoundEffect doorClosedSound, doorOpenSound;

        private bool isDoorClosedSound, isBombClosedSound = false;
        private bool isDoorOpenSound    = false;
        private bool vocabularyColor    = false;
        private bool error              = false;
        private bool drawFlash          = false;
        private bool removedGem         = false;
        private bool isBlackCollected   = false;
        private int  itemIndex          = 0;
        private int  TodoIndex          = 0;
        private int  error_pan_index    = 0;  
        KeyboardState key;
        HealthBar  health;

        // Once the player is touched by an ennemy we call our timer
        public TimeSpan timer           = TimeSpan.FromSeconds(0.0);

        public char[] toFinishInventory;
        public string[] toDoObjects;

        // Physical structure of the level.
        private Tile[,] tiles;

        // We go seek the exit tile to draw the blinking effects on it ...
        private int xCollectMsg, yCollectMsg    = 0;
        private int xExit,yExit                 = 0;
        private int xBox, yBox                  = 0;
        private int xHand, yHand                = 0;
        private int xLeit, yLeit                = 0;
        private Layer[] layers;

        // The layer which entities are drawn on top of.
        private const int EntityLayer           = 2;
        private int levelIndex = 0;
        // Inventory controlers 
        public TimeSpan  timerErr = TimeSpan.FromSeconds(0);
        public Inventory inventory;

        // Entities in the level.
        private List<Gem> gems = new List<Gem>();
        private List<Enemy> enemies = new List<Enemy>();

        // Key locations in the level.        
        private Vector2 start;
        private Vector2 doorPosition;
        private Point exit = InvalidPosition;
        private Point box = InvalidPosition;
        private Point BlackGem;
        private Point HelpEscape;
        private static readonly Point InvalidPosition = new Point(-1, -1);

        // Level game state.
        private Random random = new Random(354668); // Arbitrary, but constant seed
        private float cameraPosition;


        #region Properties

        public Player Player
        {
            get { return player; }
        }
        Player player;

        public int Score
        {
            get { return score; }
        }
        int score;

        public int HealthPlayer
        {
            get { return healthPlayer; }
            set { healthPlayer = value; }
        }
        int healthPlayer;
        
        public bool ReachedExit
        {
            get { return reachedExit; }
        }
        bool reachedExit;

        public TimeSpan TimeRemaining
        {
            get { return timeRemaining; }
            set { timeRemaining = value; } 
        }
        TimeSpan timeRemaining;

        private const int PointsPerSecond = 5;

        // Level content.        
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public bool LeitnerIsOn
        {
            get;
            set;
        }bool leitnerIsOn = false;

        public bool ExitedLeitner
        {
            get;
            set;
        }bool exitedLeitner = false;

        private SoundEffect exitReachedSound;

  
        

        //Le jeu Leitner.
        #region Déclaration LeitnerGame
        public LeitnerGame LeitnerGame
        {
            set;
            get;
        }
        #endregion
        public Snow SnowWeather
        {
            set;
            get;
        }
        #endregion
        #region Loading
        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public Level(Game game, GraphicsDeviceManager graphics, IServiceProvider serviceProvider, Stream fileStream, int levelIndex, bool isColorGame)
        {
            this.Game = game;
            // Create a new content manager to load content used just by this level.
            content = new ContentManager(serviceProvider, "Content");
            // Loading the level Contents
            LoadTiles(fileStream, isColorGame);

            this.levelIndex = levelIndex;
            // INIT FMOD 
            #region FMOD INIT
            FmodFactory.Instance.load("Gem", "position",ref gemEvent,ref position_fmod_evt);
            #endregion
            FmodFactory.Instance.load("Timer_Alert_Plateforme", "time", ref timeEvent, ref time_fmod_evt);
            FmodFactory.Instance.load("Level1_Dyn_Theme", "door_distance", ref lvl1_DynEvent, ref door_distance_evt_1);
            FmodFactory.Instance.load("Level2_Dyn_Theme", "door_distance", ref lvl2_DynEvent, ref door_distance_evt_2);
            FmodFactory.Instance.load("Level3_Dyn_Theme", "door_distance", ref lvl3_DynEvent, ref door_distance_evt_3);
            FmodFactory.Instance.load("Level4_Dyn_Theme", "door_distance", ref lvl4_DynEvent, ref door_distance_evt_4);

            FmodFactory.Instance.load("EnemyDanger", "distance", ref enemyDangerEvent,ref enemy_distance_evt);

            FmodFactory.Instance.load("Level1_THEME_song", ref lvl1_SongEvent);
            FmodFactory.Instance.load("Level2_THEME_song", ref lvl2_SongEvent);
            FmodFactory.Instance.load("Level3_THEME_song", ref lvl3_SongEvent);
            FmodFactory.Instance.load("Level4_THEME_song", ref lvl4_SongEvent);
            
            #endregion

            // Loading Sounds  
            doorClosedSound = this.Content.Load<SoundEffect>("Sounds/Triggers/doorClosed");
            doorOpenSound   = this.Content.Load<SoundEffect>("Sounds/Triggers/doorOpen");
         
            // Load background layer textures. For now, all levels must
            // use the same backgrounds and only use the left-most part of them.
            layers = new Layer[3];
            switch(levelIndex){

                case 0 :
                timeRemaining = TimeSpan.FromMinutes(2.0);
                layers[0] = new Layer(Content, "Plateforme/Backgrounds/level1/Layer0", 0.2f);   // speends at which the layers scroll. foreground scrolls the fastest
                layers[1] = new Layer(Content, "Plateforme/Backgrounds/level1/Layer1", 0.5f);   // this allows me to have 3 layers of background instead of the standard 1
                layers[2] = new Layer(Content, "Plateforme/Backgrounds/level1/Layer2", 0.8f);
               
                #region Instanciation LeitnerGame
                StartLeitnerGame(game);
                #endregion

                #region SnowWeather 
                this.SnowWeather = new Snow(game);
                this.SnowWeather.initialize();
                this.SnowWeather.LoadContent(Content);
                #endregion   


                break;
                case 1:
                timeRemaining = TimeSpan.FromMinutes(3.0);
                layers[0] = new Layer(Content, "Plateforme/Backgrounds/level2/Layer0", 0.2f);   // speends at which the layers scroll. foreground scrolls the fastest
                layers[1] = new Layer(Content, "Plateforme/Backgrounds/level2/Layer1", 0.5f);   // this allows me to have 3 layers of background instead of the standard 1
                layers[2] = new Layer(Content, "Plateforme/Backgrounds/level2/Layer2", 0.8f);
                #region Instanciation LeitnerGame
                StartLeitnerGame(game);
                #endregion

                break;
                case 2:
                timeRemaining = TimeSpan.FromMinutes(4.0);
                layers[0] = new Layer(Content, "Plateforme/Backgrounds/level3/Layer0", 0.2f);   // speends at which the layers scroll. foreground scrolls the fastest
                layers[1] = new Layer(Content, "Plateforme/Backgrounds/level3/Layer1", 0.5f);   // this allows me to have 3 layers of background instead of the standard 1
                layers[2] = new Layer(Content, "Plateforme/Backgrounds/level3/Layer2", 0.8f);
                #region Instanciation LeitnerGame
                StartLeitnerGame(game);
                #endregion
                break;
                case 3:
                timeRemaining = TimeSpan.FromMinutes(5.0);
                layers[0] = new Layer(Content, "Plateforme/Backgrounds/level4/Layer0", 0.2f);   // speends at which the layers scroll. foreground scrolls the fastest
                layers[1] = new Layer(Content, "Plateforme/Backgrounds/level4/Layer1", 0.5f);   // this allows me to have 3 layers of background instead of the standard 1
                layers[2] = new Layer(Content, "Plateforme/Backgrounds/level4/Layer2", 0.8f);
                #region Instanciation LeitnerGame
                StartLeitnerGame(game);
                #endregion

                break;
            }
            // Inventory Part
            if (isColorGame)
            {
                switch (levelIndex)
                {
                    case 0: toFinishInventory = new char[4] { 'B', 'W', 'R', 'G' };
                        toDoObjects = new string[4] { "Black", "White", "Red", "Green" };
                        inventory = new Inventory(Content, toFinishInventory , toDoObjects, 4, isColorGame);
                        break;

                    case 1: toFinishInventory = new char[5] { 'L', 'Y', 'M', 'C', 'O' };
                        toDoObjects = new string[5] { "Blue", "Yellow", "Magenta", "Cyan", "Orange" };
                        inventory = new Inventory(Content, toFinishInventory, toDoObjects, 5, isColorGame);
                        break;

                    case 2: toFinishInventory = new char[7] { 'P', 'g', 'b', 'V', 'i', 'T', 'E' };
                        toDoObjects = new string[7] { "Pink", "Gray", "Brown", "Violet", "Beige", "Turquoise","Emerald" };
                        inventory = new Inventory(Content, toFinishInventory,toDoObjects, 7, isColorGame);
                        break;

                    case 3: toFinishInventory = new char[9] { 'U', 'N', 'u', 'H', 'r', 'n', 'o', 'S', 'Z' };
                        toDoObjects = new string[9] { "Burgundy", "Navy", "Mauve", "Khaki", "Ruby", "Salmon", "Gold", "Silver", "Bronze"};
                        inventory = new Inventory(Content, toFinishInventory,toDoObjects, 9, isColorGame);
                        break;
                }
            }
            else
            {
                switch (levelIndex)
                {
                    case 0: toFinishInventory = new char[6] { 'Y', 'E', 'L', 'L', 'O', 'W' };

                        inventory = new Inventory(Content, toFinishInventory, 6, isColorGame);
                        break;

                    case 1: toFinishInventory = new char[5] { 'G', 'R', 'E', 'E', 'N' };

                        inventory = new Inventory(Content, toFinishInventory, 5, isColorGame);
                        break;

                    case 2: toFinishInventory = new char[6] { 'P', 'U', 'R', 'P', 'L', 'E' };

                        inventory = new Inventory(Content, toFinishInventory, 6, isColorGame);
                        break;

                    case 3: toFinishInventory = new char[6] { 'S', 'I', 'L', 'V', 'E', 'R' };

                        inventory = new Inventory(Content, toFinishInventory, 6, isColorGame);
                        break;
                }
            
            }                                                                        
        }

        /// <summary>
        /// Iterates over every tile in the structure file and loads its
        /// appearance and behavior. This method also validates that the
        /// file is well-formed with a player start point, exit, etc.
        /// </summary>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        private void LoadTiles(Stream fileStream, bool isColorGame)
        {
            // Load the level and ensure all of the lines are the same length.

            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            tiles = new Tile[width, lines.Count];

            // Loop over every tile position,
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    // We get our gem to campare with the player's bb 
                    if (tileType.ToString() == "B")
                    {
                        BlackGem = GetBounds(x, y).Center;
                    }

                    if (tileType.ToString() == "6")
                    {
                        xCollectMsg = x; yCollectMsg = y;
                        tiles[x, y] = LoadTile(tileType, x, y, isColorGame);
                    }else 
                    if (tileType.ToString() == "?")
                    {
                        xExit = x; yExit = y;
                        doorPosition.X = (x * 40)+40; doorPosition.Y = (y *40)+40;
                        tiles[x, y] = LoadTile(tileType, x, y, isColorGame);
                    }else
                    if (tileType.ToString() == "<")
                    {
                        xBox = x; yBox = y;
                        tiles[x, y] = LoadTile(tileType, x, y, isColorGame);
                    }else
                    if (tileType.ToString() == "0")
                    {
                        xHand = x; yHand = y;
                        tiles[x, y] = LoadTile(tileType, x, y, isColorGame);
                    }else
                    if(tileType.ToString() == "$")
                    {
                        xLeit = x; yLeit = y;
                        tiles[x, y] = LoadTile(tileType, x, y, isColorGame);
                    }
                    else
                        tiles[x, y] = LoadTile(tileType, x, y, isColorGame);
                }
            }

            // Verify that the level has a beginning and an end.
            if (Player == null)
                throw new NotSupportedException("A level must have a starting point.");
            if (exit == InvalidPosition)
                throw new NotSupportedException("A level must have an exit.");


        }

        /// <summary>
        /// Loads an individual tile's appearance and behavior.
        /// </summary>
        /// <param name="tileType">
        /// The character loaded from the structure file which
        /// indicates what should be loaded.
        /// </param>
        /// <param name="x">
        /// The X location of this tile in tile space.
        /// </param>
        /// <param name="y">
        /// The Y location of this tile in tile space.
        /// </param>
        /// <returns>The loaded tile.</returns>
        private Tile LoadTile(char tileType, int x, int y, bool isColorGame)
        {
            
            if(isColorGame){
            switch (tileType)
            {
                // Blank space
                case '.':
                    return new Tile(null, TileCollision.Passable);

                // Exit
                case '?':
                    return LoadExitTile(x, y);

                // Box to unlock
                case '<':
                    return LoadBoxTile(x, y);


                // Help Boxes 
                // Help Boxes Collect all gems
                
                case '$':   
                    return LoadHelpTile(x, y, "Main hidden");
                // Help Boxes Collect all gems
                case '+':
                    return LoadHelpTile(x, y, "EntrerOrdi");
                // Help Boxes Collect color gem
                case '6':
                    return LoadHelpTile(x, y, "Collectioner");
                // Help Boxes Jump
                case '7':
                    return LoadHelpTile(x, y, "Sauter");
                // Help Boxes Walk
                case '8':
                    return LoadHelpTile(x, y, "Marcher");
                // Help Boxes Kill
                case '9':
                    return LoadHelpTile(x, y, "Tuer");

                case '0': 
                    return LoadHelpTile(x, y, "Main hidden");
                    
                // Gem 'bLACK..25 COLORS'
                case 'B': return LoadGemTile(x, y, 'B',"Black", isColorGame);//Black
                case 'W': return LoadGemTile(x, y, 'W',"White", isColorGame);//White
                case 'R': return LoadGemTile(x, y, 'R',"Red", isColorGame);//REd
                case 'G': return LoadGemTile(x, y, 'G',"Green", isColorGame);//Green                                               
                case 'L': return LoadGemTile(x, y, 'L',"Blue", isColorGame);//Blue
                case 'Y': return LoadGemTile(x, y, 'Y',"Yellow", isColorGame);//Yellow
                case 'C': return LoadGemTile(x, y, 'C',"Cyan", isColorGame);//Cyan
                case 'M': return LoadGemTile(x, y, 'M',"Magenta", isColorGame);//Magenta
                case 'O': return LoadGemTile(x, y, 'O',"Orange", isColorGame);//Orange                                                     
                case 'P': return LoadGemTile(x, y, 'P',"Pink", isColorGame);//Pink
                case 'g': return LoadGemTile(x, y, 'g',"Gray", isColorGame);//Gray
                case 'b': return LoadGemTile(x, y, 'b',"Brown", isColorGame);//Brown
                case 'V': return LoadGemTile(x, y, 'V',"Violet", isColorGame);//Violet
                case 'i': return LoadGemTile(x, y, 'i',"Beige", isColorGame);//Beige
                case 'T': return LoadGemTile(x, y, 'T',"Turquoise", isColorGame);//Turquoise
                case 'E': return LoadGemTile(x, y, 'E',"Emerald", isColorGame);//Emerald
                case 'U': return LoadGemTile(x, y, 'U',"Burgundy", isColorGame);//Burgundy
                case 'N': return LoadGemTile(x, y, 'N',"Navy", isColorGame);//Navy
                case 'u': return LoadGemTile(x, y, 'u',"Mauve", isColorGame);//Mauve
                case 'H': return LoadGemTile(x, y, 'H',"Khaki", isColorGame);//Khaki
                case 'r': return LoadGemTile(x, y, 'r',"Ruby", isColorGame);//Ruby
                case 'n': return LoadGemTile(x, y, 'n',"Salmon", isColorGame);//Salmon
                case 'o': return LoadGemTile(x, y, 'o',"Gold", isColorGame);//Gold
                case 'S': return LoadGemTile(x, y, 'S',"Silver", isColorGame);//SILVER
                case 'Z': return LoadGemTile(x, y, 'Z',"Bronze", isColorGame);//BRONZE
                  
                  
                    
                // Floating platform
                case '-':
                    return LoadTile("BlockFloat0", TileCollision.Platform);

                // Various enemies
                case '2':
                    return LoadEnemyTile(x, y, "MonsterA");
                case '3':
                    return LoadEnemyTile(x, y, "MonsterB");
                case '4':
                    return LoadEnemyTile(x, y, "MonsterC");
                case '5':
                    return LoadEnemyTile(x, y, "MonsterD");

                // Platform block
                case '~':
                    return LoadTile("BlockPlat", TileCollision.Platform);

                // Passable block
                case ':':
                    return LoadTile("BlockPassable", TileCollision.Passable);

                // Player 1 start point
                case '1':
                    return LoadStartTile(x, y);

                // Impassable block 1 
                case '#':
                    return LoadTile("BlockE0", TileCollision.Impassable);
                
                // Impassable block 2
                case '{':
                    return LoadTile("BlockE1", TileCollision.Impassable);

                // Impassable block 3
                case '}':
                    return LoadTile("BlockE2", TileCollision.Impassable);
              
                // Impassable block 4
                case '|':
                    return LoadTile("BlockE3", TileCollision.Impassable);
               
                // Impassable block 5
                case '/':
                    return LoadTile("BlockE4", TileCollision.Impassable);

                // Impassable block 6
                case '&':
                    return LoadTile("BlockE5", TileCollision.Impassable);

                // Impassable block 7
                case '*':
                    return LoadTile("BlockE6", TileCollision.Impassable);
               
                // Impassable block 8
                case '@':
                    return LoadTile("BlockE7", TileCollision.Impassable);
               
                // Impassable block 9
                case '!':
                    return LoadTile("BlockE8", TileCollision.Impassable);
                

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
            }else 
            {
                switch (tileType)
                {
                    // Blank space
                    case '.':
                        return new Tile(null, TileCollision.Passable);

                    // Exit
                    case '?':
                        return LoadExitTile(x, y);
                    // Box to unlock
                    case '<':
                        return LoadBoxTile(x, y);

                    // Help Boxes 
                    // Help Boxes Collect all gems
                    case '$':
                        return LoadHelpTile(x, y, "Leitner");
                    // Help Boxes Collect all gems
                    case '+':
                        return LoadHelpTile(x, y, "EntrerOrdi");
                    // Help Boxes Collect color gem
                    case '6':
                    return LoadHelpTile(x, y, "Collectioner");
                    // Help Boxes Jump
                    case '7':
                        return LoadHelpTile(x, y,"Sauter");
                    // Help Boxes Walk
                    case '8':
                        return LoadHelpTile(x, y, "Marcher");
                    // Help Boxes Kill
                    case '9':
                        return LoadHelpTile(x, y, "Tuer");
                    case '0':
                        return LoadHelpTile(x, y, "Main hidden");

                    // Gem 'A..Z'TimeRemaining

                    case 'A': return LoadGemTile(x, y, 'A', isColorGame);
                    case 'B': return LoadGemTile(x, y, 'B', isColorGame);
                    case 'C': return LoadGemTile(x, y, 'C', isColorGame);
                    case 'D': return LoadGemTile(x, y, 'D', isColorGame);
                    case 'E': return LoadGemTile(x, y, 'E', isColorGame);
                    case 'F': return LoadGemTile(x, y, 'F', isColorGame);
                    case 'G': return LoadGemTile(x, y, 'G', isColorGame);
                    case 'H': return LoadGemTile(x, y, 'H', isColorGame);
                    case 'I': return LoadGemTile(x, y, 'I', isColorGame);
                    case 'J': return LoadGemTile(x, y, 'J', isColorGame);
                    case 'K': return LoadGemTile(x, y, 'K', isColorGame);
                    case 'L': return LoadGemTile(x, y, 'L', isColorGame);
                    case 'M': return LoadGemTile(x, y, 'M', isColorGame);
                    case 'N': return LoadGemTile(x, y, 'N', isColorGame);
                    case 'O': return LoadGemTile(x, y, 'O', isColorGame);
                    case 'P': return LoadGemTile(x, y, 'P', isColorGame);
                    case 'Q': return LoadGemTile(x, y, 'Q', isColorGame);
                    case 'R': return LoadGemTile(x, y, 'R', isColorGame);
                    case 'S': return LoadGemTile(x, y, 'S', isColorGame);
                    case 'T': return LoadGemTile(x, y, 'T', isColorGame);
                    case 'U': return LoadGemTile(x, y, 'U', isColorGame);
                    case 'V': return LoadGemTile(x, y, 'V', isColorGame);
                    case 'W': return LoadGemTile(x, y, 'W', isColorGame);
                    case 'X': return LoadGemTile(x, y, 'X', isColorGame);
                    case 'Y': return LoadGemTile(x, y, 'Y', isColorGame);
                    case 'Z': return LoadGemTile(x, y, 'Z', isColorGame);
                    
                    // Floating platform
                    case '-':
                        return LoadTile("BlockFloat0", TileCollision.Platform);

                    // Various enemies
                    case '2':
                        return LoadEnemyTile(x, y, "MonsterA");
                    case '3':
                        return LoadEnemyTile(x, y, "MonsterB");
                    case '4':
                        return LoadEnemyTile(x, y, "MonsterC");
                    case '5':
                        return LoadEnemyTile(x, y, "MonsterD");

                    // Platform block
                    case '~':
                        return LoadTile("BlockPlat", TileCollision.Platform);

                    // Passable block
                    case ':':
                        return LoadTile("BlockPassable", TileCollision.Passable);

                    // Player 1 start point
                    case '1':
                        return LoadStartTile(x, y);

                    // Impassable block 1 
                    case '#':
                        return LoadTile("BlockE0", TileCollision.Impassable);
                
                    // Impassable block 2
                    case '{':
                        return LoadTile("BlockE1", TileCollision.Impassable);

                    // Impassable block 3
                    case '}':
                        return LoadTile("BlockE2", TileCollision.Impassable);
              
                    // Impassable block 4
                    case '|':
                        return LoadTile("BlockE3", TileCollision.Impassable);
               
                    // Impassable block 5
                    case '/':
                        return LoadTile("BlockE4", TileCollision.Impassable);

                    // Impassable block 6
                    case '&':
                        return LoadTile("BlockE5", TileCollision.Impassable);

                    // Impassable block 7
                    case '*':
                        return LoadTile("BlockE6", TileCollision.Impassable);
               
                    // Impassable block 8
                    case '@':
                        return LoadTile("BlockE7", TileCollision.Impassable);
               
                    // Impassable block 9
                    case '!':
                        return LoadTile("BlockE8", TileCollision.Impassable);
                

                    // Unknown tile type character
                    default:
                        throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
                }
            }   
        }

        /// <summary>
        /// Creates a new tile. The other tile loading methods typically chain to this
        /// method after performing their special logic.
        /// </summary>
        /// <param name="name">
        /// Path to a tile texture relative to the Content/Tiles directory.
        /// </param>
        /// <param name="collision">
        /// The tile collision type for the new tile.
        /// </param>
        /// <returns>The new tile.</returns>
        private Tile LoadTile(string name, TileCollision collision)
        {
            return new Tile(Content.Load<Texture2D>("Plateforme/Tiles/" + name), collision);
        }
        private Tile LoadHelp(string name, TileCollision collision)
        {
            return new Tile(Content.Load<Texture2D>("Plateforme/Help/" + name), collision);
        }


        /// <summary>
        /// Loads a tile with a random appearance.
        /// </summary>
        /// <param name="baseName">
        /// The content name prefix for this group of tile variations. Tile groups are
        /// name LikeThis0.png and LikeThis1.png and LikeThis2.png.
        /// </param>
        /// <param name="variationCount">
        /// The number of variations in this group.
        /// </param>
        private Tile LoadVarietyTile(string baseName, int variationCount, TileCollision collision)
        {
            int index = random.Next(variationCount);
            return LoadTile(baseName + index, collision);
        }


        /// <summary>
        /// Instantiates a player, puts him in the level, and remembers where to put him when he is resurrected.
        /// </summary>
        private Tile LoadStartTile(int x, int y)
        {
            if (Player != null)
                throw new NotSupportedException("A level may only have one starting point.");

            start = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            player = new Player(this, start);
            health = new HealthBar(Content,new Vector2(10,10));

            return new Tile(null, TileCollision.Passable);
        }

        /// <summary>
        /// Remembers the location of the level's exit.
        /// </summary>
        private Tile LoadExitTile(int x, int y)
        {
            if (exit != InvalidPosition)
                throw new NotSupportedException("A level may only have one exit.");

            exit = GetBounds(x, y).Center;

            return LoadTile("Exit open", TileCollision.Passable);
        }

        private Tile LoadBoxTile(int x, int y)
        {
            if (box != InvalidPosition)
                throw new NotSupportedException("A level may only have one box.");

            box = GetBounds(x, y).Center;

            return LoadTile("Bomb close", TileCollision.Passable);
        
        }

        private Tile LoadHelpTile(int x, int y,string help)
        {
            HelpEscape = GetBounds(x, y).Center;
            return LoadHelp(help, TileCollision.Passable);
        }
        /// <summary>
        /// Instantiates an enemy and puts him in the level.
        /// </summary>
        private Tile LoadEnemyTile(int x, int y, string spriteSet)
        {
            Vector2 position = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            enemies.Add(new Enemy(this, position, spriteSet));

            return new Tile(null, TileCollision.Passable);
        }

        /// <summary>
        /// Instantiates a gem and puts it in the level.
        /// </summary>
        private Tile LoadGemTile(int x, int y, char gemIndex, bool isColorGame)
        {
            Point position = GetBounds(x, y).Center;
            gems.Add(new Gem(this, new Vector2(position.X, position.Y), gemIndex, isColorGame) );
             
            return new Tile(null, TileCollision.Passable);
        }
        /// <summary>
        /// Instantiates a gem and puts it in the level.
        /// </summary>
        private Tile LoadGemTile(int x, int y, char gemIndex,string name, bool isColorGame)
        {
            Point position = GetBounds(x, y).Center;
            gems.Add(new Gem(this, new Vector2(position.X, position.Y), gemIndex , name, isColorGame));

            return new Tile(null, TileCollision.Passable);
        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
        }

        
        #region Bounds and collision

        /// <summary>
        /// Gets the collision mode of the tile at a particular location.
        /// This method handles tiles outside of the levels boundries by making it
        /// impossible to escape past the left or right edges, but allowing things
        /// to jump beyond the top of the level and fall off the bottom.
        /// </summary>
        public TileCollision GetCollision(int x, int y)
        {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return TileCollision.Passable;

            return tiles[x, y].Collision;
        }

        /// <summary>
        /// Gets the bounding rectangle of a tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
        }

        /// <summary>
        /// Width of level measured in tiles.
        /// </summary>
        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        #endregion
        #region Update

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(
            GameTime gameTime, 
            KeyboardState keyboardState, 
            GamePadState gamePadState, 
            TouchCollection touchState, 
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            

                  key = Keyboard.GetState();
                // Pause while the player is dead or time is expired.
                if (!Player.IsAlive || TimeRemaining == TimeSpan.Zero)
                {
                    // Still want to perform physics on the player.
                    Player.ApplyPhysics(gameTime);

                }
                else if (ReachedExit)
                {
                    // Animate the time being converted into points.
                    int seconds = (int)Math.Round(gameTime.ElapsedGameTime.TotalSeconds * 100.0f);
                    seconds = Math.Min(seconds, (int)Math.Ceiling(TimeRemaining.TotalSeconds));
                    timeRemaining -= TimeSpan.FromSeconds(seconds);
                    score += seconds * PointsPerSecond;

                }
                else
                {
                    timeRemaining -= gameTime.ElapsedGameTime;
                    Player.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                    UpdateGems(gameTime);

                    // Falling off the bottom of the level kills the player.
                    if (Player.BoundingRectangle.Top >= Height * Tile.Height)
                    {
                        OnPlayerKilled(null);

                    }
                    UpdateEnemies(gameTime);

                    // The player has reached the exit if they are standing on the ground and
                    // his bounding rectangle contains the center of the exit tile. They can only
                    // exit when they have collected all of the gems.
                    
                    if (Player.IsAlive &&
                        Player.IsOnGround &&
                        Player.BoundingRectangle.Contains(exit) && LeitnerGame.Result == 2 && inventory.isInventoryFull())
                    {
                        OnExitReached();
                    }

                    #region BoxLeitnerExit
                    if (Player.IsAlive &&
                        Player.IsOnGround && 
                        Player.BoundingRectangle.Contains(box) &&  
                        LeitnerGame.Result == 2 && inventory.isInventoryFull())     
                    {
                        this.LeitnerIsOn = false;
                        this.ExitedLeitner = true;

                    }
                    if (Player.IsAlive &&
                        Player.IsOnGround &&
                        Player.BoundingRectangle.Contains(box) &&
                        LeitnerGame.Result == 1  && inventory.isInventoryFull())
                    {
                        LeitnerGame.Result = 0;
                        this.LeitnerIsOn = false;
                        this.ExitedLeitner = true;
                        player.OnKilled(null);
                    }
                    #endregion  
                    
                    #region BoxLeitnerEnter Succes
                    if (Player.IsAlive &&
                        Player.IsOnGround &&
                        Player.BoundingRectangle.Contains(box) 
                        && key.IsKeyDown(Keys.Enter)
                        &&  (LeitnerGame.Result == 0) && inventory.isInventoryFull())
                    {
                        this.LeitnerIsOn = true;
                    }
                    #endregion

                }

            // Clamp the time remaining at zero.
            if (timeRemaining < TimeSpan.Zero)
                timeRemaining = TimeSpan.Zero;

            health.Update(healthPlayer);
        }

        /// <summary>
        /// Animates each gem and checks to allows the player to collect them.
        /// Insert every collected gem in the inventory  
        /// </summary>
        private void UpdateGems(GameTime gameTime)
        {

            for (int i = 0; i < gems.Count; ++i)
            {
                Gem gem = gems[i];

                gem.Update(gameTime);
               
                if (gem.BoundingCircle.Intersects(Player.BoundingRectangle))
                { 
                    // We Test Our Inventory Heuristique if the gem exists in our inventory goal
                    if (inventory.gemCompare(gem) && gem.name == toDoObjects[TodoIndex])
                    {
                        TodoIndex++;
                        inventory.addItem(gem);
                        itemIndex = inventory.gemCompareInventoryList(gem);
                        gems.RemoveAt(i--);
                       
                        removedGem = true;
                      
                        OnGemCollected(gem, Player);
                        // We See if the inventory is fusll then the player wins a bonus score 
                        if (inventory.isInventoryFull())
                        {
                            #region Sound InventoryFull
                            ChangeLevelsVol(0.2f);
                            inventory.OnInventoryFull();
                            ChangeLevelsVol(1.0f);
                            #endregion                                                    
                        }
                    }
                    else
                    {
                            // If the player is not Right we will respawn him into the begin 
                            player.Reset(start);
                            FmodFactory.Instance.setParamValue(error_pan_index,ref LeitnerGame.wrongChoice_fmod_param);
                            this.LeitnerGame.wrongChoiceEvent.start();
                            error_pan_index++;
                            if (error_pan_index > 4) error_pan_index = 0;
                            error = true;
                    }
                }  
            }           
        }

        #region FMOD Song Manager
        public void playFmodSong(int levelIndex)
        {
            switch (levelIndex)
            { 
                case 0:
                    FmodFactory.Instance.start(ref lvl1_SongEvent);
                    break;
                case 1:
                    FmodFactory.Instance.start(ref lvl2_SongEvent);
                    break;
                case 2 :
                    FmodFactory.Instance.start(ref lvl3_SongEvent);
                    break;
                case 3:
                    FmodFactory.Instance.start(ref lvl4_SongEvent);
                    break;
                default:
                    FmodFactory.Instance.stop(ref lvl4_SongEvent);
                    break;

            }
        }
        public void ChangeLevelsVol(float vol)
        {
            switch (this.levelIndex)
            {
                case 0:
                    this.lvl1_SongEvent.setVolume(vol);
                    break;
                case 1:
                    this.lvl2_SongEvent.setVolume(vol);
                    break;
                case 2:
                    this.lvl3_SongEvent.setVolume(vol);
                    break;
                case 3:
                    this.lvl4_SongEvent.setVolume(vol);
                    break;
            }
        }
        #endregion
        #region FMOD Player Poistion Pan EFFECTS
        public void playerPositionTowardDoor(int levelIndex) 
        { 
            int x = player.PositionTowardsTarget(doorPosition);
            Vector2 v = player.getDistance(doorPosition);
            switch(levelIndex)
            {
                case 0:
                    if (x == 1)  //the target is on the left 
                    {
                        //Play the sound on the left  PAN using the distance
                        if(5000 +(int)v.X * 3 > 0)
                            FmodFactory.Instance.setParamValue(5000 +((int)v.X*3), ref door_distance_evt_1);
                
                    }else 
                    if(x == 2)  //the target is on the right
                        //Play the sound on the right PAN using the distance
                        if (5000 - (int)v.X  < 9946)
                            FmodFactory.Instance.setParamValue(5000 - ((int)v.X*3), ref door_distance_evt_1);

                    FmodFactory.Instance.start(ref lvl1_DynEvent);
                break;
                case  1:
                    if (x == 1)  //the target is on the left 
                    {
                        //Play the sound on the left  PAN using the distance
                        if(5000 +(int)v.X * 3 > 0)
                            FmodFactory.Instance.setParamValue(5000 +((int)v.X*3), ref door_distance_evt_2);
                
                    }else 
                    if(x == 2)  //the target is on the right
                        //Play the sound on the right PAN using the distance
                        if (5000 - (int)v.X  < 9946)
                            FmodFactory.Instance.setParamValue(5000 - ((int)v.X*3), ref door_distance_evt_2);

                    FmodFactory.Instance.start(ref lvl2_DynEvent);
                break;
                case 2:
                    if (x == 1)  //the target is on the left 
                    {
                        //Play the sound on the left  PAN using the distance
                        if(5000 +(int)v.X * 3 > 0)
                            FmodFactory.Instance.setParamValue(5000 +((int)v.X*3), ref door_distance_evt_3);
                
                    }else 
                    if(x == 2)  //the target is on the right
                        //Play the sound on the right PAN using the distance
                        if (5000 - (int)v.X  < 9946)
                            FmodFactory.Instance.setParamValue(5000 - ((int)v.X*3), ref door_distance_evt_3);

                    FmodFactory.Instance.start(ref lvl3_DynEvent);
                break;
                case 3:
                    if (x == 1)  //the target is on the left 
                    {
                        //Play the sound on the left  PAN using the distance
                        if(5000 +(int)v.X * 3 > 0)
                            FmodFactory.Instance.setParamValue(5000 +((int)v.X*3), ref door_distance_evt_4);
                
                    }else 
                    if(x == 2)  //the target is on the right
                        //Play the sound on the right PAN using the distance
                        if (5000 - (int)v.X  < 9946)
                            FmodFactory.Instance.setParamValue(5000 - ((int)v.X*3), ref door_distance_evt_4);

                    FmodFactory.Instance.start(ref lvl4_DynEvent);
                break;
                default :
                    FmodFactory.Instance.stop(ref lvl4_DynEvent);
                break;
            }

        }
        #endregion 

        private void playerPositionTowardEnnemy(Vector2 EnemyPosition) 
        {
            //int z = player.PositionTowardsTarget(EnemyPosition);
            //if(z == 1) //the target is on the left
            //if(z == 2) //the target is on the right
            Vector2 v = player.getDistance(EnemyPosition);
            if (400 - (int)v.X <= 0 )
                FmodFactory.Instance.stop(ref enemyDangerEvent);
            else
            if (v.Y < 100  && v.X < 400 && (400 - (int)v.X > 0))
            {
                FmodFactory.Instance.setParamValue(400 - (int)v.X, ref enemy_distance_evt);
                FmodFactory.Instance.start(ref enemyDangerEvent);
            }

            if(v.X > 400)
                 FmodFactory.Instance.stop(ref enemyDangerEvent);
            
        }

        private void OnEnemyKilled(Enemy enemy, Player killedBy)
        {
            enemy.OnKilled(killedBy);
        }

        /// <summary>
        /// Animates each enemy and allow them to kill the player.
        /// </summary>
        /// 
        protected bool IntersectsBottomTop(Rectangle r1, Rectangle r2) 
        {

            return (((r1.Right > r2.Left) && (r1.Left < r2.Right) && (r1.Bottom <= r2.Top) && (r1.Top > r2.Bottom)) ||
                    ((r1.Left < r2.Right) && (r1.Right > r2.Left) && (r1.Bottom <= r2.Top) && (r1.Top > r2.Bottom)) ||
                   ((r1.Right == r2.Right) && (r1.Left == r2.Left) && (r1.Bottom <= r2.Top) && (r1.Top > r2.Bottom)));

        }

        private void UpdateEnemies(GameTime gameTime)
        {

            timer += gameTime.ElapsedGameTime;
            // Blinking Effect on player 
            if ( Player.IsTouched && (int)(gameTime.TotalGameTime.TotalMilliseconds / 150) % 2 == 1)
            {
                drawFlash = true;
            }
            else 
                drawFlash = false;

            if ((timer >= TimeSpan.FromSeconds(2.0)))
            {
                drawFlash = false;
                MediaPlayer.Volume = 1.0f;
            }
            if (enemyIsAlive)
            {
                playerPositionTowardEnnemy(pos);
            }
            foreach (Enemy enemy in enemies)
            {
                enemyIsAlive = enemy.IsAlive;
                pos = enemy.Position;
               
                enemy.Update(gameTime);
                // Touching an enemy instantly kills the player or the enemy
                if (enemy.IsAlive && enemy.BoundingRectangle.Intersects(player.BoundingRectangle) && Player.IsOnGround )
                {
                    ChangeLevelsVol(0.2f);
                    Player.IsTouched = true;
                    if (Player.IsTouched && (timer >= TimeSpan.FromSeconds(2.0)))
                    {
                        #region Sound Player Touched/Impact
                        if (healthPlayer > 0)
                        {
                            Player.OnTouchedSound();
                        }
                        if (healthPlayer == 50 || healthPlayer == 100)
                        {
                            ChangeLevelsVol(0.2f);
                            health.OnHealthImpact();
                        }
                        #endregion
                        healthPlayer = healthPlayer - 20;
                        timer = TimeSpan.Zero;
                    }
                    if (healthPlayer <= 0 )
                    {
                        OnPlayerKilled(enemy);        // if the enemy is alive then he is not instantly killed , The player will be killed                        
                    }
                }
                
                else if (enemy.IsAlive && enemy.BoundingRectangle.Intersects(player.BoundingRectangle))
                {
                        OnEnemyKilled(enemy, player);
                        score += 10;
                }

                else if (enemy.IsAlive && enemy.BoundingRectangle.Intersects(player.MeleeRectangle))
                {
                    if ((Player.isAttacking))
                    {
                        OnEnemyKilled(enemy, player); // if the player is punching, and his rectangle 
                        score += 10;
                    }                                // interacts with the enemy's then the enemy will die
                }
            }
        }
        #region FMOD GemCOLLECTED 
        /// <summary>
        /// Called when a gem is collected.
        /// </summary>
        /// <param name="gem">The gem that was collected.</param>
        /// <param name="collectedBy">The player who collected this gem.</param>
        private void OnGemCollected(Gem gem, Player collectedBy)
        {
            score += Gem.pointValue;
            //FMOD
            FmodFactory.Instance.start(ref gemEvent);
            FmodFactory.Instance.update(TodoIndex + 1,ref position_fmod_evt);
        }
        #endregion

        #region FMOD TimerAlert AMAYAS
        /// <summary>
        /// Called when the time eq : 30 secs 
        /// </summary>
        public void OnTimerAlert()
        {
            TimeSpan WarningTime = TimeSpan.FromSeconds(30);
            TimeSpan StopTime    = TimeSpan.FromSeconds(0) ;
            if (this.TimeRemaining < WarningTime && this.TimeRemaining > StopTime)
            {
                if (!LeitnerIsOn)
                    FmodFactory.Instance.start(ref timeEvent);
                else
                    FmodFactory.Instance.stop(ref timeEvent); 
                

                FmodFactory.Instance.update(TimeRemaining.Seconds, ref time_fmod_evt);
            }
            else
            if (this.TimeRemaining <= StopTime)
            {
                FmodFactory.Instance.stop(ref timeEvent);
            }
             
        }
        #endregion

        /// <summary>
        /// Called when the player is killed.
        /// </summary>
        /// <param name="killedBy">
        /// The enemy who killed the player. This is null if the player was not killed by an
        /// enemy, such as when a player falls into a hole.
        /// </param>
        private void OnPlayerKilled(Enemy killedBy)
        {      
               Player.OnKilled(killedBy); // Player will die  
        }

        /// <summary>
        /// Called when the player reaches the level's exit.
        /// </summary>
        private void OnExitReached()
        {
            Player.OnReachedExit();
            reachedExit = true;
        }

        /// <summary>
        /// Restores the player to the starting point to try the level again.
        /// </summary>
        public void StartNewLife()
        {
            Player.Reset(start);
            StartLeitnerGame(this.Game);
        }

        public void StartLeitnerGame(Game game)
        {
            switch (this.levelIndex)
            {
                case 0:
                    this.LeitnerGame = new LeitnerGame(game);
                    this.LeitnerGame.NbVocabulary = 4;
                    this.LeitnerGame.Initialize();
                    this.LeitnerGame.LoadContent(game);
                    break;
                case 1:
                    this.LeitnerGame = new LeitnerGame(game);
                    this.LeitnerGame.NbVocabulary = 9;
                    this.LeitnerGame.Initialize();
                    this.LeitnerGame.LoadContent(game);
                    break;
                case 2:
                    this.LeitnerGame = new LeitnerGame(game);
                    this.LeitnerGame.NbVocabulary = 16;
                    this.LeitnerGame.Initialize();
                    this.LeitnerGame.LoadContent(game);
                    break;
                case 3:
                    this.LeitnerGame = new LeitnerGame(game);
                    this.LeitnerGame.NbVocabulary = 25;
                    this.LeitnerGame.Initialize();
                    this.LeitnerGame.LoadContent(game);
                    break;
            }
        }

        #endregion
        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i <= EntityLayer; ++i)
                layers[1].Draw(spriteBatch, cameraPosition);
            spriteBatch.End();

            ScrollCamera(spriteBatch.GraphicsDevice.Viewport);
            Matrix cameraTransform = Matrix.CreateTranslation(-cameraPosition, 0.0f, 0.0f);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
                              RasterizerState.CullCounterClockwise, null, cameraTransform);

            DrawTiles(spriteBatch);
           
            foreach (Gem gem in gems)
            {                
                gem.Draw(gameTime, spriteBatch);
            }

                if (!drawFlash)
                    Player.Draw(gameTime, spriteBatch);

            foreach (Enemy enemy in enemies)
                if (enemy.IsAlive || enemy.deathTime > 0)
                    enemy.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
                health.Draw(spriteBatch, HealthPlayer);

            spriteBatch.End();


            spriteBatch.Begin();
                inventory.DrawContainer(gameTime, spriteBatch);
                if (!error)
                    inventory.DrawGoalText(gameTime, spriteBatch, TodoIndex,LeitnerGame.Result, false);
                else
                {
                    if (timerErr < TimeSpan.FromMinutes(3.0))
                    {
                        timerErr += TimeSpan.FromSeconds(1);
                        inventory.DrawGoalText(gameTime, spriteBatch, TodoIndex, LeitnerGame.Result, true);

                    }
                    else
                    {
                        error = false;
                        timerErr = TimeSpan.FromSeconds(0);
                    }
                }
                
            spriteBatch.End();
            // We draw each collected item that exists in the inventory
            spriteBatch.Begin();
            
            if (removedGem)
            {
                inventory.Draw(gameTime, spriteBatch); 
            }
            spriteBatch.End();

            spriteBatch.Begin();
            for (int i = EntityLayer + 1; i < layers.Length; ++i)
                layers[i].Draw(spriteBatch, cameraPosition);
            spriteBatch.End();  
        }

        // this moves the world backwards, and keeps the camera inthe same position


        private void ScrollCamera(Viewport viewport)
        {
#if ZUNE
            const float ViewMargin = 0.45f;
#else
            const float ViewMargin = 0.35f;
#endif

            // Calculate the edges of the screen.
            float marginWidth = viewport.Width * ViewMargin;
            float marginLeft = cameraPosition + marginWidth;
            float marginRight = cameraPosition + viewport.Width - marginWidth;

            // Calculate how far to scroll when the player is near the edges of the screen.
            float cameraMovement = 0.0f;
            if (Player.Position.X < marginLeft)
                cameraMovement = Player.Position.X - marginLeft;
            else if (Player.Position.X > marginRight)
                cameraMovement = Player.Position.X - marginRight;

            // Update the camera position, but prevent scrolling off the ends of the level.
            float maxCameraPosition = Tile.Width * Width - viewport.Width;
            cameraPosition = MathHelper.Clamp(cameraPosition + cameraMovement, 0.0f, maxCameraPosition);
        }

        /// <summary>
        /// Draws each tile in the level.
        /// </summary>
        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // Calculate the visible range of tiles.
            int left = (int)Math.Floor(cameraPosition / Tile.Width);
            int right = left + spriteBatch.GraphicsDevice.Viewport.Width / Tile.Width;
            right = Math.Min(right, Width - 1);
            // For each tile position

            for (int y = 0; y < Height; ++y)
            {
                for (int x = left; x <= right; ++x)
                {
                    // If there is a visible tile in that position
                    Texture2D texture = tiles[x, y].Texture;
                   
                    if (texture != null )
                    {
                        // Draw it in screen space.
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        
                        if (x == xExit && y == yExit)
                        {
                            if (inventory.isInventoryFull() && LeitnerGame.Result == 2)
                                spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Tiles/Exit open"), position, Color.White);
                            else
                            {
                                spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Tiles/Exit close"), position, Color.White);
                                if (Player.BoundingRectangle.Contains(exit) && LeitnerGame.Result == 0)
                                {
                                    #region Sound Door Closed
                                    if (!isDoorClosedSound)
                                        doorClosedSound.Play();
                                    isDoorClosedSound = true;
                                    #endregion
                                    position.X = position.X - 120;
                                    position.Y = position.Y - 45;
                                    spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Help/SortieFerme"), position, Color.White);

                                }
                                else isDoorClosedSound = false;
                            }
                        }else
                        if (x == xBox && y == yBox)
                        {
                            if (LeitnerGame.Result == 2)
                            {
                                spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Tiles/Bomb open"), position, Color.White);
                                #region Sound Door Open
                                if (!isDoorOpenSound)
                                    doorOpenSound.Play();
                                isDoorOpenSound = true;
                                #endregion
                            }
                            else
                            {
                                spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Tiles/Bomb close"), position, Color.White);
                            }
                                
                        }else
                        if (x == xHand && y == yHand)
                        {
                            if (inventory.isInventoryFull() && LeitnerGame.Result == 2)

                               spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Help/Main"), position, Color.White);                                
  
                        }else
                        if (x == xCollectMsg && y == yCollectMsg)
                        {
                            if (Player.BoundingRectangle.Contains(BlackGem))
                                isBlackCollected = true;
                            if(!isBlackCollected)
                               spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Help/Collectioner"), position, Color.White); 
                        }
                        else
                        if (x == xLeit && y == yLeit)
                        {
                            if (!Player.BoundingRectangle.Contains(box) && !inventory.isInventoryFull())
                                isBombClosedSound = false;
                            if (Player.BoundingRectangle.Contains(box) && !inventory.isInventoryFull())
                            {
                                #region Sound Door Closed
                                if (!isBombClosedSound)

                                    doorClosedSound.Play();
                                isBombClosedSound = true;
                                #endregion
                                spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Help/Leitner"), position, Color.White);
                            }
                            else
                                if (Player.BoundingRectangle.Contains(box) && inventory.isInventoryFull() && LeitnerGame.Result == 2)
                                    spriteBatch.Draw(Content.Load<Texture2D>("Plateforme/Help/BravoLeitner"), position, Color.White);

                        }
                        else spriteBatch.Draw(texture, position, Color.White);
                    }
                }
            }
        }

        #endregion
    }
}

 