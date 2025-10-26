import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GaitAnalysisRoutingModule } from './gait-analysis-routing.module';
import { GaitVisualizerComponent } from './gait-visualizer/gait-visualizer.component';


@NgModule({
  declarations: [
    GaitVisualizerComponent
  ],
  imports: [
    CommonModule,
    GaitAnalysisRoutingModule
  ]
})
export class GaitAnalysisModule { }
