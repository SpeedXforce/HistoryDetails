import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CreateHistoryComponent } from './components/create-history/create-history.component';
import { HistoryListComponent } from './components/history-list/history-list.component';

@Component({
  selector: 'app-root',
  imports: [HistoryListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'history-app';
}
