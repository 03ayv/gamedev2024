using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project
{
    public class LevelTransitionScreen
    {
        private Rectangle nextLevelButton;
        private bool isVisible;
        private SpriteFont font;
        private int currentScore;
        private MouseState previousMouseState;
        private Texture2D buttonTexture;

        public LevelTransitionScreen(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;
            isVisible = false;
            nextLevelButton = new Rectangle(0, 0, 200, 50); //position based on camera
            buttonTexture = CreateButtonTexture(graphicsDevice);
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

            //position button relative to camera
            nextLevelButton.X = (int)cameraPosition.X + 300;
            nextLevelButton.Y = (int)cameraPosition.Y + 300;

            MouseState currentMouseState = Mouse.GetState();
            bool clicked = false;

            if (currentMouseState.LeftButton == ButtonState.Released && 
                previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = new Point(
                    currentMouseState.X + (int)cameraPosition.X,
                    currentMouseState.Y + (int)cameraPosition.Y
                );

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

            //semi-transparent background
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.Black * 0.7f });
            
            Rectangle fullScreen = new Rectangle(
                (int)cameraPosition.X, 
                (int)cameraPosition.Y, 
                spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height
            );
            
            spriteBatch.Draw(pixel, fullScreen, Color.White);

            //show score
            string scoreText = $"Current Score: {currentScore}";
            Vector2 scorePosition = new Vector2(
                cameraPosition.X + 300,
                cameraPosition.Y + 200
            );
            spriteBatch.DrawString(font, scoreText, scorePosition, Color.White);

            // Draw button with custom texture
            spriteBatch.Draw(buttonTexture, nextLevelButton, Color.White);
            
            //next level button text
            string buttonText = "Next Level!";
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
            int radius = 10; // Corner radius
            Color buttonColor = new Color(34, 139, 34); // Dark green

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Check if pixel is in corner regions
                    bool inButton = true;
                    if (x < radius && y < radius) // Top-left corner
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(radius, radius));
                        inButton = distance <= radius;
                    }
                    else if (x >= width - radius && y < radius) // Top-right corner
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width - radius, radius));
                        inButton = distance <= radius;
                    }
                    else if (x < radius && y >= height - radius) // Bottom-left corner
                    {
                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(radius, height - radius));
                        inButton = distance <= radius;
                    }
                    else if (x >= width - radius && y >= height - radius) // Bottom-right corner
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
    }
} 