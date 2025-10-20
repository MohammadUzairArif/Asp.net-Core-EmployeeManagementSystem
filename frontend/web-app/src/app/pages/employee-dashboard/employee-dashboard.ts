import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule, X } from 'lucide-angular';
import { LeaveService } from '../../services/leave';
import { ApplyLeave } from '../../types/ILeave';
import { AttendanceService } from '../../services/attendance-service';
@Component({
  selector: 'app-employee-dashboard',
  imports: [ReactiveFormsModule, LucideAngularModule],
  templateUrl: './employee-dashboard.html',
  styleUrl: './employee-dashboard.scss'
})
export class EmployeeDashboard {
 readonly X = X;

  fb = inject(FormBuilder);
  leaveService = inject(LeaveService);
 attendanceService = inject(AttendanceService);
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
   markAttendance() {
    this.attendanceService.markAttendance().subscribe({
      next: (res:any) => {
        alert(res.message);
      },
      error: (err) => {
       if(err.error && err.error.message){
        alert(err.error.message);
       } else{
        alert('Failed to mark attendance.');
       }
      },
    });
  }
}
