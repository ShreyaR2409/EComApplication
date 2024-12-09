import { AbstractControl, ValidatorFn } from '@angular/forms';

export function sellingPriceGreaterThanPurchasePrice(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const sellingPrice = control.get('sellingprice')?.value;
    const purchasePrice = control.get('purchaseprice')?.value;
    return sellingPrice && purchasePrice && sellingPrice <= purchasePrice ? { 'priceMismatch': true } : null;
  };
}
