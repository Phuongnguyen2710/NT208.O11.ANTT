using System;
using TicketLand_project.Models;

namespace TicketLand_project.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public int MemberId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; }
        public float CommentStar { get; set; }
        public DateTime CommentDate { get; set; }
        public member Member { get; set; }
        public string MemberAvatar { get; set; }
        public movy Movie { get; set; }
        public string MemberName { get; set; }
    }
}
