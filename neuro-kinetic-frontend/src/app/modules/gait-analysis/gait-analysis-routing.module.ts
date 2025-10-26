import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GaitVisualizerComponent } from './gait-visualizer/gait-visualizer.component';

const routes: Routes = [
  { path: '', component: GaitVisualizerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GaitAnalysisRoutingModule { }
