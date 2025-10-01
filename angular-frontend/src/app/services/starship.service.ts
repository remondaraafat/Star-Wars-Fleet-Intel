import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GetStarshipsDto } from '../models/GetStarshipsDto';
import { Observable } from 'rxjs';
import { EnrichedStarshipDto } from '../models/EnrichedStarshipDto';

@Injectable({
  providedIn: 'root',
})
export class StarshipService {
  private apiUrl = 'http://starwars.runasp.net/api/Starships';

  starships = signal<GetStarshipsDto[]>([]);

  constructor(private http: HttpClient) {}

  fetchStarships(searchTerm: string = ''): void {
    let url = this.apiUrl;
    if (searchTerm) url += `?search=${searchTerm}`;

    this.http.get<GetStarshipsDto[]>(url).subscribe({
      next: (data) => this.starships.set(data),
      error: (err) => console.error(err),
    });
  }
  getStarshipById(id: number) {
  return this.http
    .get<EnrichedStarshipDto>(`${this.apiUrl}/${id}`)
    .toPromise()
    .catch(() => null); // return null if API fails
}

}
