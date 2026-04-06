import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ChatMessage{
  role: 'user' | "assistant";
  content: string;
}

export interface ChatRequest{
  userMessage: string;
  history: ChatMessage[];
}

export interface ChatResponse{
  message: string;
  isSuccess: boolean
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private baseUrl = 'http://4.213.42.55/api/Chat'


  constructor(private http: HttpClient) { }

  sendMessage(request: ChatRequest): Observable<ChatResponse>{
    return this.http.post<ChatResponse>(
      `${this.baseUrl}/message`,
      request
    )
  }
}
