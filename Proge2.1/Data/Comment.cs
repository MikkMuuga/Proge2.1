using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Comment: Entity
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        [StringLength(512)]
        public string Content { get; set; }
        [Required]
        [StringLength(512)]
        public string User { get; set; }
    }
}
