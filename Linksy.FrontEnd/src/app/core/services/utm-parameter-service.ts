import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AddScanCodeToUrlRequest } from '../types/addScanCodeToUrl';
import { ApiResponse } from '../types/apiResponse';
import { CreateQrCodeForUtmResponse } from '../types/createQrCodeForUtm';
import { AddUtmParameterToUrlRequest } from '../types/addUtmParameterToUrl';
import { UtmParameterDetails } from '../models/utm-parameter';

@Injectable({ providedIn: 'root' })
export class UtmParameterService {
  private readonly utmApiUrl = environment.apiBaseUrl + '/UmtParameters';
  private readonly urlsApiUrl = environment.apiBaseUrl + '/urls';
  private readonly httpClient = inject(HttpClient);

  getUtmParameterDetails(
    urlId: number,
    utmParameterId: number
  ): Observable<ApiResponse<UtmParameterDetails>> {
    return this.httpClient.get<ApiResponse<UtmParameterDetails>>(
      `${this.urlsApiUrl}/${urlId}/UmtParameters/${utmParameterId}`,
      { withCredentials: true }
    );
  }

  addUtmParameterToUrl(
    urlId: number,
    body: AddUtmParameterToUrlRequest
  ): Observable<ApiResponse<unknown>> {
    return this.httpClient.post<ApiResponse<unknown>>(
      `${this.urlsApiUrl}/${urlId}/UmtParameters`,
      body,
      {
        withCredentials: true,
      }
    );
  }

  deleteUtmParameter(utmParameterId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.utmApiUrl}/${utmParameterId}`, {
      withCredentials: true,
    });
  }

  addQrCodeToUtmParameter(
    utmParameterId: number,
    body: AddScanCodeToUrlRequest
  ): Observable<ApiResponse<CreateQrCodeForUtmResponse>> {
    return this.httpClient.post<ApiResponse<CreateQrCodeForUtmResponse>>(
      `${this.utmApiUrl}/${utmParameterId}/qrcode`,
      body,
      { withCredentials: true }
    );
  }
}
