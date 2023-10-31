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
