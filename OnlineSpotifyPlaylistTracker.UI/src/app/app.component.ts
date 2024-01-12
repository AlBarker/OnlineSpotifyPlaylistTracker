import { HttpClient } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component, ViewEncapsulation } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import ColorThief from 'colorthief/dist/color-thief.mjs'
import { BehaviorSubject, Subject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterViewInit {
  public tracks: any;

  public backgrounds: Record<number, Subject<string>> = {};
  private viewInit = false;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {
    this.http.get('https://192.168.0.3:7234/Track').subscribe((res) => this.tracks = res);
  }

  ngAfterViewInit() {
    this.viewInit = true;
  }
  
  public onImgLoad(pos: number) {
    const img = document.getElementById(pos.toString()) as HTMLImageElement;

    if (!img.complete) {
      return;
    }

    const colorThief = new ColorThief();
    const x = colorThief.getColor(img);
    const hex = this.rgbToHex(x[0], x[1], x[2]);
    const toColour = img.parentElement?.parentElement;
    if (toColour) {
      toColour.style.backgroundColor = hex;
      toColour.style.color = this.getContrastYIQ(hex);
    } 
    
    if (pos === 0) {
      const rootEl = document.getElementById('root');
      if (rootEl) {
        rootEl.style.backgroundColor = hex;
        rootEl.style.color = this.getContrastYIQ(hex);
      }
    }
  }

  private rgbToHex = (r: number, g: number, b: number) => '#' + [r, g, b].map(x => {
    const hex = x.toString(16)
    return hex.length === 1 ? '0' + hex : hex
  }).join('')

  private getContrastYIQ(hexcolor: string){
    hexcolor = hexcolor.replace("#", "");
    var r = parseInt(hexcolor.substr(0,2),16);
    var g = parseInt(hexcolor.substr(2,2),16);
    var b = parseInt(hexcolor.substr(4,2),16);
    var yiq = ((r*299)+(g*587)+(b*114))/1000;
    return (yiq >= 128) ? 'black' : 'white';
}
}
