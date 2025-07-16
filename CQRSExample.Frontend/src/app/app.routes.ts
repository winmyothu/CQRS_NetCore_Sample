import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { AuthGuard } from './core/guards/auth.guard';
import { GuestRegistrationFormComponent } from './features/guest-registration-form/guest-registration-form.component';
import { AdminComponent } from './features/admin/admin.component';
import { GuestTableComponent } from './features/guest-table/guest-table.component';
import { TransactionListComponent } from './features/transaction-list/transaction-list.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { GuestListComponent } from './features/guest-list/guest-list.component';
import { UserInfoComponent } from './features/user-info/user-info.component';
import { SettingsComponent } from './features/settings/settings.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'guests', component: GuestListComponent },
      { path: 'transactions', component: TransactionListComponent },
      { path: 'user-info', component: UserInfoComponent },
      { path: 'settings', component: SettingsComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: 'guest-registration', component: GuestRegistrationFormComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];
