import { AccountInfo } from '@azure/msal-browser';

export interface User {
  [prop: string]: any;
  id: number | string | null;
  name?: string;
  member: string;
  department: string;
  company: string;
  email: string;
  isActive: boolean;
  country: string;
  role: number;
  roles: Roles;
}

export interface UserInfo {
  user: User;
  accountInfo: AccountInfo;
}

export enum Roles {
  SERVICE = 'SERVICE',
}
