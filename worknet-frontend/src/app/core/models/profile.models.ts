import { User } from "./user.models";

export interface Skill { // SkillDto does not have its own Id, Name is key within profile
  name: string;
}

export interface Experience { // Matches ExperienceDto structure more closely
  id: string | null; // Corresponds to ExperienceDto.Id, can be null for new items
  position: string;  // Was 'title' in Angular, maps to 'Position' in DTO
  company: string;
  location?: string;
  startDate?: string | null; // Store as string, convert/validate before sending
  endDate?: string | null;
  description?: string;
}

export interface Education { 
  id: string | null; // Corresponds to EducationDto.Id, can be null for new items
  degree: string;
  institution: string;
  domain?: string;       // Was 'fieldOfStudy', maps to 'Domain' in DTO
  major?: string;        // Added to match EducationDto if needed in form
  graduationYear?: number | null; 
  description?: string;
}

export interface SkillDto {
  name: string;
}

export interface EducationDto {
  id?: string | null;
  degree?: string;
  institution?: string;
  domain?: string;
  major?: string;
  graduationYear?: number | null;
  description?: string;
  // ProfileId is set by backend
}

export interface ExperienceDto {
  id?: string | null;
  position?: string;
  company?: string;
  location?: string;
  startDate?: string | null; 
  endDate?: string | null;   
  description?: string;
}

export interface ProfileDtoPayload {
  id?: string | null;
  profileType?: string | null; 
  firstName?: string;
  lastName?: string;
  gender?: string | null;      
  headline?: string;
  about?: string;
  location?: string;
  profilePhotoId?: string | null;
  userId?: string | null;
  skills?: SkillDto[];
  educations?: EducationDto[];
  experiences?: ExperienceDto[];
  files?: any[]; 
}

export interface Profile{
    id: string
    profileType: string;
    firstName: string;
    lastName: string;
    gender: string;
    headline: string;
    about: string;
    location: string;
    profilePhotoId: string;
    createdAt: string;

    skills: Skill[];
    user: User;
}