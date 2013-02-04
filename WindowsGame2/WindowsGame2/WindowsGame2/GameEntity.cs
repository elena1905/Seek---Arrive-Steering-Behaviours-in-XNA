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

namespace WindowsGame2
{
    public class GameEntity
    {
        // All game entities have the following attributes
        public Texture2D Sprite;
        public Vector2 Position;
        public Vector2 Look;
        public float Rotation;
        public bool IsAlive;
        // The local center of the sprite. Required to rotate the sprite
        public Vector2 Center;

        public GameEntity()
        {
            IsAlive = true;
        }

        // These are all virtual, meaning use dynamic binding
        // The versions of these functions in the subclasses will be the functions that get called
        public virtual void LoadContent()
        {
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}
