import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Course {
  id: number;
  name: string;
  description: string;
  idCategory: number;
}

export interface CourseCreateDTO {
  id: number;
  name: string;
  description: string;
  idCategory: number;
  lessons?: any[];
  users?: any[];
}

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  constructor(private http: HttpClient) {}

  private baseUrl = 'http://localhost:7060/api/Courses';

 

  getAllCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(this.baseUrl);
  }

  getCourse(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.baseUrl}/${id}`);
  }

  createCourse(course: CourseCreateDTO): Observable<Course> {
    return this.http.post<Course>(this.baseUrl, course);
  }
}
