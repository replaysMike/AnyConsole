using System;

namespace AnyConsole
{
    /// <summary>
    /// Built-in UI components rendering
    /// </summary>
    public static class ComponentRenderer
    {
        public static string Render(Component component)
        {
            switch (component)
            {
                case Component.DateTime:
                    return RenderDateTime();
                case Component.DateTimeUtc:
                    return RenderDateTimeUtc();
                case Component.Date:
                    return RenderDate();
                case Component.DateUtc:
                    return RenderDateUtc();
                case Component.Time:
                    return RenderTime();
                case Component.TimeUtc:
                    return RenderTimeUtc();
            }
            throw new ArgumentOutOfRangeException();
        }

        private static string RenderDateTime()
        {
            return $"{DateTime.Now}";
        }

        private static string RenderDate()
        {
            return $"{DateTime.Now.ToShortDateString()}";
        }

        private static string RenderTime()
        {
            return $"{DateTime.Now.ToString("hh:mm:ss tt")}";
        }

        private static string RenderDateTimeUtc()
        {
            return $"{DateTime.UtcNow}";
        }

        private static string RenderDateUtc()
        {
            return $"{DateTime.UtcNow.ToShortDateString()}";
        }

        private static string RenderTimeUtc()
        {
            return $"{DateTime.UtcNow.ToString("hh:mm:ss tt")}";
        }
    }
}
