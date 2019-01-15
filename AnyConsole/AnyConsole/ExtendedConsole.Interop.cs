using System;
using System.Runtime.InteropServices;

namespace AnyConsole
{
    public partial class ExtendedConsole
    {
        const int MONITOR_DEFAULTTOPRIMARY = 1;
        const uint SW_RESTORE = 9;
        const int STD_INPUT_HANDLE = -10;
        const int STD_OUTPUT_HANDLE = -11;
        const int STD_ERROR_HANDLE = -12;
        const uint MOUSE_BUTTON = 0x0000;
        const uint MOUSE_MOVED = 0x0001;
        const ushort KEYBOARD_EVENT = 0x0001;
        const ushort MOUSE_EVENT = 0x0002;
        const uint MOUSE_WHEELED = 0x0004;
        const uint MOUSE_HWHEELED = 0x0008;
        const uint MOUSE_WHEELUP = 0xFF880000;
        const uint MOUSE_WHEELDOWN = 0x00780000;
        const uint ENABLE_QUICK_EDIT = 0x0040;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode)]
        static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

        [DllImport("kernel32.dll")]
        static extern bool GetNumberOfConsoleInputEvents(IntPtr hConsoleInput, out uint lpNumberOfEvents);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleInput, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleInput, uint dwMode);

        /// <summary>
        /// True if quick edit mode is enabled
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private bool IsQuickEditModeEnabled(IntPtr hWnd)
        {
            var consoleMode = 0U;
            GetConsoleMode(hWnd, out consoleMode);
            return (consoleMode & ENABLE_QUICK_EDIT) == ENABLE_QUICK_EDIT;
        }

        /// <summary>
        /// Disable Quick Edit mode
        /// </summary>
        /// <param name="hWnd"></param>
        private void DisableQuickEditMode(IntPtr hWnd)
        {
            var consoleMode = 0U;
            if (GetConsoleMode(hWnd, out consoleMode))
            {
                consoleMode &= ~ENABLE_QUICK_EDIT;
                SetConsoleMode(hWnd, consoleMode);
            }
        }

        /// <summary>
        /// Enable Quick Edit mode
        /// </summary>
        /// <param name="hWnd"></param>
        private void EnableQuickEditMode(IntPtr hWnd)
        {
            var consoleMode = 0U;
            if (GetConsoleMode(hWnd, out consoleMode))
            {
                consoleMode &= ENABLE_QUICK_EDIT;
                SetConsoleMode(hWnd, consoleMode);
            }
        }

        [DllImport("kernel32")]
        public static extern void GetSystemInfo(ref SYSTEM_INFO pSI);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public ushort EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
            [FieldOffset(4)]
            public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
            [FieldOffset(4)]
            public MENU_EVENT_RECORD MenuEvent;
            [FieldOffset(4)]
            public FOCUS_EVENT_RECORD FocusEvent;
        };

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
            public bool bKeyDown;
            [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
            public ushort wRepeatCount;
            [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
            public ushort wVirtualKeyCode;
            [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
            public ushort wVirtualScanCode;
            [FieldOffset(10)]
            public char UnicodeChar;
            [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
            public ControlKeyState dwControlKeyState;
        }

        public struct WINDOW_BUFFER_SIZE_RECORD
        {
            public POINT dwSize;

            public WINDOW_BUFFER_SIZE_RECORD(short x, short y)
            {
                this.dwSize = new POINT(x, y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MENU_EVENT_RECORD
        {
            public uint dwCommandId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FOCUS_EVENT_RECORD
        {
            public uint bSetFocus;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSE_EVENT_RECORD
        {
            [FieldOffset(0)]
            public POINT dwMousePosition;
            [FieldOffset(4)]
            public uint dwButtonState;
            [FieldOffset(8)]
            public ControlKeyState dwControlKeyState;
            [FieldOffset(12)]
            public uint dwEventFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
            public static MONITORINFO Default
            {
                get { var inst = new MONITORINFO(); inst.cbSize = (uint)Marshal.SizeOf(inst); return inst; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x, y;
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public uint Length;
            public uint Flags;
            public uint ShowCmd;
            public POINT MinPosition;
            public POINT MaxPosition;
            public RECT NormalPosition;
            public static WINDOWPLACEMENT Default
            {
                get
                {
                    var instance = new WINDOWPLACEMENT();
                    instance.Length = (uint)Marshal.SizeOf(instance);
                    return instance;
                }
            }
        }
    }
}
