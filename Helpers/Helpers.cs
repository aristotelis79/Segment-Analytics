using System;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Helpers
{
    public static class Helpers
    {
        /// <summary>
        /// Converts a DateTime to a UNIX timestamp
        /// </summary>
        public static int ConvertToUnixTimestamp(this DateTime value)
        {
            //create Timespan by subtracting the value provided from the Unix Epoch
            var span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (int)span.TotalSeconds;
        } 


        public static string FixIllegalJavaScriptChars(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace("'", "\\'");
            return text;
        }
    }
}
