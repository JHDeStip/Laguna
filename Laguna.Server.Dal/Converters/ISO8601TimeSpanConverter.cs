using System;
using System.Linq;

namespace JhDeStip.Laguna.Server.Dal.Converters
{
    /// <summary>
    /// Class to convert an ISO 8601 time span to a System.TimeSpan.
    /// </summary>
    public static class ISO8601TimeSpanConverter
    {
        /// <summary>
        /// Converts an ISO 8601 timespan to a System.TimeSpan.
        /// </summary>
        /// <param name="timeSpanString">ISO 8601 time span.</param>
        /// <returns>System.TimeSpan from ISO 8601 time span.</returns>
        public static TimeSpan ConvertToTimeSpan(string timeSpanString)
        {
            int weeks = 0,
                days = 0,
                hours = 0,
                mins = 0,
                secs = 0;

            string trimmedString = timeSpanString;
            string[] parts;

            if (trimmedString.Length > 1 && trimmedString.Substring(0, 1) == "P")
            {
                trimmedString = timeSpanString.Substring(1);
                parts = trimmedString.Split('W');
                if (parts.Length > 1)
                    try
                    {
                        weeks = int.Parse(parts[0]);
                    }
                    catch
                    {
                        throw new FormatException("Bad day format.");
                    }

                trimmedString = parts.Last();
                parts = trimmedString.Split('D');
                if (parts.Length > 1)
                    try
                    {
                        hours = int.Parse(parts[0]);
                    }
                    catch
                    {
                        throw new FormatException("Bad day format.");
                    }

                trimmedString = parts.Last();

                if (trimmedString.Length > 1 && trimmedString.Substring(0, 1) == "T")
                    trimmedString = trimmedString.Substring(1);

                parts = trimmedString.Split('H');
                if (parts.Length > 1)
                    try
                    {
                        hours = int.Parse(parts[0]);
                    }
                    catch
                    {
                        throw new FormatException("Bad day format.");
                    }

                trimmedString = parts.Last();
                parts = trimmedString.Split('M');
                if (parts.Length > 1)
                    try
                    {
                        mins = int.Parse(parts[0]);
                    }
                    catch
                    {
                        throw new FormatException("Bad day format.");
                    }

                trimmedString = parts.Last();
                parts = trimmedString.Split('S');
                if (parts.Length > 1)
                    try
                    {
                        secs = int.Parse(parts[0]);
                    }
                    catch
                    {
                        throw new FormatException("Bad day format.");
                    }
            }

            return new TimeSpan(weeks * 7 + days, hours, mins, secs);
        }
    }
}
