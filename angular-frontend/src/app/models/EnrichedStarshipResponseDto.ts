import { FilmDto } from "./FilmDto";
import { GetStarshipsDto } from "./GetStarshipsDto";
import { PersonDto } from "./PersonDto";

export interface EnrichedStarshipResponseDto extends GetStarshipsDto {
  films: FilmDto[];
  pilots: PersonDto[];
  shieldBoost: number;
  targetingAccuracy: number;
}