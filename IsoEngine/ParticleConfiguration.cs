using System;

namespace IsoEngine
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

    }
}
