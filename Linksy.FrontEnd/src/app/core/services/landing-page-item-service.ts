import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateTextLandingPageItemRequest, CreateUrlLandingPageItemRequest, CreateYouTubeLandingPageItemRequest } from '../types/createLandingPageItem';

@Injectable({ providedIn: 'root' })
export class LandingPageItemService {
  private readonly landingPageApiUrl = environment.apiBaseUrl + '/LandingPages';
  private readonly landingPageItemApiUrl = environment.apiBaseUrl + '/LandingPageItems';
  private readonly httpClient = inject(HttpClient);

  deleteLandingPageItem(landingPageItemId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.landingPageItemApiUrl}/${landingPageItemId}`, {
      withCredentials: true,
    });
  }

  createTextItem(landingPageId: number, body: CreateTextLandingPageItemRequest): Observable<void> {
    return this.httpClient.post<void>(
      `${this.landingPageApiUrl}/${landingPageId}/LandingPageItems/texts`,
      body,
      { withCredentials: true }
    );
  }

  createYouTubeItem(
    landingPageId: number,
    body: CreateYouTubeLandingPageItemRequest
  ): Observable<void> {
    return this.httpClient.post<void>(
      `${this.landingPageApiUrl}/${landingPageId}/LandingPageItems/youtubes`,
      body,
      { withCredentials: true }
    );
  }

  createUrlItem(landingPageId: number, body: CreateUrlLandingPageItemRequest): Observable<void> {
    return this.httpClient.post<void>(
      `${this.landingPageApiUrl}/${landingPageId}/LandingPageItems/urls`,
      body,
      { withCredentials: true }
    );
  }

  createImageItem(landingPageId: number, form: FormData): Observable<void> {
    return this.httpClient.post<void>(
      `${this.landingPageApiUrl}/${landingPageId}/LandingPageItems/images`,
      form,
      { withCredentials: true }
    );
  }
}
