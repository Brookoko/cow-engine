namespace CowLibrary
{
    public struct Lens
    {
        public float focus;
        public float radius;
        public float distance;

        public Lens(float focus, float radius, float distance)
        {
            this.focus = focus;
            this.radius = radius;
            this.distance = distance;
        }
    }
}