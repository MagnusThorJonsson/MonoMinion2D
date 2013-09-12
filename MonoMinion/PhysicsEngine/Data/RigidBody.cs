using Microsoft.Xna.Framework;
using MonoMinion.Collision;

namespace MonoMinion.PhysicsEngine.Data
{
    public struct RigidBody
    {
        private SATShape shape;

        private PhysicsMaterial material;
        private PhysicsMass mass;
        private Vector2 velocity;
        private Vector2 force;
        private float angularVelocity;
        private float torque;
        private float staticFriction;
        private float dynamicFriction;

        public SATShape Shape { get { return shape; } }
        public PhysicsMaterial Material { get { return material; } }
        public PhysicsMass Mass { get { return mass; } }
        public Vector2 Velocity 
        { 
            get { return velocity; }
            set { velocity = value; }
        }
        public Vector2 Force { get { return force; } }
        public float AngularVelocity 
        { 
            get { return angularVelocity; }
            set { angularVelocity = value; }
        }
        public float Torque { get { return torque; } }
        public float StaticFriction { get { return staticFriction; } }
        public float DynamicFriction { get { return dynamicFriction; } }

        public float Orientation
        {
            get { return shape.Rotation; }
            set { shape.Rotation = value; }
        }

        public RigidBody(SATShape shape, PhysicsMaterial material, PhysicsMass mass, float staticFriction, float dynamicFriction)
        {
            this.shape = shape;
            this.material = material;
            this.mass = mass;
            this.velocity = Vector2.Zero;
            this.force = Vector2.Zero;
            this.angularVelocity = 0f;
            this.torque = 0f;

            this.staticFriction = staticFriction;
            this.dynamicFriction = dynamicFriction;
        }

        public void ApplyForce(Vector2 force)
        {
            this.force += force;
        }

        public void ClearForces()
        {
            force = Vector2.Zero;
            torque = 0f;
        }

        public void ApplyImpulse(Vector2 impulse, Vector2 contact)
        {
            velocity += mass.InverseMass * impulse;
            angularVelocity += mass.InverseInertia * (contact.X * impulse.Y - contact.Y * impulse.X); // Cross Product
        }

    }
}
