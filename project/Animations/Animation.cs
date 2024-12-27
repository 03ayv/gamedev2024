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
        public AnimationFrame CurrentFrame { get; set; }
        private List<AnimationFrame> frames;
        private int counter;
        
        // track time per frame
        private float frameTime = 0.1f; // time per frame
        private float currentFrameTime;

        private double frameMovement = 0;

        public Animation()
        {
            frames = new List<AnimationFrame>();
            currentFrameTime = 0;
        }

        public void AddFrame(AnimationFrame animationFrame)
        {
            frames.Add(animationFrame);
            CurrentFrame = frames[0];
        }

        public void Update(GameTime gameTime)
        {
            currentFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
           
            if (currentFrameTime >= frameTime)
            {
                counter++;
                currentFrameTime = 0;

                if (counter >= frames.Count)
                {
                    counter = 0;
                }

                CurrentFrame = frames[counter];
            }
            
        }
    }
}
