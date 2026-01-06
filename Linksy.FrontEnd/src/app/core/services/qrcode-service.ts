import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { BrowseQrcodesRequest, BrowseQrcodesResponse } from '../types/browseQrcodes';
import { ApiResponse } from '../types/apiResponse';
import { Observable } from 'rxjs';
import { QrcodeDetails } from '../models/qrcode';
import { CreateQrCodeRequest, CreateQrCodeResponse } from '../types/createQrCode';

@Injectable({
  providedIn: 'root',
})
export class QrcodeService {
  private readonly apiUrl = environment.apiBaseUrl + '/qrcodes';
  private readonly httpClient = inject(HttpClient);

  getQrcodes(params?: BrowseQrcodesRequest): Observable<ApiResponse<BrowseQrcodesResponse>> {
    let httpParams = new HttpParams();

    if (params) {
      if (params.pageNumber) {
        httpParams = httpParams.set('PageNumber', params.pageNumber.toString());
      }

      if (params.pageSize) {
        httpParams = httpParams.set('PageSize', params.pageSize.toString());
      }

      if (params.orders && params.orders.length > 0) {
        const sortValue = params.orders.join(',');
        httpParams = httpParams.set('Sort', sortValue);
      }

      if (params.filters) {
        Object.keys(params.filters).forEach((key) => {
          httpParams = httpParams.set(key, params.filters![key]);
        });
      }
    }

    return this.httpClient.get<ApiResponse<BrowseQrcodesResponse>>(this.apiUrl, {
      params: httpParams,
      withCredentials: true,
    });
  }

  getQrCodeById(qrcodeId: number): Observable<ApiResponse<QrcodeDetails>> {
    return this.httpClient.get<ApiResponse<QrcodeDetails>>(`${this.apiUrl}/${qrcodeId}`, {
      withCredentials: true,
    });
  }

  createQrCode(body: CreateQrCodeRequest): Observable<ApiResponse<CreateQrCodeResponse>> {
    return this.httpClient.post<ApiResponse<CreateQrCodeResponse>>(this.apiUrl, body, {
      withCredentials: true,
    });
  }

  downloadQrCode(qrcodeId: number): Observable<HttpResponse<Blob>> {
    return this.httpClient.get(`${this.apiUrl}/${qrcodeId}/download`, {
      withCredentials: true,
      observe: 'response',
      responseType: 'blob',
    });
  }

  deleteQrCode(qrcodeId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/${qrcodeId}`, {
      withCredentials: true,
    });
  }
}
