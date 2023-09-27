namespace Holtz.DesignPattern.SimpleFactory
{
    public abstract class Pizza
    {
        public string Name { get; set; } = null!;
        public abstract string Prepare();
        public abstract string Bake();
        public abstract string Pack();
    }
}
