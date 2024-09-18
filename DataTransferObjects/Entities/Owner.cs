namespace DataTransferObjets.Entities
{
    public class Owner
    {
        public Guid IdOwner { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public byte[] Photo { get; set; }
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
