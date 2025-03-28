namespace DevFreela.API.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            IsDeleted = false;
        }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public void SetAsDeleted() => IsDeleted = true;
    }
}
