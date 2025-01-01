using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project.Scenes
{
    public class GameOverScene
    {
        private Rectangle tryAgainButton;
        private Rectangle exitButton;
        private bool isVisible;
        private SpriteFont font;
        private MouseState previousMouseState;
        private Texture2D buttonTexture;
        private string title = "GAME OVER!";

        public bool IsVisible => isVisible;

        public GameOverScene(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;
            isVisible = false;
            tryAgainButton = new Rectangle(0, 0, 200, 50);
            exitButton = new Rectangle(0, 0, 200, 50);
            buttonTexture = CreateButtonTexture(graphicsDevice);
        }

        public void Show()
        {
            isVisible = true;
        }

        public void Hide()
        {
            isVisible = false;
        }

        public bool Update(Vector2 cameraPosition)
        {
            if (!isVisible) return false;

            tryAgainButton.X = (int)cameraPosition.X + 300;
            tryAgainButton.Y = (int)cameraPosition.Y + 250;

            exitButton.X = (int)cameraPosition.X + 300;
            exitButton.Y = (int)cameraPosition.Y + 350;

            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released &&
                previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = new Point(
                    currentMouseState.X + (int)cameraPosition.X,
                    currentMouseState.Y + (int)cameraPosition.Y
                );

                if (tryAgainButton.Contains(mousePosition))
                {
                    Hide();
                    return true;
                }
                else if (exitButton.Contains(mousePosition))
                {
                    System.Environment.Exit(0);
                }
            }

            previousMouseState = currentMouseState;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            if (!isVisible) return;

            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.Black * 0.9f });

            Rectangle fullScreen = new Rectangle(
                (int)cameraPosition.X,
                (int)cameraPosition.Y,
                spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height
            );

            spriteBatch.Draw(pixel, fullScreen, Color.White);

            Vector2 titleSize = font.MeasureString(title);
            Vector2 titlePosition = new Vector2(
                cameraPosition.X + (spriteBatch.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                cameraPosition.Y + 150
            );
            spriteBatch.DrawString(font, title, titlePosition, Color.White);

            spriteBatch.Draw(buttonTexture, tryAgainButton, Color.White);
            spriteBatch.Draw(buttonTexture, exitButton, Color.White);

            DrawCenteredText(spriteBatch, "TRY AGAIN", tryAgainButton);
            DrawCenteredText(spriteBatch, "EXIT", exitButton);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle button)
        {
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPosition = new Vector2(
                button.X + (button.Width - textSize.X) / 2,
                button.Y + (button.Height - textSize.Y) / 2
            );
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }

        private Texture2D CreateButtonTexture(GraphicsDevice graphicsDevice)
        {
            int width = 200;
            int height = 50;
            int radius = 10;
            Color buttonColor = new Color(139, 0, 0);  // Dark red for game over

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