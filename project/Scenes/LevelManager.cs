using Microsoft.Xna.Framework;

namespace project.Scenes
{
    public class LevelManager
    {
        private int currentLevel = 1;
        private bool isTransitioning = false;

        public int CurrentLevel => currentLevel;
        public bool IsTransitioning => isTransitioning;

        public void StartTransition()
        {
            isTransitioning = true;
        }

        public void CompleteTransition()
        {
            currentLevel++;
            isTransitioning = false;
        }

        public void Reset()
        {
            currentLevel = 1;
            isTransitioning = false;
        }
    }
}