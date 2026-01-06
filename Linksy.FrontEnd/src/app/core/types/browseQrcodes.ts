import { QrcodeListItem } from "../models/qrcode";
import { BrowseParams } from "./browseParams";
import { PagedResult } from "./pagedResult";

export interface BrowseQrcodesResponse {
  pagedResult: PagedResult<QrcodeListItem>;
}

export interface BrowseQrcodesRequest extends BrowseParams{
}