#region File Description
#endregion
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Vocabulary 
{
    /// <summary>
    /// Button Used to create the gui elements
    /// </summary>
    public class Button
    {
        #region Variables

        public Vector2 position;
        Texture2D texture;
        Rectangle rectangle;
        public Vector2 size;
        public bool isClicked;
        public bool isSelected;

        #endregion

        #region Constructor(s)

        public Button(Texture2D newtexture,int X, int Y) {

            texture = newtexture;

            size = new Vector2(X , Y);

        }

        #endregion

        #region Methods
        public void Update(KeyboardState key) {

            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            if (key.IsKeyDown(Keys.Enter)) isClicked = true;
             
        }

        public void setPosition(Vector2 newPosition) {

            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Draw(texture, rectangle, Color.White);
        
        }
        #endregion
    }

}

