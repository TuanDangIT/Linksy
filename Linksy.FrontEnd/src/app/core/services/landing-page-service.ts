import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BrowseLandingPagesRequest, BrowseLandingPagesResponse } from '../types/browseLandingPages';
import { Observable } from 'rxjs';
import { ApiResponse } from '../types/apiResponse';
import { CreateLandingPageResponse } from '../types/createLandingPage';
import { LandingPage, LandingPageDetails } from '../models/landingPage';

@Injectable({ providedIn: 'root' })
export class LandingPageService {
  private readonly apiUrl = environment.apiBaseUrl + '/LandingPages';
  private readonly httpClient = inject(HttpClient);

  getLandingPages(
    params?: BrowseLandingPagesRequest
  ): Observable<ApiResponse<BrowseLandingPagesResponse>> {
    let httpParams = new HttpParams();

    if (params) {
      if (params.pageNumber)
        httpParams = httpParams.set('PageNumber', params.pageNumber.toString());
      if (params.pageSize) httpParams = httpParams.set('PageSize', params.pageSize.toString());

      if (params.orders && params.orders.length > 0) {
        httpParams = httpParams.set('Sort', params.orders.join(','));
      }

      if (params.filters) {
        Object.keys(params.filters).forEach((key) => {
          httpParams = httpParams.set(key, params.filters![key]);
        });
      }
    }

    return this.httpClient.get<ApiResponse<BrowseLandingPagesResponse>>(this.apiUrl, {
      params: httpParams,
      withCredentials: true,
    });
  }

  getLandingPageById(landingPageId: number): Observable<ApiResponse<LandingPageDetails>> {
    return this.httpClient.get<ApiResponse<LandingPageDetails>>(`${this.apiUrl}/${landingPageId}`, {
      withCredentials: true,
    });
  }

  getPublicLandingPage(code: string): Observable<ApiResponse<LandingPage>> {
    return this.httpClient.get<ApiResponse<LandingPage>>(
      `${environment.redirectingLandingPageBaseUrl}/${code}`,
      {}
    );
  }

  createLandingPage(form: FormData): Observable<ApiResponse<CreateLandingPageResponse>> {
    return this.httpClient.post<ApiResponse<CreateLandingPageResponse>>(this.apiUrl, form, {
      withCredentials: true,
    });
  }

  publishLandingPage(landingPageId: number): Observable<void> {
    return this.httpClient.patch<void>(
      `${this.apiUrl}/${landingPageId}/publish`,
      {},
      { withCredentials: true }
    );
  }

  unpublishLandingPage(landingPageId: number): Observable<void> {
    return this.httpClient.patch<void>(
      `${this.apiUrl}/${landingPageId}/unpublish`,
      {},
      { withCredentials: true }
    );
  }

  deleteLandingPage(landingPageId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/${landingPageId}`, {
      withCredentials: true,
    });
  }

  increaseEngagement(landingPageId: number, landingPageItemId: number) {
    return this.httpClient.patch<void>(
      `${this.apiUrl}/${landingPageId}/engagements`,
      landingPageItemId
    );
  }
}
