import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../services/http';
import { IEmployee } from '../../types/IEmployee';
import { IDepartment } from '../../types/IDepartment';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './employee.html',
  styleUrls: ['./employee.scss']
})
export class Employee implements OnInit {

  httpService = inject(HttpService);
  fb = inject(FormBuilder);

  employeeList: IEmployee[] = [];
  showModal: boolean = false;
  editId: number = 0;

  // ✅ Reactive Form
  employeeForm!: FormGroup;
jobTitles = ['Software Engineer', 'Product Manager', 'Designer', 'QA Engineer', 'HR Manager'];
departments: IDepartment[] = []
  ngOnInit(): void {
    this.getLatestData();
    this.httpService.getDepartments().subscribe({
      next: (result) => {
        this.departments = result;
      }
    });

    // ✅ initialize form
    this.employeeForm = this.fb.group({
      id: [0],
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]{10,15}$/)]],
      gender: [null, Validators.required],
      jobTitle: [null, Validators.required],
      departmentId: [null, Validators.required],
      joiningDate: ['', Validators.required],
      lastWorkingDate: [''], // optional
      dateOfBirth: ['', Validators.required],
    });
  }

  getLatestData() {
    this.httpService.getEmployees().subscribe({
      next: (result) => {
        this.employeeList = result;
      }
    });
  }

  openModal() {
    this.showModal = true;
    this.editId = 0;
   
  }

  closeModal() {
    this.showModal = false;
    this.editId = 0;
    this.employeeForm.reset();
  }

  saveEmployee() {
    console.log(this.employeeForm.value);
    this.httpService.addEmployee(this.employeeForm.value).subscribe({
      next: () => {
        alert('Employee added successfully');
        this.closeModal();
        this.getLatestData();
      }
    });
    
    
  }

  editEmployee(employee: IEmployee) {
    this.editId = employee.id;
    this.showModal = true;

    this.employeeForm.patchValue({
      name: employee.name,
      email: employee.email,
      phone: employee.phone,
      jobTitle: employee.jobTitle
    });
  }

  updateEmployee() {
    if (this.employeeForm.invalid) return;

    this.httpService.updateEmployee(this.editId, this.employeeForm.value).subscribe({
      next: () => {
        alert('Employee updated successfully');
        this.closeModal();
        this.getLatestData();
      }
    });
  }

  deleteEmployee(id: number) {
    this.httpService.deleteEmployee(id).subscribe({
      next: () => {
        alert('Employee deleted successfully');
        this.getLatestData();
      }
    });
  }

  
}
