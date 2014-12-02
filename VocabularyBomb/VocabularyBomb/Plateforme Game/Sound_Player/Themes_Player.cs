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


namespace VocabularyBomb.Plateforme_Game.Sound_Player
{
    /// <summary>
    /// Composant du jeu qui implémente IUpdateable.
    /// </summary>
    public class Themes_Player 
    {
        private bool IsSongPlaying
        {
            set;
            get;

        }bool isSongPlaying = false;

        Song []theme_music;
        uint songsNumber = 4;


        public Themes_Player(ContentManager Content)
        {
            LoadContent(Content);
            // TODO : concevoir les composants enfants ici
        }

        public void LoadContent(ContentManager Content)
        {

            theme_music = new Song[songsNumber];
            for (uint i = 0; i < songsNumber; i++)
                theme_music[i] = Content.Load<Song>("Sounds/Levels/Level" + (i + 1) + "/LEVEL" + (i + 1) + "_THEME");
        
        }
      

        public void PlayNextSong(int levelIndex) 
        
        {
            switch (levelIndex)
            {
                case 0:
                    if (!isSongPlaying)
                    {
                        MediaPlayer.Play(theme_music[0]);
                        isSongPlaying = true;
                    }
                    break;

                case 1:
                    isSongPlaying = false;
                    if (!isSongPlaying)
                    {
                        MediaPlayer.Play(theme_music[1]);
                        isSongPlaying = true;
                    }
                    break;

                case 2:
                    MediaPlayer.Play(theme_music[2]);
                    break;

                case 3:
                    MediaPlayer.Play(theme_music[3]);
                    break;
            }
        }

        /// <summary>
        /// Permet au composant de jeu de se mettre à jour.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        public void Update(int levelIndex)
        {
            PlayNextSong(levelIndex);

            
        }
    }
}
