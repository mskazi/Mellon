export interface ContactSearchItem {
  id: number;
  voucherFrom: any;
  voucherName: string;
  voucherContact: string;
  voucherAddress: string;
  voucherCity: string;
  voucherPostCode: string;
  voucherCountry: string;
  voucherPhoneNo: string;
  voucherMobileNo?: string;
  contactNotes?: string;
  sysCompany: string;
  active: boolean;
}
