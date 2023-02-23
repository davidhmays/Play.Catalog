using System;

namespace Play.Catalog.Service.Entities
{
    // NOT == to DTOs. Entities define how will be saving to DB.
    
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }


};