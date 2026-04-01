import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateHistoryComponent } from '../components/create-history/create-history.component';
import { CreateHistoryDTO, HistoryResponseDTO, SearchHistoryDTO, UpdateHistoryDTO } from '../models/history';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {
  private baseUrl = 'http://20.235.201.170/api/History'

  constructor(private http: HttpClient) { }

  getHistoryIds():Observable<string[]>{
    return this.http.get<string[]>(`${this.baseUrl}/ids`);
  }

  // saveHistory(dto: CreateHistoryDTO):Observable<void>{
  //   return this.http.post<void>(`${this.baseUrl}/create`,dto);
  // }

  saveHistory(dto: CreateHistoryDTO):Observable<any>{
    return this.http.post(`${this.baseUrl}/create`,dto,{responseType: 'text'});
  }

  searchHistory(dto: SearchHistoryDTO): Observable<HistoryResponseDTO[]> {
    return this.http.post<HistoryResponseDTO[]>(`${this.baseUrl}/search`, dto);
  }

  updateHistory(dto: any): Observable<any>{
    return this,this.http.put(`${this.baseUrl}/update`,dto,{responseType:'text' as 'json'});
  }
}
