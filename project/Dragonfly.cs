using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using project.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.Interfaces;

namespace project
{
    public class Dragonfly: IGameObject
    {
        private Texture2D dragonflyTexture;
        private Animation flying;
        private Vector2 position;
        public Dragonfly(Texture2D texture)
        {
            dragonflyTexture = texture;
            flying = new Animation();
            flying.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 32)));
            flying.AddFrame(new AnimationFrame(new Rectangle(32, 0, 32, 32)));
            flying.AddFrame(new AnimationFrame(new Rectangle(64, 0, 32, 32)));
            flying.AddFrame(new AnimationFrame(new Rectangle(96, 0, 32, 32)));

            position = new Vector2(400, 100);
        }

        public void Update(GameTime gameTime)
        {
            flying.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //adjust size (1.0f = original size)
            float scale = 3f;

            //walking animation
            spriteBatch.Draw(dragonflyTexture, position, flying.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0f);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 30, 30);
        }
    }
}
