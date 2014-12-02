using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vocabulary
{
    public class GameTimer : GameComponent
    {

        #region Constructor(s)

        public GameTimer(Game game, SpriteFont font, Vector2 position) : base(game)
        {
            this.Started = true;
            this.Paused = false;
            this.Finished = false;
            this.Font = font;
            this.Time = TimeSpan.FromMinutes(0);
            this.TimeTemp = TimeSpan.FromSeconds(0);
            this.Position = position;
        }

        public GameTimer(Game game, SpriteFont font, Vector2 position, TimeSpan time)
            : base(game)
        {
            this.Started = true;
            this.Paused = false;
            this.Finished = false;
            this.Font = font;
            this.Position = position;
            this.Time = time;
            this.TimeTemp = TimeSpan.FromSeconds(0);
            this.Text = "Temps: " + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00"); 
        }

        public GameTimer(Game game, SpriteFont font, Vector2 position, TimeSpan time, bool started)
            : base(game)
        {
            this.Paused = false;
            this.Finished = false;
            this.Font = font;
            this.Position = position;
            this.Time = time;
            this.TimeTemp = TimeSpan.FromSeconds(0);

            this.Text = "Temps: " + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00"); 
            this.Started = started;
        }

        #endregion

        #region Properties

        public TimeSpan TimeTemp
        {
            get;
            set;
        }

        public TimeSpan Time
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public bool Started
        {
            get;
            set;
        }

        public bool Paused
        {
            get;
            set;
        }

        public bool Finished
        {
            get;
            set;
        }

        public SpriteFont Font
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (this.Started)
            {
                if (!this.Paused)
                {
                    if (this.Time > TimeSpan.FromSeconds(0))
                    {
                        this.TimeTemp += gameTime.ElapsedGameTime;
                        if (this.TimeTemp.TotalMilliseconds >= 1000)
                        {
                            this.Time -= TimeSpan.FromSeconds(1);
                            this.TimeTemp = TimeSpan.FromSeconds(0);
                        }
                    }
                    else
                    {
                        this.Finished = true;
                    }
                }
            }

            this.Text = this.Time.Minutes + " : " + this.Time.Seconds;

            base.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color.Red);
        }
        #endregion
    }
}
