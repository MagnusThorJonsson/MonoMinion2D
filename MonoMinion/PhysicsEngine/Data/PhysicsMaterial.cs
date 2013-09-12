

namespace MonoMinion.PhysicsEngine.Data
{
    public struct PhysicsMaterial
    {
        private float density;
        private float restitution;

        public float Density
        {
            get { return density; }
            set { density = value; }
        }

        public float Restitution
        {
            get { return restitution; }
            set { restitution = value; }
        }

        public PhysicsMaterial(float density, float restitution)
        {
            this.density = density;
            this.restitution = restitution;
        }
    }
}
