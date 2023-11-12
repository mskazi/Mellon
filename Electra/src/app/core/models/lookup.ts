export interface Department extends LookUpString {}
export interface Company extends LookUpString {}
export interface Country extends LookUpString {}
export interface VoucherType extends LookUpNumber {}
export interface VoucherDeliveryTimeType extends LookUpNumber {}
export interface VoucherConditionType extends LookUpString {}
export interface VoucherDepartmentType extends LookUpString {}

export interface LookUpString {
  id: string;
  description: string;
}

export interface LookUpNumber {
  id: number;
  description: string;
}
