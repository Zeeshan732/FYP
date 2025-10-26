import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoiceAnalyzerComponent } from './voice-analyzer.component';

describe('VoiceAnalyzerComponent', () => {
  let component: VoiceAnalyzerComponent;
  let fixture: ComponentFixture<VoiceAnalyzerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VoiceAnalyzerComponent]
    });
    fixture = TestBed.createComponent(VoiceAnalyzerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
