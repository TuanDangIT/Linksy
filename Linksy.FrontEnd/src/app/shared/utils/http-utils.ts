import { HttpErrorResponse } from '@angular/common/http';
import { ExceptionProblemDetails } from '../../core/types/exceptionProblemDetails';
import { ValidationProblemDetails } from '../../core/types/validationProblemDetails';
import { environment } from '../../../environments/environment';

type Problem = ValidationProblemDetails | ExceptionProblemDetails;

export function toErrorList(err: unknown): string[] {
  const fallback = 'Request failed. Please try again later.';

  if (!(err instanceof HttpErrorResponse)) {
    if (typeof err === 'string' && err.trim()) return [err.trim()];
    if (err instanceof Error && err.message.trim()) return [err.message.trim()];
    return [fallback];
  }

  const payload = err.error as Problem;

  if ('errors' in payload && payload.errors) {
    const list = Object.values(payload.errors)
      .flat()
      .map((m) => m.trim())
      .filter(Boolean);

    return list.length > 0 ? list : [payload.detail ?? 'Validation error'];
  }

  return [payload.detail ?? fallback];
}

export function getFileNameFromContentDisposition(value: string | null): string | null {
  if (!value) return null;

  const utf8 = /filename\*\s*=\s*UTF-8''([^;]+)/i.exec(value);
  if (utf8?.[1]) {
    try {
      return decodeURIComponent(utf8[1]);
    } catch {
      return utf8[1];
    }
  }

  const plain = /filename\s*=\s*"?([^";]+)"?/i.exec(value);
  return plain?.[1] ?? null;
}

export function saveBlob(blob: Blob, fileName: string): void {
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(url);
}

export function isValidUrl(value: string): boolean {
  try {
    const u = new URL(value);
    return u.protocol === 'http:' || u.protocol === 'https:';
  } catch {
    return false;
  }
}

export function buildShortUrl(
  code: string,
  query: Record<string, string | null | undefined | boolean>
): string {
  const params = new URLSearchParams();
  for (const [key, value] of Object.entries(query)) {
    if (value === undefined || value === null) continue;
    if (typeof value === 'boolean') {
      if (value) params.set(key, 'true');
      continue;
    }
    const trimmed = value.trim();
    if (!trimmed) continue;
    params.set(key, trimmed);
  }
  const qs = params.toString();
  const base = environment.redirectingShortenedUrlBaseUrl;
  return qs ? `${base}/${code}?${qs}` : `${base}/${code}`;
}
