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
        private float horizontalVelocity = 0f;
        private float acceleration = 0.3f;
        private float deceleration = 0.5f;
        private float maxSpeed = 4f;

        //input
        IInputReader inputReader;

        //jump
        private Vector2 velocity;
        private bool isJumping;
        private float gravity = 0.9f;
        private float jumpForce = -14f;
        private float groundLevel;

        //double jump
        private int jumpCount = 0;
        private const int MAX_JUMPS = 2;

        //set which side to face
        private bool isFacingRight = true;

        //tilemanager
        TileManager tileManager;

        //flicker!!
        private bool isInvulnerable = false;
        private float invulnerabilityTimer = 0f;
        private const float INVULNERABILITY_DURATION = 1.0f;
        private bool isVisible = true;
        private float flickerInterval = 0.1f;
        private float flickerTimer = 0f;

        private Vector2? queuedResetPosition = null;

        public Vector2 Position 
        { 
            get { return position; }
        }

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
            //flickering
            if (isInvulnerable)
            {
                invulnerabilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                flickerTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (flickerTimer >= flickerInterval)
                {
                    isVisible = !isVisible;
                    flickerTimer = 0f;
                }

                if (invulnerabilityTimer >= INVULNERABILITY_DURATION)
                {
                    isInvulnerable = false;
                    isVisible = true;
                    
                    if (queuedResetPosition.HasValue)
                    {
                        ResetPosition(queuedResetPosition.Value);
                        queuedResetPosition = null;
                    }
                }

                return; //pause movement during flickering
            }

            var direction = inputReader.ReadInput();
            //direction *= 4; //adjust speed

            //horizontal movement
            if (direction.X != 0)
            {
                Rectangle nextHorizontalBounds_ = new Rectangle(
                    (int)(position.X + direction.X) + 16,
                    (int)position.Y + 16,
                    32 * 4 - 32,
                    32 * 4 - 32
                );

                //accelerate
                horizontalVelocity += direction.X * acceleration;
                horizontalVelocity = MathHelper.Clamp(horizontalVelocity, -maxSpeed, maxSpeed);

                //set facing direction
                isFacingRight = direction.X > 0;

                //check for wall collision
                if (!tileManager.CheckCollision(nextHorizontalBounds_, true))
                {
                    position.X += direction.X;
                }
            }
            else
            {
                //decelerate when no input
                if (horizontalVelocity > 0)
                    horizontalVelocity = Math.Max(0, horizontalVelocity - deceleration);
                else if (horizontalVelocity < 0)
                    horizontalVelocity = Math.Min(0, horizontalVelocity + deceleration);
            }

            Rectangle nextHorizontalBounds = new Rectangle(
                    (int)(position.X + direction.X) + 16,
                    (int)position.Y + 16,
                    32 * 4 - 32,
                    32 * 4 - 32
                );

            if (!tileManager.CheckCollision(nextHorizontalBounds, true))
            {
                position.X += horizontalVelocity;
            }
            else
            {
                //stop momentum on collision
                horizontalVelocity = 0;
            }

            //set which side to face
            if (direction.X < 0)
                isFacingRight = false;
            else if (direction.X > 0)
                isFacingRight = true;

            currentAnimation = direction.X != 0 ? walking : idle;

            //jumping
            if (direction.Y < 0 && jumpCount < MAX_JUMPS)
            {
                velocity.Y = jumpForce;
                isJumping = true;
                jumpCount++;
            }

            //gravity
            velocity.Y += gravity;

            //vertical movement
            Rectangle nextVerticalBounds = new Rectangle(
                (int)position.X + 16,
                (int)(position.Y + velocity.Y) + 16,
                32 * 4 - 32,
                32 * 4 - 32
            );

            if (tileManager.CheckCollision(nextVerticalBounds))
            {
                Rectangle tileBounds = tileManager.GetSolidTileBounds(nextVerticalBounds);
                if (tileBounds != Rectangle.Empty && velocity.Y > 0)
                {
                    position.Y = tileBounds.Top - (32 * 4) + 30f; //adjust ground level
                    velocity.Y = 0;
                    isJumping = false;
                    jumpCount = 0;
                }
            }
            else
            {
                position.Y += velocity.Y;
            }

            currentAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isVisible) return;
            
            //adjust size (1.0f = original size)
            float scale = 4f;

            //set which side to face
            var effects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            //animation
            spriteBatch.Draw(leshyLeafTexture, position, currentAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, effects, 0f);
        }
        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)position.X + 16,
                (int)position.Y + 16,
                20 * 4 - 32,
                20 * 4 - 32
            );
        }

        public void ResetPosition(Vector2 newPosition)
        {
            position = newPosition;
            velocity = Vector2.Zero;
            isJumping = false;
            jumpCount = 0;
            isFacingRight = true;
            currentAnimation = idle;
            // Reset invulnerability states
            isInvulnerable = false;
            isVisible = true;
            invulnerabilityTimer = 0f;
            flickerTimer = 0f;
            queuedResetPosition = null;
        }

        //flicker + game pause
        public void StartInvulnerability()
        {
            isInvulnerable = true;
            invulnerabilityTimer = 0f;
            Game1.PauseGame();
        }

        public bool IsInvulnerable => isInvulnerable;

        public void UpdateInvulnerability(GameTime gameTime)
        {
            if (isInvulnerable)
            {
                invulnerabilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                flickerTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (flickerTimer >= flickerInterval)
                {
                    isVisible = !isVisible;
                    flickerTimer = 0f;
                }

                if (invulnerabilityTimer >= INVULNERABILITY_DURATION)
                {
                    isInvulnerable = false;
                    isVisible = true;
                    Game1.ResumeGame();
                    
                    if (queuedResetPosition.HasValue)
                    {
                        ResetPosition(queuedResetPosition.Value);
                        queuedResetPosition = null;
                    }
                }
            }
        }

        //wait for flickering to reset position
        public void QueuePositionReset(Vector2 newPosition)
        {
            queuedResetPosition = newPosition;
        }
    }
}

