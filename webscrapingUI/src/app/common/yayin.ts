export class Yayin {
  id: string;
  ad: string;
  yazarlar: string;
  tur: string;
  yayinlanmaTarihi: string;
  yayinciAdi: string;
  anahtarKelimelerAramaMotoru: string;
  anahtarKelimelerMakaleyeAit: string;
  ozet: string;
  alintiSayisi: number;
  doiNumarasi: string;
  url: string;

  constructor(
      id: string,
      ad: string,
      yazarlar: string,
      tur: string,
      yayinlanmaTarihi: string,
      yayinciAdi: string,
      anahtarKelimelerAramaMotoru: string,
      anahtarKelimelerMakaleyeAit: string,
      ozet: string,
      alintiSayisi: number,
      doiNumarasi: string,
      url: string
  ) {
      this.id = id;
      this.ad = ad;
      this.yazarlar = yazarlar;
      this.tur = tur;
      this.yayinlanmaTarihi = yayinlanmaTarihi;
      this.yayinciAdi = yayinciAdi;
      this.anahtarKelimelerAramaMotoru = anahtarKelimelerAramaMotoru;
      this.anahtarKelimelerMakaleyeAit = anahtarKelimelerMakaleyeAit;
      this.ozet = ozet;
      this.alintiSayisi = alintiSayisi;
      this.doiNumarasi = doiNumarasi;
      this.url = url;
  }
}
