import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StarshipDetailComponent } from './starship-detail';
import { signal } from '@angular/core';
import { EnrichedStarshipDto } from '../../models/EnrichedStarshipDto';
import { By } from '@angular/platform-browser';

describe('StarshipDetailComponent', () => {
  let component: StarshipDetailComponent;
  let fixture: ComponentFixture<StarshipDetailComponent>;

  const enrichedStarship: EnrichedStarshipDto = {
    id: 1,
    name: 'X-Wing',
    model: 'T-65',
    manufacturer: 'Incom Corporation',
    costInCredits: '149999',
    length: '12.5',
    maxAtmospheringSpeed: '1050',
    crew: '1',
    passengers: '0',
    cargoCapacity: '110',
    consumables: '1 week',
    hyperdriveRating: '1.0',
    mglt: '100',
    starshipClass: 'Starfighter',
    pilots: [],
    films: [],
    shieldBoost: 50,
    targetingAccuracy: 80,
    weaponPower: 60,
    currencySymbol: '$',
    convertedCost: 149999,
    url: 'http://fakeurl.com'
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StarshipDetailComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(StarshipDetailComponent);
    component = fixture.componentInstance;
    component.isOpen = signal(true);
    component.starship = signal({ ...enrichedStarship });
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should close modal on close()', () => {
    component.close();
    expect(component.isOpen()).toBe(false);
  });

  it('should display starship name in template', () => {
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('X-Wing');
  });

  it('should update template when starship signal changes', () => {
    component.starship.set({ ...enrichedStarship, name: 'TIE Fighter' });
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('TIE Fighter');
  });

  it('should reflect isOpen signal in template', () => {
    // Close modal and check template
    component.isOpen.set(false);
    fixture.detectChanges();
    const modal = fixture.nativeElement.querySelector('.modal');
    expect(modal).toBeNull();
  });
});
