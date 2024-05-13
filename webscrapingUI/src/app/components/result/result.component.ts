import { Component, Input } from '@angular/core';
import { Yayin } from 'src/app/common/yayin';

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.css']
})
export class ResultComponent {
  @Input() yayinListesi: Yayin[] | undefined;
}
