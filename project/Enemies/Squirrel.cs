using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using project.Animations;
using project.Interfaces;
using project.Tiles;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Enemies
{
    public class Squirrel : IGameObject
    {
        private Texture2D squirrelTexture;
        private Animation idle;
        private Animation jump;
        private Animation currentAnimation;
        private Vector2 position;
        private Vector2 speed;
        private LeshyLeaf leshyLeaf;
        private const float DETECTION_RANGE = 100f; //detect leshyleaf
        private bool hasJumped = false;

        //jump
        private Vector2 velocity = Vector2.Zero;
        private float gravity = 0.5f;
        private float jumpForce = -8f;
        private bool isJumping = false;

        public Squirrel(Texture2D texture, Vector2 startPosition, LeshyLeaf leshyLeaf)
        {
            squirrelTexture = texture;
            idle = new Animation();
            idle.AddFrame(new AnimationFrame(new Rectangle(0, 32, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(32, 32, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(64, 32, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(96, 32, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(128, 32, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(160, 32, 32, 32)));

            jump = new Animation();
            jump.AddFrame(new AnimationFrame(new Rectangle(0, 160, 32, 32)));
            jump.AddFrame(new AnimationFrame(new Rectangle(32, 160, 32, 32)));
            jump.AddFrame(new AnimationFrame(new Rectangle(64, 160, 32, 32)));
            jump.AddFrame(new AnimationFrame(new Rectangle(96, 160, 32, 32)));

            position = startPosition;
            speed = new Vector2(-2, 0);
            this.leshyLeaf = leshyLeaf;
            currentAnimation = idle;
        }

        public void Update(GameTime gameTime)
        {
            //calculate distance between leshyleaf
            float distanceToPlayer = Vector2.Distance(position, new Vector2(leshyLeaf.GetBounds().Center.X, leshyLeaf.GetBounds().Center.Y));

            if (distanceToPlayer <= DETECTION_RANGE && !hasJumped)
            {
                currentAnimation = jump;
                hasJumped = true;
                isJumping = true;
                velocity.Y = jumpForce;
            }
            else if (distanceToPlayer > DETECTION_RANGE && !isJumping)
            {
                currentAnimation = idle;
                hasJumped = false;
            }

            //gravity
            if (isJumping)
            {
                velocity.Y += gravity;
                position.Y += velocity.Y;

                //check landing
                if (position.Y >= 1185) //ground level
                {
                    position.Y = 1185;
                    isJumping = false;
                    velocity = Vector2.Zero;
                }
            }

            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scale = 3f;

            spriteBatch.Draw(
                squirrelTexture,
                position,
                currentAnimation.CurrentFrame.SourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.FlipHorizontally,
                0f
            );
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
