using System;
using System.Collections.Generic;
using TicketLand_project.Models;

namespace TicketLand_project.ViewModels
{
    public class MovieDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Trailer { get; set; }
        public string MovieCens { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public string Format { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public float AverageRating { get; set; }
        public List<schedule> Schedules { get; set; }
        public List<room> Rooms { get; set; }
        // Thêm các thuộc tính khác tùy thuộc vào nhu cầu
    }
}