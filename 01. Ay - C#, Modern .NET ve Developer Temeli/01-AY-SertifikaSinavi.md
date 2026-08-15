# 1. Ay Sertifika Sınavı

## S1. `dotnet new console` komutu ne işe yarar?

Yeni bir Console Application projesi oluşturur.

```bash
dotnet new console -n MyApp
```

`MyApp` isminde çalıştırılabilir bir .NET console projesi oluşturur.

## S2. `dotnet build` ile `dotnet run` arasındaki fark nedir?

`dotnet build` projeyi derler, hataları kontrol eder ve çıktı üretir.

```bash
dotnet build
```

`dotnet run` ise projeyi derler ve ardından çalıştırır.

```bash
dotnet run
```

Kısaca:

- `build` = derle
- `run` = derle + çalıştır

## S3. `.csproj` dosyasının temel görevi nedir?

Projenin yapılandırma dosyasıdır.

Örneğin şunları tutar:

```xml
<TargetFramework>net8.0</TargetFramework>
```

Ayrıca NuGet paketleri ve diğer proje referansları burada bulunur.

> **Özet:** `.csproj` = Bu proje nasıl derlenecek ve nelere bağımlı?

## S4. Solution `.sln` dosyası ne işe yarar?

Birden fazla projeyi tek çatı altında toplar.

Örneğin:

```text
MySolution
├── MyApp.Api
├── MyApp.Application
├── MyApp.Domain
└── MyApp.Infrastructure
```

`.sln`, bu projeleri Visual Studio ve `dotnet` araçlarıyla birlikte yönetmemizi kolaylaştırır.

## S5. Bir projeye başka bir project referansı eklemek için hangi komut kullanılır?

```bash
dotnet add reference ../OtherProject/OtherProject.csproj
```

Örneğin:

```bash
dotnet add MyApp.Api reference MyApp.Application
```

Böylece Api projesi Application projesindeki class'ları kullanabilir.

## S6. Value type ve reference type farkı nedir?

**Value Type**, değişkenin değerini taşır.

```csharp
int a = 10;
int b = a;

b = 20;
```

`a` hâlâ `10` olur.

**Reference Type** ise nesneye olan referansı taşır.

```csharp
var user1 = new User();
var user2 = user1;

user2.Name = "Ali";
```

İki değişken de aynı nesneyi gösterdiği için `user1.Name` de `"Ali"` olur.

Örnekler:

| Value Type | Reference Type |
|---|---|
| `int` | `class` |
| `bool` | `string` |
| `decimal` | `array` |
| `struct` | |

## S7. `if-else` ile switch expression arasındaki temel fark nedir?

`if-else`, genel koşulları kontrol etmek için uygundur.

```csharp
if (age >= 18)
    result = "Adult";
else
    result = "Child";
```

Switch expression ise bir değere göre sonuç üretirken daha temiz olabilir.

```csharp
var result = role switch
{
    "Admin" => "Full Access",
    "User" => "Limited Access",
    _ => "No Access"
};
```

Özellikle bir değeri başka bir değere dönüştürürken switch expression çok kullanışlıdır.

## S8. Metotlarda parametre ve return değeri ne anlama gelir?

Parametre, metoda gönderilen bilgidir.

Return, metodun dışarı verdiği sonuçtur.

```csharp
int Sum(int a, int b)
{
    return a + b;
}
```

Burada:

- `a`, `b` → parametre
- `a + b` → return edilen sonuç

## S9. `string? name` ifadesindeki `?` neyi ifade eder?

Değişkenin `null` olabileceğini belirtir.

```csharp
string? name = null;
```

Nullable Reference Types özelliğinin bir parçasıdır.

```csharp
string name;   // null olmaması beklenir
string? name;  // null olabilir
```

## S10. `required` keyword'ü ne işe yarar?

Bir property'nin nesne oluşturulurken mutlaka verilmesini zorunlu hâle getirir.

```csharp
public class User
{
    public required string Name { get; set; }
}
```

Şu kullanım hatalıdır:

```csharp
var user = new User();
```

Şöyle verilmelidir:

```csharp
var user = new User
{
    Name = "Ali"
};
```

## S11. Class ve object arasındaki fark nedir?

Class bir şablondur.

Object ise o class'tan oluşturulan gerçek nesnedir.

```csharp
class Car
{
    public string Brand { get; set; }
}
```

Bu bir class'tır.

```csharp
var car = new Car();
```

`car` ise object'tir.

Kısaca:

- Class → Tasarım
- Object → Gerçek örnek

## S12. Constructor ne işe yarar?

Nesne oluşturulurken çalışır ve nesnenin başlangıç durumunu hazırlamak için kullanılır.

```csharp
class User
{
    public string Name { get; }

    public User(string name)
    {
        Name = name;
    }
}
```

Kullanım:

```csharp
var user = new User("Ali");
```

## S13. Encapsulation nedir?

Bir nesnenin iç durumunu kontrolsüz değişikliklerden korumaktır.

Örneğin:

```csharp
class BankAccount
{
    public decimal Balance { get; private set; }

    public void Deposit(decimal amount)
    {
        if (amount > 0)
            Balance += amount;
    }
}
```

Şunu yapamayız:

```csharp
account.Balance = -5000;
```

Değişiklik belirlenen kurallar üzerinden yapılır.

> **Encapsulation:** Veriyi saklamak değil, verinin değişimini kontrol etmek.

## S14. Class, struct ve record arasındaki temel farklar nelerdir?

**Class**, bir reference type'tır ve genel amaçlı nesnelerde kullanılır.

```csharp
class User
{
    public string Name { get; set; }
}
```

**Struct**, bir value type'tır. Genellikle küçük ve basit veri yapılarında kullanılır.

```csharp
struct Point
{
    public int X;
    public int Y;
}
```

**Record** ise özellikle veri taşıyan ve değer bazlı karşılaştırılması istenen nesneler için uygundur.

```csharp
record User(string Name, int Age);

var x = new User("Ali", 25);
var y = new User("Ali", 25);

Console.WriteLine(x == y); // true
```

## S15. Interface neden kullanılır?

Bir class'ın hangi davranışları sağlayacağını belirleyen sözleşmedir.

```csharp
interface IEmailService
{
    void Send(string message);
}
```

Implementasyon:

```csharp
class EmailService : IEmailService
{
    public void Send(string message)
    {
        Console.WriteLine(message);
    }
}
```

En önemli amacı, kodun doğrudan implementasyona değil sözleşmeye bağımlı olmasını sağlamaktır.

Bu da Dependency Injection ve test yazmayı kolaylaştırır.

## S16. `List<T>` ile `Dictionary<TKey, TValue>` arasındaki fark nedir?

`List<T>`, sıralı bir eleman koleksiyonudur.

```csharp
var users = new List<string>
{
    "Ali",
    "Ahmet"
};
```

Index ile erişilir:

```csharp
users[0];
```

Dictionary ise key-value şeklinde veri tutar.

```csharp
var users = new Dictionary<int, string>();

users.Add(1, "Ali");
```

Key ile erişilir:

```csharp
users[1];
```

Kısaca:

- List → Eleman listesi
- Dictionary → Key üzerinden hızlı erişim

## S17. LINQ'te `Where`, `Select` ve `Any` ne işe yarar?

`Where` → filtreleme yapar.

```csharp
users.Where(x => x.Age > 18);
```

`Select` → veriyi dönüştürür veya belirli alanları seçer.

```csharp
users.Select(x => x.Name);
```

`Any` → koşula uyan en az bir eleman var mı diye bakar.

```csharp
users.Any(x => x.Age > 18);
```

Sonuç:

- `Where` → Filtrele
- `Select` → Dönüştür / seç
- `Any` → Var mı?

## S18. `FirstOrDefault()` hangi durumda null dönebilir?

Reference type bir koleksiyonda aranan eleman bulunamazsa `null` dönebilir.

```csharp
var user = users.FirstOrDefault(x => x.Id == 100);
```

Eğer `Id = 100` olan kullanıcı yoksa:

```csharp
user == null
```

olabilir. Bu yüzden kontrol yapmak önemlidir:

```csharp
if (user is null)
{
    // kullanıcı bulunamadı
}
```

> **Not:** Value type'larda `null` yerine tipin default değeri dönebilir. Örneğin `int` için `0`.

## S19. `try-catch` her yere yazılmalı mı? Neden?

Hayır.

```csharp
try
{
    DoSomething();
}
catch
{
}
```

şeklinde her yeri sarmak kötü bir yaklaşımdır.

`try-catch`, hatayı gerçekten yönetebildiğimiz yerde kullanılmalıdır.

Örneğin:

```csharp
try
{
    await paymentService.PayAsync();
}
catch (PaymentException ex)
{
    // ödeme hatasını uygun şekilde yönet
}
```

Özellikle boş `catch` tehlikelidir:

```csharp
catch
{
}
```

Çünkü hata gizlenmiş olur.

> **Senior kritik:** Exception'ı sadece yakalamak değil, doğru seviyede anlamlı şekilde yönetmek önemlidir.

## S20. Git'te commit, branch ve merge ne anlama gelir?

**Commit**, yaptığımız değişikliklerin kayıt noktasıdır.

```bash
git commit -m "Add payment service"
```

**Branch**, ana koddan bağımsız çalışma alanıdır.

```bash
git switch -c feature/payment
```

Örneğin:

```text
main
 |
 |------ feature/payment
```

**Merge**, bir branch'teki değişiklikleri başka bir branch'e birleştirir.

```bash
git switch main
git merge feature/payment
```

Yani:

- Commit → Değişikliği kaydet
- Branch → Ayrı çalışma alanı oluştur
- Merge → Branch'leri birleştir

---

## Sınav İçin En Kısa Tekrar

### .NET CLI ve Proje Yapısı

- `dotnet new console` → Console projesi oluşturur
- `dotnet build` → Derler
- `dotnet run` → Derler ve çalıştırır
- `.csproj` → Proje ayarları ve bağımlılıklar
- `.sln` → Birden fazla projeyi yönetir

### Tipler ve Null Yönetimi

- Value Type → Değer kopyalanır
- Reference Type → Referans kopyalanır
- `?` → `null` olabilir
- `required` → Değer verilmesi zorunlu

### Nesne Yönelimli Programlama

- Class → Şablon
- Object → Class'ın örneği
- Constructor → Nesneyi başlangıç durumuna getirir
- Encapsulation → Nesnenin durumunu kontrollü değiştirmek
- Interface → Sözleşme

### Koleksiyonlar

- List → Eleman koleksiyonu
- Dictionary → Key-Value koleksiyonu

### LINQ

- `Where` → Filtrele
- `Select` → Dönüştür
- `Any` → Var mı?
- `FirstOrDefault` → Bulamazsa default/null

### Hata Yönetimi

- `try-catch` → Her yere değil, hatanın yönetilebildiği yere

### Git

- Commit → Kaydet
- Branch → Ayrı geliştirme hattı
- Merge → Birleştir
