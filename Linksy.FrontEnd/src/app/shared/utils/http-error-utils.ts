import { HttpErrorResponse } from '@angular/common/http';
import { ExceptionProblemDetails } from '../../core/types/exceptionProblemDetails';
import { ValidationProblemDetails } from '../../core/types/validationProblemDetails';

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
