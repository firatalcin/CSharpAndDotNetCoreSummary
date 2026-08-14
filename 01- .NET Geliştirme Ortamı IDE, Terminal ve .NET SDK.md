# 01. Ay — C#, Modern .NET ve Developer Temeli

Bu doküman, `01. Ay — C#, Modern .NET ve Developer Temeli.txt` içindeki 29 konunun kod destekli tekrar özetidir. Her başlık; kısa açıklama, kritik ayrıntılar, örnek ve akılda kalacak tek cümle içerir.

> **Çalışma yöntemi:** Önce özeti oku, kodu kendin yazıp çalıştır, ardından “Akılda kalsın” cümlesini kendi sözlerinle açıklamaya çalış.

## İçindekiler

1. [.NET Geliştirme Ortamı](#1-net-geliştirme-ortamı-ide-terminal-ve-net-sdk)
2. [.NET Proje Yapısı](#2-net-proje-yapısı-solution-project-ve-csproj)
3. [.NET CLI ve Proje Yaşam Döngüsü](#3-net-cli-ve-proje-yaşam-döngüsü)
4. [Git Temelleri ve Commit İş Akışı](#4-git-temelleri-ve-commit-iş-akışı)
5. [AI Destekli Geliştirme ve Prompt](#5-ai-destekli-yazılım-geliştirme-ve-etkili-prompt)
6. [Değişkenler, Veri Tipleri ve Operatörler](#6-değişkenler-veri-tipleri-ve-operatörler)
7. [Koşullu İfadeler](#7-koşullu-ifadeler-if-else-switch)
8. [Döngüler](#8-döngüler-for-foreach-while-do-while)
9. [Metotlar](#9-metotlar-parametreler-ve-dönüş-değerleri)
10. [Null Yönetimi](#10-null-yönetimi-ve-nullable-reference-types)
11. [Class ve Object](#11-class-ve-object-temelleri)
12. [Field, Property ve Constructor](#12-field-property-ve-constructor)
13. [Encapsulation](#13-encapsulation-ve-nesne-durumunun-korunması)
14. [Referans ve Değer Tipleri](#14-referans-ve-değer-tipleri-class-ve-struct)
15. [Record ve Immutable Tasarım](#15-record-ve-immutable-nesne-tasarımı)
16. [Interface](#16-interface-ve-sözleşme-tabanlı-tasarım)
17. [Abstract Class, Interface ve Kalıtım](#17-abstract-class-interface-ve-kalıtım)
18. [Dependency Injection](#18-dependency-injection-ve-bağımlılık-yönetimi)
19. [Array ve List](#19-array-ve-listt)
20. [Dictionary ve HashSet](#20-dictionary-ve-hashset)
21. [LINQ: Filtreleme ve Dönüştürme](#21-linq-ile-filtreleme-ve-veri-dönüştürme)
22. [LINQ: Arama ve Varlık Kontrolü](#22-linq-ile-eleman-arama-ve-varlık-kontrolü)
23. [LINQ: Sıralama, Gruplama ve Toplama](#23-linq-ile-sıralama-gruplama-ve-toplama)
24. [Exception Handling](#24-exception-handling-try-catch-finally)
25. [Custom Exception ve Uygulama Hataları](#25-custom-exception-ve-uygulama-hataları)
26. [Clean Code](#26-clean-code-temelleri)
27. [Refactoring ve Sorumlulukların Ayrılması](#27-refactoring-ve-sorumlulukların-ayrılması)
28. [Git Branch, Merge ve Conflict](#28-git-branch-merge-ve-conflict-yönetimi)
29. [Pull Request ve Code Review](#29-pull-request-code-review-ve-ekip-kültürü)

---

## 1. .NET Geliştirme Ortamı: IDE, Terminal ve .NET SDK

IDE kodu rahatça yazıp yönetmemizi sağlayan arayüzdür. Terminal, işlemleri komutlarla yürüttüğümüz ortamdır. .NET SDK ise derleyici, CLI, proje şablonları ve build araçlarını içeren gerçek geliştirme setidir. Runtime yalnızca hazırlanmış uygulamayı çalıştırır; SDK geliştirme ve yayınlama araçlarını da içerir.

**Kritik noktalar**

- Visual Studio veya Rider arka planda SDK ve MSBuild kullanır.
- Terminal bilgisi; CI/CD, Docker ve otomasyonun ortak dilidir.
- Ekipte aynı SDK'yı seçmek için `global.json` kullanılabilir.
- `dotnet --info`, yüklü SDK/runtime ve ortam bilgisini gösterir.

```powershell
dotnet --list-sdks
dotnet --list-runtimes
dotnet --info
dotnet new console -n HelloDotNet
dotnet run --project HelloDotNet
```

> **Akılda kalsın:** IDE direksiyon, terminal vites, SDK ise motordur; uygulamayı asıl üreten SDK'dır.

## 2. .NET Proje Yapısı: Solution, Project ve `.csproj`

Solution (`.sln`) projeleri organize eder; doğrudan derlenen bir uygulama değildir. Project, derlenebilir birimdir. `.csproj` ise hedef framework'ü, paketleri, proje referanslarını ve derleme davranışını tanımlayan sözleşmedir.

```text
Shop.sln
├── Shop.Api/Shop.Api.csproj
├── Shop.Application/Shop.Application.csproj
├── Shop.Domain/Shop.Domain.csproj
└── Shop.Tests/Shop.Tests.csproj
```

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\Shop.Domain\Shop.Domain.csproj" />
  </ItemGroup>
</Project>
```

Bağımlılık yönü mimariyi belirler: Domain'in Infrastructure'ı bilmesi yerine Infrastructure'ın Domain sözleşmelerini uygulaması tercih edilir. Her klasörü proje yapmak gerekmez; ayrı derleme, bağımlılık, test veya deployment sınırı varsa yeni proje anlamlıdır.

> **Akılda kalsın:** Solution raf, project kitap, `.csproj` kitabın kimlik ve üretim kartıdır.

## 3. .NET CLI ve Proje Yaşam Döngüsü

CLI, bir projenin oluşturulmasından yayına kadar tekrar üretilebilir bir süreç kurar. Temel sıra `new → restore → build → test → run/publish` şeklindedir.

```powershell
dotnet new sln -n Shop
dotnet new webapi -n Shop.Api
dotnet sln Shop.sln add Shop.Api/Shop.Api.csproj

dotnet restore Shop.sln
dotnet build Shop.sln -c Release --no-restore
dotnet test Shop.sln -c Release --no-build
dotnet publish Shop.Api -c Release --no-build -o ./publish
```

- `restore`: NuGet bağımlılıklarını hazırlar.
- `build`: Kaynak kodu derler; davranışın doğru olduğunu kanıtlamaz.
- `test`: Beklenen davranışı doğrular.
- `publish`: Uygulamayı hedef ortamda dağıtılabilir hale getirir.
- `--no-restore` ve `--no-build`, önceki adımlar kontrollü yapıldıysa gereksiz tekrarı önler.

> **Akılda kalsın:** Build “yazım doğru mu?”, test “davranış doğru mu?”, publish “taşınmaya hazır mı?” sorusunu cevaplar.

## 4. Git Temelleri ve Commit İş Akışı

Git üç alanla düşünülür: **Working Directory → Staging Area → Repository**. Dosyayı değiştirmek, commit'e seçmek ve kalıcı geçmişe kaydetmek ayrı işlemlerdir.

```powershell
git status                 # Ne değişti?
git diff                   # Değişiklik ne?
git add -p                 # Commit'e hangi parçalar girecek?
git diff --staged          # Seçilen değişiklik doğru mu?
git commit -m "fix: reject negative invoice amount"
git push
```

İyi commit küçük, tek amaçlı, anlaşılır ve gerektiğinde bağımsız geri alınabilir olmalıdır. `git add .` istemediğin dosyaları da ekleyebilir. Commit yalnızca yerel geçmişe kaydeder; push uzak repoya gönderir. Paylaşılmış branch'lerde `reset`, `rebase`, `amend` ve force push geçmişi değiştirebileceği için dikkat ister.

> **Akılda kalsın:** `add` kaydetmez, seçer; `commit` yerelde kaydeder; `push` paylaşır.

## 5. AI Destekli Yazılım Geliştirme ve Etkili Prompt

AI; analiz, kod, test, refactoring ve dokümantasyon hızını artırır ancak teknik kararın ve doğrulamanın sahibi geliştiricidir. Etkili prompt formülü: **Rol + Bağlam + Görev + Kısıt + Çıktı + Kabul kriteri**.

```text
Rol: Senior .NET geliştiricisi gibi davran.
Bağlam: ASP.NET Core + EF Core kullanılan fatura API'si.
Görev: Aşağıdaki metotta null, performans ve concurrency risklerini bul.
Kısıt: Public API'yi ve iş davranışını değiştirme.
Çıktı: Önce risk tablosu, sonra küçük bir patch, sonra testler.
Kabul: Proje derlenmeli; başarı, kayıt yok ve iptal senaryoları test edilmeli.
```

AI çıktısını şu döngüden geçir: **anla → diff'i incele → build → test → güvenlik/performans kontrolü → commit**. Büyük görevleri küçük, doğrulanabilir adımlara ayır. Nedenini açıklayamadığın kodu production'a gönderme; uydurulmuş API ve paketlere özellikle dikkat et.

> **Akılda kalsın:** AI önerir, geliştirici doğrular ve sorumluluğu üstlenir.

## 6. Değişkenler, Veri Tipleri ve Operatörler

Veri tipi yalnızca bellekteki biçim değil, iş kuralının bir parçasıdır. Para için çoğunlukla `decimal`, kimlik için uygun tür, sınırlı durumlar için `enum`, evet/hayır için `bool` kullanılır.

```csharp
decimal unitPrice = 149.90m;
int quantity = 3;
decimal total = unitPrice * quantity;

int completed = 3;
int all = 4;
decimal completionRate = (decimal)completed / all; // 0.75; integer division yok

bool canShip = quantity > 0 && total > 0m; // Sağ taraf gerekirse çalışır
```

- `10 / 4` sonucu `int` türünde `2` olur; ondalık sonuç için operandlardan biri dönüştürülmelidir.
- `&&` ve `||` kısa devre yapar.
- Para hesabında yuvarlama kuralını açıkça belirle: `Math.Round(value, 2, MidpointRounding.AwayFromZero)` gibi.
- `?? 0` kullanmadan önce null'ın geçerli bir durum mu veri hatası mı olduğunu sor.

> **Akılda kalsın:** Doğru tip, yanlış durumların daha kod çalışmadan temsil edilmesini zorlaştırır.

## 7. Koşullu İfadeler: `if`, `else`, `switch`

Koşullar iş kurallarını görünür biçimde dallandırır. Karmaşık koşulları isimlendir, geçersiz girdileri guard clause ile erken durdur ve değer üretirken switch expression kullanmayı düşün.

```csharp
static decimal CalculateDiscount(Customer customer, decimal total)
{
    ArgumentNullException.ThrowIfNull(customer);
    if (total < 0) throw new ArgumentOutOfRangeException(nameof(total));

    bool isLargeVipOrder = customer.IsVip && total >= 1_000m;

    return customer.Level switch
    {
        CustomerLevel.Gold when isLargeVipOrder => total * 0.15m,
        CustomerLevel.Gold                      => total * 0.10m,
        CustomerLevel.Silver                    => total * 0.05m,
        _                                       => 0m
    };
}
```

Beklenmeyen değerlerde körlemesine boş/default sonuç vermek hatayı gizleyebilir. Aynı switch birçok yerde büyüyorsa polymorphism, Strategy veya ayrı handler'lar tasarım açısından daha uygun olabilir.

> **Akılda kalsın:** Guard clause yolu temizler; switch seçimi görünür kılar.

## 8. Döngüler: `for`, `foreach`, `while`, `do-while`

İndeks gerekiyorsa `for`, koleksiyon elemanları için `foreach`, tekrar sayısı belirsizse `while`, işlemin en az bir kez çalışması gerekiyorsa `do-while` kullanılır. Her döngünün bitiş koşulu açık olmalıdır.

```csharp
foreach (var invoice in invoices)
{
    if (!invoice.IsValid) continue;
    await sender.SendAsync(invoice, cancellationToken);
}

while (!cancellationToken.IsCancellationRequested)
{
    var message = await queue.TryReadAsync(cancellationToken);
    if (message is not null)
        await HandleAsync(message, cancellationToken);

    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
}
```

Döngünün maliyeti çoğu zaman gövdesindedir: her eleman için SQL/API çağrısı N+1 problemine dönüşebilir. Kontrolsüz `Task.WhenAll`, rate limit'i aşabilir; `DbContext` thread-safe değildir. Büyük üyelik kontrollerinde `List.Contains` yerine `HashSet` düşünülebilir.

> **Akılda kalsın:** Döngü yazarken yalnız “kaç kez?” değil, “nasıl bitecek ve her tur neye mal olacak?” diye sor.

## 9. Metotlar, Parametreler ve Dönüş Değerleri

Metodun adı, girdileri ve çıktısı onun sözleşmesidir. Query bilgi döndürür; command durum değiştirir. İsim, yan etkiyi gizlememelidir.

```csharp
public sealed record CreateInvoiceResult(Guid InvoiceId, decimal Total);

public async Task<CreateInvoiceResult> CreateInvoiceAsync(
    CreateInvoiceRequest request,
    CancellationToken cancellationToken)
{
    ArgumentNullException.ThrowIfNull(request);
    decimal total = CalculateTotal(request.Lines);
    var invoice = Invoice.Create(request.CustomerId, total);

    await repository.AddAsync(invoice, cancellationToken);
    return new(invoice.Id, invoice.Total);
}
```

- Çok parametre, fazla sorumluluk veya eksik bir model sinyali olabilir.
- `bool` ayrıntılı sonucu anlatamıyorsa `Result<T>` ya da anlamlı bir record kullan.
- Backend'de `async void` yerine `Task`/`Task<T>` kullan.
- I/O zincirinde `CancellationToken` ilet.
- `ref` değişkenin kendisini geçirir; `out`, `TryParse` benzeri ek çıktı sağlar.

> **Akılda kalsın:** İyi metot tek cümleyle açıklanabilen bir iş yapar ve sonucu belirsiz bırakmaz.

## 10. Null Yönetimi ve Nullable Reference Types

Nullable Reference Types (`string?`) compiler'a “bu değer null olabilir” sözleşmesini bildirir; runtime'da null'ı tamamen engellemez. `?.` güvenli erişim, `??` alternatif değer, `!` ise yalnızca uyarıyı susturur.

```csharp
#nullable enable

public string GetDisplayName(Customer? customer)
{
    if (customer is null)
        return "Misafir";

    return string.IsNullOrWhiteSpace(customer.Name)
        ? "İsimsiz müşteri"
        : customer.Name;
}

public void Save(Customer customer)
{
    ArgumentNullException.ThrowIfNull(customer);
    // Buradan sonra customer null değildir.
}
```

`null`, `""` ve boşluk aynı değildir. DB kolonları, entity'ler ve DTO'lar aynı nullability anlamını taşımalıdır. `FirstOrDefault` sonucunu kontrol et. Her yere `?.` eklemek, aslında hata olan null durumunu gizleyebilir.

> **Akılda kalsın:** `?` olasılığı ilan eder, `?.` yönetir, `!` yalnızca compiler'ı susturur.

## 11. Class ve Object Temelleri

Class şablon, object bu şablondan oluşturulan örnektir. Class bir referans tipidir; atama çoğunlukla yeni nesne üretmez, aynı nesneye ikinci bir referans oluşturur.

```csharp
public sealed class Customer
{
    public string Name { get; private set; }

    public Customer(string name) => Name = name;
    public void Rename(string name) => Name = name;
}

var first = new Customer("Ada");
var second = first;
second.Rename("Ece");

Console.WriteLine(first.Name); // Ece: aynı nesne
```

Class yalnız veri kabı olmak zorunda değildir; veriye ait davranışı ve kuralları da taşıyabilir. Entity, service, controller, handler ve middleware birer class olabilir ama her birinin sorumluluğu açık olmalıdır. Bağımlılıkları içeride `new`lemek yerine çoğu backend senaryosunda constructor injection tercih edilir.

> **Akılda kalsın:** Class tarif, object gerçektir; referans kopyası aynı gerçeği gösterebilir.

## 12. Field, Property ve Constructor

Field nesnenin iç verisini tutar, property kontrollü erişim sunar, constructor nesneyi geçerli başlangıç durumunda kurar.

```csharp
public sealed class BankAccount
{
    private readonly IClock _clock;        // dependency field
    public Guid Id { get; }                // salt okunur
    public decimal Balance { get; private set; }
    public string Owner { get; init; }

    public BankAccount(Guid id, string owner, IClock clock)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Sahip zorunludur.", nameof(owner));

        Id = id;
        Owner = owner;
        _clock = clock;
    }
}
```

Field'lar çoğunlukla `private`, bağımlılıklar `private readonly` tutulur. Her property'ye `public set` vermek nesne durumunu korumasız bırakır. Constructor'a DB/API çağrısı gibi ağır I/O gizleme; constructor'ın ana işi geçerli nesne oluşturmaktır.

> **Akılda kalsın:** Field içeride saklar, property kapıyı kontrol eder, constructor temeli sağlam atar.

## 13. Encapsulation ve Nesne Durumunun Korunması

Encapsulation yalnız üyeleri `private` yapmak değil, nesnenin geçersiz duruma düşmesini engellemektir. Her zaman doğru kalması gereken business kurallarına **invariant** denir.

```csharp
public sealed class Order
{
    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    public void AddItem(OrderItem item)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Onaylanan sipariş değiştirilemez.");
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public void Confirm()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("Boş sipariş onaylanamaz.");
        Status = OrderStatus.Confirmed;
    }
}
```

Mutable listeyi doğrudan dışarı açmak `private set` korumasını boşa çıkarabilir. Request validation kötü girdiyi erken yakalar; domain encapsulation ise hangi kod çağırırsa çağırsın geçersiz state'i engeller.

> **Akılda kalsın:** Nesne kendi kalesidir; state yalnız kurallı davranış kapılarından değişmelidir.

## 14. Referans ve Değer Tipleri: Class ve Struct

Value type atamasında değer kopyalanır; reference type atamasında referans kopyalanır. Asıl ayrım “stack/heap” ezberi değil, **değer semantiği ve kimlik semantiğidir**.

```csharp
public readonly record struct Money(decimal Amount, string Currency);
public sealed class Customer { public string Name { get; set; } = ""; }

var money1 = new Money(100m, "TRY");
var money2 = money1 with { Amount = 200m }; // bağımsız değer

var customer1 = new Customer { Name = "Ada" };
var customer2 = customer1;
customer2.Name = "Ece";                    // customer1 de Ece
```

`int`, `decimal`, `Guid`, `DateTime`, `enum` ve struct'lar value type; class, array, `List<T>` ve `string` reference type'tır. String immutable olduğu için değer gibi hissedilir. Struct'ı küçük, immutable ve doğal olarak değer anlamı taşıyan tiplerde kullan; büyük/mutable struct kopyalama ve davranış sorunları doğurabilir.

> **Akılda kalsın:** Entity “hangi nesne?”, value object “hangi değer?” sorusuna cevap verir.

## 15. Record ve Immutable Nesne Tasarımı

Record varsayılan olarak değer bazlı eşitlik sunar; DTO, command, query, event ve value object için güçlüdür. `init` oluşturma sonrası atamayı kapatır, `with` mevcut nesneyi değiştirmeden kopya üretir.

```csharp
public sealed record Address(string City, string District);
public sealed record CreateOrder(Guid CustomerId, IReadOnlyList<Guid> ProductIds);

var oldAddress = new Address("İstanbul", "Kadıköy");
var newAddress = oldAddress with { District = "Beşiktaş" };

Console.WriteLine(oldAddress == new Address("İstanbul", "Kadıköy")); // true
```

Record class bir reference type'tır; `record struct` value type'tır. `with` shallow copy yapar: içte mutable bir liste varsa iki kopya aynı listeyi paylaşabilir. Bu nedenle yalnız record kullanmak tam immutability sağlamaz. Kimliği önemli ve yaşam döngüsü olan domain entity'leri çoğunlukla class olarak kalır.

> **Akılda kalsın:** Record “aynı değer mi?”, entity class “aynı kimlik mi?” diye düşünür.

## 16. Interface ve Sözleşme Tabanlı Tasarım

Interface implementasyonu değil, tüketicinin ihtiyaç duyduğu davranış sözleşmesini tanımlar. Amaç somut teknolojiye değil, anlamlı abstraction'a bağımlı olmaktır.

```csharp
public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(
        Money amount,
        CancellationToken cancellationToken);
}

public sealed class OrderService(IPaymentGateway paymentGateway)
{
    public Task<PaymentResult> PayAsync(
        Money total,
        CancellationToken ct) => paymentGateway.ChargeAsync(total, ct);
}
```

Bu sözleşmenin Stripe, banka veya test fake'i gibi farklı implementasyonları olabilir. Interface'e `DbContext` gibi altyapı detayı sızdırma; business ihtiyacını anlat. Çok büyük interface'leri Interface Segregation ile böl. Her class için interface açmak gerekmez; gerçek bir sınır, alternatif implementasyon veya test ihtiyacı olmalıdır.

> **Akılda kalsın:** Tüketen sınıf “sen kimsin?” değil, “hangi sözleşmeyi sağlıyorsun?” demelidir.

## 17. Abstract Class, Interface ve Kalıtım

Interface yeteneği/sözleşmeyi; abstract class ortak kimlik, state ve kısmi implementasyonu temsil eder. Kalıtım güçlü bir **is-a**, composition ise **has-a** ilişkisidir. Genellikle önce composition düşünülür.

```csharp
public interface INotifier
{
    Task SendAsync(string message, CancellationToken ct);
}

public abstract class Document
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public abstract string Render();
}

public sealed class Invoice : Document
{
    public override string Render() => $"Invoice: {Id}";
}

public sealed class InvoiceService(INotifier notifier) { } // composition
```

Bir class tek base class'tan türeyebilir, birden çok interface uygulayabilir. Child, base class yerine sorunsuz kullanılabilmelidir; sürekli `NotSupportedException` atıyorsa abstraction hatalı olabilir. Derin kalıtım ağacı kırılganlık yaratır.

> **Akılda kalsın:** Önce composition; yetenek için interface; gerçek ortak kimlik ve davranış varsa abstract class.

## 18. Dependency Injection ve Bağımlılık Yönetimi

DI, sınıfın bağımlılıklarını kendi oluşturmak yerine dışarıdan almasıdır. Constructor injection bağımlılıkları görünür kılar ve test edilebilirliği artırır.

```csharp
builder.Services.AddTransient<IEmailFormatter, EmailFormatter>();
builder.Services.AddScoped<IInvoiceRepository, EfInvoiceRepository>();
builder.Services.AddSingleton<ISystemClock, SystemClock>();

public sealed class InvoiceService(IInvoiceRepository repository)
{
    public Task<Invoice?> GetAsync(Guid id, CancellationToken ct) =>
        repository.GetByIdAsync(id, ct);
}
```

- **Transient:** Her çözümlemede yeni instance.
- **Scoped:** Web uygulamasında çoğunlukla request boyunca aynı instance; `DbContext` tipik örnektir.
- **Singleton:** Uygulama boyunca tek instance; thread-safe olmalıdır.

Singleton'ın doğrudan scoped dependency alması captive dependency problemidir. `BackgroundService` içinde scoped servis gerekiyorsa `IServiceScopeFactory` ile scope açılır. Her yerde `IServiceProvider` kullanmak bağımlılıkları gizleyen Service Locator'a dönüşebilir.

> **Akılda kalsın:** Bağımlılığını üretme, iste; ama ömürlerinin birbirine uyduğundan emin ol.

## 19. Array ve `List<T>`

Array sabit uzunluklu ve indeks erişimi O(1) olan yapıdır. `List<T>` dinamik görünür ama içeride kapasitesi büyüyen bir array kullanır. Eleman sayısı tahmin edilebiliyorsa başlangıç kapasitesi gereksiz yeniden ayırmayı azaltır.

```csharp
string[] weekdays = ["Pzt", "Sal", "Çar", "Per", "Cum"];

var invoiceIds = new List<Guid>(capacity: 1_000);
invoiceIds.Add(Guid.NewGuid());

IReadOnlyList<Guid> GetInvoiceIds() => invoiceIds.AsReadOnly();
```

EF Core'da `ToList()` sorguyu çalıştırır ve sonuçları RAM'e alır. Bu nedenle `Where`, `Select`, `Skip` ve `Take` mümkün olduğunca önce uygulanmalıdır.

```csharp
var summaries = await db.Invoices
    .Where(x => x.IsActive)
    .Select(x => new { x.Id, x.Total })
    .Take(100)
    .ToListAsync(cancellationToken);
```

> **Akılda kalsın:** Array boyutu bilir; List büyür; `ToList` veriyi RAM'e indirir.

## 20. Dictionary ve HashSet

`Dictionary<TKey,TValue>` anahtarı değere bağlar; `HashSet<T>` yalnız benzersiz değer tutar. İyi hash dağılımıyla arama/ekleme ortalama O(1)'dir; bu mutlak “her zaman anında” garantisi değildir.

```csharp
var prices = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
{
    ["ABC-1"] = 125.50m
};

if (prices.TryGetValue("abc-1", out decimal price))
    Console.WriteLine(price);

var processedIds = new HashSet<Guid>();
bool firstTime = processedIds.Add(invoiceId); // tekrar ise false
```

Dictionary'de `Add` tekrar anahtarda hata verir; indexer (`map[key] = value`) üzerine yazar. Güvenli okuma için `TryGetValue` kullan. İkisi de eşzamanlı yazma için thread-safe değildir; gerektiğinde `ConcurrentDictionary` değerlendir. Eklenecek sayı biliniyorsa kapasite belirtmek allocation maliyetini azaltabilir.

> **Akılda kalsın:** Dictionary “bu anahtarın değeri ne?”, HashSet “bu değer daha önce görüldü mü?” sorusuna hızlı cevap verir.

## 21. LINQ ile Filtreleme ve Veri Dönüştürme

LINQ sorgusu çoğu zaman sonuç tüketilene kadar çalışmaz (**deferred execution**). EF Core'da `IQueryable` ifadeyi SQL'e taşır; `IEnumerable` sonrasındaki işlem bellekte gerçekleşir.

```csharp
IQueryable<Invoice> query = db.Invoices.AsNoTracking();

if (minimumTotal is not null)
    query = query.Where(x => x.Total >= minimumTotal.Value);

var result = await query
    .Where(x => x.Status == InvoiceStatus.Approved)
    .Select(x => new InvoiceSummary(x.Id, x.Total))
    .Take(100)
    .ToListAsync(cancellationToken);
```

`Select` yalnız tür dönüştürmez; gerekli kolonları seçerek ağ ve RAM maliyetini azaltır. Erken `ToList()`/`AsEnumerable()` sonraki filtreleri belleğe taşıyabilir. Her C# metodu SQL'e çevrilemez; sorgu kısmını provider'ın çevirebildiği ifadelerle tut.

> **Akılda kalsın:** IQueryable SQL'de düşünür; `ToList` çizgiyi çeker ve sonrası RAM'de yaşar.

## 22. LINQ ile Eleman Arama ve Varlık Kontrolü

Niyeti doğrudan anlatan operatörü seç: varlık için `Any`, tümü için `All`, ilk eşleşme için `First...`, benzersizlik kuralı için `Single...`.

```csharp
bool hasOverdue = await db.Invoices
    .AnyAsync(x => x.DueDate < today && !x.IsPaid, cancellationToken);

Invoice? first = await db.Invoices
    .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);

User? user = await db.Users
    .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
```

- `First`: Yoksa exception; birden fazla olabilir.
- `FirstOrDefault`: Yoksa default/null; ilkini alır.
- `Single`: Tam bir kayıt bekler; 0 veya 2+ kayıtta exception.
- `SingleOrDefault`: 0 kayıt kabul, 2+ kayıt hata.
- `Any()` varlık kontrolünü anlatır; `Count() > 0` gereksiz sayım yapabilir.

Önce filtrele, en son materialize et. `SingleOrDefault` performans tercihi değil, veri benzersizliği sözleşmesidir.

> **Akılda kalsın:** First sırayı, Single benzersizlik kuralını, Any yalnız varlığı sorar.

## 23. LINQ ile Sıralama, Gruplama ve Toplama

Sıralama zinciri `OrderBy` ile başlar, `ThenBy` ile sürer. İkinci bir `OrderBy`, önceki sıralamayı değiştirebilir. `Count`, `Sum`, `Min`, `Max` gibi aggregate'ler koleksiyon yerine tek sonuç üretir.

```csharp
var totalsByCustomer = await db.Invoices
    .Where(x => x.Status == InvoiceStatus.Approved)
    .GroupBy(x => x.CustomerId)
    .Select(g => new
    {
        CustomerId = g.Key,
        InvoiceCount = g.Count(),
        Total = g.Sum(x => x.Total)
    })
    .OrderByDescending(x => x.Total)
    .ThenBy(x => x.CustomerId)
    .ToListAsync(cancellationToken);
```

`ToList().Count` tüm satırları RAM'e taşır; yalnız sayı gerekiyorsa doğrudan `CountAsync` kullan. EF Core'da `GroupBy` sonrasında aggregate içeren `Select`, SQL'e çevrilebilir net bir projeksiyon oluşturur. Sayfalama yapmadan önce deterministik sıralama uygula.

> **Akılda kalsın:** Filtrele → grupla → hesapla → sırala → en son belleğe al.

## 24. Exception Handling: `try`, `catch`, `finally`

Exception beklenmeyen başarısızlığı temsil eder. Spesifik exception önce yakalanır. Aynı hatayı yeniden fırlatırken `throw;` stack trace'i korur; `throw ex;` başlangıç izini bozar.

```csharp
try
{
    await fileImporter.ImportAsync(path, cancellationToken);
}
catch (FileNotFoundException ex)
{
    logger.LogWarning(ex, "Dosya bulunamadı: {Path}", path);
    throw; // Orijinal stack trace korunur
}
catch (IOException ex)
{
    logger.LogError(ex, "Dosya okunamadı: {Path}", path);
    throw;
}
```

Boş `catch` hatayı yutar. Kaynak temizliğinde `finally` kullanılabilir; `IDisposable`/`IAsyncDisposable` için çoğunlukla `using`/`await using` daha nettir. ASP.NET Core'da beklenmeyen hataları merkezi exception handler ile HTTP `ProblemDetails` sonucuna çevirmek, her metoda try-catch eklemekten temizdir.

> **Akılda kalsın:** Yakalıyorsan anlamlı bir şey yap; yeniden atacaksan `throw;` de.

## 25. Custom Exception ve Uygulama Hataları

Beklenen business sonucu ile beklenmeyen sistem hatasını ayır. “Şifre yanlış”, validasyon veya stok yetersizliği sık ve beklenen akışsa `Result<T>` uygun olabilir. Sistem invariant'ının bozulması veya işlem sürdürülemiyorsa exception düşünülebilir.

```csharp
public sealed class PaymentException : Exception
{
    public PaymentException(string message, Exception innerException)
        : base(message, innerException) { }
}

try
{
    await gateway.ChargeAsync(amount, cancellationToken);
}
catch (HttpRequestException ex)
{
    throw new PaymentException("Ödeme sağlayıcısına ulaşılamadı.", ex);
}
```

Alt hatayı `InnerException` ile korumak teşhis bilgisini kaybetmez. API katmanında custom exception'ları merkezi handler ile uygun status code ve `ProblemDetails` çıktısına eşle. Hassas exception ayrıntılarını kullanıcıya açma; loglarda correlation/trace ID kullan.

> **Akılda kalsın:** Beklenen olumsuzluk sonuçtur; beklenmeyen ve sürdürülemez durum exception'dır.

## 26. Clean Code Temelleri

Clean Code'un amacı kısa kod değil, niyeti açık ve güvenle değiştirilebilir koddur. İsimler business dilini taşımalı; metot ve sınıflar tek ana sorumlulukta kalmalıdır.

```csharp
// Kötü: anlam belirsiz, sihirli sayı ve iç içe koşul
if (x != null && x.Status == 4 && x.Total > 0) Process(x);

// Daha açık
if (invoice is null) return;
if (invoice.Status != InvoiceStatus.Approved) return;
if (invoice.Total <= 0m) return;

SendApprovedInvoice(invoice);
```

- `Data`, `Manager`, `Helper` yerine sorumluluğu anlatan adlar seç.
- Yorum “ne”yi tekrar etmek yerine önemliyse “neden”i açıklasın.
- Sihirli sayı/string yerine enum, sabit veya value object kullan.
- Guard clause ile ana akışı sola yasla.
- Boy Scout Rule: Dokunduğun yeri küçük ölçüde daha temiz bırak; kapsam dışı büyük değişiklik yapma.

> **Akılda kalsın:** Kod bilgisayara ne yapacağını, geliştiriciye neden ve hangi niyetle yaptığını anlatmalıdır.

## 27. Refactoring ve Sorumlulukların Ayrılması

Refactoring dış davranışı değiştirmeden iç tasarımı iyileştirir. Önce mevcut davranışı testlerle güvenceye al, sonra küçük adımlarla taşı ve her adımda test et.

```csharp
// Controller yalnız HTTP akışını yönetir.
[HttpPost]
public async Task<IActionResult> Create(
    CreateInvoiceRequest request,
    CancellationToken ct)
{
    var result = await invoiceService.CreateAsync(request, ct);
    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
}

// Validasyon ayrı, iş kuralı application/domain, kalıcılık repository sorumluluğudur.
```

Pratik sıra: **karakterizasyon testi → küçük değişiklik → build/test → commit**. Controller DB sorgusu, business rule ve exception eşleme merkezi olmamalıdır. SoC, değişiklik sebeplerini ayırır; ancak her satırı yeni sınıfa taşımak over-engineering'dir. Abstraction gerçek bir değişim veya sorumluluk sınırına hizmet etmelidir.

> **Akılda kalsın:** Refactoring evin planını düzeltir; yeni oda eklemez ve yaşayanları yerinden etmez.

## 28. Git Branch, Merge ve Conflict Yönetimi

Branch geliştirmeyi izole eder; merge iki hattı birleştirir; conflict Git'in doğru sonucu seçemediği yerdir. Conflict çözümü işaretleri silmek değil, doğru nihai davranışı kurmaktır.

```powershell
git switch main
git pull
git switch -c feature/invoice-cancel

# geliştirme, küçük commit'ler...
git push -u origin feature/invoice-cancel

# Birleştirme sırasında conflict varsa:
git status
# <<<<<<<, =======, >>>>>>> bloklarını bilinçli düzenle
git add path/to/resolved-file.cs
git commit
dotnet build
dotnet test
```

`git fetch` remote referanslarını günceller, çalışma koduna doğrudan birleştirme yapmaz. `git pull` kabaca fetch + merge/rebase'dir. Rebase history'yi yeniden yazar; paylaşılmış branch'te dikkat ister. Uzun ömürlü branch daha çok conflict; küçük branch ve sık entegrasyon daha kolay review demektir. Gerekirse `git merge --abort` işlemi başlangıca döndürür.

> **Akılda kalsın:** Branch ayırır, merge buluşturur, conflict karar sorumluluğunu geliştiriciye verir.

## 29. Pull Request, Code Review ve Ekip Kültürü

Pull Request kontrollü merge sürecidir; amacı yalnız kodu birleştirmek değil, kalite kontrolü, bilgi paylaşımı ve izlenebilirliktir. İyi PR küçük, tek amaçlı ve test edilebilir olur.

```markdown
## Ne değişti?
- Negatif tutarlı faturaların oluşturulması engellendi.

## Neden?
- Domain invariant'ı API dışında yapılan çağrılarda korunmuyordu.

## Nasıl test edildi?
- `dotnet test`
- Pozitif, sıfır ve negatif tutar senaryoları

## Risk / geri dönüş
- DB şeması değişmedi; önceki sürüm yeniden deploy edilebilir.
```

Reviewer; syntax'ın yanında business logic, null, concurrency, transaction, performans, güvenlik, mimari ve testlere bakar. Yorum kişiye değil koda yöneltilir ve mümkünse gerekçeyi açıklar. Style gibi mekanik kontroller `.editorconfig`, analyzer ve CI'a bırakılabilir. CI'da en az restore/build/test; uygun projelerde static analysis ve security kontrolleri çalışmalıdır.

> **Akılda kalsın:** PR değişikliği anlatır, CI otomatik kontrol eder, review ekip aklını koda taşır, approve teknik sorumluluk alır.

---

## Hızlı Tekrar: 29 Hafıza Cümlesi

1. IDE arayüz, terminal komut ortamı, SDK üretim aracıdır.
2. Solution organize eder, project derlenir, `.csproj` kuralları tanımlar.
3. Restore hazırlar, build derler, test doğrular, publish paketler.
4. `add` seçer, `commit` kaydeder, `push` paylaşır.
5. AI önerir; geliştirici anlar, test eder ve sorumluluğu alır.
6. Veri tipi bir iş kuralıdır.
7. Guard clause temizler, switch açık seçim yapar.
8. Döngünün bitişini ve her turun maliyetini bil.
9. Metodun adı, girdisi ve çıktısı sözleşmesidir.
10. `?` ilan eder, `?.` yönetir, `!` susturur.
11. Class şablon, object örnek; referanslar aynı örneği paylaşabilir.
12. Field saklar, property korur, constructor geçerli başlatır.
13. Entity state'ini davranış ve invariant'larla korur.
14. Class kimliği, struct değeri öne çıkarır.
15. Record değer eşitliği ve immutable veri akışında parlar.
16. Interface implementasyonu değil ihtiyacı anlatır.
17. Önce composition; sonra ihtiyaç kadar abstraction ve kalıtım.
18. Bağımlılığı dışarıdan al ve lifetime uyumunu koru.
19. Array sabit, List dinamik; `ToList` materialization'dır.
20. Dictionary eşleştirir, HashSet benzersizliği denetler.
21. IQueryable SQL'de, materialization sonrası işlemler RAM'de çalışır.
22. Any varlık, First sıra, Single benzersizlik niyetidir.
23. Filtrele, grupla, hesapla, sırala, en son materialize et.
24. Exception'ı yutma; tekrar atacaksan `throw;` kullan.
25. Beklenen hata sonuç, beklenmeyen başarısızlık exception olabilir.
26. Temiz kod niyetini açıklar ve değişimi güvenli kılar.
27. Refactoring davranışı koruyarak tasarımı iyileştirir.
28. Conflict'i Git değil, doğru iş davranışını bilen geliştirici çözer.
29. İyi PR küçük, açıklanmış, test edilmiş ve ekipçe doğrulanmıştır.
