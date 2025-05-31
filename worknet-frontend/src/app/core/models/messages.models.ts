export interface Message {
  from: string;
  text: string;
  time: string;
}

export interface Contact {
  name: string;
  avatar: string;
  lastMessage: string;
  messages: Message[];
}