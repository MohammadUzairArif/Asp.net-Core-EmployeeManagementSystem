import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule, X } from 'lucide-angular';
import { Leave } from '../../services/leave';
import { ApplyLeave } from '../../types/ILeave';

@Component({
  selector: 'app-employee-dashboard',
  imports: [ReactiveFormsModule, LucideAngularModule],
  templateUrl: './employee-dashboard.html',
  styleUrl: './employee-dashboard.scss'
})
export class EmployeeDashboard {
 readonly X = X;

  fb = inject(FormBuilder);
  leaveService = inject(Leave);

  isModalOpen = false;
  leaveForm!: FormGroup;

  openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.leaveForm.reset();
  }

  ngOnInit() {
    this.leaveForm = this.fb.group({
      type: [null, Validators.required],
      reason: ['', [Validators.required, Validators.minLength(5)]],
      leaveDate: ['', Validators.required],
    });
  }

  applyLeave() {
    if (this.leaveForm.invalid) return;

    const leaveData: ApplyLeave = this.leaveForm.value;

    this.leaveService.applyLeave(leaveData).subscribe({
      next: () => {
        alert('Leave applied successfully!');
        this.closeModal();
      },
      error: () => {
        alert('Failed to apply leave.');
      },
    });
  }
}
