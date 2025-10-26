import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VoiceAnalyzerComponent } from './voice-analyzer/voice-analyzer.component';

const routes: Routes = [
  { path: '', component: VoiceAnalyzerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VoiceAnalysisRoutingModule { }
