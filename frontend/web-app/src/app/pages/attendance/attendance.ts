import { Component, inject, OnInit } from '@angular/core';


import { History, LoaderCircle, CalendarDays, LucideAngularModule, X } from 'lucide-angular';
import { AttendanceType, IAttendance } from '../../types/IAttendance';

import { AttendanceService } from '../../services/attendance-service';
import { DatePipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-attendance',
  imports: [LucideAngularModule,DatePipe],
  templateUrl: './attendance.html',
  styleUrl: './attendance.scss'
})
export class Attendance implements OnInit {
  readonly LoaderCircle = LoaderCircle;
  readonly X = X;
  readonly History = History;
  attendances: IAttendance[] = [];
 loading: boolean = true;
 filter: any = {};
  pageIndex: number = 0;
  pageSize: number = 5;
  totalCount: number = 0;
  Math = Math;

  readonly AttendanceType = AttendanceType; // for template usage
 
  CalendarDays = CalendarDays;

attendanceService = inject(AttendanceService);
route = inject(ActivatedRoute)
  employeeId!: number;

  ngOnInit() {
     this.employeeId = Number(this.route.snapshot.paramMap.get("id"));
    console.log(this.employeeId);
    
    this.getAttendance();
  }

  getAttendance() {
   

 this.filter.pageIndex = this.pageIndex;
    this.filter.pageSize = this.pageSize;
    this.filter.employeeId = this.employeeId
    this.attendanceService.getAttendanceHistory(this.filter).subscribe({
      next: (result) => {
        this.attendances = result.data;
        this.totalCount = result.totalCount;
         
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        alert('Failed to fetch attendance records');
      }
    });
  }

  getAttendanceType(type: AttendanceType): string {
    return AttendanceType[type];
  }

  onPageChange(newPage: number) {
    this.pageIndex = newPage;
    this.getAttendance();
  }

  
}