using Microsoft.AspNetCore.Mvc;
using UrlShortenerBackend.Data;
using UrlShortenerBackend.Models;
using System;
using System.Linq;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly AppDbContext _context;

    public UrlShortenerController(AppDbContext context)
    {
        _context = context;
    }

    // Define a model for the request
    public class ShortenUrlRequest
    {
        public required string OriginalUrl { get; set; }
    }

    [HttpPost("shorten")]
    public IActionResult ShortenUrl([FromBody] ShortenUrlRequest request)
{
    if (string.IsNullOrEmpty(request?.OriginalUrl) || !IsValidUrl(request.OriginalUrl))
    {
        return BadRequest("Please provide a valid URL.");
    }

    var shortUrl = new ShortUrl
    {
        OriginalUrl = request.OriginalUrl,
        ShortenedUrl = "tinyurl.com/" + GenerateShortUrl(),
        CreatedAt = DateTime.Now
    };

    _context.ShortUrls.Add(shortUrl);
    _context.SaveChanges();

    return Ok(new { shortenedUrl = shortUrl.ShortenedUrl });
}

// Method to validate the URL
private bool IsValidUrl(string url)
{
    return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
}

    private string GenerateShortUrl()
    {
        // Simple random string generator for short URL
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [HttpGet("revert")]
public IActionResult RedirectToOriginal(string shortUrl)
{
    if (string.IsNullOrEmpty(shortUrl))
    {
        return BadRequest("Short URL cannot be empty.");
    }

    // Extract the key from the short URL
    var shortUrlKey = shortUrl.Replace("tinyurl.com/", string.Empty);
    
    // Search for the URL in the database
    var url = _context.ShortUrls.FirstOrDefault(u => u.ShortenedUrl.EndsWith(shortUrlKey));

    if (url == null)
    {
        return NotFound("URL not found.");
    }

    // Return the original URL
    return Ok(new { originalUrl = url.OriginalUrl });
}
}