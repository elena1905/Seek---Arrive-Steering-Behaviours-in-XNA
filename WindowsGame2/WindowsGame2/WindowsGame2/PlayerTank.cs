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
        public Vector2 velocity;
        float mass = 1;
        float maxSpeed = 150;
        float maxForce = 50;

        Path path = new Path();

        // Used to draw texts
        private SpriteFont Font { get; set; }

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

            path.Looped = true;
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0 ; i < 5 ; i ++)
            {
                Vector2 pos = new Vector2(
                    r.Next(Game1.Instance.graphics.PreferredBackBufferWidth) 
                    ,r.Next(Game1.Instance.graphics.PreferredBackBufferHeight)
                    );
                path.AddWayPoint(pos);
            }

            Font = Game1.Instance.Content.Load<SpriteFont>(@"SpriteFont1");
            
            base.LoadContent();
        }

        Vector2 followPath()
        {
            float epsilon = 20.0f;

            float dist = (Position - path.NextWaypoint()).Length();
            if (dist < epsilon)
            {
                path.Advance();
            }
            if (!path.Looped && path.AtEnd)
            {
                return arrive(path.NextWaypoint());
            }
            else
            {
                return seek(path.NextWaypoint());
            }
        }

        Vector2 arrive(Vector2 targetPos)
        {
            float slowingDistance = 100.0f;

            Vector2 targetOffest = targetPos - Position;

            float distance = targetOffest.Length();
            if (distance > 0.0f)
            {
                float ramped = maxSpeed * (distance / slowingDistance);
                float clipped = Math.Min(ramped, maxSpeed);
                Vector2 desired = (clipped / distance) * targetOffest;
                return (desired - velocity) * 8.0f;
            }
            else
            {
                return Vector2.Zero;
            }
        }
        

        Vector2 flee(Vector2 targetPos)
        {
            Vector2 desired = targetPos - Position;

            if (desired.Length() < 100.0f)
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

        Vector2 seek(Vector2 targetPos)
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

            Vector2 acceleration = followPath() / mass;
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
            // Draw waypoints at their positions
            for (int i = 0; i < path.waypoints.Count(); i++)
            {
                Vector2 posVect = path.waypoints.ElementAt(i);
                string posText = (i + 1) + ":" + posVect.X + "," + posVect.Y;

                Game1.Instance.spriteBatch.DrawString(Font, posText, posVect, Color.Black);
            }

            // Draw the tank sprite
            Game1.Instance.spriteBatch.Draw(Sprite, Position, null, Color.White, Rotation, Center, 1, SpriteEffects.None, 1);
        }
    }
}
