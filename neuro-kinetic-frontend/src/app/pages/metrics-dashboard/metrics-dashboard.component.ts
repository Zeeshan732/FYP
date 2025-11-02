import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { PerformanceMetric, PagedResult } from '../../models/api.models';

@Component({
  selector: 'app-metrics-dashboard',
  templateUrl: './metrics-dashboard.component.html',
  styleUrls: ['./metrics-dashboard.component.scss']
})
export class MetricsDashboardComponent implements OnInit {
  metrics: PerformanceMetric[] = [];
  loading = false;
  error = '';
  
  // Pagination
  currentPage = 1;
  pageSize = 20;
  totalPages = 0;
  totalCount = 0;
  hasPrevious = false;
  hasNext = false;

  // Filters
  selectedDataset: string = '';
  datasets: string[] = [];
  sortBy: string = 'createdAt';
  sortOrder: 'asc' | 'desc' = 'desc';

  // Statistics
  avgAccuracy: number = 0;
  avgPrecision: number = 0;
  avgRecall: number = 0;
  avgF1Score: number = 0;
  avgDomainDrop: number = 0;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadMetrics();
    this.loadDatasets();
  }

  loadMetrics() {
    this.loading = true;
    this.error = '';

    const params: any = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder
    };

    const observable = this.selectedDataset
      ? this.apiService.getMetricsByDataset(this.selectedDataset, params)
      : this.apiService.getMetrics(params);

    observable.subscribe({
      next: (response: PagedResult<PerformanceMetric>) => {
        this.metrics = response.items;
        this.currentPage = response.pageNumber;
        this.totalPages = response.totalPages;
        this.totalCount = response.totalCount;
        this.hasPrevious = response.hasPrevious;
        this.hasNext = response.hasNext;
        this.calculateStatistics();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading metrics:', error);
        this.error = 'Failed to load performance metrics. Please try again.';
        this.loading = false;
      }
    });
  }

  loadDatasets() {
    // Load unique datasets from metrics
    this.apiService.getMetrics().subscribe({
      next: (response: PagedResult<PerformanceMetric>) => {
        const allMetrics = response.items;
        const uniqueDatasets = Array.from(
          new Set(allMetrics.map(m => m.dataset))
        ).filter(d => d).sort();
        this.datasets = uniqueDatasets;
      },
      error: (error) => {
        console.error('Error loading datasets:', error);
      }
    });
  }

  calculateStatistics() {
    if (this.metrics.length === 0) return;

    const metricsWithValues = this.metrics.filter(m => 
      m.accuracy != null && m.precision != null && m.recall != null && m.f1Score != null
    );

    if (metricsWithValues.length === 0) return;

    this.avgAccuracy = this.calculateAverage(metricsWithValues.map(m => m.accuracy));
    this.avgPrecision = this.calculateAverage(metricsWithValues.map(m => m.precision));
    this.avgRecall = this.calculateAverage(metricsWithValues.map(m => m.recall));
    this.avgF1Score = this.calculateAverage(metricsWithValues.map(m => m.f1Score));

    const domainDrops = this.metrics
      .filter(m => m.domainAdaptationDrop != null)
      .map(m => m.domainAdaptationDrop!);
    
    if (domainDrops.length > 0) {
      this.avgDomainDrop = this.calculateAverage(domainDrops);
    }
  }

  calculateAverage(values: number[]): number {
    if (values.length === 0) return 0;
    const sum = values.reduce((a, b) => a + b, 0);
    return Math.round((sum / values.length) * 100) / 100;
  }

  onDatasetChange() {
    this.currentPage = 1;
    this.loadMetrics();
  }

  onSortChange(sortBy: string) {
    if (this.sortBy === sortBy) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = sortBy;
      this.sortOrder = 'asc';
    }
    this.loadMetrics();
  }

  nextPage() {
    if (this.hasNext) {
      this.currentPage++;
      this.loadMetrics();
    }
  }

  previousPage() {
    if (this.hasPrevious) {
      this.currentPage--;
      this.loadMetrics();
    }
  }

  clearFilter() {
    this.selectedDataset = '';
    this.currentPage = 1;
    this.loadMetrics();
  }

  getMetricPercentage(value: number): number {
    return Math.round(value * 100);
  }

  getDomainDropColor(value: number): string {
    if (value <= 0.05) return 'text-green-400'; // <5% is excellent
    if (value <= 0.10) return 'text-yellow-400'; // <10% is good
    return 'text-red-400'; // >10% needs improvement
  }

  getMin(value1: number, value2: number): number {
    return Math.min(value1, value2);
  }
}

