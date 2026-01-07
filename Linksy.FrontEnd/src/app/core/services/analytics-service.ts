import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../types/apiResponse';
import { AnalyticsRequest, EngagementResponse } from '../types/analytics';
import {
  UtmCampaignCountsResponse,
  UtmMediumCountsResponse,
  UtmSourceCountsResponse,
} from '../types/utm-parameter-counts';

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private readonly http = inject(HttpClient);

  private readonly engagementsBase = environment.apiBaseUrl + '/Analytics/engagements';
  private readonly viewsBase = environment.apiBaseUrl + '/Analytics/views';
  private readonly countsBase = environment.apiBaseUrl + '/Analytics/counts';

  getUrlEngagements(
    urlId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getAnalytics(this.engagementsBase, `urls/${urlId}`, req);
  }

  getUtmEngagements(
    utmParameterId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getAnalytics(this.engagementsBase, `umtparameters/${utmParameterId}`, req);
  }

  getQrCodeEngagements(
    qrCodeId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getAnalytics(this.engagementsBase, `qrcodes/${qrCodeId}`, req);
  }

  getBarcodeEngagements(
    barcodeId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getAnalytics(this.engagementsBase, `barcodes/${barcodeId}`, req);
  }

  getLandingPageEngagements(
    landingPageId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getAnalytics(this.engagementsBase, `landingpages/${landingPageId}`, req);
  }

  getLandingPageViews(
    landingPageId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getAnalytics(this.viewsBase, `landingpages/${landingPageId}`, req);
  }

  private getAnalytics(
    base: string,
    endpoint: string,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    let params = new HttpParams().set('TimeRange', req.timeRange).set('Interval', req.interval);

    if (req.timeRange === 'Custom') {
      if (req.startDate) params = params.set('StartDate', req.startDate);
      if (req.endDate) params = params.set('EndDate', req.endDate);
    }

    return this.http.get<ApiResponse<EngagementResponse>>(`${base}/${endpoint}`, {
      params,
      withCredentials: true,
    });
  }

  getUrlUtmCampaignCounts(urlId: number): Observable<ApiResponse<UtmCampaignCountsResponse>> {
    return this.http.get<ApiResponse<UtmCampaignCountsResponse>>(
      `${this.countsBase}/urls/${urlId}/umtparameters/campaigns`,
      { withCredentials: true }
    );
  }

  getUrlUtmMediumCounts(urlId: number): Observable<ApiResponse<UtmMediumCountsResponse>> {
    return this.http.get<ApiResponse<UtmMediumCountsResponse>>(
      `${this.countsBase}/urls/${urlId}/umtparameters/mediums`,
      { withCredentials: true }
    );
  }

  getUrlUtmSourceCounts(urlId: number): Observable<ApiResponse<UtmSourceCountsResponse>> {
    return this.http.get<ApiResponse<UtmSourceCountsResponse>>(
      `${this.countsBase}/urls/${urlId}/umtparameters/sources`,
      { withCredentials: true }
    );
  }
}
