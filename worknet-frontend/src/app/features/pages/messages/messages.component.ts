import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Contact } from '../../../core/models/messages.models';
@Component({
  selector: 'app-messages',
  imports: [CommonModule, FormsModule],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.scss'
})
export class MessagesComponent {
  contacts: Contact[] = [
    {
      name: 'Alice Johnson',
      avatar: 'https://ui-avatars.com/api/?name=Alice+Johnson',
      lastMessage: 'Hey! How are you?',
      messages: [
        { from: 'Alice', text: 'Hey! How are you?', time: '10:00 AM' },
        { from: 'me', text: 'Good, thanks!', time: '10:01 AM' }
      ]
    },
    {
      name: 'Bob Smith',
      avatar: 'https://ui-avatars.com/api/?name=Bob+Smith',
      lastMessage: 'Got the update?',
      messages: [
        { from: 'Bob', text: 'Got the update?', time: '9:30 AM' },
        { from: 'me', text: 'Yes, looks great!', time: '9:32 AM' }
      ]
    }
  ];

  selectedContact: Contact | null = null;
  newMessage: string = '';

  sendMessage(): void {
  if (this.selectedContact && this.newMessage.trim()) {
    this.selectedContact.messages.push({
      from: 'me',
      text: this.newMessage,
      time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
    });
    this.newMessage = '';
  }
}

selectContact(contact: Contact): void {
  this.selectedContact = contact;
}
}
