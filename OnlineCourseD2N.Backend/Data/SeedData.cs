namespace OnlineCourseD2N.Backend.Data
{
    public class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Trainers.Any() || context.Courses.Any()) return;

            var trainer = new Trainer
            {
                Name = "Jane Doe",
                Expertise = "Web Development",
                Email = "jane@example.com",
                Bio = "Senior trainer in frontend technologies."
            };

            var course = new Course
            {
                Title = "Intro to Blazor",
                Description = "Learn how to build hybrid apps with Blazor.",
                Category = "Programming",
                Duration = 10,
                Level = "Beginner",
                Trainer = trainer
            };

            context.Trainers.Add(trainer);
            context.Courses.Add(course);
            context.SaveChanges();
        }
    }
}
