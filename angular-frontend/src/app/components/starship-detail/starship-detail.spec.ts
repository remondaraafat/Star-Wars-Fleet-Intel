import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StarshipDetail } from './starship-detail';

describe('StarshipDetail', () => {
  let component: StarshipDetail;
  let fixture: ComponentFixture<StarshipDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StarshipDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StarshipDetail);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
