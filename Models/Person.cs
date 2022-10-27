

namespace MCT.Functions.Models;


public class Person
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "firstName")]
    public string FirstName { get; set; }

    [JsonProperty(PropertyName = "lastName")]
    public string LastName { get; set; }

    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }

    [JsonProperty(PropertyName = "locations")]
    public Location[] Locations { get; set; }
}

public class Location
{
    [JsonProperty(PropertyName = "city")]
    public string City { get; set; }

    [JsonProperty(PropertyName = "street")]
    public string Street { get; set; }
}