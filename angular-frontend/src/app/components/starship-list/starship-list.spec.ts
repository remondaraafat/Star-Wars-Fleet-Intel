import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StarshipListComponent } from './starship-list';
import { StarshipService } from '../../services/starship.service';
import { StarshipDetailComponent } from '../starship-detail/starship-detail';
import { GetStarshipsDto } from '../../models/GetStarshipsDto';
import { EnrichedStarshipDto } from '../../models/EnrichedStarshipDto';
import { By } from '@angular/platform-browser';

describe('StarshipListComponent', () => {
  let component: StarshipListComponent;
  let fixture: ComponentFixture<StarshipListComponent>;
  let starshipServiceMock: any;

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
    starshipServiceMock = {
      starships: jasmine.createSpy('starships').and.returnValue([]),
      fetchStarships: jasmine.createSpy('fetchStarships'),
      getStarshipById: jasmine.createSpy('getStarshipById').and.returnValue(Promise.resolve(enrichedStarship))
    };

    await TestBed.configureTestingModule({
      imports: [StarshipListComponent, StarshipDetailComponent],
      providers: [{ provide: StarshipService, useValue: starshipServiceMock }],
    }).compileComponents();

    fixture = TestBed.createComponent(StarshipListComponent);
    component = fixture.componentInstance;
  });

  it('should call fetchStarships on init', () => {
    component.ngOnInit();
    expect(starshipServiceMock.fetchStarships).toHaveBeenCalled();
  });

  it('should update search term and call fetchStarships on search', () => {
    component.onSearchChange('Falcon');
    expect(component.searchTerm()).toBe('Falcon');
    expect(starshipServiceMock.fetchStarships).toHaveBeenCalledWith('Falcon');
  });

  it('should update input value when searchTerm changes', () => {
    component.searchTerm.set('Falcon');
    fixture.detectChanges();

    const input = fixture.nativeElement.querySelector('input[type="text"]') as HTMLInputElement;
    expect(input.value).toBe('Falcon');
  });

  it('should open detail modal when a starship is clicked', async () => {
    const mockStarship = { id: 1 } as GetStarshipsDto;
    (starshipServiceMock.getStarshipById as jasmine.Spy).and.returnValue(Promise.resolve(enrichedStarship));

    await component.openDetail(mockStarship);
    expect(component.selectedStarship()).toEqual(enrichedStarship);
    expect(component.isModalOpen()).toBe(true);
  });

  it('should not open modal if getStarshipById returns null', async () => {
    const mockStarship = { id: 99 } as GetStarshipsDto;
    (starshipServiceMock.getStarshipById as jasmine.Spy).and.returnValue(Promise.resolve(null));

    await component.openDetail(mockStarship);

    expect(component.selectedStarship()).toBeNull();
    expect(component.isModalOpen()).toBe(false);
  });

  it('should close the detail modal', async () => {
    component.selectedStarship.set({ ...enrichedStarship });
    component.isModalOpen.set(true);
    fixture.detectChanges();

    component.isModalOpen.set(false);
    fixture.detectChanges();

    expect(component.isModalOpen()).toBe(false);
    const modal = fixture.nativeElement.querySelector('.modal');
    expect(modal).toBeNull();
  });

  it('should render starship list in template', () => {
    starshipServiceMock.starships.and.returnValue([
      { id: 1, name: 'X-Wing' } as GetStarshipsDto,
      { id: 2, name: 'TIE Fighter' } as GetStarshipsDto
    ]);

    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    const items = compiled.querySelectorAll('.starship-item');
    expect(items.length).toBe(2);
    expect(compiled.textContent).toContain('X-Wing');
    expect(compiled.textContent).toContain('TIE Fighter');
  });

  it('should pass signals to StarshipDetailComponent correctly', async () => {
    const mockStarship = { id: 1 } as GetStarshipsDto;
    await component.openDetail(mockStarship);
    fixture.detectChanges();

    const detailDebugEl = fixture.debugElement.query(By.directive(StarshipDetailComponent));
    const detailComp: StarshipDetailComponent = detailDebugEl.componentInstance;

    expect(detailComp.starship()).toEqual(enrichedStarship);
    expect(detailComp.isOpen()).toBe(true);

    detailComp.close();
    fixture.detectChanges();
    expect(component.isModalOpen()).toBe(false);
  });

  it('should update the rendered list when starships change', () => {
    starshipServiceMock.starships.and.returnValue([
      { id: 1, name: 'X-Wing' } as GetStarshipsDto,
      { id: 2, name: 'TIE Fighter' } as GetStarshipsDto
    ]);
    fixture.detectChanges();

    const items = fixture.nativeElement.querySelectorAll('.starship-item');
    expect(items.length).toBe(2);
  });
});
