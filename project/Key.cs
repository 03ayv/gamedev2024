using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using project.Animations;
using project.Interfaces;

namespace project
{
    public class Key : IGameObject
    {
        private Texture2D keyTexture;
        private Animation idle;
        private Vector2 position;
        private bool isCollected = false;

        public Key(Texture2D texture, Vector2 startPosition)
        {
            keyTexture = texture;
            position = startPosition;

            idle = new Animation();
            idle.AddFrame(new AnimationFrame(new Rectangle(0, 80, 16, 16)));
            idle.AddFrame(new AnimationFrame(new Rectangle(16, 80, 16, 16)));
            idle.AddFrame(new AnimationFrame(new Rectangle(32, 80, 16, 16)));
            idle.AddFrame(new AnimationFrame(new Rectangle(48, 80, 16, 16)));
            idle.AddFrame(new AnimationFrame(new Rectangle(64, 80, 16, 16)));
            idle.AddFrame(new AnimationFrame(new Rectangle(80, 80, 16, 16)));

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
                spriteBatch.Draw(keyTexture, position, idle.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
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