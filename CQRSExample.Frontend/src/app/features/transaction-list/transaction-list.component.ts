import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../core/api/admin.service';
import { CommonModule } from '@angular/common';
import { PaginatedResult } from '../../shared/models/paginated-result.model';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class TransactionListComponent implements OnInit {
  transactions: any[] = [];
  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  searchTerm: string = '';
  sortField: string = 'id';
  sortOrder: string = 'asc';
  private searchSubject = new Subject<string>();
  pageSizes: number[] = [10, 50, 100];

  constructor(private adminService: AdminService) {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(searchTerm => {
      this.searchTerm = searchTerm;
      this.loadTransactions();
    });
  }

  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions(): void {
    this.adminService.getPayments(this.pageNumber, this.pageSize, this.searchTerm, this.sortField, this.sortOrder)
      .subscribe((data: PaginatedResult<any>) => {
        this.transactions = data.items;
        this.totalCount = data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data.pageSize;
      });
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchSubject.next(target.value);
  }

  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadTransactions();
  }

  onPageSizeChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.pageSize = parseInt(target.value, 10);
    this.pageNumber = 1;
    this.loadTransactions();
  }

  onSort(field: string): void {
    if (this.sortField === field) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortOrder = 'asc';
    }
    this.loadTransactions();
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  get pages(): number[] {
    const pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      pages.push(i);
    }
    return pages;
  }

  getMin(a: number, b: number): number {
    return Math.min(a, b);
  }
}
