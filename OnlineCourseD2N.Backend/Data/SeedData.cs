using BCrypt.Net;
using OnlineCourseD2N.Backend.Data;

namespace OnlineCourseD2N.Backend.Data
{
    public class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            string defaultPassword = BCrypt.Net.BCrypt.HashPassword("123456");

            // Admin & Siswa Biasa
            var adminUser = new Users { Name = "Admin RuangLes", Email = "admin@ruangles.com", Password = defaultPassword, Role = "Admin" };
            var studentUser = new Users { Name = "Budi Siswa", Email = "user@ruangles.com", Password = defaultPassword, Role = "User" };

            // Akun untuk Para Trainer
            var janeUser = new Users { Name = "Jane Doe", Email = "jane@example.com", Password = defaultPassword, Role = "Trainer" };
            var agusUser = new Users { Name = "Agus Santoso", Email = "agus@example.com", Password = defaultPassword, Role = "Trainer" };
            var sitiUser = new Users { Name = "Siti Aminah", Email = "siti@example.com", Password = defaultPassword, Role = "Trainer" };
            var rianUser = new Users { Name = "Rian Pratama", Email = "rian@example.com", Password = defaultPassword, Role = "Trainer" };

            context.Users.AddRange(adminUser, studentUser, janeUser, agusUser, sitiUser, rianUser);
            context.SaveChanges();

            var janeTrainer = new Trainer
            {
                UserId = janeUser.UserId,
                Expertise = "Web Development",
                Bio = "Senior Fullstack Developer dengan pengalaman 10 tahun di industri tech startup.",
                FotoProfil = ""
            };

            var agusTrainer = new Trainer
            {
                UserId = agusUser.UserId,
                Expertise = "Mobile Development (.NET MAUI)",
                Bio = "Microsoft MVP yang fokus pada pengembangan aplikasi lintas platform.",
                FotoProfil = ""
            };

            var sitiTrainer = new Trainer
            {
                UserId = sitiUser.UserId,
                Expertise = "Data Science & AI",
                Bio = "Data Scientist di unicorn e-commerce, ahli Python dan Machine Learning.",
                FotoProfil = ""
            };

            var rianTrainer = new Trainer
            {
                UserId = rianUser.UserId,
                Expertise = "UI/UX Design",
                Bio = "Desainer produk digital yang mencintai estetika minimalis.",
                FotoProfil = ""
            };

            context.Trainers.AddRange(janeTrainer, agusTrainer, sitiTrainer, rianTrainer);
            context.SaveChanges();


            var courses = new Course[]
            {
                new Course
                {
                    Title = "Jago Blazor Hybrid",
                    Category = "Programming",
                    Description = "Pelajari cara membuat satu kodingan untuk Web, Android, iOS, dan Windows menggunakan .NET MAUI Blazor Hybrid.",
                    Duration = 12,
                    Level = "Intermediate",
                    TrainerId = janeTrainer.TrainerId,
                    Latitude = -6.2088,
                    Longitude = 106.8456,
                    LocationAddress = "Menara BCA, Jakarta Pusat",
                    CoverImage = "" 
                },
                new Course
                {
                    Title = "Mastering .NET MAUI",
                    Category = "Mobile App",
                    Description = "Panduan lengkap membangun aplikasi mobile native dengan C# dan XAML.",
                    Duration = 20,
                    Level = "Advanced",
                    TrainerId = agusTrainer.TrainerId,
                    Latitude = -7.7956,
                    Longitude = 110.3695,
                    LocationAddress = "Malioboro Mall, Yogyakarta",
                    CoverImage = ""
                },
                new Course
                {
                    Title = "Python for Data Science",
                    Category = "Data Science",
                    Description = "Belajar analisis data mulai dari Pandas, NumPy hingga visualisasi Matplotlib.",
                    Duration = 15,
                    Level = "Beginner",
                    TrainerId = sitiTrainer.TrainerId,
                    Latitude = -6.9175,
                    Longitude = 107.6191,
                    LocationAddress = "Gedung Sate, Bandung",
                    CoverImage = ""
                },
                new Course
                {
                    Title = "UI Design Fundamental",
                    Category = "Design",
                    Description = "Dasar-dasar desain antarmuka aplikasi agar ramah pengguna dan estetik.",
                    Duration = 8,
                    Level = "Beginner",
                    TrainerId = rianTrainer.TrainerId,
                    Latitude = null,
                    Longitude = null,
                    LocationAddress = null,
                    CoverImage = ""
                },
                new Course
                {
                    Title = "React.js Zero to Hero",
                    Category = "Web Development",
                    Description = "Kursus intensif frontend modern menggunakan React dan Tailwind CSS.",
                    Duration = 25,
                    Level = "Intermediate",
                    TrainerId = janeTrainer.TrainerId, 
                    Latitude = -7.2575,
                    Longitude = 112.7521,
                    LocationAddress = "Tunjungan Plaza, Surabaya",
                    CoverImage = ""
                }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();
        }
    }
}