import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Job } from '../../../core/models/jobs.models';

@Component({
  selector: 'app-jobs',
  imports: [CommonModule, FormsModule],
  templateUrl: './jobs.component.html',
  styleUrl: './jobs.component.scss'
})
export class JobsComponent {
jobs: Job[] = [
    {
      title: 'Frontend Developer',
      company: 'TechNova',
      location: 'San Francisco, CA',
      type: 'Full-time',
      description: 'Build responsive UI with Angular and Tailwind.'
    },
    {
      title: 'Backend Engineer',
      company: 'CodeWorks',
      location: 'Remote',
      type: 'Remote',
      description: 'Work with Node.js and PostgreSQL in a microservice architecture.'
    },
    {
      title: 'UX Designer',
      company: 'DesignHive',
      location: 'New York, NY',
      type: 'Part-time',
      description: 'Design intuitive user experiences for web and mobile.'
    }
  ];

  filters = {
    location: '',
    type: ''
  };

  filteredJobs: Job[] = [];

  constructor() {
    this.filteredJobs = this.jobs;
  }

  applyFilters() {
    this.filteredJobs = this.jobs.filter(job => {
      return (
        (!this.filters.location || job.location.toLowerCase().includes(this.filters.location.toLowerCase())) &&
        (!this.filters.type || job.type === this.filters.type)
      );
    });
  }
}
