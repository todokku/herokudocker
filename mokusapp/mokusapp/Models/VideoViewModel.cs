using System;
using System.ComponentModel.DataAnnotations;

namespace herokudocker.Models
{
    public class VideoViewModel
    {
        [Required]
        [MaxLength(20)]
        public string EmbedCode { get; set; }

        [MaxLength(250)]
        public string Desc { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
