import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Course, CourseService } from '../../Services/course.service';
import { CardComponent } from '../card/card.component';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, FormsModule, CardComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {
  allCourses: Course[] = []; // todos los cursos
  courses: Course[] = []; // cursos filtrados
  categoryId: number = 2; // id de la categoría seleccionada
  constructor(private courseService: CourseService) {}

  ngOnInit() {
    this.courseService.getAllCourses().subscribe({
      next: (data) => {
        this.allCourses = data;
        this.loadCoursesByCategory(this.categoryId);
      },
      error: (err) => console.error('Error loading courses:', err)
    });
  }

  loadCoursesByCategory(categoryId: number) {
    this.courses = this.allCourses.filter(
      (course) => Number(course.idCategory) === Number(categoryId)
    );
  }

  onCategoryChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.categoryId = Number(value);
    this.loadCoursesByCategory(this.categoryId);
  }

}
