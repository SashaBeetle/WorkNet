import { Component} from '@angular/core';
import { trigger, style, transition, animate } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Profile } from '../../../core/models/profile.models';
import { selectProfileData } from '../../../ngrx/selectors/profile.selectors';
import { Store } from '@ngrx/store';
import * as ProfileActions from '../../../ngrx/actions/profile.actions'; 
import { buildProfileImageUrl } from '../../../core/Helpers/url-builder';
import { DatePipe } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { User } from '../../../core/models/user.models';
import { FormsModule } from '@angular/forms';

export interface PostDto {
  id?: string;
  createdAt?: string;     // Dates from JSON are typically strings (ISO 8601 format)
  data?: string;          // This is the actual content of the post
  userId?: string;
  user?: User;      // Nested user object
}

export interface DisplayPost {
  id?: string;
  name: string;           // e.g., "Jane Smith"
  time?: string;           // e.g., "2 hours ago" or formatted date
  content: string;
  avatarUrl?: string;      // URL for the author's avatar
  originalData: PostDto;  // Keep a reference to the original DTO if needed later
}


@Component({
  selector: 'app-home',
  imports: [CommonModule, DatePipe, FormsModule],
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
  profile$: Observable<Profile | null>;
  profileUrl: string | undefined;
  isMoreInfoVisible = false;
  newPostContent: string = '';
  currentUserName: string = '';

  constructor(private store: Store, private apiService: ApiService) {

    this.profile$ = this.store.select(selectProfileData);
    
  }

  ngOnInit(){

    this.store.dispatch(ProfileActions.loadProfile());

    this.profile$.subscribe(profileData => {
      console.log('Profile data from NgRx:', profileData);
      this.currentUserName = '' + profileData?.firstName + profileData?.lastName 
      this.profileUrl = buildProfileImageUrl(
        profileData?.profilePhotoId, 
        profileData?.user?.userName, 
        profileData?.firstName, 
        profileData?.lastName
      )
    });

   this.apiService.get<PostDto[]>('post') // Using your ApiService. 'post' will be appended to your baseUrl.
      .subscribe({
        next: (fetchedBackendPosts) => {
          // Transform PostDto[] from backend to DisplayPost[] for the template
          //this.posts = fetchedBackendPosts.map(dto => this.transformToDisplayPost(dto));
          console.log('Posts fetched and transformed successfully:', this.posts);
        },
        error: (error) => {
          console.error('Error fetching posts:', error);
        }
      });

  }
addPost(): void {
  // Обрізаємо пробіли
  const trimmedContent = this.newPostContent.trim();

  // Якщо пост порожній — не додаємо
  if (!trimmedContent) return;

  const newPost = {
    user: {
      id: 'current-user', // замініть, якщо маєте ID користувача
      userName: this.currentUserName || 'Anonymous',
      email: '',
      img: this.profileUrl || `https://ui-avatars.com/api/?name=${this.currentUserName || 'User'}`
    },
    createdAt: new Date().toISOString(),
    data: trimmedContent
  };

  // Додаємо пост в початок масиву (як у соцмережах)
  this.posts.unshift(newPost);

  // Очищаємо textarea
  this.newPostContent = '';
}
  private transformToDisplayPost(dto: PostDto): DisplayPost {
    let authorName = 'Unknown User';
    let authorAvatar = `https://ui-avatars.com/api/?name=Unknown+User&background=random&color=fff&rounded=true&size=40`;

    // if (dto.user) {
    //   if (dto.user.firstName && dto.user.lastName) {
    //     authorName = `${dto.user.firstName} ${dto.user.lastName}`;
    //   } else if (dto.user.userName) {
    //     authorName = dto.user.userName;
    //   }
    //   // Assuming PostAuthor might have profileImageUrl
    //   authorAvatar = dto.user || `https://ui-avatars.com/api/?name=${encodeURIComponent(authorName)}&background=random&color=fff&rounded=true&size=40`;
    // }

    return {
      id: dto.id,
      name: authorName,
      time: dto.createdAt,
      content: dto.data || '',
      avatarUrl: authorAvatar,
      originalData: dto
    };
  }

 
  posts = [
      {
        user: {
          id: "user123",
          userName: "SashaBeetle",
          email: '',
          img: "https://drive.google.com/thumbnail?id=1k_x-W5rTqvL9SwjsTiWcHBBbWP8OSEjp"

        },
        createdAt: "2024-06-04T10:30:00Z",
        data: "Щойно завершив роботу над новим функціоналом для WorkNet! Дуже задоволений результатом. #angular #dotnet #worknet"
      },
      {
        user: {
          id: "user456",
          userName: "DevCommunity",
          email: '',
          img: "https://img.freepik.com/premium-photo/3d-avatar-cartoon-character_113255-93283.jpg"

        },
        createdAt: "2024-06-03T15:45:10Z",
        data: "Обговорюємо найкращі практики для створення професійних мережевих платформ. Які ваші думки щодо WorkNet?"
      },
      {
        user: {
          id: "user789",
          userName: "TechRecruiterUA",
          email: '',
          img: "https://ui-avatars.com/api/?name=T"
        },
        createdAt: "2024-06-02T09:15:00Z",
        data: "Шукаю талановитих розробників для цікавого проекту. WorkNet виглядає як чудове місце для пошуку кандидатів! #recruiting #ITjobs"
      },
      {
        user: {
          id: "user101",
          userName: "JaneDoeDev",
          email: '',
          img: "https://ui-avatars.com/api/?name=A+B"
        },
        createdAt: "2024-05-30T18:00:00Z",
        data: "Ділюся своїм досвідом використання Angular та NgRx для управління станом у великих додатках. Сподіваюся, це буде корисно для спільноти WorkNet."
      }
    ];

    
}
//  posts = [
//     {
//       name: 'Jane Smith',
//       time: '2 hours ago',
//       content: 'This is an example post to mimic LinkedIn’s feed.',
//     },
//     {
//       name: 'Alex Johnson',
//       time: '5 hours ago',
//       content: 'Just launched my new project! 🚀',
//     },
//   ];

