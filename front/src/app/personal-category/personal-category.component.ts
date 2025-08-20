import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseCreateDTO, CourseService } from '../../Services/course.service';
import { CardComponent } from '../card/card.component';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-personal-category',
  standalone: true,
  imports: [CommonModule, FormsModule, CardComponent],
  templateUrl: './personal-category.component.html',
  styleUrl: './personal-category.component.css'
})
export class PersonalCategoryComponent implements OnInit {

  userId: number = 4;

  constructor(private courseService: CourseService, private auth: AuthService) {
    const id = parseInt(this.auth.getUserId());
    if (id) {
      this.userId = id;
    }
  }


  allCourses: CourseCreateDTO[] = []; // todos los cursos
  courses: CourseCreateDTO[] = []; // cursos filtrados
  categoryId: number = 0; // id de la categoría seleccionada
  // Obtener el ID del usuario desde el servicio de autenticación
  
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
    console.log(this.auth.getDecodedToken());
    this.courses = this.allCourses.filter(
      (course) => Number(course.idCategory) === Number(categoryId)
      && course.users?.some(u => u.id === this.userId)
    );
  }

  onCategoryChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.categoryId = Number(value);
    this.loadCoursesByCategory(this.categoryId);
  }

}
