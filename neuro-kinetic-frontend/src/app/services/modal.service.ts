import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private loginModalSubject = new BehaviorSubject<boolean>(false);
  private signupModalSubject = new BehaviorSubject<boolean>(false);

  // Login modal
  loginModal$ = this.loginModalSubject.asObservable();
  
  openLoginModal() {
    this.loginModalSubject.next(true);
  }
  
  closeLoginModal() {
    this.loginModalSubject.next(false);
  }

  // Signup modal
  signupModal$ = this.signupModalSubject.asObservable();
  
  openSignupModal() {
    this.signupModalSubject.next(true);
  }
  
  closeSignupModal() {
    this.signupModalSubject.next(false);
  }

  // Close all modals
  closeAllModals() {
    this.loginModalSubject.next(false);
    this.signupModalSubject.next(false);
  }
}
