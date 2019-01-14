using System;

namespace AnyConsole
{
    public partial class ExtendedConsole
    {
        private void InputThread()
        {
            // if there is no console available, no need to display anything.
            if (_screenLogBuilder == null)
                return;

            while (!_isRunning.WaitOne(1))
            {
                var hWnd = GetStdHandle(STD_INPUT_HANDLE);
                var buffer = new INPUT_RECORD[128];
                var eventsRead = 0U;
                var success = GetNumberOfConsoleInputEvents(hWnd, out eventsRead);
                if (eventsRead > 0)
                {
                    var keyRead = ReadConsoleInput(hWnd, buffer, (uint)buffer.Length, out eventsRead);
                    if (keyRead)
                    {
                        for (var z = 0; z < eventsRead; z++)
                        {
                            switch (buffer[z].EventType)
                            {
                                case KEYBOARD_EVENT:
                                    var keyEvent = buffer[z].KeyEvent;
                                    // internal keyboard handling events
                                    var key = (ConsoleKey)keyEvent.wVirtualKeyCode;
                                    switch (key)
                                    {
                                        case ConsoleKey.Home:
                                            // scroll to start
                                            _bufferYCursor = _defaultBufferHistoryLinesLength;
                                            break;
                                        case ConsoleKey.Escape:
                                        case ConsoleKey.End:
                                            // scroll to end
                                            _bufferYCursor = 0;
                                            break;
                                        case ConsoleKey.Q:
                                            // quit
                                            Dispose();
                                            break;
                                    }

                                    OnKeyPress?.Invoke(new KeyPressEventArgs(key, keyEvent.dwControlKeyState));
                                    break;
                                case MOUSE_EVENT:
                                    var mouseEvent = buffer[z].MouseEvent;
                                    if (mouseEvent.dwEventFlags == MOUSE_MOVED)
                                    {
                                        // mouse move
                                        OnMouseMove?.Invoke(new MouseMoveEventArgs(mouseEvent.dwMousePosition.x, mouseEvent.dwMousePosition.y));
                                    }
                                    else if (mouseEvent.dwEventFlags == MOUSE_BUTTON)
                                    {
                                        // mouse button press
                                        OnMousePress?.Invoke(new MousePressEventArgs((MouseButtonState)mouseEvent.dwButtonState, mouseEvent.dwControlKeyState));
                                    }
                                    else if (mouseEvent.dwEventFlags == MOUSE_WHEELED)
                                    {
                                        // mouse wheel scroll
                                        var isWheelUp = mouseEvent.dwButtonState == MOUSE_WHEELUP;
                                        var isWheelDown = mouseEvent.dwButtonState == MOUSE_WHEELDOWN;
                                        if (isWheelUp)
                                        {
                                            _bufferYCursor--;
                                            OnMouseScroll?.Invoke(new MouseScrollEventArgs(MouseScrollDirection.Up));
                                        }
                                        if (isWheelDown)
                                        {
                                            _bufferYCursor++;
                                            OnMouseScroll?.Invoke(new MouseScrollEventArgs(MouseScrollDirection.Down));
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
