import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseService, Course } from '../../Services/course.service';
import { LessonService, Lesson } from '../../Services/lesson.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-lesson',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './lesson.component.html',
  styleUrls: ['./lesson.component.css']
})
export class LessonComponent implements OnInit {
  courseId!: number;
  course!: Course;
  lessons: Lesson[] = [];

  private routeSub!: Subscription;

  
  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private lessonService: LessonService
  ) {}

  ngOnInit(): void {
    // Nos suscribimos a cambios de parámetros
    this.routeSub = this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.courseId = +idParam;
        this.loadCourse();
        this.loadLessons();
      }
    });

    const recargado = sessionStorage.getItem('recargado');
    if (!recargado) {
      sessionStorage.setItem('recargado', 'true');
      window.location.reload();
    } else {
      sessionStorage.removeItem('recargado'); // opcional, si quieres que se pueda volver a recargar más adelante
    }
  }

  loadCourse() {
    this.courseService.getCourse(this.courseId).subscribe(course => {
      this.course = course;
    });
  }

  loadLessons() {
    this.lessonService.getAllLesson().subscribe(lessons => {
      this.lessons = lessons.filter(lesson => lesson.courseId === this.courseId);
      console.log('curso', this.courseId);
      console.log('Lecciones filtradas por curso:', this.lessons);
    }, error => {
      console.error('Error al cargar lecciones:', error);
    }
    );
  }
  
  
}
