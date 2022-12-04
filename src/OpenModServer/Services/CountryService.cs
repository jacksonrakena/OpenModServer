using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenModServer.Services;

public class Country
{
    /**
     * "isoCode": "AD",
    "emojiFlag": "🇦🇩",
    "country": "Andorra"
     */
    
    [JsonPropertyName("isoCode")]
    public string IsoCode { get; set; }
    
    [JsonPropertyName("emojiFlag")]
    public string EmojiFlag { get; set; }
    
    [JsonPropertyName("country")]
    public string Name { get; set; }
}

public class CountryService
{
    public List<Country> Countries { get; private set; }
    public Dictionary<string, Country> CountriesByIsoCode { get; private set; }
    public void Initialise()
    {
        var countries = JsonSerializer.Deserialize<List<Country>>(File.ReadAllText("countries.json"));
        Countries = countries ?? throw new InvalidOperationException("Failed to load country data.");
        CountriesByIsoCode = new Dictionary<string, Country>();
        foreach (var country in Countries)
        {
            CountriesByIsoCode[country.IsoCode] = country;
        }
    }
}