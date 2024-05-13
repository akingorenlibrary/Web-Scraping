import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Yayin } from 'src/app/common/yayin';
import { SearchService } from 'src/app/services/search.service';

@Component({
  selector: 'app-detay',
  templateUrl: './detay.component.html',
  styleUrls: ['./detay.component.css']
})
export class DetayComponent {
  yayin: Yayin = {
  id: '1',
  ad: 'Örnek Yayın',
  yazarlar: 'John Doe',
  tur: 'Makale',
  yayinlanmaTarihi: '2023-01-01',
  yayinciAdi: 'Örnek Yayıncı',
  anahtarKelimelerAramaMotoru: 'örnek, anahtar, kelimeler',
  anahtarKelimelerMakaleyeAit: 'örnek, anahtar, kelimeler',
  ozet: 'Bu bir örnek yayın özeti.',
  alintiSayisi: 10,
  doiNumarasi: '1234/abcd',
  url: 'https://ornekyayin.com'
  };
  yayinId: string | undefined;
  pdfDownloadProccess:boolean=false;

  constructor(private route: ActivatedRoute,
    private searchService:SearchService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.yayinId = params.get('id') as string;
      console.log(this.yayinId);
      
      this.searchService.detay(this.yayinId).subscribe({
        next:(response)=>{
          console.log(response);
          this.yayin=response.yayin;
        },error:(err)=>{
          console.log(err);
        }
      });
    });
  }


  pdfIndir(){
    this.searchService.pdfIndir(this.yayin.id).subscribe({
      next:(response)=>{
        console.log(response);
        if(response.proccess){
          this.pdfDownloadProccess=true;
        }else{
          this.pdfDownloadProccess=false;
        }
      },error:(err)=>{
        console.log(err);
      }
    });
  }
}
