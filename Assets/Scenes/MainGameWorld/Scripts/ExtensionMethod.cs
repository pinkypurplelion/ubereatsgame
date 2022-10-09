namespace Scenes.MainGameWorld.Scripts
{
    public static class ExtensionMethod
    {
        public static float map (this float value, float fromLeft1, float toLeft1, float fromRight2, float toRight2) {
            return (value - fromLeft1) / (toLeft1 - fromLeft1) * (toRight2 - fromRight2) + fromRight2;
        }
    }
}