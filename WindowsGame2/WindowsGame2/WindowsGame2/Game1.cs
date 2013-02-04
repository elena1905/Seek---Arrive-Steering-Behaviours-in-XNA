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
 * A simple little demo to demonstrate a scene graph, sprites, audio and control with the game controller
 * Control using an attached Game Controller (using the Dpad) or using the keyboard arrow keys and the SPACE key
 */
namespace WindowsGame2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /* These are all public, because we need to use them in other classes
         */ 
        public GraphicsDeviceManager graphics;

        // Used to draw sprited (2D bitmaps)
        public SpriteBatch spriteBatch;

        // Instance is a STATIC member, meaning that there is only one of them
        // No matter how many instances are created
        public static Game1 Instance;

        // This is the scene graph. It keeps track of all the objects in the scene
        // They will all be subclasses of GameEntity, so this a great example of polymorphism in action
        public List<GameEntity> children = new List<GameEntity>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // When the constructor is called assign the static instance to be the object
            Instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Add a tank to the list of objects in the scene
            children.Add(new PlayerTank());
            children.Add(new Cursor());

            // Call load content, using dynamic binding
            // In other words, its the one in the subclass that will be called
            for (int i = 0; i < children.Count(); i++)
            {
                children[i].LoadContent();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            // You can also get access to the keyboard state and tye mouse state in a similar way...
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // Call update on all the objects in the scene
            // Uses dynamic function binding and polymorphism
            for (int i = 0; i < children.Count(); i++)
            {
                children[i].Update(gameTime);
                // If the object is dead,t then we remove it
                // Typical example would be when a bullet goes outside the screen
                if (children[i].IsAlive == false)
                {
                    children.Remove(children[i]);
                }
            }

            // Call Update in the base class
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Start drawing sprites, front to back
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Tell all the objects in the scene to draw themselves
            for (int i = 0; i < children.Count(); i++)
            {
                children[i].Draw(gameTime);
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
