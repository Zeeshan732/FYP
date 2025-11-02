import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Publication, PagedResult } from '../../models/api.models';

@Component({
  selector: 'app-research',
  templateUrl: './research.component.html',
  styleUrls: ['./research.component.scss']
})
export class ResearchComponent implements OnInit {
  featuredPublications: Publication[] = [];
  loading = false;
  error = '';

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadFeaturedPublications();
  }

  loadFeaturedPublications() {
    this.loading = true;
    this.error = '';

    this.apiService.getFeaturedPublications({
      pageNumber: 1,
      pageSize: 6,
      sortBy: 'createdAt',
      sortOrder: 'desc'
    }).subscribe({
      next: (response: PagedResult<Publication>) => {
        this.featuredPublications = response.items;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading featured publications:', error);
        this.error = 'Failed to load publications.';
        this.loading = false;
      }
    });
  }

  viewPublication(id: number) {
    this.router.navigate(['/publications', id]);
  }

  viewAllPublications() {
    this.router.navigate(['/publications']);
  }
}
