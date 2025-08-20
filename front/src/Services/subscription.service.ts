import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class SubscriptionService {

    constructor(private http: HttpClient) {}

    private baseUrl = 'http://localhost:7060/api/UserCourses';

    createSubscription(userId: number, courseId: number): Observable<any> {
    return this.http.post(
        `${this.baseUrl}?userId=${userId}&courseId=${courseId}`,
        {} // body vacío porque los IDs van por query
    );
    }

}