using Core.Entities;

namespace Entities.Concrete
{
    public class Drug : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Indications { get; set; }
        public string Contraindications { get; set; }
        public DateTime? ExprirationDate { get; set; }
    }
}
