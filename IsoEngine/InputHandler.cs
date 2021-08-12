using System;
using Microsoft.Xna.Framework.Input;

namespace IsoEngine
{
    public static class InputHandler
    {

        public enum MouseButton
        {
            LEFT,
            RIGHT,
            MIDDLE
        }

        static KeyboardState currentKeyboardState;
        static KeyboardState oldKeyboardState;

        static MouseState currentMouseState;
        static MouseState oldMouseState;

        public static int mX;
        public static int mY;
        public static Vec2 mouseVec = new Vec2();

        /// <summary>
        /// Update the current state of the InputHandler.
        /// </summary>
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

        /// <summary>
        /// Get the current KeyboardState.
        /// </summary>
        /// <returns>A KeyboardState.</returns>
        public static KeyboardState GetKeyboardState()
        {
            return currentKeyboardState;
        }

        /// <summary>
        /// Get the current MouseState.
        /// </summary>
        /// <returns>A MouseState.</returns>
        public static MouseState GetMouseState()
        {
            return currentMouseState;
        }

        /// <summary>
        /// Determine if a mouse button is pressed and was released on the previous frame.
        /// </summary>
        /// <param name="mouseButton">The mouse button to check. Defaults to LEFT.</param>
        /// <returns>Returns true if the mouse button is pressed and was released on the previous frame. Returns false otherwise.</returns>
        public static bool IsMouseFirstPressed(MouseButton mouseButton = MouseButton.LEFT)
        {
            ButtonState oldButtonState = oldMouseState.LeftButton;
            ButtonState currentButtonState = currentMouseState.LeftButton;

            if (mouseButton == MouseButton.RIGHT)
            {
                oldButtonState = oldMouseState.RightButton;
                currentButtonState = currentMouseState.RightButton;
            }
            if (mouseButton == MouseButton.MIDDLE)
            {
                oldButtonState = oldMouseState.MiddleButton;
                currentButtonState = currentMouseState.MiddleButton;
            }

            return (oldButtonState == ButtonState.Released &&
                currentButtonState == ButtonState.Pressed);
        }

        /// <summary>
        /// Determine if a mouse button is released and was pressed on the previous frame.
        /// </summary>
        /// <param name="mouseButton">The mouse button to check. Defaults to LEFT.</param>
        /// <returns>Returns true if the mouse button is released and was pressed on the last frame. Returns false otherwise.</returns>
        public static bool IsMouseFirstReleased(MouseButton mouseButton = MouseButton.LEFT)
        {
            ButtonState oldButtonState = oldMouseState.LeftButton;
            ButtonState currentButtonState = currentMouseState.LeftButton;

            if (mouseButton == MouseButton.RIGHT)
            {
                oldButtonState = oldMouseState.RightButton;
                currentButtonState = currentMouseState.RightButton;
            }
            if (mouseButton == MouseButton.MIDDLE)
            {
                oldButtonState = oldMouseState.MiddleButton;
                currentButtonState = currentMouseState.MiddleButton;
            }

            return (oldButtonState == ButtonState.Pressed &&
                currentButtonState == ButtonState.Released);
        }

        /// <summary>
        /// Determine if a given key is pressed and was released on the previous frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns true if the key is pressed and was released on the previous frame. Returns false otherwise.</returns>
        public static bool IsKeyFirstPressed(Keys key)
        {
            return (oldKeyboardState.IsKeyUp(key) &&
                currentKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Determine if a given key is released and was pressed on the previous frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns true if the key is released and was pressed on the previous frame. Returns false otherwise.</returns>
        public static bool IsKeyFirstReleased(Keys key)
        {
            return (oldKeyboardState.IsKeyDown(key) &&
                currentKeyboardState.IsKeyUp(key));
        }

    }
}
