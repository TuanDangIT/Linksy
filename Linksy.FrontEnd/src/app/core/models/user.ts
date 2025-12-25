export interface User {
  id: string;
  email: string;
  userName: string;
  firstName: string;
  lastName: string;
  gender: string;
  roles: string[];
  createdAt: Date;
  updatedAt: Date;
}
