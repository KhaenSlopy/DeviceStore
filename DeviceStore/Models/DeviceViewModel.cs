using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DeviceStore.Models
{
   
    public class DeviceViewModel
    {
        
            public string DeviceID { get; set; }
            public Guid TypeID { get; set; }
            public string Name { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public DateTime PurchaseDate { get; set; }
            public DateTime? WarrantyExpiry { get; set; }
            public string Status { get; set; }
        

    }
}
