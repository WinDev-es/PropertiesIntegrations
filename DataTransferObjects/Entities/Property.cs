using Context.Entities;

namespace DataTransferObjets.Entities
{
    public class Property
    {
        public Guid IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public Guid IdOwner { get; set; }
        public Owner Owner { get; set; }
        public ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
    }

}
