export interface Voucher {
  id: number;
  systemStatus: number;
  voucherName: string;
  voucherContact: string;
  voucherAddress: string;
  voucherCity: string;
  voucherPostCode: string;
  voucherPhoneNo: string;
  voucherMobileNo: string;
  voucherDescription: string;
  serialNo: any;
  systemCompany: string;
  navisionServiceOrderNo: string;
  navisionSalesOrderNo: string;
  actionTypeDescription: any;
  carrierVoucherNo: string;
  statusDescription: any;
  orderedBy: string;
  mellonProject: string;
  carrierName: string;
  createdBy: string;
  createdAt: string;
  dataInabilities: VoucherDataInabilities[];
  dataLines: VoucherDataLine[];
  carrierPickupDate: string;
  carrierDeliveryDate: Date;
  carrierDeliveredTo: Date;
  conditionCode: string;
  deliverSaturday: boolean;
  deliveryDescription: string;
  codAmount: number;
  systemDepertment: string;
  carrierCode: string;
  carrierDelivereyStatus: string;
  carrierActionType: number;
}

export interface VoucherDataLine {
  id: number;
  dataId: number;
  name: string;
  value: string;
  createdBy: string;
  updatedBy: string;
  createdAt: string;
  updatedAt: string;
}

export interface VoucherDataInabilities {
  id: number;
  dataId: number;
  trnDate: Date;
  reason: string;
}

export interface VoucherTrack {
  voucherDetails: Voucher;
  courierTrackResource: any;
}
