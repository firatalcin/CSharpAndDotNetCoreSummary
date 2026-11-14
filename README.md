<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:512BD4,100:9B6BFF&height=210&section=header&text=C%23%20%26%20.NET%20Core%20Summary&fontColor=ffffff&fontSize=44&fontAlignY=36&desc=Ay%20ay%20.NET%20yolculu%C4%9Fu%20%E2%80%94%20notlar%2C%20s%C4%B1navlar%20ve%20projeler&descAlignY=58&descSize=17" alt="C# & .NET Core Summary" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-14-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-Web%20API-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white" alt="ASP.NET Core" />
  <img src="https://img.shields.io/badge/Dil-T%C3%BCrk%C3%A7e-E30A17?style=for-the-badge" alt="Türkçe" />
</p>

<p align="center">
  <b>Sıfırdan ileri seviyeye, Türkçe ve uygulamalı .NET çalışma notları.</b><br/>
  Her ay bir tema: konu özetleri, ay sonu sertifika sınavı ve gerektiğinde gerçek bir proje.
</p>

<p align="center">
  <img src="https://skillicons.dev/icons?i=cs,dotnet,visualstudio,rider,git,github" alt="Teknolojiler" />
</p>

---

## 🗺️ Yol Haritası

| Ay | Tema | Notlar | Sınav | Proje | Durum |
|:--:|------|:--:|:--:|:--:|:--:|
| 1 | [C#, Modern .NET ve Developer Temeli](#-01-ay--c-modern-net-ve-developer-temeli) | 29 konu | ✅ | — | ![](https://img.shields.io/badge/-Tamamland%C4%B1-2ea44f?style=flat-square) |
| 2 | [.NET Runtime, Memory ve Async/Concurrency](#-02-ay--net-runtime-memory-ve-asyncconcurrency) | 25 konu | ✅ | — | ![](https://img.shields.io/badge/-Tamamland%C4%B1-2ea44f?style=flat-square) |
| 3 | [ASP.NET Core Web API, REST ve API Security](#-03-ay--aspnet-core-web-api-rest-ve-api-security) | 30 konu | ✅ | OrderAPI | ![](https://img.shields.io/badge/-Tamamland%C4%B1-2ea44f?style=flat-square) |

---

## [📘 01. Ay — C#, Modern .NET ve Developer Temeli](01.%20Ay%20-%20C%23%2C%20Modern%20.NET%20ve%20Developer%20Temeli)

> Sağlam temel: dili, aracı ve iyi bir geliştiricinin alışkanlıklarını öğrenmek.

.NET SDK, CLI ve proje yapısından başlayıp C#'ın temellerine iniyor: veri tipleri, kontrol akışı, metotlar ve nullable reference types. Ardından class, struct, record, interface ve kalıtımla nesne yönelimli tasarım; Dependency Injection'a giriş; koleksiyonlar ve LINQ ile veri işleme geliyor. Ay; exception yönetimi, clean code, refactoring ile Git, Pull Request ve code review kültürüyle kapanıyor.

![](https://img.shields.io/badge/OOP-239120?style=flat-square) ![](https://img.shields.io/badge/LINQ-239120?style=flat-square) ![](https://img.shields.io/badge/Dependency%20Injection-239120?style=flat-square) ![](https://img.shields.io/badge/Clean%20Code-239120?style=flat-square) ![](https://img.shields.io/badge/Git-239120?style=flat-square)

📄 [Konu Özetleri](01.%20Ay%20-%20C%23%2C%20Modern%20.NET%20ve%20Developer%20Temeli/01.%20Ay%20-%20C%23%2C%20Modern%20.Net%20ve%20Developer%20Temeli.md) &nbsp;•&nbsp; 📝 [Sertifika Sınavı](01.%20Ay%20-%20C%23%2C%20Modern%20.NET%20ve%20Developer%20Temeli/01-AY-SertifikaSinavi.md)

---

## [🧠 02. Ay — .NET Runtime, Memory ve Async/Concurrency](02.%20Ay%20-%20.NET%20Runtime%2C%20Memory%20ve%20AsyncConcurrency)

> Kaputun altı: kodun gerçekte nasıl çalıştığını, belleği nasıl kullandığını ve aynı anda nasıl iş yaptığını anlamak.

C# kodunun IL'e, oradan JIT/AOT ile makine koduna uzanan yolculuğu ve CLR'ın rolü anlatılıyor. Stack/heap, boxing, Garbage Collector nesilleri, LOH ve `IDisposable` ile bellek yönetimi derinlemesine işleniyor; `Span<T>`, `ArrayPool` ve object pooling ile performanslı kod yazımına geçiliyor. Son bölüm benchmarking ve diagnostics'ten sonra Thread, ThreadPool, Task, async/await state machine, cancellation, deadlock, race condition ve producer–consumer modellerine odaklanıyor.

![](https://img.shields.io/badge/CLR%20%26%20JIT-512BD4?style=flat-square) ![](https://img.shields.io/badge/Garbage%20Collector-512BD4?style=flat-square) ![](https://img.shields.io/badge/Span%3CT%3E-512BD4?style=flat-square) ![](https://img.shields.io/badge/async%2Fawait-512BD4?style=flat-square) ![](https://img.shields.io/badge/Concurrency-512BD4?style=flat-square)

📄 [Konu Özetleri](02.%20Ay%20-%20.NET%20Runtime%2C%20Memory%20ve%20AsyncConcurrency/02.%20Ay%20-%20.NET%20Runtime%2C%20Memory%20ve%20AsyncConcurrency.md) &nbsp;•&nbsp; 📝 [Sertifika Sınavı](02.%20Ay%20-%20.NET%20Runtime%2C%20Memory%20ve%20AsyncConcurrency/02.Ay-SertifikaSinavi.md)

---

## [🌐 03. Ay — ASP.NET Core Web API, REST ve API Security](03.%20Ay%20-%20ASP.NET%20Core%20Web%20API%2C%20REST%20ve%20API%20Security)

> Üretime hazır API: doğru tasarlanmış, gözlemlenebilir ve güvenli bir Web API inşa etmek.

HTTP ve REST semantiğinden başlayıp ASP.NET Core'un çekirdeğine iniyor: hosting, controller'lar, model binding ve DTO tasarımı. Altyapı tarafında DI lifetime'ları, Options pattern, FluentValidation, custom middleware ve ProblemDetails ile merkezi hata yönetimi var. Sayfalama, filtreleme, API versiyonlama, structured logging ve Correlation ID ile ileri API tasarımı işleniyor. Ay; JWT, rol/policy/kaynak bazlı yetkilendirme, CORS, rate limiting, OWASP API Top 10 ve entegrasyon testleriyle tamamlanıyor.

![](https://img.shields.io/badge/REST-5C2D91?style=flat-square) ![](https://img.shields.io/badge/JWT-5C2D91?style=flat-square) ![](https://img.shields.io/badge/FluentValidation-5C2D91?style=flat-square) ![](https://img.shields.io/badge/Rate%20Limiting-5C2D91?style=flat-square) ![](https://img.shields.io/badge/OWASP%20API%20Top%2010-5C2D91?style=flat-square) ![](https://img.shields.io/badge/Integration%20Tests-5C2D91?style=flat-square)

🛠️ **Proje — [OrderAPI](03.%20Ay%20-%20ASP.NET%20Core%20Web%20API%2C%20REST%20ve%20API%20Security/OrderManagement):** Dört haftada adım adım büyüyen bir sipariş yönetimi API'si. Katmanlı mimari, validasyon, global hata yönetimi, sayfalama ve versiyonlama içeriyor; JWT ile korunuyor, kaynak bazlı yetkilendirme ve kullanıcı başına rate limit uyguluyor. Davranışı `WebApplicationFactory` entegrasyon testleriyle doğrulanıyor.

📄 [Konu Özetleri](03.%20Ay%20-%20ASP.NET%20Core%20Web%20API%2C%20REST%20ve%20API%20Security/03.%20Ay%20-%20ASP.NET%20Core%2C%20REST%20API%20ve%20API%20Security%20Temeli.md) &nbsp;•&nbsp; 📝 [Sertifika Sınavı](03.%20Ay%20-%20ASP.NET%20Core%20Web%20API%2C%20REST%20ve%20API%20Security/03.Ay-SertifikaSinavi.md) &nbsp;•&nbsp; 🛠️ [OrderAPI Projesi](03.%20Ay%20-%20ASP.NET%20Core%20Web%20API%2C%20REST%20ve%20API%20Security/OrderManagement)

---

## 📚 Nasıl Kullanılır?

- **Öğrenmek için:** Ayları sırayla takip et; her konu bir öncekinin üzerine kurulu.
- **Tekrar için:** Her ayın sonundaki sertifika sınavı, o ayın en kritik sorularını cevaplarıyla topluyor.
- **Pratik için:** Projeleri klonla, çalıştır ve testleri koştur:

```bash
git clone https://github.com/firatalcin/DotNetCoreDictionary.git
```

```bash
dotnet test "03. Ay - ASP.NET Core Web API, REST ve API Security/OrderManagement/OrderManagement.slnx"
```

---

<p align="center">
  ⭐ İşine yaradıysa repoya yıldız bırakmayı unutma — yeni aylar eklendikçe burası büyümeye devam edecek.
</p>

<p align="center">
  <img src="https://capsule-render.vercel.app/api?type=waving&color=0:9B6BFF,100:512BD4&height=110&section=footer" alt="" />
</p>
