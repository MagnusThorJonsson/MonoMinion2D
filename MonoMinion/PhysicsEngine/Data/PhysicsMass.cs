using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoMinion.PhysicsEngine.Data
{
    public struct PhysicsMass
    {
        private float mass;
        private float invMass;

        private float inertia;
        private float invInertia;

        public float Mass
        {
            get { return mass; }
            set
            {
                mass = value;
                if (mass > 0f)
                    invMass = 1.0f / mass;
                else
                    invMass = 0f;
            }
        }

        public float InverseMass { get { return invMass; } }

        public float Inertia
        {
            get { return inertia; }
            set
            {
                inertia = value;
                if (inertia > 0f)
                    invInertia = 1.0f / inertia;
                else
                    invInertia = 0f;
            }
        }

        public float InverseInertia { get { return invInertia; } }

        public PhysicsMass(float mass, float inertia)
        {
            this.mass = mass;
            invMass = (mass > 0f ? 1.0f / mass : 0f);
            this.inertia = inertia;
            invInertia = (inertia > 0f ? 1.0f / inertia : 0f);
        }
    }

}
