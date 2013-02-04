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
/*
 * This class encapsulates the behaviour and drawing of a bullet
 */ 
namespace WindowsGame2
{
    class Bullet:GameEntity
    {
        public override void LoadContent()
        {         
            Sprite = Game1.Instance.Content.Load<Texture2D>("bullet");
            Center.X = Sprite.Width / 2;
            Center.Y = Sprite.Height / 2;
            IsAlive = true;
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float speed = 500.0f;

            // Move at 500 units per second in the direction we are looking
            Position += Look * speed * timeDelta;

            // If the bullet goes outside the screen, set isAlive to be false, so it gets removed
            // Note how we use the static member Instance to get access to the Game1 instance
            int width = Game1.Instance.GraphicsDevice.Viewport.Width;
            int height = Game1.Instance.GraphicsDevice.Viewport.Width;
            if ((Position.X > width) || (Position.X < 0) || (Position.Y < 0) || (Position.Y > 600))
            {
                IsAlive = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(Sprite, Position, null, Color.White, Rotation, Center, 1, SpriteEffects.None, 1);
        }       
    }
}
