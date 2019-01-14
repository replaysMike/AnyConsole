using System;

namespace AnyConsole
{
    public class MouseScrollEventArgs : EventArgs
    {
        public MouseScrollDirection Direction { get; }
        public MouseScrollEventArgs(MouseScrollDirection direction)
        {
            Direction = direction;
        }
    }

    public enum MouseScrollDirection
    {
        Up,
        Down
    }
}
