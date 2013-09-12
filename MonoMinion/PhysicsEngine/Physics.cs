using Microsoft.Xna.Framework;
using MonoMinion.PhysicsEngine.Data;
using System;
using System.Collections.Generic;

namespace MonoMinion.PhysicsEngine
{
    public class Physics
    {
        public const float EPSILON = 0.0001f;
        public const float PENETRATION_ALLOWED = 0.05f;
        public const float PENETRATION_CORRECTION = 0.4f;

        private List<RigidBody> bodies;
        private List<CollisionManifold> contacts;
        private Vector2 gravity;
        private int iterations;
        private float delta;

        public Vector2 Gravity { get { return gravity; } }
        public int Iterations { get { return iterations; } }
        public float Delta { get { return delta; } }

        public Physics(Vector2 gravity, float delta, int iterations)
        {
            this.gravity = gravity;
            this.delta = delta;
            this.iterations = iterations;

            bodies = new List<RigidBody>();
            contacts = new List<CollisionManifold>();
        }

        public void Update(GameTime gameTime)
        {
            // Get the delta time
            float dt = MathHelper.Clamp(0.0f, 0.1f, (float)gameTime.ElapsedGameTime.TotalMilliseconds);

            // While delta time is less than the timestep delta
            while (dt >= delta)
            {
                // Remove delta from accumulated delta time
                dt -= delta;

                contacts.Clear();
                for (int i = 0; i < bodies.Count; i++)
                {
                    RigidBody A = bodies[i];
                    for (int j = i + 1; j < bodies.Count; j++)
                    {
                        RigidBody B = bodies[j];
                        if (A.Mass.InverseMass > 0f && B.Mass.InverseMass > 0f)
                        {
                            CollisionManifold m = new CollisionManifold(A, B);
                            m.Solve();
                            if (m.Count > 0)
                                contacts.Add(m);
                        }
                    }
                }

                // Integrate Forces
                for (int i = 0; i < bodies.Count; i++)
                    integrateForces(bodies[i]);

                // Initialize collisions
                for (int i = 0; i < contacts.Count; i++)
                    contacts[i].Initialize(this);

                // Solve collisions
                for (int j = 0; j < iterations; j++)
                    for (int i = 0; i < contacts.Count; i++)
                        contacts[i].ApplyImpulse();

                // Integrate velocities
                for (int i = 0; i < bodies.Count; i++)
                    integrateVelocity(bodies[i]);

                // Correct positions
                for (int i = 0; i < contacts.Count; i++)
                    contacts[i].CorrectPosition();

                // Clear all forces
                for (int i = 0; i < bodies.Count; i++)
                    bodies[i].ClearForces();
            }
        }

        protected void integrateForces(RigidBody body)
        {
            if (body.Mass.InverseMass > 0f)
            {
                body.Velocity += (body.Force * body.Mass.InverseMass + gravity) * (delta / 2.0f);
                body.AngularVelocity += body.Torque * body.Mass.InverseInertia * (delta / 2.0f);
            }
        }

        protected void integrateVelocity(RigidBody body)
        {
            if (body.Mass.InverseMass > 0f)
            {
                body.Shape.Position += body.Velocity * delta;
                body.Shape.Rotation += body.AngularVelocity * delta;
                integrateForces(body);
            }
        }

        public void AddBody(RigidBody body)
        {
            bodies.Add(body);
        }

        public bool RemoveBody(RigidBody body)
        {
            return bodies.Remove(body);
        }
    }
}
