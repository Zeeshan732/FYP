import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LandingComponent } from './pages/landing/landing.component';
import { ServicesComponent } from './pages/services/services.component';
import { ContactComponent } from './pages/contact/contact.component';
import { LoginComponent } from './pages/login/login.component';
import { TechnologyDemoComponent } from './pages/technology-demo/technology-demo.component';
import { TechnologyComponent } from './pages/technology/technology.component';
import { ResearchComponent } from './pages/research/research.component';
import { ClinicalUseComponent } from './pages/clinical-use/clinical-use.component';
import { CollaborationComponent } from './pages/collaboration/collaboration.component';

const routes: Routes = [
  { path: '', redirectTo: '/landing', pathMatch: 'full' },
  { path: 'landing', component: LandingComponent },
  { path: 'home', component: HomeComponent },
  { path: 'services', component: ServicesComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'login', component: LoginComponent },
      { path: 'technology', component: TechnologyComponent },
      { path: 'technology-demo', component: TechnologyDemoComponent },
  { path: 'research', component: ResearchComponent },
  { path: 'clinical-use', component: ClinicalUseComponent },
  { path: 'collaboration', component: CollaborationComponent },
  { path: 'voice-analysis', loadChildren: () => import('./modules/voice-analysis/voice-analysis.module').then(m => m.VoiceAnalysisModule) },
  { path: 'gait-analysis', loadChildren: () => import('./modules/gait-analysis/gait-analysis.module').then(m => m.GaitAnalysisModule) },
  { path: '**', redirectTo: '/landing' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
