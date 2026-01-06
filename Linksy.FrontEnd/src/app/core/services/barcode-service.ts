import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { BrowseBarcodesRequest, BrowseBarcodesResponse } from '../types/browseBarcodes';
import { ApiResponse } from '../types/apiResponse';
import { Observable } from 'rxjs';
import { BarcodeDetails } from '../models/barcode';
import { CreateBarcodeRequest, CreateBarcodeResponse } from '../types/createBarcode';

@Injectable({
  providedIn: 'root',
})
export class BarcodeService {
  private readonly apiUrl = environment.apiBaseUrl + '/barcodes';
  private readonly httpClient = inject(HttpClient);

  getBarcodes(params?: BrowseBarcodesRequest): Observable<ApiResponse<BrowseBarcodesResponse>> {
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

    return this.httpClient.get<ApiResponse<BrowseBarcodesResponse>>(this.apiUrl, {
      params: httpParams,
      withCredentials: true,
    });
  }

  getBarcodeById(barcodeId: number): Observable<ApiResponse<BarcodeDetails>> {
    return this.httpClient.get<ApiResponse<BarcodeDetails>>(`${this.apiUrl}/${barcodeId}`, {
      withCredentials: true,
    });
  }

  createBarcode(body: CreateBarcodeRequest): Observable<ApiResponse<CreateBarcodeResponse>> {
    return this.httpClient.post<ApiResponse<CreateBarcodeResponse>>(this.apiUrl, body, {
      withCredentials: true,
    });
  }

  downloadBarcode(barcodeId: number): Observable<HttpResponse<Blob>> {
    return this.httpClient.get(`${this.apiUrl}/${barcodeId}/download`, {
      withCredentials: true,
      observe: 'response',
      responseType: 'blob',
    });
  }

  deleteBarcode(barcodeId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/${barcodeId}`, {
      withCredentials: true,
    });
  }
}
