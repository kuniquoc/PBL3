# 🌊 Da Nang Tourism - Server API

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)

**🏖️ Một hệ thống hỗ trợ khách du lịch khám phá vẻ đẹp Đà Nẵng**

[Tổng quan](#-tổng-quan) •
[Tính năng](#-tính-năng-chính) •
[Cài đặt](#-cài-đặt) •
[API Documentation](#-api-documentation) •
[Cấu trúc dự án](#-cấu-trúc-dự-án)

</div>

---

## 🌟 Tổng quan

**Da Nang Tourism Server** là một RESTful API được xây dựng với ASP.NET Core 8.0, cung cấp các dịch vụ backend cho ứng dụng du lịch Đà Nẵng. Hệ thống hỗ trợ quản lý điểm đến, lịch trình du lịch, blog du lịch, đánh giá và hệ thống xác thực người dùng.

### 🎯 Mục tiêu dự án
- Xây dựng nền tảng thông tin du lịch toàn diện cho thành phố Đà Nẵng
- Cung cấp API mạnh mẽ và bảo mật cho ứng dụng web và mobile
- Hỗ trợ quản lý lịch trình du lịch cá nhân hóa
- Tạo cộng đồng chia sẻ kinh nghiệm du lịch

## 🚀 Tính năng chính

### 🔐 Hệ thống Authentication & Authorization
- **JWT Token Authentication** với refresh token
- **Role-based access control** (Admin/User)
- **Email verification** cho đăng ký tài khoản
- **Password reset** qua email
- **Session management** với cookie security

### 🏞️ Quản lý Điểm đến (Destinations)
- **CRUD operations** cho destinations
- **Advanced filtering** và search
- **Image upload** với Cloudinary integration
- **Rating system** và reviews
- **Favorite destinations** cho users
- **Admin management** panel

### 📅 Hệ thống Lịch trình (Schedules)
- **Tạo và quản lý** lịch trình cá nhân
- **Clone schedule** từ other users
- **Public/Private** schedule sharing
- **Multi-day itinerary** planning
- **Time-based** activity scheduling
- **Collaborative** trip planning

### 📝 Blog & Content Management
- **Rich text editor** support
- **Category management** (Places, Tips, All)
- **Content moderation** (Pending, Published, Rejected)
- **SEO optimization** với meta tags
- **View tracking** và analytics
- **User-generated content**

### ⭐ Review & Rating System
- **5-star rating** system
- **Detailed reviews** với text content
- **Review moderation**
- **Aggregate ratings** cho destinations
- **User review history**

### 📧 Email Services
- **SMTP integration** với Gmail
- **Template-based emails**
- **Confirmation codes**
- **Password reset emails**
- **Notification system**

## 🛠️ Cài đặt

### Yêu cầu hệ thống
- **.NET 8.0 SDK** hoặc cao hơn
- **MySQL 8.0** hoặc MariaDB
- **Visual Studio 2022** hoặc VS Code
- **Git** cho version control

### 1. Clone repository
```bash
git clone https://github.com/your-username/danang-tourism.git
cd danang-tourism/DaNangTourism/DaNangTourism.Server
```

### 2. Cấu hình Database
```bash
# Import database schema
mysql -u root -p < ../../DatabaseScript/pbl3.sql
```

### 3. Cấu hình appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=pbl3;uid=root;password=your_password;ConvertZeroDateTime=True"
  },
  "GmailPassword": "your_gmail_app_password",
  "Secret": "your_jwt_secret_key_here",
  "AllowedHosts": "*"
}
```

### 4. Cài đặt dependencies
```bash
dotnet restore
```

### 5. Chạy ứng dụng
```bash
dotnet run
```

Server sẽ chạy tại: `https://localhost:7295` hoặc `http://localhost:5173`

## 📚 API Documentation

### 🔑 Authentication Endpoints

| Method | Endpoint | Mô tả | Auth Required |
|--------|----------|-------|---------------|
| `POST` | `/auth/login` | Đăng nhập | ❌ |
| `POST` | `/auth/register` | Đăng ký tài khoản | ❌ |
| `GET` | `/auth/logout` | Đăng xuất | ✅ |
| `GET` | `/auth/authenticated` | Kiểm tra auth status | ✅ |
| `PUT` | `/auth/update/password` | Đổi mật khẩu | ✅ |
| `POST` | `/auth/sendCode` | Gửi mã xác nhận | ❌ |
| `POST` | `/auth/resetPassword` | Reset mật khẩu | ❌ |

### 🏞️ Destination Endpoints

| Method | Endpoint | Mô tả | Auth Required |
|--------|----------|-------|---------------|
| `GET` | `/destination/home` | Destinations cho homepage | ❌ |
| `GET` | `/destination/list` | Danh sách destinations với filter | ❌ |
| `GET` | `/destination/detail/{id}` | Chi tiết destination | ❌ |
| `PUT` | `/destination/favorite` | Toggle favorite destination | ✅ |
| `GET` | `/destination/random` | Random destinations | ❌ |
| `GET` | `/destination/managelist` | Admin: Quản lý destinations | 👑 |
| `POST` | `/destination/create` | Admin: Tạo destination | 👑 |
| `PUT` | `/destination/update/{id}` | Admin: Cập nhật destination | 👑 |
| `DELETE` | `/destination/delete/{id}` | Admin: Xóa destination | 👑 |

### 📅 Schedule Endpoints

| Method | Endpoint | Mô tả | Auth Required |
|--------|----------|-------|---------------|
| `GET` | `/schedule/mySchedule` | Lịch trình của user | ✅ |
| `GET` | `/schedule/sharedSchedule` | Lịch trình public | ❌ |
| `GET` | `/schedule/detail/{id}` | Chi tiết lịch trình | ✅ |
| `POST` | `/schedule/create` | Tạo lịch trình mới | ✅ |
| `POST` | `/schedule/clone/{id}` | Clone lịch trình | ✅ |
| `PUT` | `/schedule/update/{id}` | Cập nhật lịch trình | ✅ |
| `DELETE` | `/schedule/delete/{id}` | Xóa lịch trình | ✅ |
| `POST` | `/schedule/addDestination` | Thêm destination vào lịch trình | ✅ |
| `PUT` | `/schedule/updateDestination/{id}` | Cập nhật destination trong lịch trình | ✅ |
| `DELETE` | `/schedule/removeDestination/{id}` | Xóa destination khỏi lịch trình | ✅ |

### 📝 Blog Endpoints

| Method | Endpoint | Mô tả | Auth Required |
|--------|----------|-------|---------------|
| `GET` | `/blog/home` | Top 5 blogs có nhiều view nhất | ❌ |
| `GET` | `/blog/list` | Danh sách blogs với pagination | ❌ |
| `GET` | `/blog/detail/{id}` | Chi tiết blog | ❌ |
| `GET` | `/blog/random` | Random blogs | ❌ |
| `POST` | `/blog/create` | Tạo blog mới | ✅ |
| `PUT` | `/blog/update/{id}` | Cập nhật blog | ✅ |
| `DELETE` | `/blog/delete/{id}` | Xóa blog | ✅ |
| `GET` | `/blog/myBlogs` | Blogs của user | ✅ |
| `GET` | `/blog/managelist` | Admin: Quản lý tất cả blogs | 👑 |
| `PUT` | `/blog/updateStatus/{id}` | Admin: Cập nhật trạng thái blog | 👑 |

### ⭐ Review Endpoints

| Method | Endpoint | Mô tả | Auth Required |
|--------|----------|-------|---------------|
| `GET` | `/review/list/{destinationId}` | Reviews của destination | ❌ |
| `POST` | `/review/create` | Tạo review mới | ✅ |
| `DELETE` | `/review/delete/{id}` | Xóa review | ✅ |

### 👤 Account Endpoints

| Method | Endpoint | Mô tả | Auth Required |
|--------|----------|-------|---------------|
| `GET` | `/account/get` | Thông tin tài khoản | ✅ |
| `PUT` | `/account/update` | Cập nhật thông tin | ✅ |
| `GET` | `/account/get/list` | Admin: Danh sách users | 👑 |
| `PUT` | `/account/update/role/{id}` | Admin: Cập nhật role | 👑 |
| `DELETE` | `/account/delete/{id}` | Admin: Xóa user | 👑 |

### 📊 Response Format

#### Success Response
```json
{
  "message": "Success",
  "data": {
    // Response data here
  }
}
```

#### Error Response
```json
{
  "message": "Error description"
}
```

### 🔍 Query Parameters

#### Pagination
```
?page=1&limit=10
```

#### Filtering (Destinations)
```
?search=beach&category=nature&minPrice=0&maxPrice=1000&sortBy=rating&sortType=desc
```

#### Filtering (Blogs)
```
?search=tips&type=places&page=1&limit=10
```

## 🏗️ Cấu trúc dự án

```
DaNangTourism.Server/
├── 📁 Controllers/          # API Controllers
│   ├── AuthController.cs      # Authentication & Authorization
│   ├── DestinationController.cs # Destinations management
│   ├── ScheduleController.cs   # Trip scheduling
│   ├── BlogController.cs      # Blog & content management
│   ├── ReviewController.cs    # Reviews & ratings
│   └── AccountController.cs   # User account management
├── 📁 Services/             # Business Logic Layer
│   ├── AccountService.cs      # User & auth services
│   ├── DestinationService.cs  # Destination business logic
│   ├── ScheduleService.cs     # Schedule management
│   ├── BlogService.cs         # Blog operations
│   ├── ReviewService.cs       # Review processing
│   └── EmailService.cs        # Email notifications
├── 📁 DAL/                  # Data Access Layer
│   ├── AccountRepository.cs   # User data access
│   ├── DestinationRepository.cs # Destination data
│   ├── ScheduleRepository.cs  # Schedule data
│   ├── BlogRepository.cs      # Blog data access
│   └── ReviewRepository.cs    # Review data
├── 📁 Models/               # Data Models
│   ├── Account.cs             # User model
│   ├── Blog.cs                # Blog model
│   ├── DestinationModels/     # Destination DTOs
│   ├── ScheduleModels/        # Schedule DTOs
│   ├── ReviewModels/          # Review DTOs
│   └── SecurityModels/        # Auth models
├── 📁 Helper/               # Utility classes
│   └── DataSanitization.cs    # Input validation
├── 📁 ModelBindingConverter/ # Custom converters
├── 📄 Program.cs            # Application entry point
└── 📄 appsettings.json      # Configuration
```

## 🔧 Technologies & Architecture

### Backend Framework
- **ASP.NET Core 8.0** - Web API framework
- **Entity Framework Core** - ORM (Custom implementation)
- **MySQL Connector** - Database connectivity
- **JWT Authentication** - Stateless authentication
- **Swagger/OpenAPI** - API documentation

### Database Design
- **MySQL 8.0** - Relational database
- **Normalized schema** with foreign key constraints
- **Indexing** for performance optimization
- **Stored procedures** for complex operations

### Security Features
- **JWT Token** authentication with expiration
- **Password hashing** with HMAC-SHA512
- **CORS** configuration for cross-origin requests
- **Input validation** and sanitization
- **Role-based authorization**

### Design Patterns
- **Repository Pattern** - Data access abstraction
- **Service Layer Pattern** - Business logic separation
- **Dependency Injection** - Loose coupling
- **DTO Pattern** - Data transfer objects
- **Factory Pattern** - Object creation

## 🌐 CORS Configuration

Server được cấu hình để chấp nhận requests từ:
- `http://localhost:5173` (Vite dev server)
- Tất cả headers và methods được allowed

## 🔐 Security Notes

### JWT Configuration
- **Algorithm**: HS256
- **Expiration**: Configurable (default: 7 days for remember me)
- **Secret**: Stored in appsettings.json (change in production!)

### Password Security
- **Hashing**: HMAC-SHA512 with salt
- **Minimum requirements**: 6-20 characters, letters and numbers
- **Password reset**: Secure random generation

### Email Security
- **Gmail SMTP** integration
- **App passwords** required for Gmail
- **Rate limiting** for code sending

## 📈 Performance Optimization

- **Connection pooling** for database
- **Async/await** patterns throughout
- **Efficient queries** with proper indexing
- **Caching** strategies for frequently accessed data
- **Pagination** for large datasets

## 🚀 Deployment

### Environment Variables
```bash
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DefaultConnection="production_connection_string"
export GmailPassword="production_gmail_password"
export Secret="production_jwt_secret"
```

### Docker Support (Coming Soon)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
# Docker configuration will be added
```

## 🤝 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Team

Developed by **PBL3 Team** as part of university coursework.

---

<div align="center">

**🌊 Khám phá vẻ đẹp Đà Nẵng cùng chúng tôi! 🏖️**

*Made with ❤️ in Vietnam*

</div>
