using System;

namespace Verdant
{
    public struct ParticleConfiguration
    {

        public RenderObject[] Sprites { get; set; }
        public int[] WidthRange { get; set; }
        public int[] HeightRange { get; set; }
        public float[] AngleRange { get; set; }
        public float[] VelocityMagnitudeRange { get; set; }
        public float[] AccelerationMagnitudeRange { get; set; }
        public float[] FrictionRange { get; set; }
        public int[] LifetimeRange { get; set; }

        public ParticleConfiguration(RenderObject[] sprites, int[] widthRange, int[] heightRange, float[] angleRange, float[] velocityMagnitudeRange, float[] accelerationMagnitudeRange, float[] frictionRange, int[] lifetimeRange)
        {
            Sprites = sprites;
            WidthRange = widthRange;
            HeightRange = heightRange;
            AngleRange = angleRange;
            VelocityMagnitudeRange = velocityMagnitudeRange;
            AccelerationMagnitudeRange = accelerationMagnitudeRange;
            FrictionRange = frictionRange;
            LifetimeRange = lifetimeRange;
        }

        public static float SelectFloatFromRange(float[] range)
        {
            return GameMath.RandomFloat(range[0], range[range.Length - 1]);
        }

        public static int SelectIntFromRange(int[] range)
        {
            return GameMath.Random.Next(range[0], range[range.Length - 1]);
        }

    }
}
