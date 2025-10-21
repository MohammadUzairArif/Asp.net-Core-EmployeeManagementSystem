export interface ApplyLeave{
    type:number,
    reason:string,
    leaveDate:string
}
export interface ILeave {
  id: number;
  type: number;
  reason: string;
  status: number;
  leaveDate: string;
  employeeId: number;
  employee?: any;
   employeeName?: string;
}







export enum LeaveType {
  Sick = 1,
  Casual = 2,
  Earned = 3,
}

export enum LeaveStatus {
  Pending = 0,
  Accepted = 1,
  Rejected = 2,
  Cancelled = 3,
}