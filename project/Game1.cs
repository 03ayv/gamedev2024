using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //leshy leaf sprite
        private Texture2D texture;
        private Rectangle deelRectangle;
        private int schuifOp_X = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //leshy leaf sprite
            deelRectangle = new Rectangle(schuifOp_X, 32,32,32);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //leshy leaf sprite
            texture = Content.Load<Texture2D>("LeshyLeaf");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.RosyBrown);

            // TODO: Add your drawing code here

            //leshy leaf sprite
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, new Vector2(10,10), deelRectangle, Color.White);
            _spriteBatch.End();

            //leshyleaf walking animation
            schuifOp_X += 32;
            if (schuifOp_X > 256)
                schuifOp_X = 0;
            deelRectangle.X = schuifOp_X;

            base.Draw(gameTime);
        }
    }
}
