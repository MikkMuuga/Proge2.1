using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Comment
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        [StringLength(512)]
        public string Content { get; set; }
        [Required]
        [StringLength(512)]
        public IdentityUser User { get; set; }
    }
}
