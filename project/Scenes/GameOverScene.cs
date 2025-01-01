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
        private Texture2D backgroundPixel;
        private string title = "GAME OVER!";

        public bool IsVisible => isVisible;

        public GameOverScene(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;
            isVisible = false;
            tryAgainButton = new Rectangle(300, 250, 200, 50);
            exitButton = new Rectangle(300, 350, 200, 50);
            buttonTexture = CreateButtonTexture(graphicsDevice);
            backgroundPixel = new Texture2D(graphicsDevice, 1, 1);
            backgroundPixel.SetData(new[] { Color.Black * 0.7f });
        }

        public void Show()
        {
            isVisible = true;
        }

        public bool Update(Vector2 cameraPosition)
        {
            if (!isVisible) return false;

            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released &&
                previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

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

            Rectangle fullScreen = new Rectangle(0, 0,
                spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height);

            spriteBatch.Draw(backgroundPixel, fullScreen, Color.White);

            Vector2 titlePosition = new Vector2(
                (spriteBatch.GraphicsDevice.Viewport.Width - font.MeasureString(title).X) / 2,
                150
            );
            spriteBatch.DrawString(font, title, titlePosition, Color.White);

            spriteBatch.Draw(buttonTexture, tryAgainButton, Color.White);
            spriteBatch.Draw(buttonTexture, exitButton, Color.White);

            DrawCenteredText(spriteBatch, "TRY AGAIN", tryAgainButton);
            DrawCenteredText(spriteBatch, "EXIT", exitButton);
        }

        private void Hide()
        {
            isVisible = false;
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
            Color buttonColor = new Color(139, 0, 0);  //dark red

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