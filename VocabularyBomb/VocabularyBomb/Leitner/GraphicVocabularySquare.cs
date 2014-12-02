using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Vocabulary
{
    public class GraphicVocabularySquare : GameComponent
    {

        #region Constructor
        public GraphicVocabularySquare(Game game, List<VocabularyLeitner> vocabulary)
            : base(game)
        {
            this.VocabularySquare = new List<VocabularySquare>();
            this.GapY = game.GraphicsDevice.Viewport.Height / 2 - 80;
            this.GapX = 65;

            switch (vocabulary.Count)
            {
                case 4 :
                    this.SquareSize = 120;
                    break;

                case 9:
                    this.SquareSize = 100;
                    //this.GapX = (game.Window.ClientBounds.Width  / 2) - (this.SquareSize + (this.SquareSize / 2));
                    break;

                case 16:
                    this.SquareSize = 75;
                    //this.GapX = (game.Window.ClientBounds.Width / 2) - (this.SquareSize * 2);
                    break;

                case 25:
                    this.SquareSize = 60;
                    //this.GapX = (game.Window.ClientBounds.Width / 2) - (this.SquareSize * 2 + (this.SquareSize / 2));
                    break;

                default :
                    //Quitter le jeu
                    break;
            }

            this.Size = this.SquareSize * (int)Math.Sqrt(vocabulary.Count);

            int x = this.GapX;
            int y = this.GapY;
            int index = 0;
            foreach (VocabularyLeitner v in vocabulary)
            {
                this.VocabularySquare.Add(new VocabularySquare(game, x, y, this.SquareSize, v));
                if (Math.Sqrt(vocabulary.Count) - 1 == index)
                {
                    y += this.SquareSize;
                    x = this.GapX;
                    index = 0;
                }
                else
                {
                    x += this.SquareSize;
                    index++;
                }
            }
        }
        #endregion

        #region Properties
        public int SquareSize
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public int GapX
        {
            get;
            set;
        }

        public int GapY
        {
            get;
            set;
        }

        public List<VocabularySquare> VocabularySquare
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            foreach (VocabularySquare vs in this.VocabularySquare)
            {
                vs.Update(gameTime);
            }

                base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (VocabularySquare vs in this.VocabularySquare)
            {
                vs.Draw(spriteBatch);
            }
        }
        #endregion

    }
}
