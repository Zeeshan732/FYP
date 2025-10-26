import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VoiceAnalysisRoutingModule } from './voice-analysis-routing.module';
import { VoiceAnalyzerComponent } from './voice-analyzer/voice-analyzer.component';


@NgModule({
  declarations: [
    VoiceAnalyzerComponent
  ],
  imports: [
    CommonModule,
    VoiceAnalysisRoutingModule
  ]
})
export class VoiceAnalysisModule { }
