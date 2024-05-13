import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Yayin } from '../common/yayin';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http: HttpClient) {}
  
  search(searchText: string): Observable<GetResponseYayin> {
    const requestBody = { searchText: searchText };
    return this.http.post<GetResponseYayin>('https://localhost:7075/api/WebScraping/scholar', requestBody);
  }

  dergiparksearch(searchText: string): Observable<GetResponseYayin> {
    const requestBody = { searchText: searchText };
    return this.http.post<GetResponseYayin>('https://localhost:7075/api/WebScraping/dergipark', requestBody);
  }

  detay(id: string): Observable<GetResponseYayinDetay> {
    return this.http.get<GetResponseYayinDetay>('https://localhost:7075/detay/'+id);
  }

  listeleHepsini(): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/yayinlar");
  }
  
  filtreleYayinAdi(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/YayinAdi/"+text);
  }

  filtreleYazarlar(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/Yazarlar/"+text);
  }

  filtreleUrl(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/Url/"+text);
  }

  filtreleYayinlanmaTarihiEnSon(): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/YayinlanmaTarihiEnSon");
  }

  filtreleYayinlanmaTarihiEnOnce(): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/YayinlanmaTarihiEnOnce");
  }

  filtreleYayinciAdi(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/YayinciAdi/"+text);
  }

  filtreleAnahtarKelime(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/AnahtarKelime/"+text);
  }

  filtreleOzet(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/Ozet/"+text);
  }

  filtreleAlintiSayisi(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/AlintiSayisi/"+text);
  }

  filtreleDoiNumarasi(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/DoiNumarasi/"+text);
  }

  filtreleTur(text:string): Observable<GetResponseYayin> {
    return this.http.get<GetResponseYayin>("https://localhost:7075/api/WebScraping/Filtrele/Tur/"+text);
  }

  pdfIndir(text:string): Observable<GetResponseYayinDetay> {
    return this.http.get<GetResponseYayinDetay>("https://localhost:7075/api/WebScraping/pdfIndir/"+text);
  }
}

interface GetResponseYayin{
  error:{
    proccess:boolean,
    message:string
  },
  yayinListesi:Yayin[],
  proccess:boolean
}

interface GetResponseYayinDetay{
  error:{
    proccess:boolean,
    message:string
  }
  yayin:Yayin,
  proccess:boolean
}