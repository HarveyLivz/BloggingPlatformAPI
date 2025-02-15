namespace Api.Models
{
    public class CreatePostRequestDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
