using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame2
{
    class Cursor:GameEntity
    {
        public Cursor()
        {
            Position.X = 100;
            Position.Y = 100;
        }
        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("cur");
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            Position.X = mState.X;
            Position.Y = mState.Y;

            if (mState.LeftButton == ButtonState.Pressed)
            {
                PlayerTank tank = (PlayerTank) Game1.Instance.children.ElementAt(0);
                tank.targetPos = Position;
            }
        }
        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}
