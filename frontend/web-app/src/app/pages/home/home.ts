import { Component, inject, OnInit } from '@angular/core';
import { Building2, CalendarDays, LoaderCircle, LucideAngularModule, Users, Wallet, X } from 'lucide-angular';
import { DashboardService } from '../../services/dashboard-service';
import { ChartConfiguration } from 'chart.js';
import { IDepartmentDashboard } from '../../types/IDashboard';
import { BaseChartDirective } from 'ng2-charts';
import { ILeave, LeaveStatus, LeaveType} from '../../types/ILeave';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [LucideAngularModule, BaseChartDirective,NgClass],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit {

  departmentData!: IDepartmentDashboard[];

  public barChartLegend = true;
  public barChartPlugins = [];

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Employee Count',
        backgroundColor: '#3b82f6', // blue-500
        borderRadius: 6,
      }
    ]
  };

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        grid: {
          display: false
        }
      },
      y: {
        beginAtZero: true,
        ticks: {
          stepSize: 20
        }
      }
    },
    plugins: {
      legend: {
        position: 'bottom',
        labels: {
          color: '#374151', // gray-700
          font: {
            size: 13
          }
        }
      }
    }
  };
  totalSalary!: number
employeeCount!: number
departmentCount!: number
// Leaves table
  leaves: ILeave[] = [];
  loading: boolean = true;

  // icons
  readonly Users = Users;
  readonly Building2 = Building2;
 readonly  Wallet = Wallet;
 readonly CalendarDays  = CalendarDays;
  readonly LoaderCircle = LoaderCircle;
  readonly X = X;
  readonly LeaveStatus = LeaveStatus;

 dashboardService = inject(DashboardService)

  ngOnInit(): void {
     this.getDashboardData(); 
 this.getDepartmentData();
  this.getRecentLeaves();
  }



  getDashboardData() {
   

 
    this.dashboardService.getDashboardData().subscribe({
      next: (result) => {

        this.totalSalary = result.totalSalary
        this.employeeCount = result.employeeCount
        this.departmentCount = result.departmentCount
      },
      error: () => {
        
        alert('Failed to fetch dashboard data ');
      }
    });
  }

  getDepartmentData() {
    this.dashboardService.getDepartmentData().subscribe({
      next: (result) => {

        this.barChartData.labels = result.map(dept => dept.name);
        this.barChartData.datasets[0].data = result.map(dept => dept.employeeCount);
        this.departmentData = result;
      },
      error: () => {
        alert('Failed to fetch department data');
      }
    });
  }
    getRecentLeaves() {
    this.dashboardService.getRecentLeaves().subscribe({
      next: (result) => {
        this.leaves = result;
        this.loading = false;
      }, error: () => {
        
        this.loading = false;
        alert('Failed to fetch recent leaves');
      }
    });
  }
   getLeaveType(type: LeaveType): string {
    return LeaveType[type] || 'Unknown';
  }

  getStatusText(status: LeaveStatus): string {
    return LeaveStatus[status] || 'Unknown';
  }
}
