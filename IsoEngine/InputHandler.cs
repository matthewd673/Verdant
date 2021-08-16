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

        public static KeyboardState KeyboardState { get; private set; }
        public static KeyboardState PreviousKeyboardState { get; private set; }

        public static MouseState MouseState { get; private set; }
        public static MouseState PreviousMouseState { get; private set; }

        public static int MouseX { get; private set; }
        public static int MouseY { get; private set; }
        public static Vec2Int MousePosition = new Vec2Int();

        /// <summary>
        /// Update the current state of the InputHandler.
        /// </summary>
        public static void Update()
        {
            //update keyboard & mouse states
            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();

            //update mouse positions
            MouseX = MouseState.X;
            MouseY = MouseState.Y;
            MousePosition.X = MouseX;
            MousePosition.Y = MouseY;
        }

        /// <summary>
        /// Determine if a mouse button is pressed and was released on the previous frame.
        /// </summary>
        /// <param name="mouseButton">The mouse button to check. Defaults to LEFT.</param>
        /// <returns>Returns true if the mouse button is pressed and was released on the previous frame. Returns false otherwise.</returns>
        public static bool IsMouseFirstPressed(MouseButton mouseButton = MouseButton.LEFT)
        {
            ButtonState oldButtonState = PreviousMouseState.LeftButton;
            ButtonState currentButtonState = MouseState.LeftButton;

            if (mouseButton == MouseButton.RIGHT)
            {
                oldButtonState = PreviousMouseState.RightButton;
                currentButtonState = MouseState.RightButton;
            }
            if (mouseButton == MouseButton.MIDDLE)
            {
                oldButtonState = PreviousMouseState.MiddleButton;
                currentButtonState = MouseState.MiddleButton;
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
            ButtonState oldButtonState = PreviousMouseState.LeftButton;
            ButtonState currentButtonState = MouseState.LeftButton;

            if (mouseButton == MouseButton.RIGHT)
            {
                oldButtonState = PreviousMouseState.RightButton;
                currentButtonState = MouseState.RightButton;
            }
            if (mouseButton == MouseButton.MIDDLE)
            {
                oldButtonState = PreviousMouseState.MiddleButton;
                currentButtonState = MouseState.MiddleButton;
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
            return (PreviousKeyboardState.IsKeyUp(key) &&
                KeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Determine if a given key is released and was pressed on the previous frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Returns true if the key is released and was pressed on the previous frame. Returns false otherwise.</returns>
        public static bool IsKeyFirstReleased(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) &&
                KeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Get the distance the mouse has travelled since the previous frame.
        /// </summary>
        /// <returns>A Vec2Int representing the distance the mouse has travelled.</returns>
        public static Vec2Int GetMouseDelta()
        {
            return new Vec2Int(MouseState.X - PreviousMouseState.X, MouseState.Y - PreviousMouseState.Y);
        }

    }
}
