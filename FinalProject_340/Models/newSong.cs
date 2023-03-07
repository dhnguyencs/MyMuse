﻿using Microsoft.AspNetCore.Mvc;

namespace FinalProject_340.Models
{
    public class __NewSong
    {
        [FromForm(Name = "formFile")]
        public IFormFile formFile { get; set; }

        [FromForm(Name = "title")]
        public string title { get; set; }

        [FromForm(Name = "artist")]
        public string artist { get; set; }
    }
}
