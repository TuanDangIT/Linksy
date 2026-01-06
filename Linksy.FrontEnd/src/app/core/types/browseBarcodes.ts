import { BarcodeListItem } from "../models/barcode";
import { BrowseParams } from "./browseParams";
import { PagedResult } from "./pagedResult";

export interface BrowseBarcodesResponse {
  pagedResult: PagedResult<BarcodeListItem>;
}

export interface BrowseBarcodesRequest extends BrowseParams{
}