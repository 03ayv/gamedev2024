using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using project.Input;
using System.Collections.Generic;
using System.IO;
using project.Tiles;
using project.Interfaces;
using project.Enemies;
using project.Collectibles;
using System.Linq;

namespace project.Scenes
{
    public class GameScene1 : BaseScene
    {
        public GameScene1(GraphicsDevice graphicsDevice, ContentManager content) 
            : base(graphicsDevice, content)
        {
        }

        public override void LoadContent()
        {
            //load game objects
            var leshyLeafTexture = content.Load<Texture2D>("LeshyLeaf");
            var porcupineTexture = content.Load<Texture2D>("Porcupine");
            var dragonflyTexture = content.Load<Texture2D>("Dragonfly");
            var squirrelTexture = content.Load<Texture2D>("Squirrel");
            var keyTexture = content.Load<Texture2D>("Key");
            var coinTexture = content.Load<Texture2D>("Key");
            //initialize game objects
            InitializeGameObjects(leshyLeafTexture, porcupineTexture, dragonflyTexture, squirrelTexture, keyTexture, coinTexture);
        }

        public override void Initialize()
        {
            //level-specific objects
            //initialize coins
            List<Vector2> coinPositions = GenerateCoinPositions();
            //coins = coinPositions.Select(position => new Coin(coinTexture, position)).ToList();

            //scoreManager = new ScoreManager(content.Load<SpriteFont>("File"));
        }

        public override void Update(GameTime gameTime)
        {
            CheckCollisions();
            
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            key?.Update(gameTime);

            foreach (var coin in coins)
            {
                coin.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //draw all game objects

            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            key?.Draw(spriteBatch);
            foreach (var coin in coins)
            {
                coin.Draw(spriteBatch);
            }
        }

        private void InitializeGameObjects(Texture2D leshyLeafTexture, Texture2D porcupineTexture, Texture2D dragonflyTexture, 
            Texture2D squirrelTexture, Texture2D keyTexture, Texture2D coinTexture)
        {

            enemies = new List<IGameObject>
            {
                new Porcupine(porcupineTexture, new Vector2(800, 1185)),
                new Porcupine(porcupineTexture, new Vector2(1700, 1185)),
                new Dragonfly(dragonflyTexture, new Vector2(400, 950)),
                new Dragonfly(dragonflyTexture, new Vector2(1500, 1000)),
                new Squirrel(squirrelTexture, new Vector2(300, 1185), Game1.LeshyLeaf),
                new Squirrel(squirrelTexture, new Vector2(2000, 1185), Game1.LeshyLeaf)
            };

            key = new Key(keyTexture, new Vector2(2200, 1190));
            
            List<Vector2> coinPositions = GenerateCoinPositions();
            coins = coinPositions.Select(position => new Coin(coinTexture, position)).ToList();
        }

        private int[,] LoadTileMap(string path)
        {
            string[] lines = File.ReadAllLines(path);
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;
            var map = new int[rows, cols];

            for (int y = 0; y < rows; y++)
            {
                string[] cells = lines[y].Split(',');
                for (int x = 0; x < cols; x++)
                {
                    map[y, x] = int.Parse(cells[x]);
                }
            }
            return map;
        }

        //generate multiple coins
        private List<Vector2> GenerateCoinPositions()
        {
            List<Vector2> positions = new List<Vector2>
            {
                new Vector2(50, 1195),
                new Vector2(150, 1195),
                new Vector2(250, 1160),
                new Vector2(400, 1120),
                new Vector2(500, 1120),
                new Vector2(650, 1195),
                new Vector2(750, 1195),
                new Vector2(850, 1195),
                new Vector2(930, 1100),
                new Vector2(1050, 1035),
                new Vector2(1150, 1035),
                new Vector2(1250, 1035),
                new Vector2(1350, 1000),
                new Vector2(1400, 970),
                new Vector2(1450, 950),
                new Vector2(1500, 940),
                new Vector2(1550, 930),
                new Vector2(1450, 1195),
                new Vector2(1550, 1195),
                new Vector2(1650, 1195),
                new Vector2(1750, 1195),
                new Vector2(1850, 1195),
                new Vector2(1950, 1195),
            };
            return positions;
        }

        protected override void CheckCollisions()
        {
            //check enemy collision
            foreach (var enemy in enemies)
            {
                if (Game1.LeshyLeaf.GetBounds().Intersects(enemy.GetBounds()))
                {
                    Game1.GameOver = true;
                    return;
                }
            }

            //check coin collision
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                if (!coins[i].IsCollected() && Game1.LeshyLeaf.GetBounds().Intersects(coins[i].GetBounds()))
                {
                    coins[i].Collect();
                    Game1.ScoreManager.AddPoints(10);
                }
            }

            //check key collision
            if (key != null && Game1.LeshyLeaf.GetBounds().Intersects(key.GetBounds()))
            {
                key.Collect();
                Game1.LevelManager.StartTransition();
            }
        }
    }
} 