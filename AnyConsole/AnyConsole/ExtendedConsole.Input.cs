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
            // get pointer to the console
            var hWnd = GetStdHandle(STD_INPUT_HANDLE);

            // disable quick edit mode as it steals mouse input
            var wasQuickEditModeEnabled = IsQuickEditModeEnabled(hWnd);
            if (wasQuickEditModeEnabled)
                DisableQuickEditMode(hWnd);

            while (!_isRunning.WaitOne(100))
            {
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
                                    var key = (ConsoleKey)keyEvent.wVirtualKeyCode;
                                    if (Options.InputOptions == InputOptions.UseBuiltInKeyOperations)
                                    {
                                        // internal keyboard handling events
                                        if (_isSearchEnabled && keyEvent.bKeyDown
                                            && !keyEvent.dwControlKeyState.HasFlag(ControlKeyState.LEFT_CTRL_PRESSED)
                                            && !keyEvent.dwControlKeyState.HasFlag(ControlKeyState.RIGHT_CTRL_PRESSED)
                                            && !keyEvent.dwControlKeyState.HasFlag(ControlKeyState.LEFT_ALT_PRESSED)
                                            && !keyEvent.dwControlKeyState.HasFlag(ControlKeyState.RIGHT_ALT_PRESSED)
                                        )
                                            AddSearchString(keyEvent.UnicodeChar);
                                        if (keyEvent.bKeyDown)
                                        {
                                            switch (key)
                                            {
                                                case ConsoleKey.H:
                                                    ToggleHelp();
                                                    break;
                                                case ConsoleKey.Home:
                                                    // scroll to start
                                                    _bufferYCursor = Configuration.MaxHistoryLines;
                                                    _hasLogUpdates = true;
                                                    break;
                                                case ConsoleKey.Escape:
                                                case ConsoleKey.End:
                                                    // scroll to end
                                                    _bufferYCursor = 0;
                                                    _searchLineIndex = -1;
                                                    _hasLogUpdates = true;
                                                    SetSearch(false);
                                                    break;
                                                case ConsoleKey.Enter:
                                                    if (_isSearchEnabled)
                                                        FindNext();
                                                    break;
                                                case ConsoleKey.S:
                                                    // find/search
                                                    if (keyEvent.dwControlKeyState.HasFlag(ControlKeyState.LEFT_CTRL_PRESSED))
                                                    {
                                                        ToggleSearch();
                                                    }
                                                    break;
                                                case ConsoleKey.F3:
                                                    // find/search control keys
                                                    if (keyEvent.dwControlKeyState.HasFlag(ControlKeyState.SHIFT_PRESSED))
                                                    {
                                                        FindPrevious();
                                                    }
                                                    else
                                                    {
                                                        FindNext();
                                                    }
                                                    break;
                                                case ConsoleKey.Q:
                                                    // quit
                                                    System.Diagnostics.Debug.WriteLine("Quitting....");
                                                    Close();
                                                    Dispose();
                                                    break;
                                            }
                                        }
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
                                            if (Options.InputOptions == InputOptions.UseBuiltInKeyOperations)
                                            {
                                                _hasLogUpdates = true;
                                                if (_bufferYCursor > 0)
                                                    _bufferYCursor--;
                                            }
                                            OnMouseScroll?.Invoke(new MouseScrollEventArgs(MouseScrollDirection.Up));
                                        }
                                        if (isWheelDown)
                                        {
                                            if (Options.InputOptions == InputOptions.UseBuiltInKeyOperations)
                                            {
                                                _hasLogUpdates = true;
                                                if (_bufferYCursor < _fullLogHistory.Count - LogDisplayHeight)
                                                    _bufferYCursor++;
                                            }
                                            OnMouseScroll?.Invoke(new MouseScrollEventArgs(MouseScrollDirection.Down));
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }
            }

            if (wasQuickEditModeEnabled)
                EnableQuickEditMode(hWnd);
        }
    }
}
