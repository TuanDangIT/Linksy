import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BrowseQrcodesRequest, BrowseQrcodesResponse } from '../types/browseQrcodes';
import { ApiResponse } from '../types/apiResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class QrcodeService {
  private readonly apiUrl = environment.apiBaseUrl + '/qrcodes';
  private readonly httpClient = inject(HttpClient);

  getUrls(params?: BrowseQrcodesRequest): Observable<ApiResponse<BrowseQrcodesResponse>> {
    let httpParams = new HttpParams();

    if (params) {
      if (params.pageNumber) {
        httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
      }

      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }

      if (params.orders && params.orders.length > 0) {
        params.orders.forEach((order) => {
          httpParams = httpParams.append('orders', order);
        });
      }

      if (params.filters) {
        Object.keys(params.filters).forEach((key) => {
          httpParams = httpParams.append(`filters[${key}]`, params.filters![key]);
        });
      }
    }

    return this.httpClient.get<ApiResponse<BrowseQrcodesResponse>>(this.apiUrl, {
      params: httpParams,
    });
  }
}
