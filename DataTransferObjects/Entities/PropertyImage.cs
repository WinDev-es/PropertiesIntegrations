using DataTransferObjets.Entities;

namespace Context.Entities
{
    public class PropertyImage
    {
        public Guid IdPropertyImage { get; set; }
        public string File { get; set; }
        public byte[] Img { get; set; }
        public bool Enabled { get; set; }
        public Guid IdProperty { get; set; }
        public Property Property { get; set; }
    }
}
