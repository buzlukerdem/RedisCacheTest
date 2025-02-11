<h1>REDIS</h1>

Yazılım süreçlerinde verilere daha hızlı erişilmesi istenmektedir. Ve bu verilerin bellekte saklanması olayı CACHING dir.

<br>

<h3>KATKILARI</h3>

* Veri erişim hızını arttırır.
* Sunucu yükünü azaltmaktadır.
* Çevrimiçi uygulamalarda etkili olmaktadır.

Sorgu neticesinde elde edilen veri ilk çalıştığı durumda Cache'e yüklenir ve oradan taleplere göre elde edilir.

<br>

<h3>NE TARZ VERİLER CACHE’LENİR?</h3>

* Sıklıkla talep edilecek nesne olmalıdır.
* Sık veri tabanı sorguları.
* Konfigürasyonel veriler.
* Resim - Video gibi Static Dosyalar.

<br>

**Her Veri Cachlenmemelidir!**

* Kişisel riskli veriler.
* Sık güncellenen veriler.
* Özel veriler.
* Geçici veriler.

<br>

Negatif olabilecek yanları;

* Bellek yükü: Gereksiz veri saklamak yükü arttırır ve performansta düşüşe sebep olur.
* Cachelenmiş veriler, db'de fiziksel ortamlarında değişikliğe uğrayabilirler. Bu durumda cache'de ki verilere de yansıtılması gerekir ve maliyete sebep olur.
* Kritik verilerin cache edilmesi tehlikeli olabilir.

<br>

2 Yaklaşm vardır;

<h2>1- In Memory Cache</h2>

Veriler uygulamanın çalıştığı bilgisayarın RAM'inde cacheleyen yaklaşımdır.
İşlem Sıraları;
<br>

* AddMemoryCache servisini uygulamaya eklenir.

```csharp
builder.Services.AddMemoryCache();
```
<br>

* IMemoryCache referansı ctor'dan inject talebiyle elde edilir.

```csharp
public class ValuesController : ControllerBase
{
    readonly IMemoryCache _memoryCache;

    public ValuesController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
}
```
<br>

* Set metodu ile cachlenecek veriyi ekleyebilir.

```csharp
_memoryCache.Set<string>("name", name);
```
<br>

* Set metodu ile cachlenen veriyi elde edilebilir.

```csharp
_memoryCache.Get("name");
```
<br>

**Absolute Ve Sliding propertyleri** ile cache süresinde konfigürasyonel çalışmalar yapılabilmektedir.
<br>
Kullanım;

```csharp
_memoryCache.Set<string>("ex", "Example", options: new()
{
    // Life Time: 30 seconds
    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
    // Must be get in 5 seconds
    SlidingExpiration = TimeSpan.FromSeconds(5)
});
```
<br>

<h2>2- Distributed Cache</h2>
Verileri birden fazla fiziksel makinede cacheler ve veriler farklı noktalarda tutulur.
Böylece in memory yaklaşımna göre daha güvenli bir davranış sergilenmiş olur.

<br>

İşlem sıraları;

<br>

* Microsoft.Extensions.Caching.StackExchangeRedis kütüphanesi yüklenir. PMC - Nuget vs.
* AddStackExchangeRedisCache servisi eklenir.

```csharp
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:port");
```
<br>

* IDistributedCache referansı ctor'dan inject talebiyle elde edilir.

```csharp
public class ValuesController : ControllerBase
{
    IDistributedCache _distributedCache;

    public ValuesController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
}
```

<br>

* SetStringAsync veya SetAsync metodu ile veriler cachenebilir Absolute ve Sliding property değerleri de eklenebilir.

```csharp
await _distributedCache.SetStringAsync("name", name, options: new()
{
    //config
    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
    SlidingExpiration = TimeSpan.FromMinutes(10)
});

await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
{
    AbsoluteExpiration = DateTime.Now.AddMinutes(30),
    SlidingExpiration = TimeSpan.FromMinutes(10)
});
```

<br>

<h2>Veri Türleri</h2>

* String: En Temel Veri Türüdür. Metinsel değerlerle birlikte her türlü veriyi tutabilir.
* List: Değerleri koleksiyonel olarak tutar.
* Set: Verileri rastgele düzende unique biçimde tutar.
* Sorted Set: Set türünün düzenli halidir.
* Hash: Key-Value formatında verileri tutar.
* Streams: Log tipinde veri türüdür. Sırayla olayların kaydedilmesini ve daha sonra işlenmelerini sğalar.
  


