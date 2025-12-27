import { barcodeListItem } from "../models/barcode";
import { BrowseParams } from "./browseParams";
import { PagedResult } from "./pagedResult";

export interface BrowseBarcodesResponse {
  pagedResult: PagedResult<barcodeListItem>;
}

export interface BrowseBarcodesRequest extends BrowseParams{
}