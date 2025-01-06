using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using project.Animations;
using project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Collectibles
{
    public class Gold : IGameObject, ICollectible
    {
        private Texture2D goldTexture;
        private Animation idle;
        private Vector2 position;
        private bool isCollected = false;

        public Gold(Texture2D texture, Vector2 startPosition)
        {
            goldTexture = texture;
            position = startPosition;

            idle = new Animation();
            idle.AddFrame(new AnimationFrame(new Rectangle(128, 135, 28, 28)));

        }

        public void Update(GameTime gameTime)
        {
            if (!isCollected)
            {
                idle.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isCollected)
            {
                float scale = 4f;

                //idle animation
                spriteBatch.Draw(goldTexture, position, idle.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)position.X + 16,
                (int)position.Y + 16,
                32,
                32
            );
        }

        public void Collect()
        {
            isCollected = true;
        }

        public bool IsCollected()
        {
            return isCollected;
        }
    }
}
