export interface IAttendance {
id: number;
type: AttendanceType;
date: string;

}


export enum AttendanceType {
  Present = 1,
  Leave = 2
}