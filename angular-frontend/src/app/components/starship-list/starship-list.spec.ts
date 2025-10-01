import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StarshipList } from './starship-list';

describe('StarshipList', () => {
  let component: StarshipList;
  let fixture: ComponentFixture<StarshipList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StarshipList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StarshipList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
