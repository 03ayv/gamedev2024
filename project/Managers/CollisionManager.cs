using Microsoft.Xna.Framework;
using project.Collectibles;
using project.Interfaces;
using System;
using System.Collections.Generic;

namespace project.Managers
{
    public class CollisionManager
    {
        public void CheckEnemyCollisions(List<IGameObject> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (!Game1.LeshyLeaf.IsInvulnerable && Game1.LeshyLeaf.GetBounds().Intersects(enemy.GetBounds()))
                {
                    HandleEnemyCollision();
                    return;
                }
            }
        }

        public void CheckCoinCollisions(List<Coin> coins)
        {
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                if (!coins[i].IsCollected() && Game1.LeshyLeaf.GetBounds().Intersects(coins[i].GetBounds()))
                {
                    HandleCoinCollision(coins[i]);
                }
            }
        }

        public void CheckKeyCollision(ICollectible key)
        {
            if (key != null && Game1.LeshyLeaf.GetBounds().Intersects(key.GetBounds()))
            {
                HandleKeyCollision(key);
            }
        }

        private void HandleEnemyCollision()
        {
            Game1.LeshyLeaf.StartInvulnerability();
            Game1.LivesManager.LoseLife();
            
            if (Game1.LivesManager.IsGameOver())
            {
                Game1.GameOver = true;
            }
            else
            {
                Game1.LeshyLeaf.QueuePositionReset(new Vector2(
                    Math.Max(50, Game1.LeshyLeaf.Position.X - 50), 
                    1185
                ));
            }
        }

        private void HandleCoinCollision(Coin coin)
        {
            coin.Collect();
            Game1.ScoreManager.AddPoints(10);
        }

        private void HandleKeyCollision(ICollectible key)
        {
            key.Collect();
            Game1.ScoreManager.AddPoints(100);
            
            if (key is Gold)
            {
                Game1.Instance.ShowVictoryScene(Game1.ScoreManager.GetScore());
            }
            else
            {
                Game1.LevelManager.StartTransition();
            }
        }
    }
} 