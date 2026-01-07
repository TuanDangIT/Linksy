import { Injectable, signal } from "@angular/core";
import { Toast, ToastType } from "../types/toast";

@Injectable({ providedIn: 'root' })
export class ToastService {
  toasts = signal<Toast[]>([]);

  success(message: string, durationMs = 2500): void {
    this.push('success', message, durationMs);
  }

  error(message: string, durationMs = 4500): void {
    this.push('error', message, durationMs);
  }

  private push(type: ToastType, message: string, durationMs: number): void {
    if (!message) return;

    const id = crypto?.randomUUID?.() ?? `${Date.now()}_${Math.random()}`;
    const toast: Toast = { id, type, message: message };

    this.toasts.update((current) => [...current, toast]);
    setTimeout(() => this.dismiss(id), durationMs);
  }

  dismiss(id: string): void {
    this.toasts.update((current) => current.filter((t) => t.id !== id));
  }
}
