export interface VoucherSummary {
  cDelivered: number;
  pDelivered: number;
  cIntransit: number;
  pIntransit: number;
  cForPickup: number;
  cForVoucher: number;
  pForVoucher: number;
  cCancelledMellon: number;
  cCancelledCarrier: number;
}
