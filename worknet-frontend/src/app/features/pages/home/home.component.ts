import { Component } from '@angular/core';
import { trigger, style, transition, animate } from '@angular/animations';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
    animations: [
    trigger('slideToggle', [
      transition(':enter', [
        style({ height: '0', opacity: 0, overflow: 'hidden' }),
        animate('300ms ease-out', style({ height: '*', opacity: 1 }))
      ]),
      transition(':leave', [
        style({ overflow: 'hidden' }),
        animate('300ms ease-in', style({ height: '0', opacity: 0 }))
      ])
    ])
  ]
})
export class HomeComponent {

  isMoreInfoVisible = false;

  user = {
    name: "John",
    surname: "Doe"
  }
 posts = [
    {
      name: 'Jane Smith',
      time: '2 hours ago',
      content: 'This is an example post to mimic LinkedIn’s feed.',
    },
    {
      name: 'Alex Johnson',
      time: '5 hours ago',
      content: 'Just launched my new project! 🚀',
    },
  ];
}
