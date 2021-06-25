using System;
using Microsoft.Xna.Framework.Input;

namespace IsoEngine
{
    public static class InputHandler
    {

        static KeyboardState currentKeyboardState;
        static KeyboardState oldKeyboardState;

        static MouseState currentMouseState;
        static MouseState oldMouseState;

        public static int mX;
        public static int mY;
        public static Vec2 mouseVec = new Vec2(0, 0);

        public static void Update()
        {

            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            oldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            mX = currentMouseState.X;
            mY = currentMouseState.Y;
            mouseVec.x = mX;
            mouseVec.y = mY;

        }

        public static KeyboardState GetKeyboardState()
        {
            return currentKeyboardState;
        }

        public static MouseState GetMouseState()
        {
            return currentMouseState;
        }

    }
}
