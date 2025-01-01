using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project.Scenes
{
    public class LevelTransitionScene
    {
        private Rectangle nextLevelButton;
        private bool isVisible;
        private SpriteFont font;
        private int currentScore;
        private MouseState previousMouseState;
        private Texture2D buttonTexture;
        private Texture2D backgroundPixel;

        public LevelTransitionScene(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;
            isVisible = false;
            nextLevelButton = new Rectangle(300, 300, 200, 50);
            buttonTexture = CreateButtonTexture(graphicsDevice);
            backgroundPixel = new Texture2D(graphicsDevice, 1, 1);
            backgroundPixel.SetData(new[] { Color.Black * 0.7f });
        }

        public void Show(int score)
        {
            isVisible = true;
            currentScore = score;
        }

        public void Hide()
        {
            isVisible = false;
        }

        public bool Update(Vector2 cameraPosition)
        {
            if (!isVisible) return false;

            MouseState currentMouseState = Mouse.GetState();
            bool clicked = false;

            if (currentMouseState.LeftButton == ButtonState.Released &&
                previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                if (nextLevelButton.Contains(mousePosition))
                {
                    clicked = true;
                    Hide();
                }
            }

            previousMouseState = currentMouseState;
            return clicked;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
        {
            if (!isVisible) return;

            Rectangle fullScreen = new Rectangle(0, 0,
                spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height);

            spriteBatch.Draw(backgroundPixel, fullScreen, Color.White);

            string scoreText = $"Current Score: {currentScore}";
            Vector2 scorePosition = new Vector2(300, 200);
            spriteBatch.DrawString(font, scoreText, scorePosition, Color.White);

            spriteBatch.Draw(buttonTexture, nextLevelButton, Color.White);

            string buttonText = "NEXT LEVEL";
            Vector2 textSize = font.MeasureString(buttonText);
            Vector2 textPosition = new Vector2(
                nextLevelButton.X + (nextLevelButton.Width - textSize.X) / 2,
                nextLevelButton.Y + (nextLevelButton.Height - textSize.Y) / 2
            );
            spriteBatch.DrawString(font, buttonText, textPosition, Color.White);
        }

        private Texture2D CreateButtonTexture(GraphicsDevice graphicsDevice)
        {
            int width = 200;
            int height = 50;
            int radius = 10;
            Color buttonColor = new Color(34, 139, 34);

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool inButton = true;
                    if (x < radius && y < radius)
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(radius, radius));
                        inButton = distance <= radius;
                    }
                    else if (x >= width - radius && y < radius)
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width - radius, radius));
                        inButton = distance <= radius;
                    }
                    else if (x < radius && y >= height - radius)
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(radius, height - radius));
                        inButton = distance <= radius;
                    }
                    else if (x >= width - radius && y >= height - radius)
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width - radius, height - radius));
                        inButton = distance <= radius;
                    }

                    data[y * width + x] = inButton ? buttonColor : Color.Transparent;
                }
            }

            texture.SetData(data);
            return texture;
        }

        public bool IsVisible => isVisible;
    }
}