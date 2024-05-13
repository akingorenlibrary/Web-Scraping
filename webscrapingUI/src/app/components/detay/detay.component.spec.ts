import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetayComponent } from './detay.component';

describe('DetayComponent', () => {
  let component: DetayComponent;
  let fixture: ComponentFixture<DetayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DetayComponent]
    });
    fixture = TestBed.createComponent(DetayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
