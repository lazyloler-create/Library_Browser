using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Library.Data;

public class GetAdditionalInformation
{
    private readonly Logger _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public GetAdditionalInformation(Logger logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string?> GetTitle(string q)
    {
        return (await GetFirstVolumeInfo(q))?.title;
    }

    public async Task<string?> GetDescription(string q) =>
        (await GetFirstVolumeInfo(q))?.Description;

    public async Task<List<string>?> GetAuthors(string q) =>
        (await GetFirstVolumeInfo(q))?.authors;

    public async Task<string?> GetPublishedDate(string q) =>
        (await GetFirstVolumeInfo(q))?.publishedDate;

    public async Task<List<(string? Type, string? Identifier)>?> GetIndustryIdentifiers(string q)
    {
        var identifiers = (await GetFirstVolumeInfo(q))?.industryIdentifiers;
        return identifiers?
            .Select(identifier => (identifier.Type, identifier.Identifier))
            .ToList();
    }

    public async Task<int?> GetPageCount(string q) =>
        (await GetFirstVolumeInfo(q))?.pageCount;

    public async Task<List<string>?> GetCategories(string q) =>
        (await GetFirstVolumeInfo(q))?.categories;

    public async Task<double?> GetAverageRating(string q) =>
        (await GetFirstVolumeInfo(q))?.averageRating;

    public async Task<int?> GetRatingsCount(string q) =>
        (await GetFirstVolumeInfo(q))?.ratingsCount;

    public async Task<string?> GetSubtitle(string q) =>
        (await GetFirstVolumeInfo(q))?.bookSubtitle;

    public async Task<string?> GetPublisher(string q) =>
        (await GetFirstVolumeInfo(q))?.Publisher;

    public async Task<string?> GetLanguage(string q) =>
        (await GetFirstVolumeInfo(q))?.bookLanguage;

    public async Task<string?> GetPrintType(string q) =>
        (await GetFirstVolumeInfo(q))?.printType;

    public async Task<string?> GetMaturityRating(string q) =>
        (await GetFirstVolumeInfo(q))?.maturityRating;

    public async Task<bool?> GetAllowAnonLogging(string q) =>
        (await GetFirstVolumeInfo(q))?.allowAnonLogging;

    public async Task<string?> GetContentVersion(string q) =>
        (await GetFirstVolumeInfo(q))?.contentVersion;

    public async Task<bool?> GetTextReadingMode(string q) =>
        (await GetFirstVolumeInfo(q))?.readingModes?.text;

    public async Task<bool?> GetImageReadingMode(string q) =>
        (await GetFirstVolumeInfo(q))?.readingModes?.image;

    public async Task<string?> GetPreviewLink(string q) =>
        (await GetFirstVolumeInfo(q))?.previewLink;

    public async Task<string?> GetInfoLink(string q) =>
        (await GetFirstVolumeInfo(q))?.infoLink;

    public async Task<string?> GetCanonicalVolumeLink(string q) =>
        (await GetFirstVolumeInfo(q))?.canonicalVolumeLink;

    public async Task<string?> GetSmallThumbnail(string q) =>
        (await GetFirstVolumeInfo(q))?.imageLinks?.smallThumbnail;

    public async Task<string?> GetThumbnail(string q) =>
        (await GetFirstVolumeInfo(q))?.imageLinks?.thumbnail;

    public async Task<bool?> GetContainsEpubBubbles(string q) =>
        (await GetFirstVolumeInfo(q))?.panelizationSummary?.containsEpubBubbles;

    public async Task<bool?> GetContainsImageBubbles(string q) =>
        (await GetFirstVolumeInfo(q))?.panelizationSummary?.containsImageBubbles;

    private async Task<GoogleVolumeInfo?> GetFirstVolumeInfo(string q)
    {
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_BOOKS_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("GOOGLE_BOOKS_API_KEY is missing or empty");
            throw new InvalidOperationException("GOOGLE_BOOKS_API_KEY is missing or empty");
        }

        var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(q)}" +
                  $"&key={Uri.EscapeDataString(apiKey)}";

        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GoogleBooksResponse>();
        return result?.Items?.FirstOrDefault()?.VolumeInfo;
    }

    private sealed class GoogleBooksResponse
    {
        [JsonPropertyName("items")]
        public List<GoogleBookItem>? Items { get; set; }
    }

    private sealed class GoogleBookItem
    {
        [JsonPropertyName("volumeInfo")]
        public GoogleVolumeInfo? VolumeInfo { get; set; }
    }

    private sealed class GoogleVolumeInfo
    {
        [JsonPropertyName("title")]
        public string? title {get; set;}

        [JsonPropertyName("description")]
        public string? Description {get; set;}

        [JsonPropertyName("authors")]
        public List<string>? authors {get; set;}

        [JsonPropertyName("publishedDate")]
        public string? publishedDate {get; set;}

        [JsonPropertyName("industryIdentifiers")]
        public List<IndustryIdentifier>? industryIdentifiers {get; set;}

        [JsonPropertyName("pageCount")]
        public int? pageCount {get; set;}

        [JsonPropertyName("categories")]
        public List<string>? categories {get; set;}

        [JsonPropertyName("averageRating")]
        public double? averageRating {get; set;}

        [JsonPropertyName("ratingsCount")]
        public int? ratingsCount {get; set;}

        [JsonPropertyName("subtitle")]
        public string? bookSubtitle{get; set;}

        [JsonPropertyName("publisher")]
        public string? Publisher {get; set;}

        [JsonPropertyName("language")]
        public string? bookLanguage {get; set;}

        [JsonPropertyName("printType")]
        public string? printType {get; set;}

        [JsonPropertyName("maturityRating")]
        public string? maturityRating {get; set;}

        [JsonPropertyName("allowAnonLogging")]
        public bool? allowAnonLogging {get; set;}

        [JsonPropertyName("contentVersion")]
        public string? contentVersion {get; set;}

        [JsonPropertyName("readingModes")]
        public ReadingModes? readingModes {get; set;}

        [JsonPropertyName("previewLink")]
        public string? previewLink {get; set;}

        [JsonPropertyName("infoLink")]
        public string? infoLink {get; set;}

        [JsonPropertyName("canonicalVolumeLink")]
        public string? canonicalVolumeLink {get; set;}

        [JsonPropertyName("imageLinks")]
        public ImageLinks? imageLinks {get; set;}

        [JsonPropertyName("panelizationSummary")]
        public PanelizationSummary? panelizationSummary {get; set;}
    }

    private sealed class ReadingModes
    {
        [JsonPropertyName("text")]
        public bool? text {get; set;}

        [JsonPropertyName("image")]
        public bool? image {get; set;}
    }

    private sealed class ImageLinks
    {
        [JsonPropertyName("smallThumbnail")]
        public string? smallThumbnail {get; set;}

        [JsonPropertyName("thumbnail")]
        public string? thumbnail {get; set;}
    }

    private sealed class PanelizationSummary
    {
        [JsonPropertyName("containsEpubBubbles")]
        public bool? containsEpubBubbles {get; set;}

        [JsonPropertyName("containsImageBubbles")]
        public bool? containsImageBubbles {get; set;}
    }
    private sealed class IndustryIdentifier
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }
    }
}
