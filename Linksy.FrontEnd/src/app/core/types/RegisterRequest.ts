export type Gender = 'Male' | 'Female' | 'Undefined';

export interface RegisterRequest {
  email: string;
  firstName: string;
  lastName: string;
  gender: Gender;
  username: string;
  password: string;
  confirmPassword: string;
}
