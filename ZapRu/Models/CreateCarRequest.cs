using Org.BouncyCastle.Asn1.Crmf;

using System.Text.Json.Serialization;

public class CreateCarRequest
{
    [JsonPropertyName("carModelID")]
    public int CarModelID { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("mileage")]
    public int? Mileage { get; set; }

    [JsonPropertyName("condition")]
    public string Condition { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("parts")]
    public List<PartRequest> Parts { get; set; }
}