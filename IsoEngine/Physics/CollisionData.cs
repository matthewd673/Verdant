using System;

namespace IsoEngine.Physics
{
    internal class CollisionData
    {

        internal Body a;
        internal Body b;
        Vec2 normal;
        float pen;
        Vec2 cp;

        public CollisionData(Body a, Body b, Vec2 normal, float pen, Vec2 cp)
        {
            this.a = a;
            this.b = b;
            this.normal = normal;
            this.pen = pen;
            this.cp = cp;
        }

        public void PenetrationResolution()
        {
            Vec2 penResolution = normal * pen / (a.InvMass + b.InvMass);
            a.Components[0].Position = a.Components[0].Position + (penResolution * a.InvMass);
            b.Components[0].Position = b.Components[0].Position + (penResolution * -b.InvMass);
        }

        public void CollisionResolution()
        {
            //closing velocity
            Vec2 colArm1 = cp - a.Components[0].Position;
            Vec2 rotVel1 = new Vec2(-a.AngleSpeed * colArm1.Y, a.AngleSpeed * colArm1.X);
            Vec2 closVel1 = a.Velocity + rotVel1;
            Vec2 colArm2 = cp - b.Components[0].Position;
            Vec2 rotVel2 = new Vec2(-b.AngleSpeed * colArm2.Y, b.AngleSpeed * colArm2.X);
            Vec2 closVel2 = b.Velocity + rotVel2;

            //impulse augmentation
            float impAug1 = Vec2.Cross(colArm1, normal);
            impAug1 = impAug1 * a.InvInertia * impAug1;
            float impAug2 = Vec2.Cross(colArm2, normal);
            impAug2 = impAug2 * b.InvInertia * impAug2;

            Vec2 relVel = closVel1 - closVel2;
            float sepVel = Vec2.Dot(relVel, normal);
            float newSepVel = -sepVel * Math.Min(a.Elasticity, b.Elasticity);
            float vSepDiff = newSepVel - sepVel;

            float impulse = vSepDiff / (a.InvMass + b.InvMass + impAug1 + impAug2);
            Vec2 impulseVec = normal * impulse;

            //change velocities
            a.Velocity += impulseVec * a.InvMass;
            b.Velocity += impulseVec * -b.InvMass;

            a.AngleSpeed += a.InvInertia * Vec2.Cross(colArm1, impulseVec);
            b.AngleSpeed -= b.InvInertia * Vec2.Cross(colArm2, impulseVec);
        }

    }
}
