import { Component } from '@angular/core';
import { ChatMessage, ChatService } from '../../services/chat.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chatbot',
  imports: [FormsModule, CommonModule],
  templateUrl: './chatbot.component.html',
  styleUrl: './chatbot.component.scss'
})
export class ChatbotComponent {
messages: ChatMessage[] = [];
userInput = '';
isLoading = false;
isOpen = false;

constructor(private chatService: ChatService){
  this.messages.push({
    role: 'assistant',
    content: 'Hi! I am your History Assistant 🤖 How can I help you today?'
  });
}

toggleChat(): void{
  this.isOpen = !this.isOpen;
}

sendMessage(): void{
  if(!this.userInput.trim() || this.isLoading) return;

  const userMessage  = this.userInput.trim();
  this.userInput = '';

  this.messages.push({
    role:'user',
    content: userMessage
  });

  this.isLoading = true;
  this.scrollToBottom();

  this.chatService.sendMessage({
    userMessage: userMessage,
    history:[...this.messages.slice(0,-1)]
  }).subscribe({
    next: (response)=> {
      this.messages.push({
        role:'assistant',
        content: response.message
      });
      this.isLoading = false;
      this.scrollToBottom();
    },
    error:()=>{
      this.messages.push({
        role:'assistant',
        content:'Something went wrong please try again'
      });
      this.isLoading=false;
    }
  });
}

onKeyPress(event: KeyboardEvent): void{
  if(event.key ==='Enter'){
    this.sendMessage();
  }
}

scrollToBottom(): void{
  setTimeout(()=>{
    const chatBody = document.querySelector('.chat-body');
    if(chatBody){
      chatBody.scrollTop = chatBody.scrollHeight;
    }
  },100);

}

}




