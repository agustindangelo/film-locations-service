namespace FilmLocations.Api.Models;
public class FilmLocation
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Locations { get; set; }
    public string? FunFacts { get; set; }
    public string? ProductionCompany { get; set; }
    public string? Distributor { get; set; }
    public string? Director { get; set; }
    public string? Writer { get; set; }
    public string? Actor1 { get; set; }
    public string? Actor2 { get; set; }
    public string? Actor3 { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public string? DataLoadedAt { get; set; }
}
