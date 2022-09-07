# DistributedCacheLinkage
透過DistributedCache實作自動同步寫入與讀取暫存與實體，支援Repository或單一實例

# Quickstart

## 環境設定

    //若使用記憶體暫存
    services.AddDistributedMemoryCache();
    
    //若使用Redis
    services.AddStackExchangeRedisCache(options => {
        options.Configuration = ""; //你的redis連線字串
        options.InstanceName = "";//必須給一個名稱
    });
    
    //如果在AWS Lambda(Serverless Application)上可能需要額外加上此程式碼
    services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

## IEntityRepository

使用情境：Repository模式

Step 1 在你的Repository裡引用IEntityRepository<TEntity, TEntityKey>後只要像平常一樣實作Add()、Update()、Remove()、GetEntities()即可。

    public class CorpRepository : IEntityRepository<Corp, int>
    
> TEntity：你的實體物件。
> 
> TEntityKey：你的主要索引資料型態，以資料庫來講就是primary key。


Step 2 DI兩個項目

    services.AddScoped<IEntityRepository<Corp, int>, CorpRepository>();
    services.AddScoped<RepositoryProxy<Corp, int>>();

Step 3 在你要用的地方將RepositoryProxy加入後就可以直接使用

    private readonly RepositoryProxy<Corp, int> repositoryProxy;
    public CorpController(RepositoryProxy<Corp, int> repositoryProxy)
    {
        this.repositoryProxy = repositoryProxy;
    }
>    
    var corp = repositoryProxy.GetEntities(item => item.CorpId == id).FirstOrDefault();
注意事項

  1. GetEntities雖然可以不給條件，但若是使用資料庫時可能導致耗用大量記憶體(若使用redis可能會導致IO與費用問題)。
  1. 雖無強制，但應確實實作IDisposable




## IEntityOne

使用情境：需要暫存特定某一份資料時使用，某一個設定、某一個資料集合(List or Array)、某一個字串...等

Step 1 在你的服務或模型裡引用IEntityOne<TEntity>。

    public class SettingModel : IEntityOne<List<Setting>>
    
> TEntity：你的實體物件。

Step 2 DI兩個項目

    services.AddScoped<IEntityOne<List<Setting>>, SettingModel>();
    services.AddScoped<OneObjectProxy<List<Setting>>>();

Step 3 在你要用的地方將OneObjectProxy加入後就可以直接使用

    private readonly OneObjectProxy<List<Setting>> oneObjectProxy;
    public SettingController(OneObjectProxy<List<Setting>> oneObjectProxy)
    {
        this.oneObjectProxy = oneObjectProxy;
    }
>    
    var settings = oneObjectProxy.GetObject();
    settings.Add(new Setting { Code = "aaa", Value = "bbb" });
    var tf = oneObjectProxy.PutObject(settings);



## IEntityMany

使用情境：沒有使用Repository模式下，但資料原始來源是資料集合時

Step 1 在你的服務或模型裡引用IEntityMany<TEntity, TEntityKey>。

    public class IconsModel : IEntityMany<Icon, int>
    
> TEntity：你的實體物件。
> 
> TEntityKey：你的主要索引資料型態，以資料庫來講就是primary key。

Step 2 DI兩個項目

    services.AddScoped<IEntityMany<Icon, int>, IconsModel>();
    services.AddScoped<ManyObjectProxy<Icon, int>>();

Step 3 在你要用的地方將ManyObjectProxy加入後就可以直接使用

    private readonly ManyObjectProxy<Icon, int> manyObjectProxy;
    public IconController(ManyObjectProxy<Icon, int> manyObjectProxy)
    {
        this.manyObjectProxy = manyObjectProxy;
    }
>    
    var settings = manyObjectProxy.GetObject(id);
    var obj = new Icon { Id = 3, Content = "bbb" };
    var tf = manyObjectProxy.PutObject(obj, obj.Id);

### IEntityOne與IEntityMany差異
- IEntityOne是整份資料進整份資料出，在記憶體中屬於整份存放。
- IEntityMany則是單筆進單筆出，每筆在記憶體中屬於分開存放。

## 注意事項

- 由於使用Json儲存資料，所以物件必須要能夠序列化的型態或結構才可以使用，否則會發生序列化錯誤。
- 雖無強制，但應確實實作IDisposable
- 專案雖為.NET 6，但實際從.NET Core 3.1就開始使用，並無變更程式碼，若是自行下載且你需要的是3.1時只要變更專案版本即可。