import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BrowseUrlsRequest, BrowseUrlsResponse } from '../types/browseUrls';
import { Observable } from 'rxjs';
import { ApiResponse } from '../types/apiResponse';
import { CreateShortenedUrlRequest, CreateShortenedUrlResponse } from '../types/createShortenedUrl'; 
import { UrlDetails } from '../models/url';
import {
  AddScanCodeToUrlRequest,
  AddBarcodeToUrlResponse,
  AddQrCodeToUrlResponse,
} from '../types/addScanCodeToUrl';

@Injectable({
  providedIn: 'root',
})
export class ShortenedUrlService {
  private readonly apiUrl = environment.apiBaseUrl + '/urls';
  private readonly httpClient = inject(HttpClient);

  getUrls(params?: BrowseUrlsRequest): Observable<ApiResponse<BrowseUrlsResponse>> {
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

    return this.httpClient.get<ApiResponse<BrowseUrlsResponse>>(this.apiUrl, {
      params: httpParams,
      withCredentials: true,
    });
  }

  getUrlById(urlId: number): Observable<ApiResponse<UrlDetails>> {
    return this.httpClient.get<ApiResponse<UrlDetails>>(`${this.apiUrl}/${urlId}`, {
      withCredentials: true,
    });
  }

  createUrl(body: CreateShortenedUrlRequest): Observable<ApiResponse<CreateShortenedUrlResponse>> {
    return this.httpClient.post<ApiResponse<CreateShortenedUrlResponse>>(this.apiUrl, body, {
      withCredentials: true,
    });
  }

  deleteUrl(urlId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/${urlId}`, {
      withCredentials: true,
    });
  }

  activateUrl(urlId: number): Observable<void> {
    return this.httpClient.patch<void>(
      `${this.apiUrl}/${urlId}/activate`,
      {},
      {
        withCredentials: true,
      }
    );
  }

  deactivateUrl(urlId: number): Observable<void> {
    return this.httpClient.patch<void>(
      `${this.apiUrl}/${urlId}/deactivate`,
      {},
      {
        withCredentials: true,
      }
    );
  }

  addQrCodeToUrl(
    urlId: number,
    body: AddScanCodeToUrlRequest
  ): Observable<ApiResponse<AddQrCodeToUrlResponse>> {
    return this.httpClient.post<ApiResponse<AddQrCodeToUrlResponse>>(
      `${this.apiUrl}/${urlId}/qrcode`,
      body,
      { withCredentials: true }
    );
  }

  addBarcodeToUrl(
    urlId: number,
    body: AddScanCodeToUrlRequest
  ): Observable<ApiResponse<AddBarcodeToUrlResponse>> {
    return this.httpClient.post<ApiResponse<AddBarcodeToUrlResponse>>(
      `${this.apiUrl}/${urlId}/barcode`,
      body,
      { withCredentials: true }
    );
  }
}
