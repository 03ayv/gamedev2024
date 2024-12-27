using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using project.Animations;
using project.Input;
using project.Interfaces;
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
        private Vector2 position;

        //input
        IInputReader inputReader;

        //jump
        private Vector2 velocity;
        private bool isJumping;
        private float gravity = 0.5f;
        private float jumpForce = -8f;
        private float groundLevel;

        //double jump
        private int jumpCount = 0;
        private const int MAX_JUMPS = 2;

        public LeshyLeaf(Texture2D texture, IInputReader inputReader)
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

            position = new Vector2(0, 300);

            //jump
            velocity = Vector2.Zero;
            groundLevel = 300;

            //read input
            this.inputReader = inputReader;
        }

        public void Update(GameTime gameTime)
        {
            var direction = inputReader.ReadInput();
            direction *= 4; //speed of keyboard movement
            position += direction;

            //jumping
            if (direction.Y < 0 && !isJumping)
            {
                velocity.Y = jumpForce;
                isJumping = true;
            }
            //gravity
            velocity.Y += gravity;
            position.Y += velocity.Y;
            //ground collision
            if (position.Y >= groundLevel)
            {
                position.Y = groundLevel;
                velocity.Y = 0;
                isJumping = false;
            }

            walking.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //adjust size (1.0f = original size)
            float scale = 4f;

            //walking animation
            spriteBatch.Draw(leshyLeafTexture, position, walking.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
