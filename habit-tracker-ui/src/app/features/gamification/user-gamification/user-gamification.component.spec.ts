import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserGamificationComponent } from './user-gamification.component';

describe('UserGamificationComponent', () => {
  let component: UserGamificationComponent;
  let fixture: ComponentFixture<UserGamificationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserGamificationComponent]
    });
    fixture = TestBed.createComponent(UserGamificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
