import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuestRegistrationFormComponent } from './guest-registration-form.component';

describe('GuestRegistrationFormComponent', () => {
  let component: GuestRegistrationFormComponent;
  let fixture: ComponentFixture<GuestRegistrationFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuestRegistrationFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuestRegistrationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
