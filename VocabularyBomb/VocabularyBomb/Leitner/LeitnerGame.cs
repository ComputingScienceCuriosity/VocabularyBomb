using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using VocabularyBomb;

namespace Vocabulary
{

    public class LeitnerGame : GameComponent
    {
        private SoundEffect BoomSound;
        bool boomPlayed = false;
        #region Propriétés

        #region FMOD Bomb Timer

        public FMOD.EventParameter BombWick_fmod_param { set; get; }  public FMOD.EventParameter bombWick_fmod_param = null;
        public FMOD.EventParameter BombWick_fmod_param_2 { set; get; }public FMOD.EventParameter bombWick_fmod_param_2 = null;
        public FMOD.EventParameter BombWick_fmod_param_3 { set; get; }public FMOD.EventParameter bombWick_fmod_param_3 = null;
        public FMOD.EventParameter BombWick_fmod_param_4 { set; get; }public FMOD.EventParameter bombWick_fmod_param_4 = null;

        public FMOD.Event BombWickEvent { set; get; }public FMOD.Event bombWickEvent = null;
        public FMOD.Event BombWickEvent_2 { set; get; }public FMOD.Event bombWickEvent_2 = null;
        public FMOD.Event BombWickEvent_3 { set; get; }public FMOD.Event bombWickEvent_3 = null;
        public FMOD.Event BombWickEvent_4 { set; get; }public FMOD.Event bombWickEvent_4 = null;


        public FMOD.EventParameter WrongChoice_fmod_param { set; get; }public FMOD.EventParameter wrongChoice_fmod_param = null;
        public FMOD.Event WrongChoiceEvent { set; get; }public FMOD.Event wrongChoiceEvent = null;

        public FMOD.EventParameter RightChoice_fmod_param { set; get; }public FMOD.EventParameter rightChoice_fmod_param = null;
        public FMOD.Event RightChoiceEvent { set; get; }public FMOD.Event rightChoiceEvent = null;
        //FMOD Ressources 
        private int rightNumber = 0;
        private int wrongNumber = 0;


        #endregion
        //Le gestionnaire des sons
        #region Déclaration Son du mauvais choix
        private SoundEffect WrongSound
        {
            set;
            get;
        }
        #endregion

        #region Déclaration Son du bon choix
        private SoundEffect RightSound
        {
            set;
            get;
        }
        #endregion

        //Le gestionnaire de contenu permettant de charger
        //des textures, fonts, ...
        #region Déclaration ContentManager
        private ContentManager Content
        {
            set;
            get;
        }
        #endregion

        //Liste contenant tout le vocabulaire à charger dans le jeu.
        #region Déclaration VocabularyList
        public List<VocabularyLeitner> VocabularyList
        {
            get;
            set;
        }
        #endregion

        //La souris du jeu.
        #region Déclaration MouseGame
        public MouseGame MouseGame
        {
            set;
            get;
        }
        #endregion

        //Compte à rebours du jeu.
        #region Déclaration GameTimer
        public GameTimer GameTimer
        {
            get;
            set;
        }
        #endregion

        //Vrai si l'utilisateur a choisi la mauvaise couleur.
        #region Déclaration ProgressBar
        public ProgressBar ProgressBar
        {
            set;
            get;
        }
        #endregion

        //Vrai si l'utilisateur a choisi la mauvaise couleur.
        #region Déclaration Wrong
        public static bool Wrong
        {
            set;
            get;
        }
        #endregion

        //Vrai si l'utilisateur a choisi la bonne couleur.
        #region Déclaration Right
        public static bool Right
        {
            set;
            get;
        }
        #endregion

        //Temps de départ donner au joueur lors du jeu.
        #region Déclaration Time
        public TimeSpan Time
        {
            set;
            get;
        }
        #endregion

        //Nombre de mot de vocabulaire à afficher (4, 9, 16 ou 25)
        #region Déclaration NbVocabulary
        public int NbVocabulary
        {
            set;
            get;
        }
        #endregion

        //Nom de la couleur que doit trouver le joueur.
        #region Déclaration SelectedVocabularyName
        public Label SelectedVocabularyName
        {
            set;
            get;
        }
        #endregion

        //La palette de couleur où le joueur choisi ses couleurs.
        #region Déclaration VocabularyPalette
        public GraphicVocabularySquare VocabularyPalette
        {
            set;
            get;
        }
        #endregion

        //Label d'information affichant au joueur si ça réponse 
        //est correct ou non.
        #region Déclaration Information
        public Label Information
        {
            set;
            get;
        }
        #endregion

        //Label d'information affichant au joueur si ça réponse 
        //est correct ou non.
        #region Déclaration ProgressBarLabel
        public Label ProgressBarLabel
        {
            set;
            get;
        }
        #endregion

        //Affiche la couleur correcte quand le joueur se trompe.
        #region Déclaration DisplayCorrectVocabulary
        public VocabularySquare DisplayCorrectVocabulary
        {
            set;
            get;
        }
        #endregion

        //Affiche de l'image de la bombe dans le jeu.
        #region Déclaration BombImage
        public Image BombImage
        {
            set;
            get;
        }
        #endregion

        //Affiche de l'image de l'explosion de la bombe.
        #region Déclaration BoomImage
        public Boom BoomImage
        {
            set;
            get;
        }
        #endregion

        //Affiche le Background du jeu.
        #region Déclaration Background
        public Image Background
        {
            set;
            get;
        }
        #endregion

        //Affiche de la méche de la bombe.
        #region Déclaration MecheBomb
        public Meche MecheBomb
        {
            set;
            get;
        }
        #endregion

        //Vrai si l'utilisateur à gagné le jeu sinon faux.
        #region Déclaration Result
        public int Result 
        {
            set;
            get;
        }
        
        #endregion

        //Résultat à incrémenter selon le nombre de
        //vocabulaire à apprendre.
        #region Déclaration Increase
        public int Increase
        {
            set;
            get;
        }
        #endregion

        //Label d'instruction affichant au joueur de
        //séléctionner la couleur demandé.
        #region Déclaration Instruction
        public Label Instruction
        {
            set;
            get;
        }
        #endregion

        //Booléen pour ajuster l'incrémentation
        #region Déclaration BoolIncrease
        public bool BoolIncrease
        {
            set;
            get;
        }
        #endregion

        #endregion

        #region Constructeur(s)
        public LeitnerGame(Game game) :base(game)
        {
            this.Content = game.Content;
        }
        #endregion

        #region Méthode Initialize
        public override void Initialize()
        {

            //Initialisation de la liste de couleur.
            #region Instanciation VocabularyList 
            VocabularyList = new List<VocabularyLeitner>();
            #endregion

            //Ajout des couleurs à la liste de vocabulaire.
            #region Ajout des couleurs 
            VocabularyList.Add(new VocabularyLeitner("BLACK", Content.Load<Texture2D>("Leitner/Button/Black"), Content.Load<Texture2D>("Leitner/PressedButton/Black")));
            VocabularyList.Add(new VocabularyLeitner("WHITE", Content.Load<Texture2D>("Leitner/Button/White"), Content.Load<Texture2D>("Leitner/PressedButton/White")));
            VocabularyList.Add(new VocabularyLeitner("RED", Content.Load<Texture2D>("Leitner/Button/Red"), Content.Load<Texture2D>("Leitner/PressedButton/Red")));
            VocabularyList.Add(new VocabularyLeitner("GREEN", Content.Load<Texture2D>("Leitner/Button/Green"), Content.Load<Texture2D>("Leitner/PressedButton/Green")));
            VocabularyList.Add(new VocabularyLeitner("BLUE", Content.Load<Texture2D>("Leitner/Button/Blue"), Content.Load<Texture2D>("Leitner/PressedButton/Blue")));
            VocabularyList.Add(new VocabularyLeitner("YELLOW", Content.Load<Texture2D>("Leitner/Button/Yellow"), Content.Load<Texture2D>("Leitner/PressedButton/Yellow")));
            VocabularyList.Add(new VocabularyLeitner("CYAN", Content.Load<Texture2D>("Leitner/Button/Cyan"), Content.Load<Texture2D>("Leitner/PressedButton/Cyan")));
            VocabularyList.Add(new VocabularyLeitner("MAGENTA", Content.Load<Texture2D>("Leitner/Button/Magenta"), Content.Load<Texture2D>("Leitner/PressedButton/Magenta")));
            VocabularyList.Add(new VocabularyLeitner("ORANGE", Content.Load<Texture2D>("Leitner/Button/Orange"), Content.Load<Texture2D>("Leitner/PressedButton/Orange")));
            VocabularyList.Add(new VocabularyLeitner("PINK", Content.Load<Texture2D>("Leitner/Button/Pink"), Content.Load<Texture2D>("Leitner/PressedButton/Pink")));
            VocabularyList.Add(new VocabularyLeitner("GRAY", Content.Load<Texture2D>("Leitner/Button/Gray"), Content.Load<Texture2D>("Leitner/PressedButton/Gray")));
            VocabularyList.Add(new VocabularyLeitner("BROWN", Content.Load<Texture2D>("Leitner/Button/Brown"), Content.Load<Texture2D>("Leitner/PressedButton/Brown")));
            VocabularyList.Add(new VocabularyLeitner("VIOLET", Content.Load<Texture2D>("Leitner/Button/Violet"), Content.Load<Texture2D>("Leitner/PressedButton/Violet")));
            VocabularyList.Add(new VocabularyLeitner("BEIGE", Content.Load<Texture2D>("Leitner/Button/Beige"), Content.Load<Texture2D>("Leitner/PressedButton/Beige")));
            VocabularyList.Add(new VocabularyLeitner("TURQUOISE", Content.Load<Texture2D>("Leitner/Button/Turquoise"), Content.Load<Texture2D>("Leitner/PressedButton/Turquoise")));
            VocabularyList.Add(new VocabularyLeitner("EMERALD", Content.Load<Texture2D>("Leitner/Button/Emerald"), Content.Load<Texture2D>("Leitner/PressedButton/Emerald")));
            VocabularyList.Add(new VocabularyLeitner("BURGUNDY", Content.Load<Texture2D>("Leitner/Button/Burgundy"), Content.Load<Texture2D>("Leitner/PressedButton/Burgundy")));
            VocabularyList.Add(new VocabularyLeitner("NAVY", Content.Load<Texture2D>("Leitner/Button/Navy"), Content.Load<Texture2D>("Leitner/PressedButton/Navy")));
            VocabularyList.Add(new VocabularyLeitner("MAUVE", Content.Load<Texture2D>("Leitner/Button/Mauve"), Content.Load<Texture2D>("Leitner/PressedButton/Mauve")));
            VocabularyList.Add(new VocabularyLeitner("KHAKI", Content.Load<Texture2D>("Leitner/Button/Khaki"), Content.Load<Texture2D>("Leitner/PressedButton/Khaki")));
            VocabularyList.Add(new VocabularyLeitner("RUBY", Content.Load<Texture2D>("Leitner/Button/Ruby"), Content.Load<Texture2D>("Leitner/PressedButton/Ruby")));
            VocabularyList.Add(new VocabularyLeitner("SALMON", Content.Load<Texture2D>("Leitner/Button/Salmon"), Content.Load<Texture2D>("Leitner/PressedButton/Salmon")));
            VocabularyList.Add(new VocabularyLeitner("GOLD", Content.Load<Texture2D>("Leitner/Button/Gold"), Content.Load<Texture2D>("Leitner/PressedButton/Gold")));
            VocabularyList.Add(new VocabularyLeitner("SILVER", Content.Load<Texture2D>("Leitner/Button/Silver"), Content.Load<Texture2D>("Leitner/PressedButton/Silver")));
            VocabularyList.Add(new VocabularyLeitner("BRONZE", Content.Load<Texture2D>("Leitner/Button/Bronze"), Content.Load<Texture2D>("Leitner/PressedButton/Bronze")));
            #endregion

            //Initialisation du temps et du nombre à incrémenter.
            #region Instanciation Time
            switch (this.NbVocabulary)
            {
                case 4:
                    this.Time = TimeSpan.FromSeconds(60);
                    this.Increase = (150 / (this.NbVocabulary*2)) + 1;
                    break;

                case 9:
                    this.Time = TimeSpan.FromSeconds(90);
                    this.Increase = (150 / (this.NbVocabulary * 2));
                    break;

                case 16:
                    this.Time = TimeSpan.FromSeconds(120);
                    this.Increase = (150 / this.NbVocabulary) + 1;
                    break;

                case 25:
                    this.Time = TimeSpan.FromSeconds(160);
                    this.Increase = 150 / this.NbVocabulary;
                    break;

                default:
                    throw new Exception("Nombre de carré incorrecte.");
            }
            #endregion

            //Variable de statut du joueur.
            Result = 0;

            //Lancement du système Leitner
            #region Lancement Système Leitner

            //Découpe de la liste de vocabulaire.
            this.VocabularyList = VocabularyList.GetRange(0, this.NbVocabulary);

            //Mélange de la liste de vocabualaire.
            ShuffleList.Shuffle<VocabularyLeitner>(this.VocabularyList);

            //Initialisation du système Leitner.
            Leitner.Instance.reset(this.VocabularyList);

            //Récupèration du premier mot de vocabulaire.
            Leitner.Instance.next();
            #endregion

            base.Initialize();
        }
        #endregion

        #region Méthode LoadContent
        public void LoadContent(Game game)
        {

            //Récupération de la hauteur et de la largeur 
            //de la fenêtre du jeu.
            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;

            //Chargement de la Font Ravie.
            SpriteFont ravieFont = Content.Load<SpriteFont>("Fonts/RavieFont");
            SpriteFont comicFont = Content.Load<SpriteFont>("Fonts/ComicFont");
            SpriteFont quartzFont = Content.Load<SpriteFont>("Fonts/QuartzFont");
            SpriteFont progressBarLabelFont = Content.Load<SpriteFont>("Fonts/ProgressBarLabelFont");

            //SOUNDS 
            BoomSound = Content.Load<SoundEffect>("Sounds/Triggers/boom");
            #region FMOD 
            FmodFactory.Instance.load("BombWick_lvl_1", "time_lvl_1", ref bombWickEvent, ref bombWick_fmod_param);
            FmodFactory.Instance.load("BombWick_lvl_2", "time_lvl_2", ref bombWickEvent_2, ref bombWick_fmod_param_2);
            FmodFactory.Instance.load("BombWick_lvl_3", "time_lvl_3", ref bombWickEvent_3, ref bombWick_fmod_param_3);
            FmodFactory.Instance.load("BombWick_lvl_4", "time_lvl_4", ref bombWickEvent_4, ref bombWick_fmod_param_4);

            FmodFactory.Instance.load("RightChoice", "ratio", ref rightChoiceEvent, ref rightChoice_fmod_param);
            FmodFactory.Instance.load("WrongChoice", "ratio", ref wrongChoiceEvent, ref wrongChoice_fmod_param);
            #endregion

            //Initialisation du background.
            #region Instanciation BombImage et MecheBomb
            Texture2D backgroundTexture = game.Content.Load<Texture2D>("Leitner/Background");
            this.Background = new Image(game, backgroundTexture, 0,0, width, height);
            #endregion

            //Initialisation de la palette de couleur
            #region Instanciation VocabularyPalette

            //Mélange de la liste de vocabulaire.
            ShuffleList.Shuffle<VocabularyLeitner>(this.VocabularyList);

            //Création de la palette de couleur.
            this.VocabularyPalette = new GraphicVocabularySquare(game, this.VocabularyList);
            #endregion

            //Initialisation du label des informations sur l'état du jeu.
            #region Instanciation InformationLabel
            this.Information = new Label(game,
                ravieFont,
                "",
                new Vector2(this.VocabularyPalette.GapX + this.VocabularyPalette.Size + 5,
                    height / 2 - ravieFont.MeasureString("Félicitation, tu\nas choisi la bonne\ncouleur.").Y),
                Color.Red);
            #endregion

            //Initialisation du label des instructions du jeu.
            #region Instanciation Instruction
            string info = "Selectionne l'image\ncorrespondant au mot :";
            this.Instruction = new Label(game,
                ravieFont,
                info,
                new Vector2(this.VocabularyPalette.GapX, 20),
                Color.White);
            #endregion

            //Initialisation de l'affichage du nom de la couleur
            //que l'utilisateur doit trouver.
            #region Instanciation SelectedVocabularyNameLabel

            //Calcul de la position en hauteur du composant.
            int yy = 85;

            //Calcul de la position en longueur du composant.
            int xx = this.VocabularyPalette.GapX + (this.VocabularyPalette.Size / 2)
                - ((int)(comicFont.MeasureString(Leitner.Instance.SelectedVocabulary.Nom).X / 2));

            //Création du composant.
            this.SelectedVocabularyName = new Label(game, comicFont, Leitner.Instance.SelectedVocabulary.Nom,
                new Vector2(xx, yy),
                    Color.White);
            #endregion

            //Initialisation du label qui affiche le mot correcte 
            //au joueur quand il répond faux.
            #region Instanciation DisplayCorrectVocabulary
            this.DisplayCorrectVocabulary =
                new VocabularySquare(game, width / 2 + 50, height / 2, 80, new VocabularyLeitner("WHITE", Content.Load<Texture2D>("Leitner/Button/White"), Content.Load<Texture2D>("Leitner/Button/White")));
            #endregion

            //Initialisation de la barre de progression
            #region Instanciation ProgressBar
            this.ProgressBar = new ProgressBar(Content, new Vector2(width - 230, 50),0,150,0);
            #endregion

            //Initialisation du label de la progressBar.
            #region Instanciation ProgressBarLabel
            this.ProgressBarLabel = new Label(game,
                progressBarLabelFont,
                "Progression désamorçage",
                new Vector2(width - 230 , 25),
                Color.Red);
            #endregion

            //Initialisation de la bombe et de sa méche.
            #region Instanciation BombImage et MecheBomb
            Texture2D bombTexture = game.Content.Load<Texture2D>("Leitner/Bomb/Bomb");
            int bombHeight = 120;
            int bombWidth = 100;
            this.BombImage = new Image(game, bombTexture, width - bombWidth - 40, height - bombHeight - 30, bombWidth, bombHeight);

            Texture2D mecheTexture = game.Content.Load<Texture2D>("Leitner/Bomb/Meche");
            int mecheHeight = 145;
            int mecheWidth = 33;
            this.MecheBomb = new Meche(game, mecheTexture, width - ((bombWidth / 2) + 23) - mecheWidth, height - bombHeight - mecheHeight - 15, mecheWidth, mecheHeight, (int)this.Time.TotalSeconds);
            #endregion

            //Initialisation de l'explosion de la bombe.
            #region Instanciation BoomImage
            Texture2D boomTexture = game.Content.Load<Texture2D>("Leitner/Bomb/Boom");
            int boomHeight = 50;
            int boomWidth = 100;
            this.BoomImage = new Boom(game, boomTexture, (width / 2) - (boomWidth / 2), (height / 2) - (bombHeight / 2), boomWidth, boomHeight);
            #endregion

            //Initialisation du Compte à rebours.
            #region Instanciation GameTimer
            this.GameTimer = new GameTimer(game, quartzFont, 
                new Vector2(width - 40 - (bombWidth / 2) - (quartzFont.MeasureString("00 : 00").X / 2 - 7), height - 32 - (bombHeight / 2)), this.Time);
            #endregion

            //Initialisation de la souris du jeu.
            #region Instanciation MouseGame
            this.MouseGame = new MouseGame(game, this.Content.Load<Texture2D>("Leitner/Mouse/Main"), 20, 20);
            #endregion
        }
        #endregion

        #region FMOD TIMER FONCTION
        public void BombWickFmodSound(TimeSpan alarm,ref FMOD.Event evt, ref FMOD.EventParameter evntParam) 
        {
            if (this.GameTimer.Time < alarm && !this.MecheBomb.Finished && this.GameTimer.Time > TimeSpan.FromSeconds(0.0) && this.Result == 0)
            {
                FmodFactory.Instance.start(ref evt);
                FmodFactory.Instance.update(this.GameTimer.Time.Seconds, ref evntParam);
            }
        }
        #endregion 

        #region Update Method
        public override void Update(GameTime gameTime)
        {
            this.MouseGame.Update(gameTime);
            this.GameTimer.Update(gameTime);
            this.VocabularyPalette.Update(gameTime);
            this.MecheBomb.Update(gameTime);

            switch (this.NbVocabulary)
            {
                case 4:
                    #region FMOD
                    this.BombWickFmodSound(TimeSpan.FromSeconds(60),ref bombWickEvent,ref bombWick_fmod_param); 
                    #endregion 
                    this.Increase = (150 / (this.NbVocabulary * 2)) + 1;

                    break;

                case 9:
                    #region FMOD
                    this.BombWickFmodSound(TimeSpan.FromSeconds(90),ref bombWickEvent_2, ref bombWick_fmod_param_2); 
                    #endregion 
                    if (this.BoolIncrease)
                    {
                        this.Increase = (150 / (this.NbVocabulary * 2)) + 1;
                        this.BoolIncrease = false;
                    }
                    else
                    {
                        this.Increase = (150 / (this.NbVocabulary * 2));
                        this.BoolIncrease = true;
                    }
                    break;

                case 16:
                    #region FMOD
                    this.BombWickFmodSound(TimeSpan.FromSeconds(120),ref bombWickEvent_3, ref bombWick_fmod_param_3); 
                    #endregion 
                    this.Increase = (150 / this.NbVocabulary) + 1;
                    if (this.BoolIncrease)
                    {
                        this.Increase = (150 / this.NbVocabulary) + 1;
                        this.BoolIncrease = false;
                    }
                    else
                    {
                        this.Increase = (150 / this.NbVocabulary);
                        this.BoolIncrease = true;
                    }
                    break;

                case 25:
                    #region FMOD
                    this.BombWickFmodSound(TimeSpan.FromSeconds(160),ref bombWickEvent_4, ref bombWick_fmod_param_4); 
                    #endregion 
                    this.Increase = 150 / this.NbVocabulary;

                    break;

                default:
                    throw new Exception("Nombre de carré incorrecte.");
            }


            if (LeitnerGame.Right)
            {
                #region FMOD sound FailedChoice
                FmodFactory.Instance.start(ref rightChoiceEvent);
                FmodFactory.Instance.setParamValue(rightNumber, ref rightChoice_fmod_param);
                rightNumber++;
                wrongNumber = 0;
                if(rightNumber > 4)
                   rightNumber = 0;
                #endregion

                LeitnerGame.Wrong = false;
                LeitnerGame.Right = false;
                this.ProgressBar.Increase(this.Increase);
                Leitner.Instance.SelectedVocabulary.Known = true;
                Leitner.Instance.next();
                this.SelectedVocabularyName.Text = Leitner.Instance.SelectedVocabulary.Nom;
                this.Information.Text = "Félicitation, tu\nas choisi la bonne\ncouleur.";
                this.Information.Color = Color.LimeGreen;
            }
            else if (LeitnerGame.Wrong)
            {
                #region FMOD sound FailedChoice
                FmodFactory.Instance.start(ref wrongChoiceEvent);
                FmodFactory.Instance.setParamValue(wrongNumber, ref wrongChoice_fmod_param);
                wrongNumber++;
                rightNumber = 0;
                if (wrongNumber > 4)
                    wrongNumber = 0;
                #endregion

                LeitnerGame.Wrong = false;
                LeitnerGame.Right = false;
                this.ProgressBar.Decrease(this.Increase);
                this.DisplayCorrectVocabulary.VocabularyLeitner.Image = Leitner.Instance.SelectedVocabulary.Image;
                Leitner.Instance.next();
                this.SelectedVocabularyName.Text = Leitner.Instance.SelectedVocabulary.Nom;
                this.Information.Text = "Dommage, la bonne\ncouleur était : ";
                this.Information.Color = Color.DarkRed;
            }

            #region FMOD Update Timer
           
            #endregion

            if (this.GameTimer.Finished && this.MecheBomb.Finished)
            {

                this.BoomImage.Increase(20);
                if (!boomPlayed)
                {
                    BoomSound.Play();
                    boomPlayed = true;
                }
            }

            if (this.BoomImage.Finished)
            {
                this.Result = 1;
            }

            if (this.ProgressBar.Value >= 150)
            {
                this.Result = 2;
            }

        }
        #endregion

        #region Draw Method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //Dessine le background du jeu.
            #region Draw Background
            this.Background.Draw(spriteBatch);
            #endregion

            //Dessine la bombe.
            #region Draw Bomb
            this.BombImage.Draw(spriteBatch);
            #endregion

            //Dessine la méche de la bombe.
            #region Draw MecheBomb
            this.MecheBomb.Draw(spriteBatch);
            #endregion

            //Dessine la palette de vocabulaire.
            #region Draw VocabularyPalette
            this.VocabularyPalette.Draw(spriteBatch);
            #endregion

            //Dessine le nom de la couleur selectionné 
            //par le système Leitner.
            #region Draw Instruction
            this.Instruction.Draw(spriteBatch);
            #endregion

            //Dessine le label de la progressBar.
            #region Draw ProgressBarLabel
            this.ProgressBarLabel.Draw(spriteBatch);
            #endregion

            //Dessine le nom de la couleur selectionné 
            //par le système Leitner.
            #region Draw SelectedVocabularyName
            this.SelectedVocabularyName.Draw(spriteBatch);
            #endregion

            //Dessine la couleur correcte que le joueur 
            //aurait du choisir et Dessine le label d'information du jeu.
            #region Draw DisplayCorrectVocabulary
            if (VocabularySquare.DisplayCorrectImage == 1)
            {
                this.DisplayCorrectVocabulary.Draw(spriteBatch);
                this.Information.Draw(spriteBatch);
            }
            else if (VocabularySquare.DisplayCorrectImage == 2)
            {
                this.Information.Draw(spriteBatch);
            }
            #endregion

            //Dessine le compte à rebours.
            #region Draw GameTimer
            this.GameTimer.Draw(spriteBatch);
            #endregion

            //Dessine la barre de progression.
            #region Draw ProgressBar
            this.ProgressBar.Draw(spriteBatch);
            #endregion

            //Dessine l'image de l'explosion de la bombe.
            #region Draw BoomImage
            if (this.MecheBomb.Finished)
            {
                
                this.BoomImage.Draw(spriteBatch);
            }
            #endregion

            //Dessine la souris.
            #region Draw MouseGame
            this.MouseGame.Draw(spriteBatch);
            #endregion
            spriteBatch.End();

        }
        #endregion
    }
}
