import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GaitVisualizerComponent } from './gait-visualizer.component';

describe('GaitVisualizerComponent', () => {
  let component: GaitVisualizerComponent;
  let fixture: ComponentFixture<GaitVisualizerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GaitVisualizerComponent]
    });
    fixture = TestBed.createComponent(GaitVisualizerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
