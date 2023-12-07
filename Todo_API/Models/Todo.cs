namespace Todo_API.Models
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Done { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime DoneAt { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
