import { Component, Input, ChangeDetectionStrategy, signal, WritableSignal } from '@angular/core';
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
  
  @Input() starship!: WritableSignal<EnrichedStarshipDto | null>;
  @Input() isOpen!: WritableSignal<boolean>;

close() {
  this.isOpen.set(false);
}

}
