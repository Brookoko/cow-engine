namespace CowLibrary
{
    public struct Lens
    {
        public readonly float focus;
        public readonly float radius;
        public readonly float distance;

        public Lens(float focus, float radius, float distance)
        {
            this.focus = focus;
            this.radius = radius;
            this.distance = distance;
        }
    }
}