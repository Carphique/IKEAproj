using ikea.Models;

namespace ikea.Data
{
    public static class DbSeeder
    {
        private const string ImageBaseUrl = "http://localhost:5000/images/products/";

        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Products.Any())
                return;

            var livingRoom = new Category { Name = "Вітальня", ImageUrl = ImageBaseUrl + "livingroom-sofa-klippan.jpg" };
            var bedroom = new Category { Name = "Спальня", ImageUrl = ImageBaseUrl + "bedroom-bed-malm.jpg" };
            var kitchen = new Category { Name = "Кухня", ImageUrl = ImageBaseUrl + "kitchen-cart-forhoja.jpg" };
            var bathroom = new Category { Name = "Ванна кімната", ImageUrl = ImageBaseUrl + "bathroom-vanity-godmorgon.jpg" };
            var kids = new Category { Name = "Дитяча кімната", ImageUrl = ImageBaseUrl + "kids-bed-stuva.jpg" };
            var storage = new Category { Name = "Зберігання та організація", ImageUrl = ImageBaseUrl + "storage-shelf-kallax.jpg" };

            context.Categories.AddRange(livingRoom, bedroom, kitchen, bathroom, kids, storage);
            await context.SaveChangesAsync();

            var products = new List<Product>
            {
                Make("KLIPPAN", "Двомісний диван, текстильна оббивка", 12999, "180x88x66 см", "Сірий", livingRoom.Id, "livingroom-sofa-klippan.jpg"),
                Make("EKTORP", "Тримісний диван", 18999, "218x88x88 см", "Бежевий", livingRoom.Id, "livingroom-sofa-ektorp.jpg"),
                Make("LACK", "Журнальний столик", 899, "90x55x45 см", "Білий", livingRoom.Id, "livingroom-table-lack.jpg"),
                Make("POÄNG", "Крісло", 2499, "68x82x100 см", "Дуб/бежевий", livingRoom.Id, "livingroom-chair-poang.jpg"),
                Make("KIVIK", "Кутовий диван", 24999, "280x95x83 см", "Темно-синій", livingRoom.Id, "livingroom-sofa-kivik.jpg"),

                Make("MALM", "Ліжко двоспальне", 7999, "160x200 см", "Білий", bedroom.Id, "bedroom-bed-malm.jpg"),
                Make("HEMNES", "Комод, 6 шухляд", 5499, "108x96 см", "Білий", bedroom.Id, "bedroom-dresser-hemnes.jpg"),
                Make("SONGESAND", "Каркас ліжка", 6999, "140x200 см", "Білий", bedroom.Id, "bedroom-bed-songesand.jpg"),
                Make("BRIMNES", "Ліжко з ящиками", 8999, "160x200 см", "Чорний", bedroom.Id, "bedroom-bed-brimnes.jpg"),
                Make("KLEPPSTAD", "Шафа", 3999, "117x176 см", "Білий", bedroom.Id, "bedroom-wardrobe-kleppstad.jpg"),

                Make("FÖRHÖJA", "Кухонний візок", 3499, "48x69x83 см", "Білий/бамбук", kitchen.Id, "kitchen-cart-forhoja.jpg"),
                Make("METOD", "Кухонна шафа навісна", 4299, "60x40x80 см", "Білий", kitchen.Id, "kitchen-cabinet-metod.jpg"),
                Make("KUNGSFORS", "Настінна рейка", 799, "112x27 см", "Нержавіюча сталь", kitchen.Id, "kitchen-rail-kungsfors.jpg"),
                Make("IKEA 365+", "Набір посуду, 18 предметів", 1999, "-", "Білий", kitchen.Id, "kitchen-dishes-ikea365.jpg"),
                Make("VARDAGEN", "Каструля з кришкою", 899, "24 см", "Нержавіюча сталь", kitchen.Id, "kitchen-pot-vardagen.jpg"),

                Make("GODMORGON", "Тумба під раковину", 6999, "80x47x58 см", "Білий глянець", bathroom.Id, "bathroom-vanity-godmorgon.jpg"),
                Make("BROGRUND", "Тримач для рушників", 399, "36 см", "Нержавіюча сталь", bathroom.Id, "bathroom-towel-brogrund.jpg"),
                Make("ENUDDEN", "Дзеркало з полицею", 899, "60x67 см", "Білий", bathroom.Id, "bathroom-mirror-enudden.jpg"),
                Make("SALJAN", "Килимок для ванної", 349, "60x90 см", "Сірий", bathroom.Id, "bathroom-mat-saljan.jpg"),

                Make("STUVA", "Дитяче ліжко з ящиками", 6499, "90x200 см", "Білий", kids.Id, "kids-bed-stuva.jpg"),
                Make("MAMMUT", "Дитячий стіл", 1299, "77x54 см", "Зелений", kids.Id, "kids-table-mammut.jpg"),
                Make("SOLGUL", "М'яка іграшка", 299, "28 см", "Різнокольоровий", kids.Id, "kids-toy-solgul.jpg"),
                Make("SNIGLAR", "Дитяче ліжечко", 2999, "60x120 см", "Береза", kids.Id, "kids-crib-sniglar.jpg"),

                Make("KALLAX", "Стелаж", 3299, "77x147 см", "Білий", storage.Id, "storage-shelf-kallax.jpg"),
                Make("BILLY", "Книжкова шафа", 2499, "80x28x202 см", "Дуб", storage.Id, "storage-bookcase-billy.jpg"),
                Make("TROFAST", "Система зберігання", 1899, "99x44x56 см", "Білий/сірий", storage.Id, "storage-system-trofast.jpg"),
                Make("SKUBB", "Органайзер для одягу", 599, "44x24x86 см", "Білий", storage.Id, "storage-organizer-skubb.jpg"),
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        private static Product Make(string name, string description, decimal price, string dimensions, string color, int categoryId, string imageFile)
        {
            var product = new Product
            {
                ArticleNumber = Guid.NewGuid().ToString("N")[..8].ToUpper(),
                Name = name,
                Description = description,
                Price = price,
                Dimensions = dimensions,
                Color = color,
                StockQuantity = 15,
                CategoryId = categoryId,
            };

            product.Images.Add(new ProductImage
            {
                Url = ImageBaseUrl + imageFile,
                IsMain = true
            });

            return product;
        }
    }
}