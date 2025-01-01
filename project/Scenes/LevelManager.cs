using Microsoft.Xna.Framework;

namespace project.Scenes
{
    public class LevelManager
    {
        private int currentLevel = 1;
        private bool isTransitioning = false;
        private const int MAX_LEVEL = 2;

        public int CurrentLevel => currentLevel;
        public bool IsTransitioning => isTransitioning;

        public void StartTransition()
        {
            if (currentLevel < MAX_LEVEL)
            {
                isTransitioning = true;
            }
        }

        public void CompleteTransition()
        {
            if (currentLevel < MAX_LEVEL)
            {
                currentLevel++;
                isTransitioning = false;
                System.Diagnostics.Debug.WriteLine($"Transitioning to Level {currentLevel}");
            }
        }

        public void Reset()
        {
            currentLevel = 1;
            isTransitioning = false;
        }
    }
}