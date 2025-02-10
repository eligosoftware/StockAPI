using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be minimum 5 characters")]
        [MaxLength(280, ErrorMessage = "Title must be over 280 characters")]
        public string Title { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Content must be minimum 5 characters")]
        [MaxLength(280, ErrorMessage = "Content must be over 280 characters")]
        public string Content { get; set; }
    }
}