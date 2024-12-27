using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace project.Input
{
    public class KeyboardReader : IInputReader
    {
        private KeyboardState previousState;
        public Vector2 ReadInput()
        {
            var direction = Vector2.Zero;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
                direction = new Vector2(-1, 0);
            
            if (state.IsKeyDown(Keys.Right))
                direction = new Vector2(1, 0);

            if (state.IsKeyDown(Keys.Up) && !previousState.IsKeyDown(Keys.Up))
                direction.Y = -1;
            previousState = state;

            return direction;
        }
    }
}
