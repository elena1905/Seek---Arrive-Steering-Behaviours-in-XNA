using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame2
{
    class Path:GameEntity
    {
        List<Vector2> waypoints = new List<Vector2>();

        // Used to draw texts
        private SpriteFont Font { get; set; }

        bool looped;

        private int current = 0;

        public bool Looped
        {
            get { return looped; }
            set { looped = value; }
        }

        public bool AtEnd
        {
            get { return (current == (waypoints.Count - 1)); }
        }

        public void AddWayPoint(Vector2 point)
        {
            waypoints.Add(point);
        }

        public Vector2 NextWaypoint()
        {
            return waypoints[current];
        }

        public void Advance()
        {
            if (looped)
            {
                current = (current + 1) % waypoints.Count;
            }
            else
            {
                if (! AtEnd)
                {
                    current++;
                }
            }
        }

        public override void LoadContent()
        {
            Font = Game1.Instance.Content.Load<SpriteFont>(@"Fonts\font1");
            
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw positions
            Game1.Instance.spriteBatch.DrawString(Font, "hello", new Vector2(50, 50), Color.Black);
        }
    }
}
