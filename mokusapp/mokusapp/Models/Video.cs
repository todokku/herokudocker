using System;

namespace herokudocker.Models
{
    public class Video
    {
        public int Id { get; set; }

        public string EmbedCode { get; set; }

        public string Desc { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
