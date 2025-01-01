using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace project.Scenes
{
    public class SceneManager
    {
        private Dictionary<string, BaseScene> scenes;
        private BaseScene currentScene;
        private GraphicsDevice graphicsDevice;
        private ContentManager content;

        public SceneManager(GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            scenes = new Dictionary<string, BaseScene>();
        }

        public void AddScene(string sceneName, BaseScene scene)
        {
            scenes[sceneName] = scene;
        }

        public void LoadScene(string sceneName)
        {
            if (scenes.ContainsKey(sceneName))
            {
                currentScene = scenes[sceneName];
                currentScene.LoadContent();
                currentScene.Initialize();
            }
        }

        public void Update(GameTime gameTime)
        {
            currentScene?.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            currentScene?.Draw(gameTime);
        }

        public BaseScene GetCurrentScene()
        {
            return currentScene;
        }
    }
} 