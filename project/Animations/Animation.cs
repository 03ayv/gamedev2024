using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Animations
{
    public class Animation
    {
        private List<AnimationFrame> frames;
        private int currentFrameIndex;
        private float frameTimer;
        private float frameInterval = 0.1f;
        private bool shouldLoop;

        public Animation(bool shouldLoop = true)
        {
            frames = new List<AnimationFrame>();
            currentFrameIndex = 0;
            frameTimer = 0;
            this.shouldLoop = shouldLoop;
        }

        public void AddFrame(AnimationFrame frame)
        {
            frames.Add(frame);
        }

        public void Update(GameTime gameTime)
        {
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimer >= frameInterval)
            {
                frameTimer = 0;
                if (currentFrameIndex < frames.Count - 1)
                {
                    currentFrameIndex++;
                }
                else if (shouldLoop)
                {
                    currentFrameIndex = 0;
                }
            }
        }

        public AnimationFrame CurrentFrame
        {
            get { return frames[currentFrameIndex]; }
        }

        public bool IsLastFrame
        {
            get { return currentFrameIndex == frames.Count - 1; }
        }
    }
}
