import { Component, Input, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EnrichedStarshipDto } from '../../models/EnrichedStarshipDto';

@Component({
  selector: 'app-starship-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './starship-detail.html',
  styleUrls: ['./starship-detail.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StarshipDetailComponent {
  @Input() starship = signal<EnrichedStarshipDto | null>(null);
  @Input() isOpen = signal<boolean>(false);

  closeModal() {
    this.isOpen.set(false);
  }
}
