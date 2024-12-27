using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using project.Input;
using SharpDX.Direct2D1.Effects;
using System;

namespace project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //leshy leaf sprite
        private Texture2D leshyLeafTexture;
        LeshyLeaf leshyLeaf;

        //porcupine sprite
        private Texture2D porcupineTexture;
        Porcupine porcupine;

        //dragonfly sprite
        private Texture2D dragonflyTexture;
        Dragonfly dragonfly;

        //squirrel sprite
        private Texture2D squirrelTexture;
        Squirrel squirrel;

        //game over
        private bool gameOver = false;
        private Color backgroundColor = Color.RosyBrown;

        //camera
        private Vector2 cameraPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //game object textures
            leshyLeafTexture = Content.Load<Texture2D>("LeshyLeaf");
            porcupineTexture = Content.Load<Texture2D>("Porcupine");
            dragonflyTexture = Content.Load<Texture2D>("Dragonfly");
            squirrelTexture = Content.Load<Texture2D>("Squirrel");
            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            leshyLeaf = new LeshyLeaf(leshyLeafTexture, new KeyboardReader());
            porcupine = new Porcupine(porcupineTexture);
            dragonfly = new Dragonfly(dragonflyTexture);
            squirrel = new Squirrel(squirrelTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!gameOver)
            {
                leshyLeaf.Update(gameTime);
                porcupine.Update(gameTime);
                dragonfly.Update(gameTime);
                squirrel.Update(gameTime);

                // Check collisions
                if (leshyLeaf.GetBounds().Intersects(porcupine.GetBounds()) ||
                    leshyLeaf.GetBounds().Intersects(dragonfly.GetBounds()) ||
                    leshyLeaf.GetBounds().Intersects(squirrel.GetBounds()))
                {
                    gameOver = true;
                    backgroundColor = Color.DarkRed;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            //camera position centers leshyleaf horizontally
            cameraPosition = new Vector2(leshyLeaf.GetBounds().Center.X - GraphicsDevice.Viewport.Width / 6, 0);

            //camera transform matrix
            Matrix cameraTransform = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: cameraTransform);

            leshyLeaf.Draw(_spriteBatch);
            porcupine.Draw(_spriteBatch);
            dragonfly.Draw(_spriteBatch);
            squirrel.Draw(_spriteBatch);

            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
