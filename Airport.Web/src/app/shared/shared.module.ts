import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { RouterModule } from '@angular/router';
import { FormatTimeSpanPipe } from './pipes/format-time-span.pipe';

@NgModule({
  declarations: [HeaderComponent, FooterComponent, FormatTimeSpanPipe],
  imports: [CommonModule, RouterModule],
  exports: [HeaderComponent, FooterComponent, FormatTimeSpanPipe],
})
export class SharedModule {}
