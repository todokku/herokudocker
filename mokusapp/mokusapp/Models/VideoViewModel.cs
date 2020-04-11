using System;
using System.ComponentModel.DataAnnotations;

namespace herokudocker.Models
{
    public class VideoViewModel
    {
        [Required]
        public string EmbedCode { get; set; }

        public string Desc { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
