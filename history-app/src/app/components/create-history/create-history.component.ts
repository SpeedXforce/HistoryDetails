import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { HistoryService } from '../../services/history.service';
import { CreateHistoryDTO } from '../../models/history';

@Component({
  selector: 'app-create-history',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule
  ],
  templateUrl: './create-history.component.html',
  styleUrls: ['./create-history.component.scss']
})
export class CreateHistoryComponent implements OnInit {

  form!: FormGroup;
  historyIds: string[] = [];
  isSaving = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private historyService: HistoryService,
    private dialogRef: MatDialogRef<CreateHistoryComponent>
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadHistoryIds();
  }

  buildForm(): void {
    this.form = this.fb.group({
      historyId: ['',  Validators.required],
      timestamp: ['',  Validators.required],
      value:     [null, Validators.required],
      status:    ['',  Validators.required]
    });
  }

  loadHistoryIds(): void {
    this.historyService.getHistoryIds().subscribe({
      next: (ids) => this.historyIds = ids,
      error: () => this.errorMessage = 'Failed to load History IDs.'
    });
  }

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    this.errorMessage = '';

    const formValue = this.form.value;

    const dto: CreateHistoryDTO = {
      ...formValue,
      timestamp: formValue.timestamp
        ? new Date(formValue.timestamp).toISOString()
        : null
    };

    this.historyService.saveHistory(dto).subscribe({
      next: (response: any) => {
        this.isSaving = false;
        this.dialogRef.close('saved');
      },
      error: (err) => {
        console.error("error",err);
        this.errorMessage = 'Failed to save. Please try again.';
        this.isSaving = false;
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  isInvalid(field: string): boolean {
    const control = this.form.get(field);
    return !!(control?.invalid && control?.touched);
  }
}