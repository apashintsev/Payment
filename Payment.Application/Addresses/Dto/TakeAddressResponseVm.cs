namespace Payment.Application.Addresses.Dto;

public class TakeAddressResponseVm
{
    public Guid Id { get; set; }
    public string Currency { get; set; }
    public string ConvertTo { get; set; }
    public string Address { get; set; }
    public string Tag { get; set; }
    public string ForeignId { get; set; }
}
