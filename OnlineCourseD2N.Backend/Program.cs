using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OnlineCourseD2N.Backend.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SETUP DASAR (Controller, JSON, CORS)
// ==========================================
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ==========================================
// 2. DATABASE
// ==========================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 3. SETUP JWT (DENGAN PENGAMAN CRASH) 🛡️
// ==========================================

// A. Ambil Token dari appsettings dengan aman
var jwtKey = builder.Configuration.GetSection("AppSettings:Token").Value;

// B. Cek: Apakah Token Kosong/Null?
if (string.IsNullOrEmpty(jwtKey))
{
    // Kalau kosong, kita isi pakai key darurat biar gak crash
    Console.WriteLine("⚠️ WARNING: Token tidak ditemukan di appsettings! Menggunakan kunci fallback.");
    jwtKey = "kunci_darurat_sementara_yang_sangat_panjang_sekali_agar_tidak_error_saat_dev_mode_12345_minimal_512_bit";
}

// C. Daftarkan Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // 👇 Pakai variabel 'jwtKey' yang sudah kita amankan di atas
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// ==========================================
// 4. SWAGGER
// ==========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================
// 5. SETUP FOLDER UPLOAD
// ==========================================
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/uploads"
});

// ==========================================
// 6. AUTO SEEDING DB (DENGAN TRY-CATCH)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // Pastikan DB ada
        SeedData.Initialize(context);     // Isi Data
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saat Seeding DB: {ex.Message}");
    }
}

// ==========================================
// 7. PIPELINE REQUEST
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Urutan Wajib: Auth -> Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();