import { ContactMode } from './contac';

export interface ContactEditItem {
  id: number;
  voucherFrom: any;
  voucherName: string;
  voucherContact: string;
  voucherAddress: string;
  voucherCity: string;
  voucherPostCode: string;
  voucherCountry: string;
  voucherPhoneNo: string;
  voucherMobileNo: string;
  contactNotes: string;
  mode: ContactMode;
  active: boolean;
}
