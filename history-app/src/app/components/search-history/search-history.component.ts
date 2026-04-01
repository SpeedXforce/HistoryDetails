import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators,ReactiveFormsModule,FormsModule } from '@angular/forms';
import { HistoryResponseDTO, SearchHistoryDTO } from '../../models/history';
import { HistoryService } from '../../services/history.service';
import { CommonModule } from '@angular/common';
import { timestamp } from 'rxjs';

@Component({
  selector: 'app-search-history',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './search-history.component.html',
  styleUrl: './search-history.component.scss'
})
export class SearchHistoryComponent {
searchForm!: FormGroup

historyIds: string[]=[];
minute: number[] = [0,15,30,45];

allResults: HistoryResponseDTO[] = [];
pagedResults: HistoryResponseDTO[] = [];

currentPage: number =1;
pageSize: number = 10;
totalPages: number = 0;
pages: number[] = [];

errorMessage: string = '';
isSearching: boolean = false;
hasSearched: boolean = false;

originalValues: {[index: number]: {statusTag:string,value:number}}={};

constructor(
private fb: FormBuilder,
private historyService: HistoryService
){}

ngOnInit(): void{
  this.buildForm();
  this.loadHistoryIds();
}

buildForm(): void{
  this.searchForm = this.fb.group({
    historyId: ['',Validators.required],
    fromDate: ['',Validators.required],
    toDate: ['',Validators.required],
    minute: ['', Validators.required]
  })
}

loadHistoryIds(): void{
  this.historyService.getHistoryIds().subscribe({
    next: (ids) => this.historyIds = ids,
    error: () => this.errorMessage = 'Failed to load IDs'
  });
}

onSearch(): void{
  if(this.searchForm.invalid){
    this.searchForm.markAllAsTouched();
    return;
  }

  this.isSearching = true;
  this.errorMessage = '';

  const formValue = this.searchForm.value;
  const dto: SearchHistoryDTO = {
    historyId: formValue.historyId,
    fromDate: formValue.fromDate,
    toDate: formValue.toDate,
    minute: Number(formValue.minute)
  };

  this.historyService.searchHistory(dto).subscribe({
    next: (results) => {
      this.allResults = results;
      this.currentPage = 1;
      this.setupPagination();
      this.isSearching = false;
      this.hasSearched = true;

      this.storeOriginalValues();
    },
    error: () => {
      this.errorMessage = 'Search failed!';
      this.isSearching = false;
      this.hasSearched = true;
    }
  });
}

setupPagination(): void{
  this.totalPages = Math.ceil(this.allResults.length/this.pageSize);
  this.pages = Array.from({length: this.totalPages},(_,i) => i+1);
  this.loadPage(1);
}

loadPage(page: number): void{
this.currentPage = page;
const start = (page-1)*this.pageSize;
const end =  start + this.pageSize;
this.pagedResults = this.allResults.slice(start,end);
}

storeOriginalValues(): void{
  this.allResults.forEach((row, index)=> {
    this.originalValues[index]={
      statusTag: row.statusTag,
      value: row.value
    };
  });
}

onFieldBlur(row: HistoryResponseDTO,index: number): void{
 const original = this.originalValues[index];

 const hasChanged = row.statusTag !== original.statusTag || row.value !==original.value;

 if(!hasChanged){
  return;
 }

 const dto = {
  id: row.id,
  status: row.statusTag,
  value: row.value
 }

 this.historyService.updateHistory(dto).subscribe({
  next:() =>{
    this.originalValues[index]={
        statusTag: row.statusTag,
        value: row.value
    };
    alert('Updated successfully!');
  },
  error: () => {
    row.statusTag = original.statusTag;
    row.value = original.value;
    alert('Update Failed');
  }
 });
}

goToPage(page: number): void{
if(page < 1 || page > this.totalPages) return;
this.loadPage(page);
}

get startRecord(): number{
  return ((this.currentPage - 1)*this.pageSize)+1;
}

get endRecord(): number{
  return Math.min(this.currentPage*this.pageSize, this.allResults.length);
}

formatMinute(m: number): string{
  return m<10?'0'+m:m.toString();
}

isInvalid(field: string): boolean{
  const control =  this.searchForm.get(field);
  return !!(control?.invalid && control?.touched)
}

}
