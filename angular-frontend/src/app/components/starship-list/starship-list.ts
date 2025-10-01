import { Component, OnInit, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { StarshipService } from '../../services/starship.service';
import { GetStarshipsDto } from '../../models/GetStarshipsDto';
import { StarshipDetailComponent } from '../starship-detail/starship-detail'; 
import { EnrichedStarshipDto } from '../../models/EnrichedStarshipDto';
@Component({
  selector: 'app-starship-list',
  standalone: true,
  imports: [CommonModule, FormsModule, StarshipDetailComponent],
  templateUrl: './starship-list.html',
  styleUrls: ['./starship-list.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StarshipListComponent implements OnInit {
  searchTerm = signal<string>('');
  selectedStarship = signal<EnrichedStarshipDto | null>(null);
  isModalOpen = signal(false);

  constructor(public readonly starshipService: StarshipService) {}

  ngOnInit(): void {
    this.starshipService.fetchStarships();
  }

  onSearchChange(value: string): void {
    this.searchTerm.set(value);
    this.starshipService.fetchStarships(value);
  }

  async openDetail(starship: GetStarshipsDto) {
    const fullStarship = await this.starshipService.getStarshipById(starship.id);
    this.selectedStarship.set(fullStarship ?? null);
    this.isModalOpen.set(true);
  }

  // ✅ Getter for template
  get starships(): GetStarshipsDto[] {
    return this.starshipService.starships();
  }
}
