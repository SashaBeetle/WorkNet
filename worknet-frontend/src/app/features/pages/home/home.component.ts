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
import { Post, PostPayload } from '../../../core/models/post.models';
import { selectPostsData } from '../../../ngrx/selectors/post.selectors';
import * as PostActions from '../../../ngrx/actions/post.actions';
import { Router } from '@angular/router';
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
  posts$: Observable<Post[] | null>
  isMoreInfoVisible = false;
  currentUserName: string = '';
  newPostContent: string = '';

  constructor(private store: Store, private apiService: ApiService, private router: Router) {
    this.profile$ = this.store.select(selectProfileData);
    this.posts$ = this.store.select(selectPostsData)
  }

  ngOnInit(){

    this.store.dispatch(ProfileActions.loadProfile());
    this.store.dispatch(PostActions.loadPosts());

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
          // console.log('Posts fetched and transformed successfully:', this.posts);
        },
        error: (error) => {
          console.error('Error fetching posts:', error);
        }
      });

  }
addPost(): void {

const payload = {
  data: this.newPostContent
};

this.apiService.post<PostDto>('post', payload) 
      .subscribe({
        next: (fetchedPost) => {
          //this.posts.push(fetchedPost);
          // console.log('Post fetched and transformed successfully:', this.posts);
        },
        error: (error) => {
          console.error('Error fetching posts:', error);
        }
      });

  // Обрізаємо пробіли
  // const trimmedContent = this.newPostContent.trim();

  // // Якщо пост порожній — не додаємо
  // if (!trimmedContent) return;

  // const newPost = {
  //   user: {
  //     id: 'current-user', // замініть, якщо маєте ID користувача
  //     userName: this.currentUserName || 'Anonymous',
  //     email: '',
  //     img: this.profileUrl || `https://ui-avatars.com/api/?name=${this.currentUserName || 'User'}`
  //   },
  //   createdAt: new Date().toISOString(),
  //   data: trimmedContent
  // };

  // // Додаємо пост в початок масиву (як у соцмережах)
  // this.posts.unshift(newPost);

  // // Очищаємо textarea
  this.newPostContent = '';
  window.location.reload();
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
}


