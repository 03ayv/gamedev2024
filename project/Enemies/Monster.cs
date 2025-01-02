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
    public class Monster : IGameObject
    {
        private Texture2D monsterTexture;
        private Animation walking;
        private Animation death;
        private Animation currentAnimation;
        private Vector2 position;
        private Vector2 speed;

        //set which side to face
        private bool isFacingRight = false;

        //distance to move
        private float patrolDistance = 170f;
        private float startX;

        //damage & flickering
        private bool isInvulnerable = false;
        private float invulnerabilityTimer = 0f;
        private const float INVULNERABILITY_DURATION = 0.5f;
        private bool isVisible = true;
        private float flickerInterval = 0.1f;
        private float flickerTimer = 0f;
        private const float ATTACK_RANGE = 100f;
        private int health = 3;
        private bool isDying = false;
        private bool deathAnimationComplete = false;

        public Monster(Texture2D texture, Vector2 startPosition)
        {
            monsterTexture = texture;
            walking = new Animation();
            walking.AddFrame(new AnimationFrame(new Rectangle(0, 64, 64, 64)));
            walking.AddFrame(new AnimationFrame(new Rectangle(64, 64, 64, 64)));
            walking.AddFrame(new AnimationFrame(new Rectangle(128, 64, 64, 64)));
            walking.AddFrame(new AnimationFrame(new Rectangle(192, 64, 64, 64)));
            walking.AddFrame(new AnimationFrame(new Rectangle(256, 64, 64, 64)));
            walking.AddFrame(new AnimationFrame(new Rectangle(320, 64, 64, 64)));
            walking.AddFrame(new AnimationFrame(new Rectangle(384, 64, 64, 64)));
            walking.AddFrame(new AnimationFrame(new Rectangle(448, 64, 64, 64)));

            death = new Animation(false);  // Set to not loop
            death.AddFrame(new AnimationFrame(new Rectangle(0, 256, 64, 64)));
            death.AddFrame(new AnimationFrame(new Rectangle(64, 256, 64, 64)));
            death.AddFrame(new AnimationFrame(new Rectangle(128, 256, 64, 64)));
            death.AddFrame(new AnimationFrame(new Rectangle(192, 256, 64, 64)));
            death.AddFrame(new AnimationFrame(new Rectangle(256, 256, 64, 64)));
            death.AddFrame(new AnimationFrame(new Rectangle(320, 256, 64, 64)));

            currentAnimation = walking;
            position = startPosition;
            startX = startPosition.X;
            speed = new Vector2(-1, 0);
        }

        public void Update(GameTime gameTime)
        {
            if (isDying)
            {
                if (!deathAnimationComplete)
                {
                    death.Update(gameTime);
                    if (death.IsLastFrame)
                    {
                        deathAnimationComplete = true;
                    }
                }
                return;
            }

            if (health > 0)
            {
                Move();
                walking.Update(gameTime);
                UpdateInvulnerability(gameTime);

                // Check if LeshyLeaf is attacking within range
                if (Game1.LeshyLeaf.IsAttacking)
                {
                    float distance = Vector2.Distance(position, Game1.LeshyLeaf.Position);
                    if (distance <= ATTACK_RANGE && !isInvulnerable)
                    {
                        TakeDamage();
                    }
                }
            }
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

        private void TakeDamage()
        {
            health--;
            isInvulnerable = true;
            isVisible = false;
            
            if (health <= 0)
            {
                isDying = true;
                currentAnimation = death;
            }
        }

        private void UpdateInvulnerability(GameTime gameTime)
        {
            if (isInvulnerable)
            {
                invulnerabilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                flickerTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (flickerTimer >= flickerInterval)
                {
                    isVisible = !isVisible;
                    flickerTimer = 0;
                }

                if (invulnerabilityTimer >= INVULNERABILITY_DURATION)
                {
                    isInvulnerable = false;
                    isVisible = true;
                    invulnerabilityTimer = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible || isDying)
            {
                float scale = 3f;
                var effects = isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(monsterTexture, position, currentAnimation.CurrentFrame.SourceRectangle, 
                    Color.White, 0f, Vector2.Zero, scale, effects, 0f);
            }
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)position.X + 32,
                (int)position.Y + 32,
                64 * 2,
                64 * 2
            );
        }
    }
}
