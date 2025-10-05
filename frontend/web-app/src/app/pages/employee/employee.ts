import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpService } from '../../services/http';
import { IEmployee } from '../../types/IEmployee';
import { IDepartment } from '../../types/IDepartment';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './employee.html',
  styleUrls: ['./employee.scss']
})
export class Employee implements OnInit {
 Math = Math; // ✅ expose Math to your template
  httpService = inject(HttpService);
  fb = inject(FormBuilder);

  employeeList: IEmployee[] = [];
  showModal: boolean = false;
  editId: number = 0;

searchControl = new FormControl('');
filter: any = {

};   //  isme saare filters store honge
pageIndex : number  = 0;
pageSize : number  = 5;
totalCount : number  = 0;


  // ✅ Reactive Form
  employeeForm!: FormGroup;
jobTitles = ['Software Engineer', 'Product Manager', 'Designer', 'QA Engineer', 'HR Manager'];
departments: IDepartment[] = []

  ngOnInit(): void {
    this.getLatestData();
    // jab search change ho
   this.searchControl.valueChanges.pipe(debounceTime(300), distinctUntilChanged()).subscribe((result: string | null) => {
    console.log(result);
    
    this.filter.search = result || ''; 
       this.pageIndex = 0; // reset to first page on new search  // agar null ho to empty string
    this.getLatestData();                 // call reload
  });
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
     this.filter.pageIndex = this.pageIndex;
  this.filter.pageSize = this.pageSize;
    this.httpService.getEmployees(this.filter).subscribe({
      next: (result) => {
        this.employeeList = result.data;
        this.totalCount = result.totalCount;
      }
    });
  }
 // page change handler
// page change handler
onPageChange(newPage: number) {
  this.pageIndex = newPage;
  this.getLatestData();
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
    console.log(employee);
    
    this.showModal = true;

    this.employeeForm.patchValue(employee);
    this.employeeForm.get('gender')?.disable();
    this.employeeForm.get('dateOfBirth')?.disable();
    this.employeeForm.get('joiningDate')?.disable();
  }

  updateEmployee() {
    if (this.employeeForm.invalid) return;

    this.httpService.updateEmployee(this.editId,this.employeeForm.value).subscribe({
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
