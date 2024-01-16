namespace Holtz.Refit.Domain
{
    public class Beer
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Style { get; set; } = null!;
    }
}
