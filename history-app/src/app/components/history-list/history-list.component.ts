import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateHistoryComponent } from '../create-history/create-history.component';
import { CommonModule } from '@angular/common';
import { SearchHistoryComponent } from '../search-history/search-history.component';
import { ChatbotComponent } from '../chatbot/chatbot.component';

@Component({
  selector: 'app-history-list',
  imports: [CommonModule,SearchHistoryComponent,ChatbotComponent],
  templateUrl: './history-list.component.html',
  styleUrl: './history-list.component.scss'
})
export class HistoryListComponent {
  successMessage = '';

  constructor(private dialog:MatDialog){
  }

  openCreateDialog(): void{
    const dialogRef = this.dialog.open(CreateHistoryComponent, {
      width:'500px',
      disableClose:true
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result === 'saved'){
        this.successMessage = "History Saved Successfully";
        setTimeout(()=>{
          this.successMessage= '';
        },3000);
      }
    });
  }
}
