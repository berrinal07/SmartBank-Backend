# SmartBank Backend

SmartBank, temel bankacılık işlemlerini simüle etmek amacıyla geliştirilmiş bir backend projesidir.

Backend tarafı **ASP.NET Core Web API**, **Entity Framework Core** ve **PostgreSQL** kullanılarak geliştirilmiştir. Proje; kullanıcı yönetimi, kimlik doğrulama, hesap yönetimi, para transferleri, işlem geçmişi ve bildirim gibi temel bankacılık işlemlerini desteklemektedir.

##  Özellikler

* Kullanıcı  giriş işlemleri
* JWT Access Token ve Refresh Token authentication
* BCrypt ile şifre hashleme
* Kullanıcı profil görüntüleme 
* Şifre değiştirme
* Vadesiz hesap oluşturma
* Vadeli hesap oluşturma
* Hesap listeleme
* Hesap detaylarını görüntüleme
* Benzersiz hesap numarası oluşturma
* Hesaplar arası para transferi
* Kullanıcılar arası para transferi
* Yetersiz bakiye kontrolü
* Geçersiz hesap kontrolü
* İşlem geçmişi
* Pagination destekli işlem listeleme
* Transfer sonrası bildirim oluşturma
* Bildirimleri listeleme

##  Kullanılan Teknolojiler

* .NET 10
* ASP.NET Core Web API
* C#
* Entity Framework Core
* PostgreSQL 17
* Npgsql
* JWT
* BCrypt
* FluentValidation

##  Mimari

Proje katmanlı bir mimari kullanmaktadır.

```text
SmartBank.API
       ↓
SmartBank.Application
       ↓
SmartBank.Domain
       ↑
SmartBank.Infrastructure
       ↓
PostgreSQL
```

### SmartBank.API

API'nin dış dünyaya açılan katmanıdır.

* Controllers
* HTTP request/response işlemleri
* Dependency Injection
* API yapılandırması

### SmartBank.Application

Uygulamanın iş mantığının bulunduğu katmandır.

* Services
* Interfaces
* DTOs
* Business logic

### SmartBank.Domain

Uygulamanın temel domain yapısını içerir.

* Entities
* Enums
* Domain modelleri

### SmartBank.Infrastructure

Veritabanı ve dış kaynaklarla iletişimden sorumlu katmandır.

* Entity Framework Core
* AppDbContext
* Repository'ler
* PostgreSQL bağlantısı
* Migration'lar

##  Proje Yapısı

```text
SmartBank-Backend/
│
├── SmartBank.slnx
│
├── src/
│   ├── SmartBank.API/
│   │   ├── Controllers/
│   │   └── Program.cs
│   │
│   ├── SmartBank.Application/
│   │   ├── Auth/
│   │   ├── DTOs/
│   │   └── ...
│   │
│   ├── SmartBank.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── ...
│   │
│   └── SmartBank.Infrastructure/
│       ├── Data/
│       ├── Repositories/
│       └── ...
│
└── .gitignore
```

##  Veritabanı

SmartBank backend tarafında **PostgreSQL 17** kullanılmaktadır.

Ana veritabanı:

```text
smartbank_db
```

Temel tablolar:

```text
Users
Accounts
Transactions
Notifications
__EFMigrationsHistory
```

Temel ilişkiler:

```text
User
 ├── Accounts
 │      └── Transactions
 │
 └── Notifications
```

##  Authentication

Kimlik doğrulama JWT tabanlı olarak gerçekleştirilmektedir.

Authentication akışı:

```text
Register
   ↓
BCrypt ile şifre hashleme
   ↓
Login
   ↓
Access Token + Refresh Token
   ↓
Yetkilendirilmiş API istekleri
   ↓
Access Token süresi dolarsa
   ↓
Refresh Token
   ↓
Yeni Access Token
```

##  Hesap Yönetimi

Sistem iki farklı hesap türünü desteklemektedir:

* Vadesiz Hesap
* Vadeli Hesap

Her hesap için benzersiz bir hesap numarası oluşturulur.

Kullanıcılar:

* Yeni hesap oluşturabilir
* Hesaplarını listeleyebilir
* Hesap detaylarını görüntüleyebilir
* Birden fazla hesaba sahip olabilir

##  Para Transferleri

Transfer sistemi hem kullanıcının kendi hesapları arasında hem de farklı kullanıcıların hesapları arasında transfer yapılmasını desteklemektedir.

Transfer sırasında:

* Gönderen hesap kontrol edilir
* Alıcı hesap kontrol edilir
* Bakiye kontrol edilir
* Transfer tutarı kontrol edilir
* İşlem kaydı oluşturulur
* İlgili hesap bakiyeleri güncellenir
* Bildirim oluşturulur

##  İşlem Geçmişi

Kullanıcıların gerçekleştirdiği transfer işlemleri Transaction kayıtları üzerinden tutulmaktadır.

İşlem geçmişi listeleme işlemi pagination desteği ile gerçekleştirilmektedir.

##  Bildirimler

Transfer işlemleri sonrasında kullanıcılar için bildirim oluşturulmaktadır.

Kullanıcılar:

* Bildirimlerini listeleyebilir
* Okunmamış bildirimlerini görebilir
* Bildirimleri okundu olarak işaretleyebilir

##  Kullanıcı Profili

Kullanıcılar kendi profil bilgilerini görüntüleyebilir ve güncelleyebilir.

Desteklenen işlemler:

* Ad güncelleme
* Soyad güncelleme
* E-posta güncelleme
* Telefon numarası güncelleme
* Şifre değiştirme

##  Gereksinimler

Projeyi çalıştırmak için aşağıdakilerin kurulu olması gerekir:

* .NET 10 SDK
* PostgreSQL 17
* Git

.NET sürümünü kontrol etmek için:

```bash
dotnet --version
```

##  Kurulum

Repository'yi klonlayın:

```bash
git clone https://github.com/berrinal07/SmartBank-Backend.git
```

Proje klasörüne girin:

```bash
cd SmartBank-Backend
```

NuGet paketlerini yükleyin:

```bash
dotnet restore
```

##  PostgreSQL Yapılandırması

PostgreSQL üzerinde `smartbank_db` isimli bir database oluşturulmalıdır.

Uygulamanın connection string'i kendi PostgreSQL bilgilerinizle yapılandırılmalıdır.

> Gerçek şifreler, connection string içerisindeki hassas bilgiler veya JWT secret gibi gizli bilgiler GitHub'a gönderilmemelidir.

##  Database Migration

Mevcut migration'ları veritabanına uygulamak için:

```bash
dotnet ef database update
```

Yeni migration oluşturmak için:

```bash
dotnet ef migrations add MigrationName
```

##  Uygulamayı Çalıştırma

Backend'i çalıştırmak için:

```bash
dotnet run --project src/SmartBank.API
```

API çalıştıktan sonra uygulamanın kullandığı API adresi üzerinden endpointlere erişilebilir.

##  Test

Backend geliştirme sürecinde API endpointleri manuel olarak test edilmiştir.

Test edilen temel işlemler:

* Register
* Login
* Token refresh
* Account oluşturma
* Account listeleme
* Account detay
* Para transferi
* İşlem geçmişi
* Bildirimler
* Profil güncelleme
* Şifre değiştirme

##  Git Branch Strategy

Projede iki temel branch kullanılmaktadır:

```text
main
  ↑
develop
```

### main

Stabil sürüm için kullanılmaktadır.

### develop

Aktif geliştirme branch'idir.

Yeni geliştirmeler öncelikle `develop` branch'i üzerinde yapılmaktadır.

##  Mobile Application

SmartBank'in Flutter ile geliştirilmiş mobil uygulaması ayrı bir repository'de bulunmaktadır.

**SmartBank Mobile**

Mobil uygulama backend API ile iletişim kurmaktadır.

##  Proje Durumu

SmartBank backend tarafında aşağıdaki temel modüller geliştirilmiştir:

* Authentication
* User / Profile Management
* Account Management
* Transfers
* Transactions
* Notifications
