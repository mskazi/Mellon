export interface Department extends LookUpString {}
export interface Company extends LookUpString {}

export interface LookUpString {
  id: string;
  description: string;
}

export interface LookUpNumber {
  id: number;
  description: string;
}
