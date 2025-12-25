export interface ApiResponse<T> {
  code: number;
  status: string;
  data: T;
  date: string;
}
