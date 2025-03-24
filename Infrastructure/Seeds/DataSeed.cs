using Domain.Models.ProductManagement;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Seeds;

/// <summary>
/// Provides a method for initializing product and category data in the database.
/// </summary>
public static class DataSeed
{
    /// <summary>
    /// Initializes the product and category data in the database if no products already exist.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to get the application context.</param>
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        ApplicationDbContext? context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Dynamically resolve the Products DbSet or table using reflection
        object? productDbSet = GetDbSet(context, "Products") ?? throw new InvalidOperationException("Products DbSet was not found.");
        object? categoryDbSet = GetDbSet(context, "ProductCategories") ?? throw new InvalidOperationException("Categories DbSet was not found.");

        // Seed categories
        List<ProductCategory> categories =
        [
            new ProductCategory { Name = "Electronics", Description = "Devices and gadgets for everyday use" },
            new ProductCategory { Name = "Home Appliances", Description = "Appliances for the home, kitchen, and cleaning" },
            new ProductCategory { Name = "Health & Fitness", Description = "Products for health and physical well-being" },
            new ProductCategory { Name = "Gaming", Description = "Products for gaming and entertainment" },
            new ProductCategory { Name = "Furniture", Description = "Furniture and accessories for home and office" },
            new ProductCategory { Name = "Office Supplies", Description = "Items for your workspace and productivity" }
        ];

        if (context.ProductCategories.Count() == 1)
        {
            context.ProductCategories.AddRange(categories);
            context.SaveChanges();
        }

        List<ProductCategory> categoryList = [.. context.ProductCategories];
        categoryList.RemoveAt(0);

        // Seed products with foreign keys (use the category Ids)
        List<Product> products =
        [
            new Product { Name = "Wireless Earbuds", ImgUri = "https://example.com/images/wireless-earbuds.jpg", Price = 49.99M, Description = "High-quality wireless earbuds with noise cancellation.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Smartphone Charger", ImgUri = "https://example.com/images/smartphone-charger.jpg", Price = 19.99M, Description = "Fast charging smartphone charger with USB-C compatibility.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Bluetooth Speaker", ImgUri = "https://example.com/images/bluetooth-speaker.jpg", Price = 79.99M, Description = "Portable Bluetooth speaker with deep bass and long battery life.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Gaming Laptop", ImgUri = "https://example.com/images/gaming-laptop.jpg", Price = 999.99M, Description = "High-performance gaming laptop with powerful GPU and fast processor.", ProductCategoryId = categoryList[3].Id },
            new Product { Name = "Fitness Tracker", ImgUri = "https://example.com/images/fitness-tracker.jpg", Price = 59.99M, Description = "Track your daily activity and workouts with this sleek fitness tracker.", ProductCategoryId = categoryList[2].Id },
            new Product { Name = "4K TV", ImgUri = "https://example.com/images/4k-tv.jpg", Price = 799.99M, Description = "Ultra HD 4K TV with smart features and vivid picture quality.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Noise-Canceling Headphones", ImgUri = "https://example.com/images/noise-canceling-headphones.jpg", Price = 199.99M, Description = "Premium over-ear headphones with active noise canceling technology.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Mechanical Keyboard", ImgUri = "https://example.com/images/mechanical-keyboard.jpg", Price = 89.99M, Description = "Durable mechanical keyboard with customizable RGB lighting.", ProductCategoryId = categoryList[3].Id },
            new Product { Name = "Smartwatch", ImgUri = "https://example.com/images/smartwatch.jpg", Price = 149.99M, Description = "Advanced smartwatch with fitness tracking and heart rate monitoring.", ProductCategoryId = categoryList[2].Id },
            new Product { Name = "Electric Scooter", ImgUri = "https://example.com/images/electric-scooter.jpg", Price = 299.99M, Description = "Fast and eco-friendly electric scooter with long battery life.", ProductCategoryId = categoryList[2].Id },
            new Product { Name = "Instant Pot", ImgUri = "https://example.com/images/instant-pot.jpg", Price = 99.99M, Description = "Multi-functional pressure cooker that makes meal prep easy.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Air Fryer", ImgUri = "https://example.com/images/air-fryer.jpg", Price = 129.99M, Description = "Cook crispy and healthy meals with this advanced air fryer.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Robot Vacuum", ImgUri = "https://example.com/images/robot-vacuum.jpg", Price = 249.99M, Description = "Automated robot vacuum cleaner with smart mapping technology.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Gaming Mouse", ImgUri = "https://example.com/images/gaming-mouse.jpg", Price = 49.99M, Description = "High-precision gaming mouse with customizable buttons.", ProductCategoryId = categoryList[3].Id },
            new Product { Name = "Smart Light Bulb", ImgUri = "https://example.com/images/smart-light-bulb.jpg", Price = 29.99M, Description = "Control your lighting with this smart, energy-efficient light bulb.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Digital Camera", ImgUri = "https://example.com/images/digital-camera.jpg", Price = 599.99M, Description = "Capture stunning photos with this high-resolution digital camera.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Electric Toothbrush", ImgUri = "https://example.com/images/electric-toothbrush.jpg", Price = 69.99M, Description = "Electric toothbrush with advanced brushing modes and timer.", ProductCategoryId = categoryList[2].Id },
            new Product { Name = "Laptop Cooling Pad", ImgUri = "https://example.com/images/laptop-cooling-pad.jpg", Price = 39.99M, Description = "Keep your laptop cool during heavy use with this cooling pad.", ProductCategoryId = categoryList[3].Id },
            new Product { Name = "Smart Thermostat", ImgUri = "https://example.com/images/smart-thermostat.jpg", Price = 199.99M, Description = "Control your home temperature remotely with this smart thermostat.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Tablet", ImgUri = "https://example.com/images/tablet.jpg", Price = 299.99M, Description = "Portable tablet with a high-resolution display and long battery life.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Wireless Charging Pad", ImgUri = "https://example.com/images/wireless-charging-pad.jpg", Price = 25.99M, Description = "Fast wireless charging pad compatible with most devices.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "LED Monitor", ImgUri = "https://example.com/images/led-monitor.jpg", Price = 199.99M, Description = "Full HD LED monitor with vivid colors and fast refresh rate.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Smart Plug", ImgUri = "https://example.com/images/smart-plug.jpg", Price = 19.99M, Description = "Control your electronics remotely with this smart plug.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Home Security Camera", ImgUri = "https://example.com/images/home-security-camera.jpg", Price = 99.99M, Description = "Monitor your home with this smart security camera.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Standing Desk", ImgUri = "https://example.com/images/standing-desk.jpg", Price = 299.99M, Description = "Adjustable standing desk for a more ergonomic workspace.", ProductCategoryId = categoryList[4].Id },
            new Product { Name = "3D Printer", ImgUri = "https://example.com/images/3d-printer.jpg", Price = 399.99M, Description = "Create 3D models with this precision 3D printer.", ProductCategoryId = categoryList[4].Id },
            new Product { Name = "VR Headset", ImgUri = "https://example.com/images/vr-headset.jpg", Price = 499.99M, Description = "Experience virtual reality with this immersive VR headset.", ProductCategoryId = categoryList[3].Id },
            new Product { Name = "Smart Doorbell", ImgUri = "https://example.com/images/smart-doorbell.jpg", Price = 149.99M, Description = "Smart doorbell with video capabilities and motion detection.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Bluetooth Headphones", ImgUri = "https://example.com/images/bluetooth-headphones.jpg", Price = 79.99M, Description = "Wireless over-ear Bluetooth headphones with deep bass.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Air Purifier", ImgUri = "https://example.com/images/air-purifier.jpg", Price = 149.99M, Description = "Clean the air in your home with this high-efficiency air purifier.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "WiFi Router", ImgUri = "https://example.com/images/wifi-router.jpg", Price = 129.99M, Description = "Fast and secure WiFi router with wide coverage.", ProductCategoryId = categoryList[0].Id },
            new Product { Name = "Electric Kettle", ImgUri = "https://example.com/images/electric-kettle.jpg", Price = 49.99M, Description = "Boil water quickly with this electric kettle.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Coffee Maker", ImgUri = "https://example.com/images/coffee-maker.jpg", Price = 99.99M, Description = "Brew fresh coffee with this modern coffee maker.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Wireless Keyboard", ImgUri = "https://example.com/images/wireless-keyboard.jpg", Price = 59.99M, Description = "Sleek and comfortable wireless keyboard.", ProductCategoryId = categoryList[4].Id },
            new Product { Name = "Smart Lock", ImgUri = "https://example.com/images/smart-lock.jpg", Price = 179.99M, Description = "Secure your home with this smart lock.", ProductCategoryId = categoryList[1].Id },
            new Product { Name = "Microwave Oven", ImgUri = "https://example.com/images/microwave-oven.jpg", Price = 199.99M, Description = "Compact microwave oven with advanced features.", ProductCategoryId = categoryList[1].Id }
        ];

        if (context.Products.Any())
        {
            ProductCategory defaultCategory = context.ProductCategories.First(x => x.Name == "Uncategorized");
            foreach (Product? product in context.Products.Where(x => x.ProductCategoryId == defaultCategory.Id).ToList())
            {
                product.ProductCategoryId = products.First(x => x.Name == product.Name).ProductCategoryId;
            }

            context.SaveChanges();
            return;
        }
        else
        {
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }

    /// <summary>
    /// Helper method to dynamically resolve a DbSet by name.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="dbSetName">The name of the DbSet.</param>
    /// <returns>The DbSet if found; otherwise, null.</returns>
    private static object? GetDbSet(object context, string dbSetName)
    {
        // Attempt to get the DbSet property by name
        PropertyInfo? dbSetProperty = context.GetType().GetProperty(dbSetName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        // Return null if the property is not found
        return dbSetProperty?.GetValue(context);
    }
}
