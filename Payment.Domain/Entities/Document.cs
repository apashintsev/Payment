namespace Payment.Domain.Entities;

public class Document : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Type { get; private set; }
    public DateTime UploadDate { get; private set; }

    public string FileUrl { get; set; }
}
