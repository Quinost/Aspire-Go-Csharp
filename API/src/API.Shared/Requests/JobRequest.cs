namespace API.Shared.Requests;

public class JobRequest(string data, string data2, int data3)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Data { get; set; } = data;
    public string Data2 { get; set; } = data2;
    public int Data3 { get; set; } = data3;
}
