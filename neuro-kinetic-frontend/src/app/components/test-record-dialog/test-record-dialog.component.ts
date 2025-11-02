import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserTestRecord } from '../../models/api.models';

@Component({
  selector: 'app-test-record-dialog',
  templateUrl: './test-record-dialog.component.html',
  styleUrls: ['./test-record-dialog.component.scss']
})
export class TestRecordDialogComponent implements OnInit, OnChanges {
  @Input() visible: boolean = false;
  @Input() record: UserTestRecord | null = null;
  @Input() isEditMode: boolean = false;
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() save = new EventEmitter<UserTestRecord>();
  @Output() cancel = new EventEmitter<void>();

  recordForm!: FormGroup;
  testResultOptions = [
    { label: 'Positive', value: 'Positive' },
    { label: 'Negative', value: 'Negative' },
    { label: 'Uncertain', value: 'Uncertain' }
  ];
  statusOptions = [
    { label: 'Completed', value: 'Completed' },
    { label: 'Pending', value: 'Pending' },
    { label: 'Failed', value: 'Failed' }
  ];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['record'] && this.record) {
      this.initializeForm();
    }
  }

  initializeForm() {
    const recordData: Partial<UserTestRecord> = this.record || {};
    
    this.recordForm = this.fb.group({
      userName: [{ value: recordData.userName || '', disabled: true }],
      testDate: [{ value: recordData.testDate ? new Date(recordData.testDate) : new Date(), disabled: true }],
      testResult: [recordData.testResult || 'Uncertain', Validators.required],
      accuracy: [recordData.accuracy || 0, [Validators.required, Validators.min(0), Validators.max(100)]],
      status: [recordData.status || 'Pending', Validators.required],
      voiceRecordingUrl: [{ value: recordData.voiceRecordingUrl || '', disabled: !this.isEditMode }],
      analysisNotes: [recordData.analysisNotes || '']
    });
  }

  onSave() {
    if (this.recordForm.valid) {
      const formValue = this.recordForm.getRawValue();
      const updatedRecord: UserTestRecord = {
        ...this.record!,
        ...formValue,
        testDate: formValue.testDate instanceof Date 
          ? formValue.testDate.toISOString() 
          : this.record!.testDate
      };
      this.save.emit(updatedRecord);
    }
  }

  onCancel() {
    this.visibleChange.emit(false);
    this.cancel.emit();
  }

  onClose() {
    this.visibleChange.emit(false);
  }
}

