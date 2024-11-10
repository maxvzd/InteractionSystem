public static class AngleHelper
{
    public static float GetRealAngle(float angle)
    {
        return angle > 180 ? angle - 360 : angle;
    }
}