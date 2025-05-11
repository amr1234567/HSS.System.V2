using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Queues;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.Seeding
{
    internal class City
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string[] Cities { get; set; }
    }

    public class RandomLat
    {
        public static (double Latitude, double Longitude) GetRandomLatLng()
        {
            Random random = new Random();

            // Latitude ranges from -90 to 90 degrees
            double latitude = random.NextDouble() * 180 - 90;

            // Longitude ranges from -180 to 180 degrees
            double longitude = random.NextDouble() * 360 - 180;

            return (latitude, longitude);
        }
        public static string CreateSalt()
        {
            byte[] saltBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        public static string HashPasswordWithSalt(string salt, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

    }

    public class SeedingData
    {
        private static readonly string[] femaleNames =
        {
            "فاطمة", "منى", "عائشة", "سميرة", "نورهان", "حنان", "أمل", "مروة",
            "نهى", "رانيا", "ليلى", "دينا", "ابتسام", "أسماء", "سارة", "ريحام",
            "ياسمين", "ندى", "مي", "إسراء", "شروق", "هبة", "غادة", "سحر",
            "أماني", "منار", "رندا", "بسمة", "وفاء", "إيناس", "ريمة", "هلا"
        };

        private static readonly string[] maleNames =
        {
            "محمد", "أحمد", "محمود", "حسن", "علي", "عمر", "خالد", "يوسف",
            "ياسر", "طارق", "ماهر", "إيهاب", "مصطفى", "هاني", "شريف", "نبيل",
            "عادل", "سامي", "تامر", "عمرو", "وائل", "رامي", "فادي", "مصطفى", "رجب",
            "عبد الله", "جمال", "إبراهيم", "هشام", "كريم", "أسامة", "صلاح", "أيمن",
            "بسام", "رشاد", "فهد", "حسين", "زياد", "ماجد", "أنور", "نادر"
        };

        private static readonly string[] familyNames =
        {
            "عبد الله", "حسين", "إبراهيم", "سعيد", "مصطفى", "كمال", "سالم", "نصر",
            "فتحي", "جمال", "سليمان", "السيد", "محمود", "عبد الرحمن", "المورسي",
            "رشاد", "عبد العزيز", "هقازي", "الجوهري", "الخالّي", "صلاح", "قاسم",
            "فريد", "زكي", "نصار", "عبد المنعم", "الباز", "يوسف", "مرسي", "الشناوي"
        };

        private static readonly City[] governments =
        {
            new() { Code = "01", Name = "القاهرة", Cities = new[] { "مدينة نصر", "المعادي" } },
            new() { Code = "02", Name = "الإسكندرية", Cities = new[] { "المنشية", "سيدي جابر" } },
            new() { Code = "03", Name = "الجيزة", Cities = new[] { "الدقي", "الهرم" } },
            new() { Code = "04", Name = "القليوبية", Cities = new[] { "بنها", "شبرا الخيمة" } },
            new() { Code = "05", Name = "بورسعيد", Cities = new[] { "بورسعيد" } },
            new() { Code = "06", Name = "السويس", Cities = new[] { "السويس" } },
            new() { Code = "07", Name = "الإسماعيلية", Cities = new[] { "الإسماعيلية", "فايد" } },
            new() { Code = "08", Name = "دمياط", Cities = new[] { "دمياط الجديدة", "رأس البر" } },
            new() { Code = "09", Name = "الشرقية", Cities = new[] { "الزقازيق", "بلبيس" } },
            new() { Code = "10", Name = "المنوفية", Cities = new[] { "شبين الكوم", "منوف" } },
            new() { Code = "11", Name = "كفر الشيخ", Cities = new[] { "كفر الشيخ", "دسوق" } },
            new() { Code = "12", Name = "البحيرة", Cities = new[] { "دمنهور", "إدكو" } },
            new() { Code = "13", Name = "الغربية", Cities = new[] { "طنطا", "المحلة الكبرى" } },
            new() { Code = "14", Name = "الدقهلية", Cities = new[] { "المنصورة", "ميت غمر" } },
            new() { Code = "15", Name = "البحر الأحمر", Cities = new[] { "سفاجا", "قصير" } },
            new() { Code = "16", Name = "الوادي الجديد", Cities = new[] { "دخلة", "بريس" } },
            new() { Code = "17", Name = "الفيوم", Cities = new[] { "إبشواي", "طامية" } },
            new() { Code = "18", Name = "بني سويف", Cities = new[] { "الوسطى", "سخا" } },
            new() { Code = "19", Name = "المنيا", Cities = new[] { "ملوي", "أبو قرقاص" } },
            new() { Code = "20", Name = "أسيوط", Cities = new[] { "ديروط", "أبنوب" } },
            new() { Code = "21", Name = "سوهاج", Cities = new[] { "طهطا", "جهينة" } },
            new() { Code = "22", Name = "قنا", Cities = new[] { "نجع حمادي", "قوص" } },
            new() { Code = "23", Name = "الأقصر", Cities = new[] {"أسنا", "أرمانت" } },
            new() { Code = "24", Name = "أسوان", Cities = new[] {  "كم أمبو", "إدفو" } },
            new() { Code = "25", Name = "مطروح", Cities = new[] { "مرسى مطروح", "العلمين"} },
            new() { Code = "26", Name = "شمال سيناء", Cities = new[] { "العريش", "رفح" } },
            new() { Code = "27", Name = "جنوب سيناء", Cities = new[] {  "طابا" } },
        };

        private static readonly List<Specialization> specializations =
        [
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب القلب",
                Description = "رعاية القلب والجهاز الدوري وتشخيص أمراض القلب.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='red' width='24px' height='24px'>
                            <path d='M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 
                                     2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09
                                     C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5
                                     c0 3.78-3.4 6.86-8.55 11.54L12 21.35z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "جراحة العظام",
                Description = "علاج وتشخيص أمراض الجهاز العضلي الهيكلي والمفاصل.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='gray' width='24px' height='24px'>
                            <path d='M16 4c-1.66 0-3 1.34-3 3v2h2V7c0-.55.45-1 1-1
                                     s1 .45 1 1v2h2V7c0-1.66-1.34-3-3-3zM8 4
                                     C6.34 4 5 5.34 5 7v2h2V7c0-.55.45-1 1-1
                                     s1 .45 1 1v2h2V7c0-1.66-1.34-3-3-3zM5 13v2
                                     c0 1.66 1.34 3 3 3h2v-2H8c-.55 0-1-.45-1-1v-2H5z
                                     M16 13h-2v2c0 .55-.45 1-1 1h-2v2h2c1.66 0 3-1.34 
                                     3-3v-2z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب الأطفال",
                Description = "الرعاية الطبية للأطفال والرضع والمراهقين.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='blue' width='24px' height='24px'>
                            <circle cx='12' cy='12' r='10'/>
                            <path d='M8 14c1.333 1 4.667 1 6 0'/>
                            <circle cx='9' cy='10' r='1'/>
                            <circle cx='15' cy='10' r='1'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب الأورام",
                Description = "تشخيص وعلاج السرطان ومتابعة المرضى.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='purple' width='24px' height='24px'>
                            <path d='M12 2C10.34 2 9 3.34 9 5v6H7v2h2v6c0 1.66 
                                     1.34 3 3 3s3-1.34 3-3v-6h2v-2h-2V5c0-1.66-1.34-3-3-3z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب الأعصاب",
                Description = "تشخيص وعلاج اضطرابات الجهاز العصبي المركزي والطرفي.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='orange' width='24px' height='24px'>
                            <path d='M12 2C8.13 2 5 5.13 5 9c0 3.25 2.5 5.94 5.78 
                                     6.8L11 18v2H9v2h6v-2h-2v-2l.22-.2C16.5 14.94 19 
                                     12.25 19 9c0-3.87-3.13-7-7-7z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "أمراض الجهاز الهضمي",
                Description = "تشخيص وعلاج أمراض المعدة والأمعاء والكبد.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='green' width='24px' height='24px'>
                            <path d='M12 2C7 2 4 5 4 9v6c0 4 3 7 7 11 4-4 7-7 7-11V9
                                     c0-4-3-7-7-7zm0 2c3.31 0 6 2.69 6 6v6c0 3.31-2.69 
                                     6-6 6s-6-2.69-6-6V9c0-3.31 2.69-6 6-6z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب الجلد",
                Description = "تشخيص وعلاج أمراض الجلد والشعر والأظافر.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='pink' width='24px' height='24px'>
                            <path d='M12 2C8 2 5 5 5 8c0 3.5 2 5 2 5s-2 1.5-2 5c0 3 3 4 
                                     7 4s7-1 7-4c0-3.5-2-5-2-5s2-1.5 2-5c0-3-3-6-7-6z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "الطب النفسي",
                Description = "علاج الاضطرابات النفسية والسلوكية والعاطفية.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='teal' width='24px' height='24px'>
                            <path d='M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 
                                     10-4.48 10-10S17.52 2 12 2zm-1 14.5H9v-1.5h2v1.5zm0-3H9v-6h2v6zm4 
                                     3h-2v-2h2v2zm0-3h-2v-3h2v3z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "الغدد الصماء",
                Description = "تشخيص وعلاج الاضطرابات الهرمونية والأيضية.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='brown' width='24px' height='24px'>
                            <circle cx='12' cy='12' r='10'/>
                            <path d='M12 6v6l4 2'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب الرئة",
                Description = "تشخيص وعلاج أمراض الجهاز التنفسي والرئتين.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='lightblue' width='24px' height='24px'>
                            <path d='M7 2c-2.21 0-4 1.79-4 4v12c0 2.21 1.79 4 4 
                                     4s4-1.79 4-4V6c0-2.21-1.79-4-4-4zm10 0c-2.21 0-4 
                                     1.79-4 4v12c0 2.21 1.79 4 4 4s4-1.79 4-4V6c0-2.21-1.79-4-4-4z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "مسالك بولية",
                Description = "رعاية وتشخيص أمراض الجهاز البولي والتناسلي الذكري.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='navy' width='24px' height='24px'>
                            <path d='M12 2C8 2 5 6 5 10c0 4 3 7 7 11 4-4 7-7 7-11 0-4-3-8-7-8z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب الكلى",
                Description = "تشخيص وعلاج اضطرابات وظائف الكلى والأمراض المرتبطة بها.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='maroon' width='24px' height='24px'>
                            <path d='M12 2C8 2 4 5 4 9v6c0 4 4 7 8 7s8-3 8-7V9c0-4-4-7-8-7z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب الروماتيزم",
                Description = "تشخيص وعلاج أمراض المفاصل والعضلات والأمراض المناعية الذاتية.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='darkorange' width='24px' height='24px'>
                            <path d='M12 4a8 8 0 1 0 0 16 8 8 0 0 0 0-16zm2 9h-4v-2h4v2z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "طب العيون",
                Description = "تشخيص وعلاج أمراض العيون والرؤية.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='black' width='24px' height='24px'>
                            <path d='M12 4.5C7 4.5 2.73 7.61 1 12c1.73 4.39 6 7.5 11 7.5s9.27-3.11 11-7.5c-1.73-4.39-6-7.5-11-7.5zm0 13c-3.04 0-5.5-2.46-5.5-5.5S8.96 6.5 12 6.5 17.5 8.96 17.5 12 15.04 17.5 12 17.5zm0-9c-1.93 0-3.5 1.57-3.5 3.5S10.07 15.5 12 15.5 15.5 13.93 15.5 12 13.93 8.5 12 8.5z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "أنف وأذن وحنجرة",
                Description = "تشخيص وعلاج أمراض الأنف والأذن والحنجرة.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='brown' width='24px' height='24px'>
                            <path d='M12 2C8 2 4 6 4 12s4 10 8 10 8-4 8-10S16 2 12 2zm0 2
                                     c2.21 0 4 2.24 4 5s-1.79 5-4 5-4-2.24-4-5 1.79-5 4-5z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "الجراحة العامة",
                Description = "إجراءات جراحية لعلاج مجموعة واسعة من الحالات الطبية.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='darkred' width='24px' height='24px'>
                            <path d='M2 21l3-3 11-11-3-3L2 15l-1 6zM14.5 4.5l3 3 2-2-3-3-2 2z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "الجراحة التجميلية",
                Description = "إجراءات تجميلية وترميمية لتحسين مظهر المرضى.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='magenta' width='24px' height='24px'>
                            <path d='M12 2C9.79 2 8 3.79 8 6s1.79 4 4 4 4-1.79 4-4-1.79-4-4-4zm0 6
                                     c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2zM4 22c0-4 4-6 8-6s8 2 8 6H4z'/>
                         </svg>"
            },
            new Specialization
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = "أمراض النساء والتوليد",
                Description = "رعاية صحة المرأة، الولادة، ومتابعة أمراض الجهاز التناسلي.",
                Icon = @"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='hotpink' width='24px' height='24px'>
                            <path d='M12 2a5 5 0 0 0-5 5c0 3.31 2.69 6 6 6s6-2.69 6-6a5 5 0 0 0-5-5zm0 2a3 3 0 0 1 3 3c0 1.66-1.34 3-3 3s-3-1.34-3-3a3 3 0 0 1 3-3zm-4 9c-2.67 0-8 1.34-8 4v3h8v-3H4c.67-.67 2.67-2 4-2s3.33 1.33 4 2h-4v3h8v-3c0-2.66-5.33-4-8-4z'/>
                         </svg>"
            }
        ];
        public static async Task SeedAsync(AppDbContext context)
        {
            await SeedPatientsAsync(context, 10);
            await SeedTestsAndSpecilizations(context);
            await SeedHospitals(context, 3);
            await SeedDepartmentsWithEmployees(context);
        }

        private static async Task SeedPatientsAsync(AppDbContext context, int v)
        {
            if (!context.Patients.Any())
            {
                var random = new Random();
                // Seed the governorates with sample cities.
                var patients = new List<Patient>();
                for (int i = 0; i < v; i++) // Reduced number for demonstration
                {
                    var randomlatandLng = RandomLat.GetRandomLatLng();
                    var gender = random.Next(2) == 0 ? Gender.Male : Gender.Female;
                    var government = governments[random.Next(governments.Length)];
                    var birthDate = GenerateEgyptianBirthDate(random);
                    var salt = RandomLat.CreateSalt();
                    var patient = new Patient
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = (gender == Gender.Male
                            ? maleNames[random.Next(maleNames.Length)]
                            : femaleNames[random.Next(femaleNames.Length)]) 
                            + " " + 
                            string.Join(" ", Enumerable.Range(0, 4)
                                .Select(_ => familyNames[random.Next(familyNames.Length)])),
                        NationalId = GenerateNationalID(birthDate, government.Code, gender == Gender.Male),
                        PhoneNumber = GenerateEgyptianPhoneNumber(random),
                        Address = $"{government.Name}, {government.Cities[random.Next(government.Cities.Length)]}, " +
                                  $"ش. {random.Next(1, 200)}",
                        BirthOfDate = birthDate,
                        Gender = gender,
                        Role = UserRole.Patient,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Lat = randomlatandLng.Latitude,
                        Lng = randomlatandLng.Longitude,
                        Salt = salt,
                        HashPassword = RandomLat.HashPasswordWithSalt(salt, "@Aa123456789")
                    };

                    // Add to context and save in batches
                    patients.Add(patient);
                    if (i % 1000 == 0)
                    {
                        await context.Patients.AddRangeAsync(patients);
                        await context.SaveChangesAsync();
                        patients.Clear();
                    }
                }
                await context.Patients.AddRangeAsync(patients);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedHospitals(AppDbContext context, int hospitalsCount)
        {
            if (!context.Hospitals.Any())
            {
                var random = new Random();
                var hospitals = new List<Hospital>();
                for (int i = 0; i < hospitalsCount; i++)
                {
                    var hospital = new Hospital
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "مستشفى " + familyNames[random.Next(familyNames.Length)],
                        Address = $"{governments[random.Next(governments.Length)].Name}, ش. {random.Next(1, 200)}",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Lat = random.NextDouble() * 180 - 90,
                        Lng = random.NextDouble() * 360 - 180
                    };
                    hospitals.Add(hospital);
                }
                await context.Hospitals.AddRangeAsync(hospitals);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedDepartmentsWithEmployees(AppDbContext context)
        {
            var hospitals = await context.Hospitals.ToListAsync();
            //Seed Clinics
            if (!context.Clinics.Any())
            {
                var random = new Random();
                var clinics = new List<Clinic>();
                foreach (var hospital in hospitals)
                {
                    foreach(var specialization in specializations)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            var clinicID = Guid.NewGuid().ToString();
                            var department = new Clinic
                            {
                                Id = clinicID,
                                Name = "عيادة " + specialization.Name,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                NumberOfShifts = 4,
                                EndAt = new TimeSpan(23, 59, 59),
                                StartAt = new TimeSpan(0, 0, 0),
                                HospitalId = hospital.Id,
                                PeriodPerAppointment = new TimeSpan(0, 30, 0),
                                SpecializationId = specializations[random.Next(specializations.Count)].Id,
                                Queue = new ClinicQueue
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow,
                                    PeriodPerAppointment = new TimeSpan(0, 30, 0),
                                },
                                Doctors = new List<Doctor>()
                                {
                                     GenerateDoctor(clinicID, "عيادة " + specialization.Name, specialization.Id, specialization.Name, hospital.Id, new TimeSpan(0,0,0), new TimeSpan(5,59,59)),
                                     GenerateDoctor(clinicID, "عيادة " + specialization.Name, specialization.Id, specialization.Name, hospital.Id, new TimeSpan(6,0,0), new TimeSpan(11,59,59)),
                                     GenerateDoctor(clinicID, "عيادة " + specialization.Name, specialization.Id, specialization.Name, hospital.Id, new TimeSpan(12,0,0), new TimeSpan(17,59,59)),
                                     GenerateDoctor(clinicID, "عيادة " + specialization.Name, specialization.Id, specialization.Name, hospital.Id, new TimeSpan(18,0,0), new TimeSpan(23,59,59)),
                                }
                            };
                            clinics.Add(department);
                        }
                    }
                    await context.Clinics.AddRangeAsync(clinics);

                    var radiologyTests = await context.Tests
                        .OfType<RadiologyTest>()
                        .ToListAsync();
                    var radiologyCenterId1 = Guid.NewGuid().ToString();
                    var radiologyCenterId2 = Guid.NewGuid().ToString();

                    var radiologyCenters = new List<RadiologyCenter>()
                    {
                        new()
                        {
                            Id = radiologyCenterId1,
                            Name = "مركز الأشعة 1",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Tests = radiologyTests,
                            EndAt = new TimeSpan(23, 59, 59),
                            StartAt = new TimeSpan(0, 0, 0),
                            HospitalId = hospital.Id,
                            NumberOfShifts = 4,
                            PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            Queue = new RadiologyCenterQueue
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            },
                            RadiologyTesters = [
                                    CreateRadiologyTester(radiologyCenterId1, "مركز الأشعة 1", hospital.Id, new TimeSpan(0,0,0), new TimeSpan(5,59,59)),
                                    CreateRadiologyTester(radiologyCenterId1, "مركز الأشعة 1", hospital.Id, new TimeSpan(6,0,0), new TimeSpan(11,59,59)),
                                    CreateRadiologyTester(radiologyCenterId1, "مركز الأشعة 1", hospital.Id, new TimeSpan(12,0,0), new TimeSpan(17,59,59)),
                                    CreateRadiologyTester(radiologyCenterId1, "مركز الأشعة 1", hospital.Id, new TimeSpan(18,0,0), new TimeSpan(23,59,59)),
                                ],
                        },
                        new()
                        {
                            Id = radiologyCenterId2,
                            Name = "مركز الأشعة 1",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Tests = radiologyTests,
                            EndAt = new TimeSpan(23, 59, 59),
                            StartAt = new TimeSpan(0, 0, 0),
                            HospitalId = hospital.Id,
                            NumberOfShifts = 4,
                            PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            Queue = new RadiologyCenterQueue
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            },
                            RadiologyTesters = [
                                    CreateRadiologyTester(radiologyCenterId2, "مركز الأشعة 1", hospital.Id, new TimeSpan(0,0,0), new TimeSpan(5,59,59)),
                                    CreateRadiologyTester(radiologyCenterId2, "مركز الأشعة 1", hospital.Id, new TimeSpan(6,0,0), new TimeSpan(11,59,59)),
                                    CreateRadiologyTester(radiologyCenterId2, "مركز الأشعة 1", hospital.Id, new TimeSpan(12,0,0), new TimeSpan(17,59,59)),
                                    CreateRadiologyTester(radiologyCenterId2, "مركز الأشعة 1", hospital.Id, new TimeSpan(18,0,0), new TimeSpan(23,59,59)),
                                ],
                        }
                    };
                    await context.RadiologyCenters.AddRangeAsync(radiologyCenters);

                    var medicalLabTests = await context.Tests
                        .OfType<MedicalLabTest>()
                        .ToListAsync();
                    var medicalLabId1 = Guid.NewGuid().ToString();
                    var medicalLabId2 = Guid.NewGuid().ToString();
                    var medicalLabs = new List<MedicalLab>()
                    {
                        new()
                        {
                            Id = medicalLabId1,
                            Name = "معمل التحاليل 1",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Tests = medicalLabTests,
                            EndAt = new TimeSpan(23, 59, 59),
                            StartAt = new TimeSpan(0, 0, 0),
                            HospitalId = hospital.Id,
                            NumberOfShifts = 4,
                            PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            Queue = new MedicalLabQueue
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            },
                            MedicalLabTesters = [
                                    CreateMedicalLabTester(medicalLabId1,"معمل التحاليل 1", hospital.Id, new TimeSpan(0,0,0), new TimeSpan(5,59,59)),
                                    CreateMedicalLabTester(medicalLabId1,"معمل التحاليل 1", hospital.Id, new TimeSpan(6,0,0), new TimeSpan(11,59,59)),
                                    CreateMedicalLabTester(medicalLabId1,"معمل التحاليل 1", hospital.Id, new TimeSpan(12,0,0), new TimeSpan(17,59,59)),
                                    CreateMedicalLabTester(medicalLabId1,"معمل التحاليل 1", hospital.Id, new TimeSpan(18,0,0), new TimeSpan(23,59,59)),
                                ]

                        },
                        new()
                        {
                            Id = medicalLabId2,
                            Name = "معمل التحاليل 2",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Tests = medicalLabTests,
                            EndAt = new TimeSpan(23, 59, 59),
                            StartAt = new TimeSpan(0, 0, 0),
                            HospitalId = hospital.Id,
                            NumberOfShifts = 4,
                            PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            Queue = new MedicalLabQueue
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow,
                                PeriodPerAppointment = new TimeSpan(0, 30, 0),
                            },
                            MedicalLabTesters = [
                                    CreateMedicalLabTester(medicalLabId2,"معمل التحاليل 2", hospital.Id, new TimeSpan(0,0,0), new TimeSpan(5,59,59)),
                                    CreateMedicalLabTester(medicalLabId2,"معمل التحاليل 2", hospital.Id, new TimeSpan(6,0,0), new TimeSpan(11,59,59)),
                                    CreateMedicalLabTester(medicalLabId2,"معمل التحاليل 2", hospital.Id, new TimeSpan(12,0,0), new TimeSpan(17,59,59)),
                                    CreateMedicalLabTester(medicalLabId2,"معمل التحاليل 2", hospital.Id, new TimeSpan(18,0,0), new TimeSpan(23,59,59)),
                                ]
                        }
                    };
                    await context.MedicalLabs.AddRangeAsync(medicalLabs);
                    
                    var receptionId = Guid.NewGuid().ToString();
                    var reception = new Reception
                    {
                        Id = receptionId,
                        Name = "استقبال " + hospital.Name,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        HospitalId = hospital.Id,
                        NumberOfShifts = 4,
                        EndAt = new TimeSpan(23, 59, 59),
                        StartAt = new TimeSpan(0, 0, 0),
                        Receptionists = [
                                GenerateReceptionist(receptionId, "استقبال " + hospital.Name, hospital.Id, new TimeSpan(0,0,0), new TimeSpan(5,59,59)),
                                GenerateReceptionist(receptionId, "استقبال " + hospital.Name, hospital.Id, new TimeSpan(6,0,0), new TimeSpan(11,59,59)),
                                GenerateReceptionist(receptionId, "استقبال " + hospital.Name, hospital.Id, new TimeSpan(12,0,0), new TimeSpan(17,59,59)),
                                GenerateReceptionist(receptionId, "استقبال " + hospital.Name, hospital.Id, new TimeSpan(18,0,0), new TimeSpan(23,59,59)),
                            ]
                    };
                    await context.Receptions.AddAsync(reception);
                }
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTestsAndSpecilizations(AppDbContext context)
        {
            var radiologyTests = new List<Test>
            {
                new RadiologyTest() {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Chest X-Ray",
                    Description = "Standard chest radiography",
                    TestPrice = 120.0,
                    EstimatedDurationInMinutes = 15,
                    BodyPart = "Chest",
                    RequiresContrast = "some data",
                    PreparationInstructions = "Remove metal objects from chest area",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new RadiologyTest() {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Abdominal CT Scan",
                    Description = "Computed tomography of abdomen",
                    TestPrice = 450.0,
                    EstimatedDurationInMinutes = 30,
                    BodyPart = "Abdomen",
                    RequiresContrast = "some data",
                    PreparationInstructions = "Fast for 4 hours prior to scan",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                 new MedicalLabTest() {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Complete Blood Count",
                    Description = "Standard blood cell analysis",
                    TestPrice = 25.0,
                    EstimatedDurationInMinutes = 5,
                    SampleType = "Blood",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new MedicalLabTest() {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Urinalysis",
                    Description = "Complete urine analysis",
                    TestPrice = 15.0,
                    SampleType = "Urine",
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    EstimatedDurationInMinutes = 10,
                }
            };
            await context.Tests.AddRangeAsync(radiologyTests);
            await context.Specializations.AddRangeAsync(specializations);
            await context.SaveChangesAsync();
        }



        private static Doctor GenerateDoctor(string clinicId, string clinicName, string specilizationId, string specilizationName, string hospitalId, TimeSpan startAt, TimeSpan EndAt)
        {
            var random = new Random();
            Gender[] genders = [Gender.Male, Gender.Female];

            var gender = genders[random.Next(genders.Length)];
            var birthDate = GenerateEgyptianBirthDate(random);
            var salt = "l0PdIajCiI4gTwOtXJS+YQ==";
            var password = HashPasswordWithSalt(salt, "@Aa123456789");

            var diagDoctor = new Doctor
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = (gender == Gender.Male
                            ? maleNames[random.Next(maleNames.Length)]
                            : femaleNames[random.Next(femaleNames.Length)])
                            + " " +
                            string.Join(" ", Enumerable.Range(0, 4)
                                .Select(_ => familyNames[random.Next(familyNames.Length)])),
                NationalId = GenerateNationalID(birthDate, governments[random.Next(governments.Length)].Code, true),
                PhoneNumber = GenerateEgyptianPhoneNumber(random),
                Address = clinicName,
                BirthOfDate = birthDate,
                Gender = gender,
                HashPassword = password,
                Salt = salt,
                Salary = random.Next(15000, 30000),
                ClinicId = clinicId,
                SpecializationId = specilizationId,
                Role = UserRole.Doctor,
                EndAt = EndAt,
                StartAt = startAt,
                SpecializationName = specilizationName,
                HospitalId = hospitalId,
                PositionName = "طبيب",
            };
            return diagDoctor;
        }

        private static RadiologyTester CreateRadiologyTester(string radiologyCenterId, string radiologyCenterName, string hospitalId, TimeSpan startAt, TimeSpan endAt)
        {
            var random = new Random();
            Gender[] genders = [Gender.Male, Gender.Female];

            var gender = genders[random.Next(genders.Length)];
            var birthDate = GenerateEgyptianBirthDate(random);
            var salt = "l0PdIajCiI4gTwOtXJS+YQ==";
            var password = HashPasswordWithSalt(salt, "@Aa123456789");

            var tester = new RadiologyTester()
            {
                Id = Guid.NewGuid().ToString(),
                BirthOfDate = birthDate,
                Gender = gender,
                HashPassword = password,
                Salt = salt,
                Salary = random.Next(15000, 30000),
                HospitalId = hospitalId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EndAt = endAt,
                StartAt = startAt,
                Name = (gender == Gender.Male
                            ? maleNames[random.Next(maleNames.Length)]
                            : femaleNames[random.Next(femaleNames.Length)])
                            + " " +
                            string.Join(" ", Enumerable.Range(0, 4)
                                .Select(_ => familyNames[random.Next(familyNames.Length)])),
                RadiologyCenterId = radiologyCenterId,
                RadiologyCenterName = radiologyCenterName,
                PhoneNumber = GenerateEgyptianPhoneNumber(random),
                NationalId = GenerateNationalID(birthDate, governments[random.Next(governments.Length)].Code, gender == Gender.Male),
                Address = "some address",
                Role = UserRole.RadiologyTester,
                PositionName = "فني أشعة",
            };
            return tester;
        }
        private static MedicalLabTester CreateMedicalLabTester(string medicalLabId, string medicalLabName, string hospitalId, TimeSpan startAt, TimeSpan endAt)
        {
            var random = new Random();
            Gender[] genders = [Gender.Male, Gender.Female];

            var gender = genders[random.Next(genders.Length)];
            var birthDate = GenerateEgyptianBirthDate(random);
            var salt = "l0PdIajCiI4gTwOtXJS+YQ==";
            var password = HashPasswordWithSalt(salt, "@Aa123456789");

            var tester = new MedicalLabTester()
            {
                Id = Guid.NewGuid().ToString(),
                BirthOfDate = birthDate,
                Gender = gender,
                HashPassword = password,
                Salt = salt,
                Salary = random.Next(15000, 30000),
                HospitalId = hospitalId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EndAt = endAt,
                StartAt = startAt,
                Name = (gender == Gender.Male
                            ? maleNames[random.Next(maleNames.Length)]
                            : femaleNames[random.Next(femaleNames.Length)])
                            + " " +
                            string.Join(" ", Enumerable.Range(0, 4)
                                .Select(_ => familyNames[random.Next(familyNames.Length)])),
                MedicalLabId = medicalLabId,
                MedicalLabName = medicalLabName,
                PhoneNumber = GenerateEgyptianPhoneNumber(random),
                NationalId = GenerateNationalID(birthDate, governments[random.Next(governments.Length)].Code, gender == Gender.Male),
                Address = "some address",
                Role = UserRole.RadiologyTester,
                PositionName = "فني معمل",
            };
            return tester;
        }
        private static Receptionist GenerateReceptionist(string receptionId, string receptionName, string hospitalId, TimeSpan startAt, TimeSpan EndAt)
        {
            var random = new Random();
            Gender[] genders = [Gender.Male, Gender.Female];

            var gender = genders[random.Next(genders.Length)];
            var birthDate = GenerateEgyptianBirthDate(random);
            var salt = "l0PdIajCiI4gTwOtXJS+YQ==";
            var password = HashPasswordWithSalt(salt, "@Aa123456789");

            var diagDoctor = new Receptionist
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = (gender == Gender.Male
                            ? maleNames[random.Next(maleNames.Length)]
                            : femaleNames[random.Next(femaleNames.Length)])
                            + " " +
                            string.Join(" ", Enumerable.Range(0, 4)
                                .Select(_ => familyNames[random.Next(familyNames.Length)])),
                NationalId = GenerateNationalID(birthDate, governments[random.Next(governments.Length)].Code, true),
                PhoneNumber = GenerateEgyptianPhoneNumber(random),
                Address = receptionName,
                BirthOfDate = birthDate,
                Gender = gender,
                HashPassword = password,
                Salt = salt,
                Salary = random.Next(15000, 30000),
                Role = UserRole.Doctor,
                EndAt = EndAt,
                StartAt = startAt,
                HospitalId = hospitalId,
                ReceptionId = receptionId,
                PositionName = "موظف استقبال",
            };
            return diagDoctor;
        }

        private static string HashPasswordWithSalt(string salt, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private static DateTime GenerateEgyptianBirthDate(Random random)
        {
            var currentYear = DateTime.Now.Year;
            var age = random.Next(18, 100);
            var year = currentYear - age;
            return new DateTime(year, random.Next(1, 13), random.Next(1, 29));
        }
        private static string GenerateNationalID(DateTime birthDate, string governorateCode, bool isMale)
        {

            // Step 1: Determine the century code
            int centuryCode = birthDate.Year >= 2000 ? 3 : 2;

            // Step 2: Extract year, month, and day of birth
            string yearOfBirth = birthDate.Year.ToString().Substring(2, 2); // Last 2 digits of the year
            string monthOfBirth = birthDate.Month.ToString("D2"); // Month as 2 digits
            string dayOfBirth = birthDate.Day.ToString("D2"); // Day as 2 digits

            // Step 4: Generate a serial number
            Random random = new Random();
            int serialNumber = random.Next(1, 10_000); // Random number between 1 and 999
            string serialNumberString = serialNumber.ToString("D3");

            // Adjust gender based on serial number's last digit
            if (isMale && serialNumber % 2 == 0) serialNumber++; // Make it odd for male
            if (!isMale && serialNumber % 2 != 0) serialNumber--; // Make it even for female
            serialNumberString = serialNumber.ToString("D3");

            // Step 5: Concatenate the parts
            string partialID = $"{centuryCode}{yearOfBirth}{monthOfBirth}{dayOfBirth}{governorateCode}{serialNumberString}";

            // Step 6: Calculate the checksum
            int checksum = CalculateChecksum(partialID);

            // Combine everything into the full ID
            return $"{partialID}{checksum}";
        }
        private static string GenerateEgyptianPhoneNumber(Random random)
        {
            var prefixes = new[] { "010", "011", "012", "015" };
            return $"{prefixes[random.Next(prefixes.Length)]}{random.Next(10000000, 99999999)}";
        }
        private static int CalculateChecksum(string partialID)
        {
            // Luhn algorithm for checksum calculation
            int sum = 0;
            bool doubleDigit = true;

            for (int i = partialID.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(partialID[i].ToString());

                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9) digit -= 9;
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return (10 - (sum % 10)) % 10;
        }

    }
}
