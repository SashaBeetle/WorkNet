import { Store } from '@ngrx/store';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { FormsModule } from '@angular/forms';
import { filter, Observable, take } from 'rxjs';
import { User } from '../../../core/models/user.models';
import { selectCurrentUser } from '../../../ngrx/selectors/user.selectors';
import { Skill, Experience, Education, ProfileDtoPayload } from '../../../core/models/profile.models';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})

export class ProfileComponent {
  user$: Observable<User | null>;

  profileId: string | null = null; // Populate this if editing an existing profile (e.g., in ngOnInit)
  userId: string | null = null;    // Populate this from auth service (e.g., in ngOnInit)

  name: string = '';    // Will map to FirstName
  surname: string = ''; // Will map to LastName
  gender: string = '';  // String values "male", "female", "other"
  userType: string = '';// String values "hr", "seeker", maps to ProfileType

  location: string = '';
  title: string = '';   // Will map to Headline
  about: string = '';

  // Structured Professional Details (using updated interfaces)
  skills: Skill[] = [];
  experience: Experience[] = [];
  education: Education[] = [];

  profilePhotoId: string | null = null; // This will store the ID from the backend
  profilePhotoUrl: string | ArrayBuffer | null | undefined; // For preview & current photo
  selectedFile: File | null = null;
  isLoading: boolean = false; 

  successMessage = '';
  errorMessage = '';

  constructor(private apiService: ApiService, private store: Store) {
    this.user$ = this.store.select(selectCurrentUser);

    this.user$.pipe(
      filter((user): user is User => user !== null), // Ensure user is not null
      take(1)                                       // Get the value once for initialization
    ).subscribe(currentUser => {
      this.userId = currentUser.id;
    });

  }

  async ngOnInit(){
    this.loadProfileData(); 
  }

  call(){
    this.loadProfileData();
  }

  loadProfileData(): void {
    this.isLoading = true;
    this.successMessage = ''; // Clear previous messages
    this.errorMessage = '';

    this.apiService.get<ProfileDtoPayload>('profile').subscribe({
      next: (response) => {
        this.isLoading = false;

        this.profileId = response.id || null;
        this.name = response.firstName || '';
        this.surname = response.lastName || '';
        this.title = response.headline || '';
        this.about = response.about || '';
        this.location = response.location || '';

        if (response.gender) {
          this.gender = response.gender.toLowerCase();
        } else {
          this.gender = '';
        }

        // Map profileType from backend "Candidate", "Recruiter" to frontend "seeker", "hr"
        if (response.profileType) {
          switch (response.profileType) { // Assuming backend sends "Candidate" or "Recruiter"
            case 'Candidate':
              this.userType = 'seeker';
              break;
            case 'Recruiter':
              this.userType = 'hr';
              break;
            default:
              this.userType = ''; // Or handle as an unknown type
          }
        } else {
          this.userType = '';
        }

        this.profilePhotoId = response.profilePhotoId || null;
        if(this.profilePhotoId != null && this.profilePhotoId != undefined){
          this.profilePhotoUrl = 'https://drive.google.com/thumbnail?id=' + this.profilePhotoId;
        }
        // Populate structured data ensuring they are arrays
        this.skills = response.skills ? response.skills.map(s => ({ name: s.name })) : [];
        
        this.experience = response.experiences ? response.experiences.map(exp => ({
          id: exp.id || null,
          position: exp.position || '',
          company: exp.company || '',
          location: exp.location || '',
          // Dates from backend are expected to be strings (e.g., YYYY-MM-DD) or null
          startDate: exp.startDate,
          endDate: exp.endDate,
          description: exp.description || ''
        })) : [];
        
        this.education = response.educations ? response.educations.map(edu => ({
          id: edu.id || null,
          degree: edu.degree || '',
          institution: edu.institution || '',
          domain: edu.domain || '',
          major: edu.major || '',
          graduationYear: edu.graduationYear || null,
          description: edu.description || ''
        })) : [];

      },
      error: (err: any) => {
        this.isLoading = false;
        console.error('--- ERROR in loadProfileData ---');
        console.dir(err); // Use console.dir for detailed object logging
        if (err && err.message === undefined) {
          console.warn('CAUGHT ERROR WITH UNDEFINED MESSAGE in loadProfileData');
        }

        if (err.status === 404) {
          this.errorMessage = 'No profile found. You can create one now.';
          this.profileId = null;
        } else {
          // Try to get a meaningful message, defaulting if none found
          this.errorMessage = err?.error?.message || err?.message || 'Failed to load profile data.';
        }
        this.successMessage = '';
      }
    });
  }

  onFileSelected(event: Event): void {

    const element = event.currentTarget as HTMLInputElement;
    const fileList: FileList | null = element.files;

    if (fileList && fileList[0]) {
      this.selectedFile = fileList[0];
      this.errorMessage = ''; // Clear previous errors
      this.successMessage = ''; // Clear previous success messages

      // 1. Show a local preview of the selected image
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.profilePhotoUrl = e.target.result; 
      };
      reader.readAsDataURL(this.selectedFile);

    } else {
      this.selectedFile = null;
    }

    this.uploadSelectedPhoto()
    console.log(fileList);
    console.log(this.selectedFile);
  }

  uploadSelectedPhoto(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'No file selected to upload.';
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name); 

    this.successMessage = 'Uploading photo...';

    this.apiService.post('profile/upload-file', formData).subscribe({
      next: (response: any) => {
        if (response && response.result) {
          let fileIdFromResponse: string | null = null;

          if (typeof response.result === 'string') {
            fileIdFromResponse = response.result; // If 'result' is the ID string directly
          } else if (response.result.id) {
            fileIdFromResponse = response.result.id; // If 'result' is an object with an 'id' property
          }


          if (fileIdFromResponse) {
            this.profilePhotoId = fileIdFromResponse; // <<< Store the received file ID
            this.successMessage = 'Photo uploaded successfully! ID: ' + this.profilePhotoId;
            this.errorMessage = '';
            // The preview (this.profilePhotoUrl) is already showing the selected image.
            // If the backend returns a permanent URL (viewLink), you could update profilePhotoUrl to that:
            // if(viewLink) this.profilePhotoUrl = viewLink;
          } else {
            this.errorMessage = 'Photo uploaded, but its ID was not found in the server response.';
            this.successMessage = '';
          }
        } else {
          this.errorMessage = 'Invalid response received from server after photo upload.';
          this.successMessage = '';
        }
        this.selectedFile = null; // Clear the file selection after processing
      },
      error: (err: any) => {
        this.errorMessage = err?.error?.message || err?.error?.result || err?.message || 'Failed to upload photo.';
        this.successMessage = '';
        this.selectedFile = null; // Clear the file selection
        // Optionally, revert the preview to a default or previously saved image
        // this.profilePhotoUrl = 'assets/default-avatar.png';
      }
    });
  }

  addSkill(): void {
    this.skills.push({ name: '' }); // SkillDto only needs name from frontend
  }

  removeSkill(index: number): void {
    this.skills.splice(index, 1);
  }

  // --- Experience Management ---
  addExperience(): void {
    this.experience.push({
      id: null, // New experience items won't have an ID yet
      position: '', // Changed from 'title'
      company: '',
      location: '',
      startDate: null,
      endDate: null,
      description: ''
    });
  }

  removeExperience(index: number): void {
    this.experience.splice(index, 1);
  }

  // --- Education Management ---
  addEducation(): void {
    this.education.push({
      id: null, // New education items won't have an ID yet
      degree: '',
      institution: '',
      domain: '', // Changed from 'fieldOfStudy'
      major: '',  // Added, ensure your form has an input for this if needed
      graduationYear: null,
      description: ''
    });
  }

  removeEducation(index: number): void {
    this.education.splice(index, 1);
  }

  // Helper to convert date string to ISO YYYY-MM-DD or null
  private formatDateString(dateStr: string | undefined | null): string | null {
    if (!dateStr || dateStr.trim() === '' || dateStr.trim().toLowerCase() === 'present') {
      return null;
    }
    try {
      // Attempt to create a date. This is a basic conversion.
      // For more robust parsing of formats like "Jan 2020", you'd need a library like date-fns or moment.
      // Assuming input is YYYY-MM-DD or directly parsable by Date constructor for YYYY-MM-DD output.
      const date = new Date(dateStr);
      if (isNaN(date.getTime())) return null; // Invalid date string
      // Return date in YYYY-MM-DD format
      const year = date.getFullYear();
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      return `${year}-${month}-${day}`;
    } catch (e) {
      return null;
    }
  }

  onSubmit() {
    let profileTypeForPayload: string | null = null;
  if (this.userType && this.userType !== '') {
    switch (this.userType.toLowerCase()) { 
      case 'seeker': 
        profileTypeForPayload = 'Candidate'; 
        break;
      case 'hr':
        profileTypeForPayload = 'Recruiter'; 
        break;

    }
  }

    let genderForPayload: string | null = null;
  if (this.gender && this.gender !== '') {
    switch (this.gender.toLowerCase()) {
      case 'male':
        genderForPayload = 'Male'; // Assuming C# enum member is 'Male'
        break;
      case 'female':
        genderForPayload = 'Female'; // Assuming C# enum member is 'Female'
        break;
      case 'other':
        genderForPayload = 'Other'; // Assuming C# enum member is 'Other'
        break;
      default:
        genderForPayload = null; 
        break;
    }
  }

    const payload: ProfileDtoPayload = {
      id: this.profileId, // Include the profile's ID if updating
      userId: this.userId, // Include the logged-in user's ID

      firstName: this.name,
      lastName: this.surname,
      gender: genderForPayload, // Send null if not selected
      profileType: profileTypeForPayload, // Send null if not selected

      profilePhotoId: this.profilePhotoId,
      headline: this.title,
      about: this.about,
      location: this.location,

      skills: this.skills
        .filter(skill => skill.name && skill.name.trim() !== '')
        .map(skill => ({
          name: skill.name.trim()
          // ProfileId is not sent from client for SkillDto
        })),

      experiences: this.experience
        .filter(exp => exp.position && exp.position.trim() !== '') // Filter out empty entries
        .map(exp => ({
          id: exp.id || null, 
          position: exp.position,
          company: exp.company,
          location: exp.location,
          startDate: this.formatDateString(exp.startDate),
          endDate: this.formatDateString(exp.endDate),
          description: exp.description,
        })),

      educations: this.education
        .filter(edu => edu.degree && edu.degree.trim() !== '') // Filter out empty entries
        .map(edu => ({
          id: edu.id || null, // Send null if it's a new item without an ID
          degree: edu.degree,
          institution: edu.institution,
          domain: edu.domain,
          major: edu.major, // Add this to your form if you collect it
          graduationYear: edu.graduationYear ? Number(edu.graduationYear) : null,
          description: edu.description,
        })),
      
      files: [] 
    };

    this.apiService.put('profile', payload).subscribe({ // Change to POST for creation
      next: (response: any) => { // Backend might return the created profile with ID
        this.successMessage = 'Profile created successfully!';
        this.errorMessage = '';
        if (response && response.id) {
          this.profileId = response.id; // Store the new profile ID
        }
        // Potentially update other fields from response
      },
      error: err => {
        this.errorMessage = err?.error?.message || 'Failed to create profile.';
        this.successMessage = '';
      }
    });
  }
}