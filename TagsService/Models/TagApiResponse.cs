namespace TagsService.Models
{
    public class TagApiResponse
    {
        public bool HasMore { get; set; }
        public List<Tag> Items { get; set; }
    }
}
