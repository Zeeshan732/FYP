import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private sidebarCollapsedSubject = new BehaviorSubject<boolean>(false);
  public sidebarCollapsed$: Observable<boolean> = this.sidebarCollapsedSubject.asObservable();

  constructor() {
    // Load saved state
    const saved = localStorage.getItem('sidebarCollapsed');
    if (saved !== null) {
      this.sidebarCollapsedSubject.next(saved === 'true');
    }
  }

  getSidebarCollapsed(): boolean {
    return this.sidebarCollapsedSubject.value;
  }

  setSidebarCollapsed(collapsed: boolean): void {
    this.sidebarCollapsedSubject.next(collapsed);
    localStorage.setItem('sidebarCollapsed', collapsed.toString());
  }

  toggleSidebar(): void {
    const current = this.sidebarCollapsedSubject.value;
    this.setSidebarCollapsed(!current);
  }
}

