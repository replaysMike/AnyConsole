using System;

namespace AnyConsole
{
    public class MousePressEventArgs : EventArgs
    {
        /// <summary>
        /// The status of the mouse buttons
        /// </summary>
        public MouseButtonState ButtonState { get; }

        /// <summary>
        /// The status of the control keys
        /// </summary>
        public ControlKeyState ControlKeyState { get; }

        /// <summary>
        /// New mouse event args
        /// </summary>
        /// <param name="buttonState">The status of the mouse buttons</param>
        /// <param name="controlKeyState">The status of the control key state</param>
        public MousePressEventArgs(MouseButtonState buttonState, ControlKeyState controlKeyState)
        {
            ButtonState = buttonState;
            ControlKeyState = controlKeyState;
        }
    }

    /// <summary>
    /// Mouse button status
    /// </summary>
    public enum MouseButtonState
    {
        Left1 = 0x0001,
        Right = 0x0002,
        Middle = 0x0004,
        Middle2 = 0x0008,
        Middle3 = 0x0010,
    }
}
