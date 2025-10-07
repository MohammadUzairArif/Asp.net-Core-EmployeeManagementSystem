import { Component, inject } from '@angular/core';
import { HttpService } from '../../services/http';
import { IDepartment } from '../../types/IDepartment';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterOutlet],
  templateUrl: './departments.html',
  styleUrls: ['./departments.scss'],
})
export class Departments {
   Math = Math; // ✅ expose Math to your template
  httpService = inject(HttpService);

  departments: IDepartment[] = [];
  showModal: boolean = false;
  newDepartment: string = '';
  editId: number = 0;   // 0 = Add mode, >0 = Edit mode

  filter: any = {

};   //  isme saare filters store honge
pageIndex : number  = 0;
pageSize : number  = 5;
totalCount : number  = 0;

  ngOnInit() {
    this.getLatestData();
  }

  getLatestData() {
    this.filter.pageIndex = this.pageIndex;
  this.filter.pageSize = this.pageSize;
    this.httpService.getDepartments(this.filter).subscribe({
      next: (result) => {
       this.departments = result.data;
        this.totalCount = result.totalCount;
      },
    });
  }
// page change handler
// page change handler
onPageChange(newPage: number) {
  this.pageIndex = newPage;
  this.getLatestData();
}
  // Open Add Modal
  openModal() {
    this.showModal = true;
    this.newDepartment = '';
    this.editId = 0;  // Add mode
  }

  // Close Modal
  closeModal() {
    this.showModal = false;
    this.newDepartment = '';
    this.editId = 0;
  }

  // Save New Department
  saveDepartment() {
    if (!this.newDepartment.trim()) return;
    this.httpService.addDepartment(this.newDepartment).subscribe({
      next: () => {
        alert('Department added successfully');
        this.closeModal();
        this.getLatestData();
      },
    });
  }

  // Open Edit Modal
  editDepartment(department: IDepartment) {
    this.newDepartment = department.name;
    this.editId = department.departmentId;
    this.showModal = true;
  }

  // Update Department
  updateDepartment() {
    if (!this.newDepartment.trim()) return;
    this.httpService.updateDepartment(this.editId, this.newDepartment).subscribe({
      next: () => {
        alert('Department updated successfully');
        this.closeModal();
        this.getLatestData();
        this.editId = 0;
      },
    });
  }

  // Delete Department
  deleteDepartment(id: number) {
    this.httpService.deleteDepartment(id).subscribe({
      next: () => {
        alert('Department deleted successfully');
        this.getLatestData();
      },
    });
  }
}

 