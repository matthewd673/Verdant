using System;

namespace Verdant.Physics
{
    internal class CollisionData
    {

        internal PhysicsEntity a;
        internal PhysicsEntity b;
        Vec2 normal;
        float pen;
        Vec2 cp;

        /// <summary>
        /// Initialize a new CollisionData.
        /// </summary>
        /// <param name="a">The first colliding Body.</param>
        /// <param name="b">The second colliding Body.</param>
        /// <param name="normal">The normal of the collision.</param>
        /// <param name="pen">The penetration depth of the collision.</param>
        /// <param name="cp">The contact point of the collision.</param>
        public CollisionData(PhysicsEntity a, PhysicsEntity b, Vec2 normal, float pen, Vec2 cp)
        {
            this.a = a;
            this.b = b;
            this.normal = normal;
            this.pen = pen;
            this.cp = cp;
        }

        /// <summary>
        /// Move the colliding bodies to resolve the penetration of the collision.
        /// </summary>
        public void PenetrationResolution()
        {
            if (a.InvMass + b.InvMass == 0)
                return;
            Vec2 penResolution = normal * (pen / (a.InvMass + b.InvMass));
            a.Components[0].Position += (penResolution * a.InvMass);
            b.Components[0].Position += (penResolution * -b.InvMass);
        }

        /// <summary>
        /// Move and rotate the colliding bodies to resolve the collision.
        /// </summary>
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

            float impulse = 0;
            if (a.InvMass + b.InvMass != 0)
                impulse = vSepDiff / (a.InvMass + b.InvMass + impAug1 + impAug2);
            Vec2 impulseVec = normal * impulse;

            //change velocities
            a.Velocity += impulseVec * a.InvMass;
            b.Velocity += impulseVec * -b.InvMass;

            a.AngleSpeed += a.InvInertia * Vec2.Cross(colArm1, impulseVec);
            b.AngleSpeed -= b.InvInertia * Vec2.Cross(colArm2, impulseVec);
        }

    }
}
