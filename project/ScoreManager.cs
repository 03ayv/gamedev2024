using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project
{
    public class ScoreManager
    {
        private int score;
        private SpriteFont font;
        private Vector2 position;

        public ScoreManager(SpriteFont font)
        {
            this.font = font;
            score = 0;
        }

        public void AddPoints(int points)
        {
            score += points;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            position = new Vector2(
                cameraPosition.X + 710, //adjust to fit screen
                cameraPosition.Y + 20
            );

            string scoreText = $"Score: {score}";
            spriteBatch.DrawString(font, scoreText, position, Color.White);
        }

        public int GetScore()
        {
            return score;
        }

        public void Reset()
        {
            score = 0;
        }
    }
} 