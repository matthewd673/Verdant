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
        public static Vec2 mouseVec = new Vec2();

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

        public static bool IsMouseFirstPressed()
        {
            return (oldMouseState.LeftButton == ButtonState.Released &&
                currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool IsMouseFirstReleased()
        {
            return (oldMouseState.LeftButton == ButtonState.Pressed &&
                currentMouseState.LeftButton == ButtonState.Released);
        }

    }
}
