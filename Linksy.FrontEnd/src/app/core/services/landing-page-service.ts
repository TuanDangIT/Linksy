import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BrowseLandingPagesRequest, BrowseLandingPagesResponse } from '../types/browseLandingPages';
import { Observable } from 'rxjs';
import { ApiResponse } from '../types/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class LandingPageService {
  private readonly apiUrl = environment.apiBaseUrl + '/landingpages';
  private readonly httpClient = inject(HttpClient);

  getUrls(params?: BrowseLandingPagesRequest): Observable<ApiResponse<BrowseLandingPagesResponse>> {
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

    return this.httpClient.get<ApiResponse<BrowseLandingPagesResponse>>(this.apiUrl, {
      params: httpParams,
    });
  }
}
