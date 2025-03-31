using System;
using System.Diagnostics;
using DeviceStore.Data;
using DeviceStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult GenerateDeviceID()
        {
            string prefix = "IT" + DateTime.Now.ToString("yyMM");
            int count = _context.Devices.Count() + 1;
            string deviceID = prefix + count.ToString("D4"); 
            return Json(deviceID);
        }

        public JsonResult GetCategories()
        {
            try
            {
                var categories = _context.Categories.ToList();
                return Json(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, data = ex.Message });
            }
            
            
        }
        public IActionResult GetDeviceCount()
        {
            try
            {
                var total = _context.Devices.Count();
                var inuse = _context.Devices.Count(d => d.Status == "In Use");
                var instore = _context.Devices.Count(d => d.Status == "In Store");
                var damaged = _context.Devices.Count(d => d.Status == "Damaged");
                var lost = _context.Devices.Count(d => d.Status == "LostStore");
                return Json(new
                {
                    success = true,
                    total,
                    inuse,
                    instore,
                    damaged,
                    lost
                });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult GetCategoryCounts()
        {
            try
            {
                var total = _context.Categories.Count();
                var active = _context.Categories.Count(c => c.Status == "true");
                var inactive = _context.Categories.Count(c => c.Status == "false");

                return Json(new
                {
                    success = true,
                    total,
                    active,
                    inactive
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetDeviceData()
        {
            try
            {
                var devices = (from d in _context.Devices
                               join c in _context.Categories on d.TypeID equals c.TypeID
                               select new
                               {
                                   d.DeviceID,
                                   Type = c.Name,  
                                   d.Name,
                                   d.Brand,
                                   d.Model,
                                   PurchaseDate = d.PurchaseDate.ToString("yyyy-MM-dd"),  
                                   WarrantyExpiry = d.WarrantyExpiry.HasValue ? d.WarrantyExpiry.Value.ToString("yyyy-MM-dd") : null,
                                   d.Status
                               }).ToList();


                return Json(devices);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        

        }
        public JsonResult GetEditDevice(string id)
        {
            var device = _context.Devices
                .Where(d => d.DeviceID == id)
                .Select(d => new
                {
                    d.DeviceID,
                    d.Name,
                    d.Brand,
                    d.Model,
                    d.TypeID,
                    PurchaseDate = d.PurchaseDate.ToString("yyyy-MM-dd"), 
                    WarrantyExpiry = d.WarrantyExpiry.HasValue ? d.WarrantyExpiry.Value.ToString("yyyy-MM-dd") : null,
                    d.Status
                }).FirstOrDefault();

            return Json(device);
        }

        public JsonResult GetDevicesByStatus(string status)
        {
            try
            {
                var devices = (from d in _context.Devices
                               join c in _context.Categories on d.TypeID equals c.TypeID
                               where d.Status.ToLower() == status.ToLower() 
                               select new
                               {
                                   d.DeviceID,
                                   Type = c.Name, 
                                   d.Name,
                                   d.Brand,
                                   d.Model,
                                   PurchaseDate = d.PurchaseDate.ToString("yyyy-MM-dd"),  
                                   WarrantyExpiry = d.WarrantyExpiry.HasValue ? d.WarrantyExpiry.Value.ToString("yyyy-MM-dd") : null,
                                   d.Status
                               }).ToList();


                return Json(devices);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult GetDevicebyType(Guid categoryId)
        {
            try
            {
                var devices = (from d in _context.Devices
                               join c in _context.Categories on d.TypeID equals c.TypeID
                               where d.TypeID == categoryId
                               select new
                               {
                                   d.DeviceID,
                                   Type = c.Name,  
                                   d.Name,
                                   d.Brand,
                                   d.Model,
                                   PurchaseDate = d.PurchaseDate.ToString("yyyy-MM-dd"),  
                                   WarrantyExpiry = d.WarrantyExpiry.HasValue ? d.WarrantyExpiry.Value.ToString("yyyy-MM-dd") : null,
                                   d.Status
                               }).ToList();

                if (devices.Count > 0) {
                    return Json(new { success = true, devices });
                }
                else
                {
                    return Json(new { success = false});
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DeviceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var device = new Device
            {
                DeviceID = model.DeviceID,
                TypeID = model.TypeID,
                Name = model.Name,
                Brand = model.Brand,
                Model = model.Model,
                PurchaseDate = model.PurchaseDate,
                WarrantyExpiry = model.WarrantyExpiry,
                Status = model.Status
            };

            _context.Devices.Add(device);
            _context.SaveChanges();
            return Ok();
        }
        public JsonResult AddDevice(Device device)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Devices.Add(device);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Device added successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            return Json(new { success = false, message = "Invalid data." });
        }
        public IActionResult DeviceEdit([Bind("DeviceID, Name, Brand, Model, TypeID,PurchaseDate, WarrantyExpiry, Status")] Device device)
        {
            if (ModelState.IsValid)
            {
                var existingDevice = _context.Devices.Find(device.DeviceID);
                if (existingDevice != null)
                {
                    existingDevice.Name = device.Name;
                    existingDevice.Brand = device.Brand;
                    existingDevice.Model = device.Model;
                    existingDevice.TypeID = device.TypeID;
                    existingDevice.PurchaseDate = device.PurchaseDate;
                    existingDevice.WarrantyExpiry = device.WarrantyExpiry;
                    existingDevice.Status = device.Status;

                    _context.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        }
}
