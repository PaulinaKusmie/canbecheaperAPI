CanBeCheaper API

REST API for the CanBeCheaper price tracking app.

📖 About
CanBeCheaper API is a RESTful backend built with .NET Core that powers the CanBeCheaper Android app. It handles user authentication (with email confirmation code), product management, price types, units, and price recording.

✨ Features

🔐 Authentication – register, login, email confirmation via code
📧 Mail Service – sends confirmation codes via email
📦 Products – create and manage products
🏷️ Types – define custom price categories
📐 Units – manage units of measurement (kg, g, l, ml, pcs...)
💲 Product Prices – record and retrieve prices per product
📊 Min / Max prices – automatically track cheapest and most expensive entries


🛠️ Tech Stack
TechnologyVersionPurpose.NET Core8.0FrameworkASP.NET Core Web API8.0REST APIEntity Framework Core8.0ORMPomelo MySQL8.0MySQL EF Core driverMySQL8.0Database (DbOliwia230Context)JWT Bearer-AuthenticationSwagger / Swashbuckle-API documentationBCrypt.Net-Password hashingMailService-Email confirmation codes


📁 Project Structure
```
canbecheaperAPI/
├── DTO/
│   ├── Price/
│   │   └── PriceRequest.cs
│   ├── Product/
│   │   ├── ProductDTO.cs
│   │   └── ProductRequest.cs
│   ├── ProductPrice/
│   │   ├── ProductPriceRequest.cs
│   │   └── ProductPriceResponse.cs
│   ├── Type/
│   │   ├── TypeRequest.cs
│   │   └── TypeResponse.cs
│   ├── Unit/
│   │   ├── UnitRequest.cs
│   │   └── UnitResponse.cs
│   └── User/
│       ├── ConfirmCodeRequest.cs
│       ├── LoginRequest.cs
│       ├── RegisterRequest.cs
│       └── UserResponse.cs
├── Endpoints/
│   ├── PriceEndpoints.cs
│   ├── ProductEndpoints.cs
│   ├── ProductPriceEndpoints.cs
│   ├── TypeEndpoints.cs
│   ├── UnitEndpoints.cs
│   └── UserEndpoints.cs
├── Models/
│   ├── CheaperPrice.cs
│   ├── CheaperProduct.cs
│   ├── CheaperProductPrice.cs
│   ├── CheaperType.cs
│   ├── CheaperUnit.cs
│   ├── CheaperUser.cs
│   └── DbOliwia230Context.cs
├── Utility/
│   ├── CodeGenerator.cs
│   ├── MailService.cs
│   └── SendMail.cs
├── appsettings.json
└── Program.cs
