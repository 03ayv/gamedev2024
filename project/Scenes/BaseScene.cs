using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using project.Collectibles;
using project.Enemies;
using project.Interfaces;
using project.Tiles;
using System.Collections.Generic;
using project.Managers;

namespace project.Scenes
{
    public abstract class BaseScene
    {
        protected GraphicsDevice graphicsDevice;
        protected ContentManager content;
        protected SpriteBatch spriteBatch;
        
        //game objects
        protected List<IGameObject> enemies;
        protected ICollectible key;
        protected List<Coin> coins;
        protected ScoreManager scoreManager;
        
        //constants
        protected const int TILE_WIDTH = 16;
        protected const int TILE_HEIGHT = 16;
        protected const float SCALE = 5f;


        public BaseScene(GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            
            enemies = new List<IGameObject>();
            coins = new List<Coin>();
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        protected abstract void CheckCollisions();
        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
} 