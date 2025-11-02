import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { AdminDashboardAnalytics, UsageStatistic } from '../../models/api.models';

declare var Chart: any;

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  analytics: AdminDashboardAnalytics | null = null;
  loading = false;
  error = '';
  
  // Chart references
  @ViewChild('usageByDayChart') usageByDayChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('usageByMonthChart') usageByMonthChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('usageByYearChart') usageByYearChartRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('resultDistributionChart') resultDistributionChartRef!: ElementRef<HTMLCanvasElement>;

  charts: any[] = [];

  constructor(
    private apiService: ApiService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    // Check if user is admin
    this.authService.currentUser$.subscribe(user => {
      if (user?.role !== 'Admin') {
        this.router.navigate(['/home']);
      } else {
        this.loadAnalytics();
      }
    });
  }

  loadAnalytics() {
    this.loading = true;
    this.error = '';

    // Fetch real analytics from backend API
    this.apiService.getAdminDashboardAnalytics().subscribe({
      next: (data: AdminDashboardAnalytics) => {
        this.analytics = data;
        this.loading = false;
        
        // Wait for view to initialize charts
        setTimeout(() => {
          this.initCharts();
        }, 100);
      },
      error: (error) => {
        console.error('Error loading analytics:', error);
        // Fallback to dummy data if API fails
        if (error.status === 404 || error.status === 0) {
          console.warn('Analytics endpoint not available, using dummy data');
          this.analytics = this.getDummyAnalytics();
          this.loading = false;
          setTimeout(() => {
            this.initCharts();
          }, 100);
        } else {
          this.error = 'Failed to load analytics. Please try again.';
          this.loading = false;
        }
      }
    });
  }

  initCharts() {
    if (!this.analytics) return;

    // Destroy existing charts
    this.charts.forEach(chart => chart.destroy());
    this.charts = [];

    // Check if Chart.js is loaded
    if (typeof (window as any).Chart === 'undefined') {
      console.error('Chart.js is not loaded. Please check the CDN script.');
      return;
    }

    const Chart = (window as any).Chart;

    // Usage by Day Chart
    if (this.usageByDayChartRef) {
      const ctx = this.usageByDayChartRef.nativeElement.getContext('2d');
      const chart = new Chart(ctx, {
        type: 'line',
        data: {
          labels: this.analytics.usageByDay.map((u: UsageStatistic) => u.label),
          datasets: [{
            label: 'Tests per Day',
            data: this.analytics.usageByDay.map((u: UsageStatistic) => u.count),
            borderColor: 'rgb(16, 185, 129)',
            backgroundColor: 'rgba(16, 185, 129, 0.1)',
            tension: 0.4,
            fill: true
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              labels: { color: 'rgb(209, 213, 219)' }
            }
          },
          scales: {
            x: { ticks: { color: 'rgb(209, 213, 219)' }, grid: { color: 'rgba(255, 255, 255, 0.1)' } },
            y: { ticks: { color: 'rgb(209, 213, 219)' }, grid: { color: 'rgba(255, 255, 255, 0.1)' } }
          }
        }
      });
      this.charts.push(chart);
    }

    // Usage by Month Chart
    if (this.usageByMonthChartRef) {
      const ctx = this.usageByMonthChartRef.nativeElement.getContext('2d');
      const chart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: this.analytics.usageByMonth.map((u: UsageStatistic) => u.label),
          datasets: [{
            label: 'Tests per Month',
            data: this.analytics.usageByMonth.map((u: UsageStatistic) => u.count),
            backgroundColor: 'rgba(16, 185, 129, 0.8)',
            borderColor: 'rgb(16, 185, 129)',
            borderWidth: 1
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              labels: { color: 'rgb(209, 213, 219)' }
            }
          },
          scales: {
            x: { ticks: { color: 'rgb(209, 213, 219)' }, grid: { color: 'rgba(255, 255, 255, 0.1)' } },
            y: { ticks: { color: 'rgb(209, 213, 219)' }, grid: { color: 'rgba(255, 255, 255, 0.1)' } }
          }
        }
      });
      this.charts.push(chart);
    }

    // Usage by Year Chart
    if (this.usageByYearChartRef) {
      const ctx = this.usageByYearChartRef.nativeElement.getContext('2d');
      const chart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: this.analytics.usageByYear.map((u: UsageStatistic) => u.label),
          datasets: [{
            label: 'Tests per Year',
            data: this.analytics.usageByYear.map((u: UsageStatistic) => u.count),
            backgroundColor: 'rgba(59, 130, 246, 0.8)',
            borderColor: 'rgb(59, 130, 246)',
            borderWidth: 1
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              labels: { color: 'rgb(209, 213, 219)' }
            }
          },
          scales: {
            x: { ticks: { color: 'rgb(209, 213, 219)' }, grid: { color: 'rgba(255, 255, 255, 0.1)' } },
            y: { ticks: { color: 'rgb(209, 213, 219)' }, grid: { color: 'rgba(255, 255, 255, 0.1)' } }
          }
        }
      });
      this.charts.push(chart);
    }

    // Result Distribution Chart
    if (this.resultDistributionChartRef) {
      const ctx = this.resultDistributionChartRef.nativeElement.getContext('2d');
      const chart = new Chart(ctx, {
        type: 'doughnut',
        data: {
          labels: ['Positive', 'Negative', 'Uncertain'],
          datasets: [{
            data: [
              this.analytics.testResultsDistribution.positive,
              this.analytics.testResultsDistribution.negative,
              this.analytics.testResultsDistribution.uncertain
            ],
            backgroundColor: [
              'rgba(239, 68, 68, 0.8)',
              'rgba(34, 197, 94, 0.8)',
              'rgba(234, 179, 8, 0.8)'
            ],
            borderColor: [
              'rgb(239, 68, 68)',
              'rgb(34, 197, 94)',
              'rgb(234, 179, 8)'
            ],
            borderWidth: 2
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              position: 'bottom',
              labels: { color: 'rgb(209, 213, 219)' }
            }
          }
        }
      });
      this.charts.push(chart);
    }
  }

  // Dummy data generator - fallback when API is unavailable
  getDummyAnalytics(): AdminDashboardAnalytics {
    const now = new Date();
    const days: any[] = [];
    const months: any[] = [];
    const years: any[] = [];

    // Generate last 30 days
    for (let i = 29; i >= 0; i--) {
      const date = new Date(now);
      date.setDate(date.getDate() - i);
      days.push({
        date: date.toISOString(),
        count: Math.floor(Math.random() * 50) + 10,
        label: date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' })
      });
    }

    // Generate last 12 months
    for (let i = 11; i >= 0; i--) {
      const date = new Date(now);
      date.setMonth(date.getMonth() - i);
      months.push({
        date: date.toISOString(),
        count: Math.floor(Math.random() * 200) + 100,
        label: date.toLocaleDateString('en-US', { month: 'short', year: 'numeric' })
      });
    }

    // Generate last 5 years
    for (let i = 4; i >= 0; i--) {
      const date = new Date(now);
      date.setFullYear(date.getFullYear() - i);
      years.push({
        date: date.toISOString(),
        count: Math.floor(Math.random() * 2000) + 1000,
        label: date.getFullYear().toString()
      });
    }

    return {
      totalUsers: 1250,
      totalTests: 3420,
      positiveCases: 856,
      negativeCases: 2314,
      uncertainCases: 250,
      averageAccuracy: 82.5,
      usageByDay: days,
      usageByMonth: months,
      usageByYear: years,
      recentTests: [],
      testResultsDistribution: {
        positive: 856,
        negative: 2314,
        uncertain: 250
      }
    };
  }

  refreshData() {
    this.loadAnalytics();
  }

  navigateToTestRecords() {
    this.router.navigate(['/test-records']);
  }

  getPositivePercentage(): number {
    if (!this.analytics || this.analytics.totalTests === 0) return 0;
    return Math.round((this.analytics.positiveCases / this.analytics.totalTests) * 100);
  }

  getNegativePercentage(): number {
    if (!this.analytics || this.analytics.totalTests === 0) return 0;
    return Math.round((this.analytics.negativeCases / this.analytics.totalTests) * 100);
  }

  getUncertainPercentage(): number {
    if (!this.analytics || this.analytics.totalTests === 0) return 0;
    return Math.round((this.analytics.uncertainCases / this.analytics.totalTests) * 100);
  }

  getTestsPerUser(): number {
    if (!this.analytics || this.analytics.totalUsers === 0) return 0;
    return Math.round((this.analytics.totalTests / this.analytics.totalUsers) * 10) / 10;
  }

  // Helper for template
  Math = Math;
}

