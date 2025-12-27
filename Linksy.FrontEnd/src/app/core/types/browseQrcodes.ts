import { qrcodeListItem } from "../models/qrcode";
import { BrowseParams } from "./browseParams";
import { PagedResult } from "./pagedResult";

export interface BrowseQrcodesResponse {
  pagedResult: PagedResult<qrcodeListItem>;
}

export interface BrowseQrcodesRequest extends BrowseParams{
}