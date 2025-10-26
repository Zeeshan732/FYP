import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClinicalUseComponent } from './clinical-use.component';

describe('ClinicalUseComponent', () => {
  let component: ClinicalUseComponent;
  let fixture: ComponentFixture<ClinicalUseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ClinicalUseComponent]
    });
    fixture = TestBed.createComponent(ClinicalUseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
