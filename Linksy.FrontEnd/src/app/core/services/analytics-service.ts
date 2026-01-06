import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../types/apiResponse';
import { AnalyticsRequest, EngagementResponse } from '../types/analytics';

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl + '/Analytics/engagements';

  getUrlEngagements(
    urlId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getEngagements(`urls/${urlId}`, req);
  }

  getUtmEngagements(
    utmParameterId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getEngagements(`umtparameters/${utmParameterId}`, req);
  }

  getQrCodeEngagements(
    qrCodeId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getEngagements(`qrcodes/${qrCodeId}`, req);
  }

  getBarcodeEngagements(
    barcodeId: number,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    return this.getEngagements(`barcodes/${barcodeId}`, req);
  }

  private getEngagements(
    endpoint: string,
    req: AnalyticsRequest
  ): Observable<ApiResponse<EngagementResponse>> {
    let params = new HttpParams().set('TimeRange', req.timeRange).set('Interval', req.interval);

    if (req.timeRange === 'Custom') {
      if (req.startDate) params = params.set('StartDate', req.startDate);
      if (req.endDate) params = params.set('EndDate', req.endDate);
    }

    return this.http.get<ApiResponse<EngagementResponse>>(`${this.base}/${endpoint}`, {
      params,
      withCredentials: true,
    });
  }
}