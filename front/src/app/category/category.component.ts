// category.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Course, CourseService } from '../../Services/course.service';
import { CardComponent } from '../card/card.component';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, CardComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {
  courses: Course[] = [];
  categoryId: number = 2; // O puedes tomarlo dinámicamente

  constructor(private courseService: CourseService) {}

  ngOnInit(): void {
    this.courseService.getAllCourses().subscribe({
      next: (data) => {
        // Filtrar cursos por categoría
        this.courses = data.filter(course => course.idCategory === this.categoryId);
      },
      error: (err) => console.error('Error loading courses:', err)
    });
  }
}
