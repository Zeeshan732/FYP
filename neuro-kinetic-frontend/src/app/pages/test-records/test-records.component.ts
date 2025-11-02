import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { UserTestRecord, PagedResult } from '../../models/api.models';
import { Router } from '@angular/router';
import { MessageService, ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-test-records',
  templateUrl: './test-records.component.html',
  styleUrls: ['./test-records.component.scss']
})
export class TestRecordsComponent implements OnInit {
  records: UserTestRecord[] = [];
  loading = false;
  error = '';
  
  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  totalCount = 0;
  hasPrevious = false;
  hasNext = false;

  // Filters
  filterStatus: string = '';
  filterResult: string = '';
  sortBy: string = 'testDate';
  sortOrder: 'asc' | 'desc' = 'desc';

  // User info
  currentUser: any = null;
  isAdmin = false;

  // Dialog state
  showDialog: boolean = false;
  selectedRecord: UserTestRecord | null = null;
  isEditMode: boolean = false;

  constructor(
    private apiService: ApiService,
    private authService: AuthService,
    private router: Router,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      this.isAdmin = user?.role === 'Admin';
      this.loadRecords();
    });
  }

  loadRecords() {
    this.loading = true;
    this.error = '';

    const params: any = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      sortOrder: this.sortOrder
    };

    // Filter by user if not admin
    if (!this.isAdmin && this.currentUser?.id) {
      params.userId = this.currentUser.id;
    }

    // Apply filters
    if (this.filterStatus) {
      params.status = this.filterStatus;
    }
    if (this.filterResult) {
      params.testResult = this.filterResult;
    }

    this.apiService.getUserTestRecords(params).subscribe({
      next: (response: PagedResult<UserTestRecord>) => {
        this.records = response.items;
        this.currentPage = response.pageNumber;
        this.totalPages = response.totalPages;
        this.totalCount = response.totalCount;
        this.hasPrevious = response.hasPrevious;
        this.hasNext = response.hasNext;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading test records:', error);
        this.error = 'Failed to load test records. Please try again.';
        this.loading = false;
      }
    });
  }

  onFilterChange() {
    this.currentPage = 1;
    this.loadRecords();
  }

  onSortChange(sortBy: string) {
    if (this.sortBy === sortBy) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = sortBy;
      this.sortOrder = 'asc';
    }
    this.loadRecords();
  }

  clearFilters() {
    this.filterStatus = '';
    this.filterResult = '';
    this.currentPage = 1;
    this.loadRecords();
  }

  nextPage() {
    if (this.hasNext) {
      this.currentPage++;
      this.loadRecords();
    }
  }

  previousPage() {
    if (this.hasPrevious) {
      this.currentPage--;
      this.loadRecords();
    }
  }

  deleteRecord(event: Event, id: number) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Are you sure you want to delete this test record?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.deleteUserTestRecord(id).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Test record deleted successfully'
            });
            this.loadRecords();
          },
          error: (error) => {
            console.error('Error deleting record:', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Failed to delete test record. Please try again.'
            });
          }
        });
      }
    });
  }

  viewRecord(id: number) {
    this.apiService.getUserTestRecord(id).subscribe({
      next: (record) => {
        this.selectedRecord = record;
        this.isEditMode = false;
        this.showDialog = true;
      },
      error: (error) => {
        console.error('Error loading record:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load test record. Please try again.'
        });
      }
    });
  }

  editRecord(id: number) {
    this.apiService.getUserTestRecord(id).subscribe({
      next: (record) => {
        this.selectedRecord = record;
        this.isEditMode = true;
        this.showDialog = true;
      },
      error: (error) => {
        console.error('Error loading record:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load test record. Please try again.'
        });
      }
    });
  }

  onSaveRecord(updatedRecord: UserTestRecord) {
    this.apiService.updateUserTestRecord(updatedRecord.id, updatedRecord).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Test record updated successfully'
        });
        this.showDialog = false;
        this.loadRecords();
      },
      error: (error) => {
        console.error('Error updating record:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to update test record. Please try again.'
        });
      }
    });
  }

  onCancelDialog() {
    this.showDialog = false;
    this.selectedRecord = null;
  }

  takeNewTest() {
    this.router.navigate(['/patient-test']);
  }

  getResultBadgeColor(result: string): string {
    switch (result) {
      case 'Positive':
        return 'bg-red-500/20 border-red-500 text-red-400';
      case 'Negative':
        return 'bg-green-500/20 border-green-500 text-green-400';
      case 'Uncertain':
        return 'bg-yellow-500/20 border-yellow-500 text-yellow-400';
      default:
        return 'bg-gray-500/20 border-gray-500 text-gray-400';
    }
  }

  getStatusBadgeColor(status: string): string {
    switch (status) {
      case 'Completed':
        return 'bg-green-500/20 border-green-500 text-green-400';
      case 'Pending':
        return 'bg-yellow-500/20 border-yellow-500 text-yellow-400';
      case 'Failed':
        return 'bg-red-500/20 border-red-500 text-red-400';
      default:
        return 'bg-gray-500/20 border-gray-500 text-gray-400';
    }
  }

  hasActiveFilters(): boolean {
    return !!(this.filterStatus || this.filterResult);
  }

  // Helper for template
  Math = Math;
}

