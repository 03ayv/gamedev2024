using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace project
{
    public class StartScreen
    {
        private Rectangle playButton;
        private Rectangle exitButton;
        private bool isVisible = true;
        private SpriteFont font;
        private MouseState previousMouseState;
        private Texture2D buttonTexture;
        private string title = "LESHY LEAF";

        public StartScreen(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;
            playButton = new Rectangle(0, 0, 200, 50);
            exitButton = new Rectangle(0, 0, 200, 50);
            buttonTexture = CreateButtonTexture(graphicsDevice);
        }

        public bool Update(Vector2 cameraPosition)
        {
            if (!isVisible) return false;

            playButton.X = (int)cameraPosition.X + 300;
            playButton.Y = (int)cameraPosition.Y + 250;
            
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

                if (playButton.Contains(mousePosition))
                {
                    isVisible = false;
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

            //semi-transparent background
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.Black * 0.9f });
            
            Rectangle fullScreen = new Rectangle(
                (int)cameraPosition.X, 
                (int)cameraPosition.Y, 
                spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height
            );
            
            spriteBatch.Draw(pixel, fullScreen, Color.White);

            //title
            Vector2 titleSize = font.MeasureString(title);
            Vector2 titlePosition = new Vector2(
                cameraPosition.X + (spriteBatch.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                cameraPosition.Y + 150
            );
            spriteBatch.DrawString(font, title, titlePosition, Color.White);

            //buttons
            spriteBatch.Draw(buttonTexture, playButton, Color.White);
            spriteBatch.Draw(buttonTexture, exitButton, Color.White);

            //text
            string playText = "PLAY";
            string exitText = "EXIT";
            
            DrawCenteredText(spriteBatch, playText, playButton);
            DrawCenteredText(spriteBatch, exitText, exitButton);
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
    }
} 