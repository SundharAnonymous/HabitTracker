import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface AIRecommendation {
  recommendation: string;
}

@Injectable({
  providedIn: 'root'
})
export class AiService {
  private apiUrl = environment.apiUrl + '/AI';

  constructor(private http: HttpClient) {}

  // Calls the backend endpoint to get personalized habit recommendations
  getRecommendations(): Observable<AIRecommendation> {
    return this.http.get<AIRecommendation>(`${this.apiUrl}/recommendations/`);
  }
}
