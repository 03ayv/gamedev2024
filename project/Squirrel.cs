using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using project.Animations;
using project.Interfaces;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    public class Squirrel : IGameObject
    {
        private Texture2D squirrelTexture;
        private Animation walking;
        private Vector2 position;
        private Vector2 speed;

        public Squirrel(Texture2D texture)
        {
            squirrelTexture = texture;
            walking = new Animation();
            walking.AddFrame(new AnimationFrame(new Rectangle(0, 64, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(32, 64, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(64, 64, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(96, 64, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(128, 64, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(160, 64, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(192, 64, 32, 32)));

            position = new Vector2(500, 300);
            speed = new Vector2(-2, 0);
        }

        public void Update(GameTime gameTime)
        {
            Move();
            walking.Update(gameTime);
        }
        private void Move()
        {
            position += speed;

            //speed += acceleration;

            /*
            if(position.X>600 || position.X < 0)
            {
                speed.X *= -1;
                //acceleration.X *= -1;
            }
            if (position.Y > 400 || position.Y < 0)
            {
                speed.Y *= -1;
                //acceleration *= -1;
            }
            */
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //adjust size (1.0f = original size)
            float scale = 3f;

            //walking animation
            spriteBatch.Draw(squirrelTexture, position, walking.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0f);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 30, 30);
        }
    }
}
