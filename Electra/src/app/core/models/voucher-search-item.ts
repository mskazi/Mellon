export interface VoucherServiceItem {
  id: number;
  voucherName: string;
  systemStatus: number;
  voucherContact?: string;
  voucherAddress: string;
  voucherCity: string;
  voucherPostCode: string;
  voucherPhoneNo: string;
  voucherMobileNo: string;
  voucherDescription?: string;
  serialNo: string;
  systemCompany: string;
  navisionServiceOrderNo: string;
  navisionSalesOrderNo: string;
  actionTypeDescription: string;
  carrierVoucherNo: string;
  statusDescription: string;
  orderedBy: string;
  mellonProject: string;
  carrierName: string;
  createdBy: string;
  createdAt: Date;
}

export interface VoucherWarehouseItem extends VoucherBaseItem {
  systemStatus: number;
  systemCompany: string;
  navisionServiceOrderNo: string;
  navisionSalesOrderNo: string;
  actionTypeDescription: string;
  carrierVoucherNo: string;
  statusDescription: string;
  orderedBy: string;
  mellonProject: string;
  carrierName: string;
  createdBy: string;
  createdAt: string;
}

export interface VoucherSearchItem extends VoucherBaseItem {
  systemStatus: any;
  systemCompany: string;
  carrierVoucherNo: string;
  orderedBy: string;
  carrierName: string;
  carrierDeliveryStatus: string;
  createdAt: string;
}

export interface VoucherBaseItem {
  id: number;
  voucherName: string;
  voucherContact?: string;
  voucherAddress: string;
  voucherCity: string;
  voucherPostCode: string;
  voucherPhoneNo: string;
  voucherMobileNo: string;
  voucherDescription?: string;
}
