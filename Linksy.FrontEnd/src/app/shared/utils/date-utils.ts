const defaultOptions: Intl.DateTimeFormatOptions = {
  year: 'numeric',
  month: 'short',
  day: 'numeric',
  hour: '2-digit',
  minute: '2-digit',
};

export function formatDate(
  value: string | Date | null | undefined,
  locale: string = 'en-US',
  options: Intl.DateTimeFormatOptions = defaultOptions
): string {
  if (!value) return '-';

  const date = value instanceof Date ? value : new Date(value);
  if (Number.isNaN(date.getTime())) return '-';

  return new Intl.DateTimeFormat(locale, options).format(date);
}

function parseDateOnly(
  value: string | null | undefined
): { y: number; m: number; d: number } | null {
  const v = (value ?? '').trim();
  if (!/^\d{4}-\d{2}-\d{2}$/.test(v)) return null;

  const [ys, ms, ds] = v.split('-');
  const y = Number(ys);
  const m = Number(ms);
  const d = Number(ds);

  if (!Number.isFinite(y) || !Number.isFinite(m) || !Number.isFinite(d)) return null;
  if (m < 1 || m > 12) return null;
  if (d < 1 || d > 31) return null;

  return { y, m, d };
}

export function toUtcDayStartIso(dateOnly: string | null | undefined): string | null {
  const p = parseDateOnly(dateOnly);
  if (!p) return null;

  const dt = new Date(Date.UTC(p.y, p.m - 1, p.d, 0, 0, 0));
  return dt.toISOString().replace('.000Z', 'Z');
}

export function toUtcDayEndIso(dateOnly: string | null | undefined): string | null {
  const p = parseDateOnly(dateOnly);
  if (!p) return null;

  const dt = new Date(Date.UTC(p.y, p.m - 1, p.d, 23, 59, 59));
  return dt.toISOString().replace('.000Z', 'Z');
}

export function buildUtcDateRangeFilter(
  fromDateOnly: string | null | undefined,
  toDateOnly: string | null | undefined
): string | null {
  const fromIso = toUtcDayStartIso(fromDateOnly);
  const toIso = toUtcDayEndIso(toDateOnly);

  if (!fromIso && !toIso) return null;
  if (!fromIso && toIso) return `to-${toIso}`;
  if (fromIso && !toIso) return `${fromIso}-to`;

  return `${fromIso}-to-${toIso}`;
}

