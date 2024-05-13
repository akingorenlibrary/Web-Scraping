using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebScraping.Configurations;
using WebScraping.Entities;

namespace WebScraping.Services
{
    public class YayinService
    {
        private readonly IMongoCollection<Yayin> _yayinCollection;
        public YayinService(IOptions<DatabaseSettings> databaseSetting)
        {
            var mongoClient = new MongoClient(databaseSetting.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);
            _yayinCollection = mongoDb.GetCollection<Yayin>(databaseSetting.Value.CollectionName);
        }

        public async Task<List<Yayin>> GetAsync() => await _yayinCollection.Find(_ => true).ToListAsync();
        public async Task<Yayin> GetAsync(string id) => await _yayinCollection.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Yayin yayin)=>await _yayinCollection.InsertOneAsync(yayin);

        public async Task<List<Yayin>> GetContainsYayinAdiAsync(string text) => await _yayinCollection.Find(x => x.Ad.Contains(text)).ToListAsync();
        public async Task<List<Yayin>> GetContainsYazarlarAsync(string text) => await _yayinCollection.Find(x => x.Yazarlar.Contains(text)).ToListAsync();
        public async Task<List<Yayin>> GetContainsTurAsync(string text) => await _yayinCollection.Find(x => x.Tur.Contains(text)).ToListAsync();
        public async Task<List<Yayin>> GetYayinlanmaTarihiEnSonAsync() =>
            await _yayinCollection.Find(_ => true)
                           .SortByDescending(y => y.YayinlanmaTarihi)
                           .ToListAsync();

        public async Task<List<Yayin>> GetYayinlanmaTarihiEnOnceAsync() =>
            await _yayinCollection.Find(_ => true)
                                  .SortBy(y => y.YayinlanmaTarihi)
                                  .ToListAsync();

        public async Task<List<Yayin>> GetContainsYayinciAdiAsync(string text) => await _yayinCollection.Find(x => x.YayinciAdi.Contains(text)).ToListAsync();
        public async Task<List<Yayin>> GetContainsAnahtarKelimeAsync(string text) => await _yayinCollection.Find(x => x.AnahtarKelimelerMakaleyeAit.Contains(text)).ToListAsync();
        public async Task<List<Yayin>> GetContainsOzetAsync(string text) => await _yayinCollection.Find(x => x.Ozet.Contains(text)).ToListAsync();
        public async Task<List<Yayin>> GetAlintiSayisiAsync(int alintiSayisi) => await _yayinCollection.Find(x => x.AlintiSayisi==alintiSayisi).ToListAsync();
        public async Task<List<Yayin>> GetDoiNumarasiAsync(string doiNumarasi) => await _yayinCollection.Find(x => x.DoiNumarasi.Contains(doiNumarasi)).ToListAsync();
        public async Task<List<Yayin>> GetUrlAsync(string url) => await _yayinCollection.Find(x => x.Url.Contains(url)).ToListAsync();
    }
}
