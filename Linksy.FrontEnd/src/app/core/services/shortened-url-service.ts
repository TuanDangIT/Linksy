import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BrowseUrlsRequest, BrowseUrlsResponse } from '../types/browseUrls';
import { Observable } from 'rxjs';
import { ApiResponse } from '../types/apiResponse';
import { CreateShortenedUrlRequest } from '../types/createShortenedUrlRequest';
import { CreateShortenedUrlResponse } from '../types/createShortenedUrlResponse';

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
}
