import { Component, inject } from '@angular/core';
import { HttpService } from '../../services/http';
import { IDepartment } from '../../types/IDepartment';

@Component({
  selector: 'app-departments',
  imports: [],
  templateUrl: './departments.html',
  styleUrl: './departments.scss'
})
export class Departments {
  httpService = inject(HttpService)
  departments:IDepartment[]=[]
  ngOnInit(){
    this.httpService.getDepartments().subscribe({
      next:(result)=>{
        this.departments = result
      }
    })
  }
}
