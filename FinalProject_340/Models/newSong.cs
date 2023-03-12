using Microsoft.AspNetCore.Mvc;

namespace FinalProject_340.Models
{
    public class _n_song
    {
        [FromForm(Name = "formFile")]
        public IFormFile formFile   { get; set; }

        [FromForm(Name = "albumArt")]
        public IFormFile albumArt   { get; set; }

        [FromForm(Name = "title")]
        public String title         { get; set; }

        [FromForm(Name = "album")]
        public String album         { get; set; }

        [FromForm(Name = "artist")]
        public String artist        { get; set; }

    }
}
