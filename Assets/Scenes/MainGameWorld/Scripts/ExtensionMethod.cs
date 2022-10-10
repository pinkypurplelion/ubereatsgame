namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Extension methods for in-built C# types.
    /// </summary>
    /// <author>Liam Angus</author>
    public static class ExtensionMethod
    {
        /// <summary>
        /// Will map a value from one range to another.
        /// </summary>
        /// <param name="value">The value to be mapped</param>
        /// <param name="fromLeft1">The minimum value of the original range</param>
        /// <param name="toLeft1">The minimum value of the new range</param>
        /// <param name="fromRight2">The maximum value of the original range</param>
        /// <param name="toRight2">The maximum value of the new range</param>
        /// <returns>Will return a new float in place, mapped from the original range to the new range</returns>
        public static float map (this float value, float fromLeft1, float toLeft1, float fromRight2, float toRight2) {
            return (value - fromLeft1) / (toLeft1 - fromLeft1) * (toRight2 - fromRight2) + fromRight2;
        }
    }
}