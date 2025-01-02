using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project.Managers
{
    public class LivesManager
    {
        private int currentLives;
        private const int MAX_LIVES = 3;
        private Texture2D heartTexture;
        private Vector2 position;
        private float scale = 1.5f;
        private float padding = -50f;

        public LivesManager(Texture2D heartTexture, Vector2 position)
        {
            this.heartTexture = heartTexture;
            this.position = position;
            currentLives = MAX_LIVES;
        }

        public void LoseLife()
        {
            if (currentLives > 0)
                currentLives--;
        }

        public bool IsGameOver()
        {
            return currentLives <= 0;
        }

        public void Reset()
        {
            currentLives = MAX_LIVES;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < currentLives; i++)
            {
                Vector2 heartPosition = new Vector2(
                    position.X + (i * (heartTexture.Width * scale + padding)),
                    position.Y
                );
                spriteBatch.Draw(heartTexture, heartPosition, null, Color.Red, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
