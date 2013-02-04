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
    // PlayerTank extends GameEntity, so we cal add it to the scene graph (i.e. the list<GameEntity> in Game1)
    class PlayerTank:GameEntity
    {
        

        // The sound effect to play when firing
        SoundEffect Sound;
        public Vector2 targetPos;
        public Vector2 velocity;
        float mass = 1;
        float maxSpeed = 150;
        float maxForce = 50;

        // gets called from LoadContennt in game1
        public override void LoadContent()
        {
            // Set up the initial positions and look vectors
            Position.X = 100;
            Position.Y = 100;
            Look.X = 0;
            Look.Y = -1;
            IsAlive = true;
            Rotation = 0;            

            // Load the sprite and the audio file from the content pipeline. Note! No file extension below
            // Also note how we get access to the static member Instance in the class Game1
            Sprite = Game1.Instance.Content.Load<Texture2D>("smalltank");
            Sound = Game1.Instance.Content.Load<SoundEffect>("GunShot");

            Center.X = Sprite.Width / 2;
            
            Center.Y = Sprite.Height / 2;

            base.LoadContent();

        }

        Vector2 arrive()
        {
            float slowing = 100.0f;
            Vector2 targetOffest = targetPos - Position;
            float dist = targetOffest.Length();
            float ramped = maxSpeed * (dist / slowing);
            float clipped = Math.Min(maxSpeed, ramped);
            Vector2 desired = (clipped / dist) * targetOffest;
            return (desired - velocity) * 8.0f;
        }
        

        Vector2 flee()
        {

            Vector2 desired = targetPos - Position;
            float dist = desired.Length();
            if (dist < 50)
            {
                desired.Normalize();
                desired *= maxSpeed;

                Vector2 force = velocity - desired;
                if (force.Length() > maxForce)
                {
                    return Vector2.Normalize(force) * maxForce;
                }
                return force;
            }
            else
            {
                return Vector2.Zero;
            }
        }

        Vector2 seek()
        {
            Vector2 desired = targetPos - Position;
            desired.Normalize();
            desired *= maxSpeed;

            Vector2 force = desired - velocity;
            if (force.Length() > maxForce)
            {
                return Vector2.Normalize(force) * maxForce;
            }
            return force;
        }

        float lastShot = 1.0f;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculate the timeDelta I.e. How much time has elapsed since the last time this function was called
            // We use this in the updates
            float timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            float speed = 100.0f;

            Vector2 acceleration = arrive() / mass;
            velocity = velocity + acceleration * timeDelta;
            if (velocity.Length() > maxSpeed)
            {
                velocity = Vector2.Normalize(velocity) * maxSpeed;
            }
            Position += velocity * timeDelta;
            if (velocity.Length() > 0)
            {
                Look = Vector2.Normalize(velocity);
            }

            Vector2 basis = new Vector2(0, -1);
            Rotation = (float) Math.Acos(Vector2.Dot(basis, Look));
            if (Look.X < 0)
            {
                Rotation *= -1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the tank sprite
            Game1.Instance.spriteBatch.Draw(Sprite, Position, null, Color.White, Rotation, Center, 1, SpriteEffects.None, 1);
        }        
    }
}
