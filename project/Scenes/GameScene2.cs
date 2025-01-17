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
using System;

namespace project.Scenes
{
    public class GameScene2 : BaseScene
    {
        public GameScene2(GraphicsDevice graphicsDevice, ContentManager content) 
            : base(graphicsDevice, content)
        {
        }

        public override void LoadContent()
        {
            var leshyLeafTexture = content.Load<Texture2D>("LeshyLeaf");
            var porcupineTexture = content.Load<Texture2D>("Porcupine");
            var dragonflyTexture = content.Load<Texture2D>("Dragonfly");
            var squirrelTexture = content.Load<Texture2D>("Squirrel");
            var goldTexture = content.Load<Texture2D>("Key");
            var coinTexture = content.Load<Texture2D>("Key");
            var monsterTexture = content.Load<Texture2D>("Monster");
            InitializeGameObjects(leshyLeafTexture, porcupineTexture, dragonflyTexture, squirrelTexture, goldTexture, coinTexture, monsterTexture);
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game1.IsGamePaused)
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
        }

        public override void Draw(GameTime gameTime)
        {
            key?.Draw(spriteBatch);
            foreach (var coin in coins)
            {
                coin.Draw(spriteBatch);
            }
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        private void InitializeGameObjects(Texture2D leshyLeafTexture, Texture2D porcupineTexture, 
            Texture2D dragonflyTexture, Texture2D squirrelTexture, Texture2D goldTexture, 
            Texture2D coinTexture, Texture2D monsterTexture)
        {
            //more enemies
            enemies = new List<IGameObject>
            {
                new Porcupine(porcupineTexture, new Vector2(800, 1185)),
                //new Porcupine(porcupineTexture, new Vector2(1700, 1185)),
                new Dragonfly(dragonflyTexture, new Vector2(400, 950)),
                new Dragonfly(dragonflyTexture, new Vector2(100, 1100)),
                new Dragonfly(dragonflyTexture, new Vector2(1500, 1000)),
                new Squirrel(squirrelTexture, new Vector2(300, 1185), Game1.LeshyLeaf),
                new Squirrel(squirrelTexture, new Vector2(1500, 1185), Game1.LeshyLeaf),
                new Monster(monsterTexture, new Vector2(2000,1092))
                //new Squirrel(squirrelTexture, new Vector2(2000, 1185), Game1.LeshyLeaf)
            };

            key = new Gold(goldTexture, new Vector2(2170, 1200));

            
            List<Vector2> coinPositions = GenerateCoinPositions();
            coins = coinPositions.Select(position => new Coin(coinTexture, position)).ToList();
        }

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
                new Vector2(1650, 1195),
                new Vector2(1750, 1195),
                new Vector2(1850, 1195),
                new Vector2(1950, 1195),
            };
            return positions;
        }

        protected override void CheckCollisions()
        {
            foreach (var enemy in enemies)
            {
                if (!Game1.LeshyLeaf.IsInvulnerable && Game1.LeshyLeaf.GetBounds().Intersects(enemy.GetBounds()))
                {
                    Game1.LeshyLeaf.StartInvulnerability();
                    Game1.LivesManager.LoseLife();
                    if (Game1.LivesManager.IsGameOver())
                    {
                        Game1.GameOver = true;
                    }
                    else
                    {
                        Game1.LeshyLeaf.QueuePositionReset(new Vector2(Math.Max(50, Game1.LeshyLeaf.Position.X - 50), 1185));
                    }
                    return;
                }
            }

            for (int i = coins.Count - 1; i >= 0; i--)
            {
                if (!coins[i].IsCollected() && Game1.LeshyLeaf.GetBounds().Intersects(coins[i].GetBounds()))
                {
                    coins[i].Collect();
                    Game1.ScoreManager.AddPoints(10);
                }
            }

            if (key != null && Game1.LeshyLeaf.GetBounds().Intersects(key.GetBounds()))
            {
                key.Collect();
                key.Collect();
                Game1.ScoreManager.AddPoints(100); // Bonus points for collecting gold
                Game1.Instance.ShowVictoryScene(Game1.ScoreManager.GetScore());
            }
        }
    }
} 