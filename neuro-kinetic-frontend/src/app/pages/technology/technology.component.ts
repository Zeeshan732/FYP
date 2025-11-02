import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-technology',
  templateUrl: './technology.component.html',
  styleUrls: ['./technology.component.scss']
})
export class TechnologyComponent implements OnInit {
  performanceMetrics: any[] = [];
  loading = false;

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadDashboardMetrics();
  }

  loadDashboardMetrics() {
    this.loading = true;
    this.apiService.getMetrics({ pageNumber: 1, pageSize: 5 }).subscribe({
      next: (response) => {
        this.performanceMetrics = response.items.slice(0, 5); // Top 5 for display
        this.loading = false;
      },
      error: (error: any) => {
        console.error('Error loading metrics:', error);
        this.loading = false;
      }
    });
  }

  navigateToMetrics() {
    this.router.navigate(['/metrics']);
  }

  navigateToCrossValidation() {
    this.router.navigate(['/cross-validation']);
  }

  navigateToDemo() {
    this.router.navigate(['/technology-demo']);
  }

  scrollToSection(sectionId: string) {
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  getDomainDropColor(value: number): string {
    if (value <= 0.05) return 'text-green-400';
    if (value <= 0.10) return 'text-yellow-400';
    return 'text-red-400';
  }

  // Helper for template
  Math = Math;
}
