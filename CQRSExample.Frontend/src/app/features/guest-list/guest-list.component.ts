import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../core/api/admin.service';
import { CommonModule } from '@angular/common';
import { PaginatedResult } from '../../shared/models/paginated-result.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-guest-list',
  templateUrl: './guest-list.component.html',
  styleUrls: ['./guest-list.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class GuestListComponent implements OnInit {
  guests: any[] = [];
  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.loadGuests();
  }

  loadGuests(): void {
    this.adminService.getRegistrations(this.pageNumber, this.pageSize)
      .subscribe((data: PaginatedResult<any>) => {
        this.guests = data.items;
        this.totalCount = data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data.pageSize;
      });
  }

  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadGuests();
  }

  get totalPages(): number {
    return this.getCeil(this.totalCount / this.pageSize);
  }

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  getMin(a: number, b: number): number {
    return Math.min(a, b);
  }

  getCeil(num: number): number {
    return Math.ceil(num);
  }
}
