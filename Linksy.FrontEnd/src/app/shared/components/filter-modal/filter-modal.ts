import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

type FilterRow = { field: string; value: string };

@Component({
  selector: 'app-filter-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './filter-modal.html',
  styleUrl: './filter-modal.css',
})
export class FilterModal {
  @Input() isOpen = false;
  @Input() availableFilters: string[] = [];

  @Output() cancel = new EventEmitter<void>();
  @Output() apply = new EventEmitter<Record<string, string>>();

  rows = signal<FilterRow[]>([]);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen']?.currentValue === true) {
      const defaultField = this.firstUnusedField();
      if (!defaultField) {
        this.rows.set([]);
        return;
      }
      this.rows.set([{ field: defaultField, value: '' }]);
    }
  }

  private usedFields(exceptIndex?: number): Set<string> {
    const used = new Set<string>();
    this.rows().forEach((row, index) => {
      if (index === exceptIndex) return;
      const field = row.field?.trim();
      if (field) used.add(field);
    });
    return used;
  }

  private firstUnusedField(): string | null {
    const used = this.usedFields();
    const first = this.availableFilters.find((f) => !used.has(f));
    return first ?? null;
  }

  availableFiltersForRow(index: number): string[] {
    const usedElsewhere = this.usedFields(index);
    const current = this.rows()[index]?.field;

    return this.availableFilters.filter((f) => f === current || !usedElsewhere.has(f));
  }

  canAddRow(): boolean {
    return this.firstUnusedField() !== null;
  }

  addRow(): void {
    const field = this.firstUnusedField();
    if (!field) return;

    this.rows.update((current) => [...current, { field, value: '' }]);
  }

  updateField(index: number, field: string): void {
    // Safety: don’t allow duplicates even if somehow forced
    if (this.usedFields(index).has(field)) return;

    this.rows.update((current) => current.map((row, i) => (i === index ? { ...row, field } : row)));
  }

  updateValue(index: number, value: string): void {
    this.rows.update((current) => current.map((row, i) => (i === index ? { ...row, value } : row)));
  }

  isDateField(field: string): boolean {
    return field === 'CreatedAt' || field === 'UpdatedAt';
  }

  onBackdropClick(): void {
    this.cancel.emit();
  }

  onApply(): void {
    const result: Record<string, string> = {};

    for (const row of this.rows()) {
      const key = row.field?.trim();
      const value = row.value?.trim();
      if (key && value) result[key] = value;
    }

    this.apply.emit(result);
  }
}
