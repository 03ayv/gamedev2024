using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project.Scenes
{
    public class VictoryScene
    {
        private Rectangle playAgainButton;
        private Rectangle exitButton;
        private bool isVisible;
        private SpriteFont font;
        private MouseState previousMouseState;
        private Texture2D buttonTexture;
        private Texture2D backgroundPixel;
        private string title = "YOU WIN!";
        private int finalScore;

        public bool IsVisible => isVisible;

        public VictoryScene(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;
            isVisible = false;
            playAgainButton = new Rectangle(300, 350, 200, 50);
            exitButton = new Rectangle(300, 450, 200, 50);
            buttonTexture = CreateButtonTexture(graphicsDevice);
            backgroundPixel = new Texture2D(graphicsDevice, 1, 1);
            backgroundPixel.SetData(new[] { Color.Black * 0.7f });
        }

        public void Show(int score)
        {
            isVisible = true;
            finalScore = score;
        }

        public void Hide()
        {
            isVisible = false;
        }

        public bool Update(Vector2 cameraPosition)
        {
            if (!isVisible) return false;

            MouseState currentMouseState = Mouse.GetState();
            Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (playAgainButton.Contains(mousePosition))
                {
                    Game1.LivesManager.Reset();
                    Game1.LeshyLeaf.ResetPosition(new Vector2(50, 1185));
                    Hide();
                    return true;
                }
                else if (exitButton.Contains(mousePosition))
                {
                    Game1.Instance.Exit();
                }
            }

            previousMouseState = currentMouseState;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            if (!isVisible) return;

            Rectangle fullScreen = new Rectangle(0, 0,
                spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height);

            spriteBatch.Draw(backgroundPixel, fullScreen, Color.White);

            Vector2 titlePosition = new Vector2(
                (spriteBatch.GraphicsDevice.Viewport.Width - font.MeasureString(title).X) / 2,
                150
            );
            spriteBatch.DrawString(font, title, titlePosition, Color.Gold);

            string scoreText = $"Final Score: {finalScore}";
            Vector2 scorePosition = new Vector2(
                (spriteBatch.GraphicsDevice.Viewport.Width - font.MeasureString(scoreText).X) / 2,
                250
            );
            spriteBatch.DrawString(font, scoreText, scorePosition, Color.White);

            spriteBatch.Draw(buttonTexture, playAgainButton, Color.White);
            spriteBatch.Draw(buttonTexture, exitButton, Color.White);

            DrawCenteredText(spriteBatch, "PLAY AGAIN", playAgainButton);
            DrawCenteredText(spriteBatch, "EXIT", exitButton);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle buttonBounds)
        {
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPosition = new Vector2(
                buttonBounds.X + (buttonBounds.Width - textSize.X) / 2,
                buttonBounds.Y + (buttonBounds.Height - textSize.Y) / 2
            );
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }

        private Texture2D CreateButtonTexture(GraphicsDevice graphicsDevice)
        {
            int width = 200;
            int height = 50;
            int radius = 10;
            Color buttonColor = new Color(34, 139, 34);  // Forest Green

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    data[y * width + x] = buttonColor;
                }
            }

            texture.SetData(data);
            return texture;
        }
    }
} 