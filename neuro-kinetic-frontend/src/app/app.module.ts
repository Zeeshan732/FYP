import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { TechnologyComponent } from './pages/technology/technology.component';
import { ResearchComponent } from './pages/research/research.component';
import { ClinicalUseComponent } from './pages/clinical-use/clinical-use.component';
import { CollaborationComponent } from './pages/collaboration/collaboration.component';
import { LandingComponent } from './pages/landing/landing.component';
import { ServicesComponent } from './pages/services/services.component';
import { ContactComponent } from './pages/contact/contact.component';
import { LoginComponent } from './pages/login/login.component';
import { SignupComponent } from './pages/signup/signup.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoginModalComponent } from './components/modals/login-modal/login-modal.component';
import { SignupModalComponent } from './components/modals/signup-modal/signup-modal.component';
import { TechnologyDemoComponent } from './pages/technology-demo/technology-demo.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TechnologyComponent,
    ResearchComponent,
    ClinicalUseComponent,
    CollaborationComponent,
    LandingComponent,
    ServicesComponent,
    ContactComponent,
    LoginComponent,
    SignupComponent,
    NavigationComponent,
    FooterComponent,
    LoginModalComponent,
    SignupModalComponent,
    TechnologyDemoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
