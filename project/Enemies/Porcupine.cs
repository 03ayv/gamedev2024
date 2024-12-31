using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using project.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project.Interfaces;

namespace project.Enemies
{
    public class Porcupine : IGameObject
    {
        private Texture2D porcupineTexture;
        private Animation walking;
        private Vector2 position;
        private Vector2 speed;

        //set which side to face
        private bool isFacingRight = false;

        //distance to move
        private float patrolDistance = 170f;
        private float startX;

        public Porcupine(Texture2D texture, Vector2 startPosition)
        {
            porcupineTexture = texture;
            walking = new Animation();
            walking.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(32, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(64, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(96, 0, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(128, 0, 32, 32)));

            position = startPosition;
            startX = startPosition.X; //initial position
            speed = new Vector2(-1, 0);
        }

        public void Update(GameTime gameTime)
        {
            Move();
            walking.Update(gameTime);
        }

        private void Move()
        {
            position += speed;

            //check position relative to initial position
            if (position.X < startX - patrolDistance || position.X > startX + patrolDistance)
            {
                speed.X *= -1;
                isFacingRight = speed.X > 0;
            }

            if (position.Y > 1185 || position.Y < 100)
            {
                speed.Y *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //adjust size (1.0f = original size)
            float scale = 3f;

            //set which side to face
            var effects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            //walking animation
            spriteBatch.Draw(porcupineTexture, position, walking.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, effects, 0f);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)position.X + 16,
                (int)position.Y + 16,
                32,
                32
            );
        }
    }
}
