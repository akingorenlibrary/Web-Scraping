import {
  Yayin
} from 'src/app/common/yayin';
import {
  SearchService
} from './../../services/search.service';
import {
  Component
} from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  searchText: string = "";
  buttonDisabled = false;
  successMessage: string = "";
  errorMessage: String = "";
  loading: boolean = false;
  result: boolean = false;
  yayinListesi: Yayin[] = [];
  selectedOption: string = "";
  selectedFilter: string = '';
  filtreleMetin: boolean = false;
  inputFiltreleMetin: string = "";
  filtreleButtonText: string = "Filtrele";

  constructor(
    private searchService: SearchService
  ) {}

  ngOnInit(): void {
    this.loadYayinlar();
  }

  loadYayinlar() {
    this.errorMessage = "";
    this.successMessage = "";
    this.searchService.listeleHepsini().subscribe({
      next: (response) => {
        this.loading = false;
        this.buttonDisabled = false;
        if (response.proccess) {
          this.result = true;
          this.yayinListesi = response.yayinListesi;
        }
      },
      error: (err) => {
        console.log(err);
        this.errorMessage = err.error.message;
        this.buttonDisabled = false;
      }
    });
  }

  filtrele() {
    if (this.filtreleButtonText == "Filtrele") {


      if (this.selectedFilter === "ad") {
        this.searchService.filtreleYayinAdi(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "yazarlar") {
        this.searchService.filtreleYazarlar(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "tur") {
        this.searchService.filtreleTur(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "yayinciadi") {
        this.searchService.filtreleYayinciAdi(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "yayinlanmatarihienson") {
        this.searchService.filtreleYayinlanmaTarihiEnSon().subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "yayinlanmatarihienonce") {
        this.searchService.filtreleYayinlanmaTarihiEnOnce().subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }
      else if (this.selectedFilter === "yayinciadi") {
        this.searchService.filtreleYayinciAdi(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }
      else if (this.selectedFilter === "anahtarkelime") {
        this.searchService.filtreleAnahtarKelime(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "ozet") {
        this.searchService.filtreleOzet(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "alintisayisi") {
        this.searchService.filtreleAlintiSayisi(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "doinumarasi") {
        this.searchService.filtreleDoiNumarasi(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "url") {
        this.searchService.filtreleUrl(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "yazarlar") {
        this.searchService.filtreleYazarlar(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }else if (this.selectedFilter === "tur") {
        this.searchService.filtreleTur(this.inputFiltreleMetin).subscribe({
          next: (response: GetResponseYayin) => {
            this.filtreleButtonText = "Filtreyi kaldır";
            console.log(response);
            this.loading = false;
            this.buttonDisabled = false;
            if (response.proccess) {
              this.result = true;
              this.yayinListesi = response.yayinListesi;
            }
          },
          error: (err: GetResponseYayin) => {
            console.log(err);
            this.loading = false;
            this.errorMessage = err.error.message;
            this.buttonDisabled = false;
          }
        });
      }


    } else if (this.filtreleButtonText == "Filtreyi kaldır") {
      this.filtreleButtonText = "Filtrele";
      this.filtreleMetin=false;
      this.loadYayinlar();
    }
  }

  changeFilter() {
    if (
      this.selectedFilter === "ad" ||
      this.selectedFilter === "yazarlar" ||
      this.selectedFilter === "tur" ||
      this.selectedFilter === "yayinciadi" ||
      this.selectedFilter === "anahtarkelime" ||
      this.selectedFilter === "ozet" ||
      this.selectedFilter === "alintisayisi" ||
      this.selectedFilter === "doinumarasi" ||
      this.selectedFilter === "url"
    ) {
      this.filtreleMetin = true;
    } else {
      this.filtreleMetin = false;
    }
  }

  search(text: string): void {
    this.loading = true;
    this.errorMessage = "";
    this.successMessage = "";
    this.buttonDisabled = true;

    if (this.selectedOption == "dergipark") {
      this.searchService.dergiparksearch(text).subscribe({
        next: (response: GetResponseYayin) => {
          console.log(response);
          this.loading = false;
          this.buttonDisabled = false;
          if (response.proccess) {
            this.result = true;
            this.yayinListesi = response.yayinListesi;
          }
        },
        error: (err: GetResponseYayin) => {
          console.log(err);
          this.loading = false;
          this.errorMessage = err.error.message;
          this.buttonDisabled = false;
        }
      });
    } else if (this.selectedOption == "scholar") {
      this.searchService.search(text).subscribe({
        next: (response: GetResponseYayin) => {
          console.log(response);
          this.buttonDisabled = false;
          this.loading = false;
          if (response.proccess) {
            this.result = true;
            this.yayinListesi = response.yayinListesi;
          }
        },
        error: (err: GetResponseYayin) => {
          console.log(err);
          this.loading = false;
          this.errorMessage = err.error.message;
          this.buttonDisabled = false;
        }
      });
    } else {
      this.loading = false;
      this.errorMessage = "Api seçin";
      this.buttonDisabled = false;
    }
  }
}

interface GetResponseYayin {
  error: {
      proccess: boolean,
      message: string
    },
    yayinListesi: Yayin[],
    proccess: boolean
}
