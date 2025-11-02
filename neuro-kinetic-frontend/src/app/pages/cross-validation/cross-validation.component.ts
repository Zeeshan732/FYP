import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CrossValidationResult } from '../../models/api.models';

@Component({
  selector: 'app-cross-validation',
  templateUrl: './cross-validation.component.html',
  styleUrls: ['./cross-validation.component.scss']
})
export class CrossValidationComponent implements OnInit {
  results: CrossValidationResult[] = [];
  loading = false;
  error = '';
  
  // Filters
  selectedDataset: string = '';
  datasets: string[] = [];
  
  // Statistics
  avgAccuracy: number = 0;
  avgPrecision: number = 0;
  avgRecall: number = 0;
  avgF1Score: number = 0;
  avgDomainDrop: number = 0;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadCrossValidationResults();
    this.loadDatasets();
  }

  loadCrossValidationResults() {
    this.loading = true;
    this.error = '';

    const observable = this.selectedDataset
      ? this.apiService.getCrossValidationByDataset(this.selectedDataset)
      : this.apiService.getCrossValidationResults();

    observable.subscribe({
      next: (response: any) => {
        // Handle both PagedResult and array responses
        if (response.items) {
          this.results = response.items;
        } else if (Array.isArray(response)) {
          this.results = response;
        } else {
          this.results = [];
        }
        this.calculateStatistics();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading cross-validation results:', error);
        this.error = 'Failed to load cross-validation results. Please try again.';
        this.loading = false;
      }
    });
  }

  loadDatasets() {
    // Get unique datasets from results
    this.apiService.getAllCrossValidationResults().subscribe({
      next: (results: CrossValidationResult[]) => {
        const uniqueDatasets = Array.from(
          new Set(results.map(r => r.datasetName))
        ).filter(d => d).sort();
        this.datasets = uniqueDatasets;
      },
      error: (error) => {
        console.error('Error loading datasets:', error);
      }
    });
  }

  calculateStatistics() {
    if (this.results.length === 0) return;

    const resultsWithValues = this.results.filter(r => 
      r.accuracy != null && r.precision != null && r.recall != null && r.f1Score != null
    );

    if (resultsWithValues.length === 0) return;

    this.avgAccuracy = this.calculateAverage(resultsWithValues.map(r => r.accuracy));
    this.avgPrecision = this.calculateAverage(resultsWithValues.map(r => r.precision));
    this.avgRecall = this.calculateAverage(resultsWithValues.map(r => r.recall));
    this.avgF1Score = this.calculateAverage(resultsWithValues.map(r => r.f1Score));

    const domainDrops = this.results
      .filter(r => r.domainAdaptationDrop != null)
      .map(r => r.domainAdaptationDrop!);
    
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
    this.loadCrossValidationResults();
  }

  clearFilter() {
    this.selectedDataset = '';
    this.loadCrossValidationResults();
  }

  getMetricPercentage(value: number): number {
    return Math.round(value * 100);
  }

  getDomainDropColor(value: number): string {
    if (value <= 0.05) return 'text-green-400'; // <5% is excellent
    if (value <= 0.10) return 'text-yellow-400'; // <10% is good
    return 'text-red-400'; // >10% needs improvement
  }

  groupByFold(): { [key: number]: CrossValidationResult[] } {
    const grouped: { [key: number]: CrossValidationResult[] } = {};
    this.results.forEach(result => {
      const fold = result.foldNumber || 0;
      if (!grouped[fold]) {
        grouped[fold] = [];
      }
      grouped[fold].push(result);
    });
    return grouped;
  }

  getFoldNumbers(): number[] {
    const folds = Array.from(new Set(this.results.map(r => r.foldNumber).filter(f => f != null))) as number[];
    return folds.sort((a, b) => a - b);
  }

  getResultsByFold(fold: number): CrossValidationResult[] {
    return this.results.filter(r => r.foldNumber === fold);
  }
}

