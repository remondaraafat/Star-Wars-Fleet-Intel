import { FilmDto } from "./FilmDto";
import { GetStarshipsDto } from "./GetStarshipsDto";
import { PersonDto } from "./PersonDto";

export interface EnrichedStarshipDto extends GetStarshipsDto {
  films: FilmDto[];
  pilots: PersonDto[];
  shieldBoost: number;
  targetingAccuracy: number;
}