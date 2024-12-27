using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //game object textures
            leshyLeafTexture = Content.Load<Texture2D>("LeshyLeaf");
            porcupineTexture = Content.Load<Texture2D>("Porcupine");
            InitializeGameObjects();
        }

        private void InitializeGameObjects()
        {
            leshyLeaf = new LeshyLeaf(leshyLeafTexture);
            porcupine = new Porcupine(porcupineTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            leshyLeaf.Update(gameTime);
            porcupine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.RosyBrown);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            leshyLeaf.Draw(_spriteBatch);
            porcupine.Draw(_spriteBatch);

            _spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
