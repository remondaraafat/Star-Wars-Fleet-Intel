import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StarshipListComponent } from './components/starship-list/starship-list';
import { StarshipDetailComponent } from './components/starship-detail/starship-detail';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,StarshipListComponent,StarshipDetailComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('angular-frontend');
}
