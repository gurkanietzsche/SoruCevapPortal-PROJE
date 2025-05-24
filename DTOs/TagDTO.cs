namespace SoruCevapPortal.DTOs
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TagCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}