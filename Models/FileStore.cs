using System;

namespace uploaddownload.Models
{
    public class FileStore
    {
        public int FileId { get; set; }
        public string FileName { get; set; } = null!;
        public byte[] FileData { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long? ContentLength { get; set; }
        public DateTime InsertionDate { get; set; }
    }
}
