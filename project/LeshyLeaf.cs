using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using project.Animations;
using project.Input;
using project.Interfaces;
using project.Tiles;
using SharpDX.Direct3D9;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    public class LeshyLeaf: IGameObject
    {
        private Texture2D leshyLeafTexture;
        private Animation walking;
        private Animation idle;
        private Animation currentAnimation;

        private Vector2 position;

        //input
        IInputReader inputReader;

        //jump
        private Vector2 velocity;
        private bool isJumping;
        private float gravity = 0.6f;
        private float jumpForce = -8f;
        private float groundLevel;

        //double jump
        private int jumpCount = 0;
        private const int MAX_JUMPS = 2;

        //set which side to face
        private bool isFacingRight = true;

        //tilemanager
        TileManager tileManager;

        public LeshyLeaf(Texture2D texture, IInputReader inputReader, TileManager tileManager)
        {
            leshyLeafTexture = texture;

            walking = new Animation();
            walking.AddFrame(new AnimationFrame(new Rectangle(0,40,32,32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(32,40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(64, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(96, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(128, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(160, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(192, 40, 32, 32)));
            walking.AddFrame(new AnimationFrame(new Rectangle(224, 40, 32, 32)));

            idle = new Animation();
            idle.AddFrame(new AnimationFrame(new Rectangle(0, 8, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(32, 8, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(64, 8, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(96, 8, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(128, 8, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(160, 8, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(192, 8, 32, 32)));
            idle.AddFrame(new AnimationFrame(new Rectangle(224, 8, 32, 32)));

            position = new Vector2(0, 1185);

            //set current animation
            currentAnimation = idle;

            //jump
            velocity = Vector2.Zero;
            groundLevel = 1185;

            //read input
            this.inputReader = inputReader;

            
            this.tileManager = tileManager;
        }

        public void Update(GameTime gameTime)
        {
            var direction = inputReader.ReadInput();
            direction *= 4; //speed of keyboard movement
            //position += direction;

            //horizontal movement
            position.X += direction.X;

            //check ground collision
            Rectangle nextBounds = new Rectangle(
                (int)position.X + 16,
                (int)(position.Y + velocity.Y) + 16,
                32 * 4 - 32,
                32 * 4 - 32);

            if (tileManager.CheckCollision(nextBounds))
            {
                Rectangle tileBounds = tileManager.GetSolidTileBounds(nextBounds);
                if (tileBounds != Rectangle.Empty)
                {
                    if (velocity.Y > 0) // Falling
                    {
                        position.Y = tileBounds.Top - (32 * 4) + 30; //adjust ground level
                        velocity.Y = 0;
                        isJumping = false;
                        jumpCount = 0;
                    }
                }
            }
            else
            {
                position.Y += velocity.Y;
            }

            //set which side to face
            if (direction.X < 0)
                isFacingRight = false;
            else if (direction.X > 0)
                isFacingRight = true;

            //switch between animations
            if (direction.X != 0)
            {
                currentAnimation = walking;
            }
            else
            {
                currentAnimation = idle;
            }

            //jumping
            if (direction.Y < 0 && jumpCount < MAX_JUMPS)
            {
                velocity.Y = jumpForce;
                isJumping = true;
                jumpCount++;
            }
            //gravity & vertical movement
            velocity.Y += gravity;
            position.Y += velocity.Y;

            currentAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //adjust size (1.0f = original size)
            float scale = 4f;

            //set which side to face
            var effects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            //animation
            spriteBatch.Draw(leshyLeafTexture, position, currentAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, effects, 0f);
        }
        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 32, 32);
        }
    }
}
