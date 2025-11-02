import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Publication, PagedResult } from '../../models/api.models';

@Component({
  selector: 'app-publications',
  templateUrl: './publications.component.html',
  styleUrls: ['./publications.component.scss']
})
export class PublicationsComponent implements OnInit {
  publications: Publication[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  totalCount = 0;
  hasPrevious = false;
  hasNext = false;
  searchTerm = '';
  loading = false;
  error = '';
  sortBy = 'createdAt';
  sortOrder: 'asc' | 'desc' = 'desc';
  filterType: string = '';
  filterYear: number | null = null;
  availableYears: number[] = [];

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadPublications();
    // Load available years for filter
    this.loadAvailableYears();
  }

  loadAvailableYears() {
    // Get all publications to extract unique years
    this.apiService.getAllPublications().subscribe({
      next: (pubs: Publication[]) => {
        const years = pubs
          .map(pub => pub.year)
          .filter(year => year != null)
          .map(year => year!)
          .filter((year, index, self) => self.indexOf(year) === index)
          .sort((a, b) => b - a); // Sort descending
        this.availableYears = years;
      },
      error: (error) => {
        console.error('Error loading years:', error);
      }
    });
  }

  loadPublications() {
    this.loading = true;
    this.error = '';

    // Combine search term with filters
    let combinedSearch = this.searchTerm;
    if (this.filterType) {
      combinedSearch = combinedSearch 
        ? `${combinedSearch} type:${this.filterType}` 
        : `type:${this.filterType}`;
    }
    if (this.filterYear) {
      combinedSearch = combinedSearch 
        ? `${combinedSearch} year:${this.filterYear}` 
        : `year:${this.filterYear}`;
    }

    this.apiService.getPublications({
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder,
      searchTerm: combinedSearch || undefined
    }).subscribe({
      next: (response: PagedResult<Publication>) => {
        this.publications = response.items;
        this.currentPage = response.pageNumber;
        this.totalPages = response.totalPages;
        this.totalCount = response.totalCount;
        this.hasPrevious = response.hasPrevious;
        this.hasNext = response.hasNext;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading publications:', error);
        this.error = 'Failed to load publications. Please try again.';
        this.loading = false;
      }
    });
  }

  search() {
    this.currentPage = 1;
    this.loadPublications();
  }

  onSearchChange() {
    // Optional: Implement debounced search
    if (this.searchTerm.length === 0 || this.searchTerm.length >= 3) {
      this.search();
    }
  }

  onFilterChange() {
    this.currentPage = 1;
    this.loadPublications();
  }

  clearFilters() {
    this.filterType = '';
    this.filterYear = null;
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadPublications();
  }

  hasActiveFilters(): boolean {
    return !!(this.filterType || this.filterYear || this.searchTerm);
  }

  nextPage() {
    if (this.hasNext) {
      this.currentPage++;
      this.loadPublications();
    }
  }

  previousPage() {
    if (this.hasPrevious) {
      this.currentPage--;
      this.loadPublications();
    }
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadPublications();
    }
  }

  onSortChange(sortBy: string) {
    if (this.sortBy === sortBy) {
      // Toggle sort order if same column
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = sortBy;
      this.sortOrder = 'asc';
    }
    this.loadPublications();
  }

  viewPublication(id: number) {
    // Navigate to publication detail page (if implemented) or open link
    // For now, we can navigate to a detail page or open the publication link
    this.router.navigate(['/publications', id]);
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPages = Math.min(5, this.totalPages);
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPages - 1);
    
    if (endPage - startPage < maxPages - 1) {
      startPage = Math.max(1, endPage - maxPages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  // Helper methods for template
  getMin(value1: number, value2: number): number {
    return Math.min(value1, value2);
  }

  getMax(value1: number, value2: number): number {
    return Math.max(value1, value2);
  }
}

