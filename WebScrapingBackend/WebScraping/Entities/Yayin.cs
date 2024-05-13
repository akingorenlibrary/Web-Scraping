using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebScraping.Entities
{
    public class Yayin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        [BsonElement("Ad")]
        public string Ad { get; set; }


        [BsonElement("Yazarlar")]
        public string Yazarlar { get; set; }


        [BsonElement("Tur")]
        public string Tur { get; set; }


        [BsonElement("YayinlanmaTarihi")]
        public DateTime YayinlanmaTarihi { get; set; }


        [BsonElement("YayinciAdi")]
        public string YayinciAdi { get; set; }


        [BsonElement("AnahtarKelimelerAramaMotoru")]
        public string AnahtarKelimelerAramaMotoru { get; set; }


        [BsonElement("AnahtarKelimelerMakaleyeAit")]
        public string AnahtarKelimelerMakaleyeAit { get; set; }


        [BsonElement("Ozet")]
        public string Ozet { get; set; }


        [BsonElement("AlintiSayisi")]
        public int AlintiSayisi { get; set; }


        [BsonElement("DoiNumarasi")]
        public string DoiNumarasi { get; set; }


        [BsonElement("Url")]
        public string Url { get; set; }

        [BsonElement("PdfLinki")]
        public string PdfLinki { get; set; }
    }
}
