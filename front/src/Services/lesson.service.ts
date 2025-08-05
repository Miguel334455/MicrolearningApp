import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Lesson {
  id: number;
  content: string;
  courseId: number;
}

@Injectable({
  providedIn: 'root'
})

export class LessonService {
    constructor(private http: HttpClient) {}
    private baseUrl = 'http://localhost:7060/api/Lessons';

    getAllLesson(): Observable<Lesson[]> {
        return this.http.get<Lesson[]>(this.baseUrl);
    }
    
    getLesson(id: number): Observable<Lesson> {
        return this.http.get<Lesson>(`${this.baseUrl}/${id}`);
    }

    createLesson(lesson: Lesson): Observable<Lesson> {
        return this.http.post<Lesson>(this.baseUrl, lesson);
    }

}