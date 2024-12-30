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
        private Vector2 speed;
        //private Vector2 acceleration;

        //fly up and down
        private float originalY;
        private float time = 0f;  //time sine wave
        private float amplitude = 20f;  //distance
        private float frequency = 2f;   //speed

        public Dragonfly(Texture2D texture)
        {
            dragonflyTexture = texture;
            flying = new Animation();
            flying.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 32)));
            flying.AddFrame(new AnimationFrame(new Rectangle(32, 0, 32, 32)));
            flying.AddFrame(new AnimationFrame(new Rectangle(64, 0, 32, 32)));
            flying.AddFrame(new AnimationFrame(new Rectangle(96, 0, 32, 32)));

            position = new Vector2(400, 900);
            speed = new Vector2(-1, 0);
            originalY = position.Y;
            //acceleration = new Vector2(0.1f, 0.1f);
        }

        public void Update(GameTime gameTime)
        {
            Move();
            flying.Update(gameTime);
        }

        private void Move()
        {
            time += 0.05f;
            position.Y = originalY + (float)(Math.Sin(time * frequency) * amplitude);

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
            return new Rectangle((int)position.X, (int)position.Y, 30, 31);
        }
    }
}
