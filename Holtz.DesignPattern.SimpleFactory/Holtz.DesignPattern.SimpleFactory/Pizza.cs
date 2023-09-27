namespace Holtz.DesignPattern.SimpleFactory
{
    public abstract class Pizza
    {
        public string Name { get; set; } = null!;
        public abstract void Prepare();
        public abstract void Bake();
        public abstract void Pack();
    }
}
