import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-guest-table',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './guest-table.component.html',
  styleUrl: './guest-table.component.css'
})
export class GuestTableComponent {

}
