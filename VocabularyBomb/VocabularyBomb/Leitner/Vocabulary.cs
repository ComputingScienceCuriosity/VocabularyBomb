using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Vocabulary
{
    public class VocabularyLeitner
    {

        #region Constructor
        public VocabularyLeitner(string nom, Texture2D color, Texture2D colorPressed)
        {
            this.Nom = nom;
            this.Known = false;
            this.Image = color;
            this.ImagePressed = colorPressed;
        }
        #endregion

        #region Properties
        public bool Known
        {
            get;
            set;
        }

        public string Nom
        {
            get;
            set;
        }

        public Texture2D Image
        {
            get;
            set;
        }

        public Texture2D ImagePressed
        {
            get;
            set;
        }
        #endregion

        #region Méthode ToString
        public override string  ToString()
        {
 	        return "Réussie : " + this.Known + " Nom : " + this.Nom;
        }
        #endregion
    }
}
