using Microsoft.Xna.Framework;
using System;

namespace MonoMinion.PhysicsEngine.Data
{
    public struct CollisionManifold
    {
        private RigidBody a;
        private RigidBody b;
        private float penetration;
        private Vector2 normal;
        private Vector2[] contacts;

        private float minRestitution;
        private float sFriction;
        private float dFriction;

        public RigidBody A { get { return a; } }
        public RigidBody B { get { return b; } }
        public float Penetration { get { return penetration; } }
        public Vector2 Normal { get { return normal; } }
        public Vector2[] Contacts { get { return contacts; } }
        public int Count { get { return contacts.Length; } }

        public CollisionManifold(RigidBody A, RigidBody B)
        {
            a = A;
            b = B;
            penetration = 0f;
            normal = Vector2.Zero;
            contacts = new Vector2[2];

            minRestitution = 0f;
            sFriction = 0f;
            dFriction = 0f;
        }

        public void Initialize(Physics physics)
        {
            minRestitution = MathHelper.Min(A.Material.Restitution, B.Material.Restitution);

            // Friction
            sFriction = (float)Math.Sqrt(A.StaticFriction * A.StaticFriction);
            dFriction = (float)Math.Sqrt(A.DynamicFriction * A.DynamicFriction);

            for (int i = 0; i < Count; i++)
            {
                Vector2 rA = contacts[i] - A.Shape.Position;
                Vector2 rB = contacts[i] - B.Shape.Position;

                Vector2 rV = B.Velocity + (new Vector2(-B.AngularVelocity * rB.Y, B.AngularVelocity * rB.X)) - 
                             A.Velocity - (new Vector2(-A.AngularVelocity * rA.Y, A.AngularVelocity * rA.X));

                // Determine if we should perform a resting collision or not
                // The idea is if the only thing moving this object is gravity,
                // then the collision should be performed without any restitution
                if (rV.LengthSquared() < (physics.Delta * physics.Gravity).LengthSquared() + Physics.EPSILON)
                    minRestitution = 0f;
            }
        }

        public void Solve()
        {
        }

        public void ApplyImpulse()
        {
            // If both bodies have infinite mass we quit
            if (Math.Abs(A.Mass.Mass + B.Mass.Mass) <= Physics.EPSILON)
            {
                a.Velocity = Vector2.Zero;
                b.Velocity = Vector2.Zero;
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                Vector2 rA = contacts[i] - A.Shape.Position;
                Vector2 rB = contacts[i] - B.Shape.Position;

                Vector2 rV = B.Velocity + (new Vector2(-B.AngularVelocity * rB.Y, B.AngularVelocity * rB.X)) -
                             A.Velocity - (new Vector2(-A.AngularVelocity * rA.Y, A.AngularVelocity * rA.X));

                // Do not resolve if velocities are separating
                float contactVelocity = Vector2.Dot(rV, Normal);
                if (contactVelocity > 0)
                    return;

                float raCrossN = (rA.X * normal.Y - rA.Y * normal.X);
                float rbCrossN = (rB.X * normal.Y - rB.Y * normal.X);
                float invMassSum = A.Mass.InverseMass + B.Mass.InverseMass + (float)Math.Sqrt(raCrossN) * A.Mass.InverseInertia + (float)Math.Sqrt(rbCrossN) * B.Mass.InverseInertia;

                // Impulse Scalar
                float j = -(1.0f + minRestitution) * contactVelocity;
                j /= invMassSum;
                j /= (float)Count;

                // Apply impulse
                Vector2 impulse = normal * j;
                B.ApplyImpulse(-impulse, rA);
                A.ApplyImpulse(impulse, rB);

                // Friction impulse
                rV = B.Velocity + (new Vector2(-B.AngularVelocity * rB.Y, B.AngularVelocity * rB.X)) -
                     A.Velocity - (new Vector2(-A.AngularVelocity * rA.Y, A.AngularVelocity * rA.X));

                Vector2 t = rV - (normal * Vector2.Dot(rV, normal));
                t.Normalize();

                // j tangent magnitude
                float jt = -Vector2.Dot(rV, t);
                jt /= invMassSum;
                jt /= (float)Count;

                // Don't apply tiny impulses
                if (Math.Abs(jt) <= Physics.EPSILON)
                    return;

                // Coulumb's law
                Vector2 tangentImpulse = Vector2.Zero;
                if (Math.Abs(jt) < j * sFriction)
                    tangentImpulse = t * jt;
                else
                    tangentImpulse = t * -j * dFriction;

                // Apply friction impulse
                A.ApplyImpulse(-tangentImpulse, rA);
                B.ApplyImpulse(tangentImpulse, rB);
            }
        }

        public void CorrectPosition()
        {
            Vector2 correction = (MathHelper.Max(penetration - Physics.PENETRATION_ALLOWED, 0f)) * normal * Physics.PENETRATION_CORRECTION;
            A.Shape.Position -= correction * A.Mass.InverseMass;
            B.Shape.Position += correction * B.Mass.InverseMass;
        }
    }
}
