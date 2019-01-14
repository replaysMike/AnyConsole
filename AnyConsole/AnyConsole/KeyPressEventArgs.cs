using System;

namespace AnyConsole
{
    public class KeyPressEventArgs : EventArgs
    {
        public ConsoleKey Key { get; }
        public ControlKeyState KeyState { get; }
        public KeyPressEventArgs(ConsoleKey key, ControlKeyState keyState)
        {
            Key = key;
            KeyState = keyState;
        }
    }

    
}
