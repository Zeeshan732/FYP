import { Component, HostListener, OnInit } from '@angular/core';
import { ModalService } from '../../services/modal.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {
  mobileMenuOpen = false;
  isScrolled = false;
  lastScrollY = 0;
  isNavbarVisible = true;

  constructor(private modalService: ModalService) {}

  ngOnInit() {
    this.lastScrollY = window.scrollY;
  }

  @HostListener('window:scroll', ['$event'])
  onScroll() {
    const currentScrollY = window.scrollY;
    
    // Show navbar when scrolling up or at the top
    if (currentScrollY < this.lastScrollY || currentScrollY < 100) {
      this.isNavbarVisible = true;
    } 
    // Hide navbar when scrolling down (but not at the very top)
    else if (currentScrollY > this.lastScrollY && currentScrollY > 100) {
      this.isNavbarVisible = false;
    }
    
    // Update scroll state for styling
    this.isScrolled = currentScrollY > 50;
    this.lastScrollY = currentScrollY;
  }

  toggleMobileMenu() {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  closeMobileMenu() {
    this.mobileMenuOpen = false;
  }

  openLoginModal() {
    this.modalService.openLoginModal();
    this.closeMobileMenu();
  }

  openSignupModal() {
    this.modalService.openSignupModal();
    this.closeMobileMenu();
  }
}
