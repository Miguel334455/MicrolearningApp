import { Component, OnInit, Inject, PLATFORM_ID  } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseService, Course } from '../../Services/course.service';
import { LessonService, Lesson } from '../../Services/lesson.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
import { SubscriptionService } from '../../Services/subscription.service';
import { AuthService } from '../../Services/auth.service';
import { isPlatformBrowser } from '@angular/common';

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
  currentLessonIndex: number = 0;

  private routeSub!: Subscription;

  
  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private route: ActivatedRoute,
    private courseService: CourseService,
    private lessonService: LessonService,
    private subscriptionService: SubscriptionService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const value = sessionStorage.getItem('recargado');
      console.log(value);

      const recargado = sessionStorage.getItem('recargado');
      if (!recargado) {
        sessionStorage.setItem('recargado', 'true');
        window.location.reload();
      } else {
        sessionStorage.removeItem('recargado');
      }
    }

    // Nos suscribimos a cambios de parámetros
    this.routeSub = this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.courseId = +idParam;
        this.loadCourse();
        this.loadLessons();
      }
    });
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

  nextLesson() {
    if (this.currentLessonIndex < this.lessons.length - 1) {
      this.currentLessonIndex++;
    }
  }
  previousLesson() {
    if (this.currentLessonIndex > 0) {
      this.currentLessonIndex--;
    }
  }

subscribeToCourse() {
  if (!this.courseId) return;

  let userId: number | null = null;
  userId = this.authService.getUserId() ? +this.authService.getUserId() : null;
  console.log('User ID:', userId);
  if (isPlatformBrowser(this.platformId)) {
    const storedUser = sessionStorage.getItem('userId');
    if (storedUser) {
      userId = +storedUser;
    }
  }

    if (userId) {
        this.subscriptionService.createSubscription(userId, this.courseId).subscribe({
            next: () => alert('Suscripción exitosa'),
            error: (err) => {
              if (err.status === 409) {
                alert('Ya estás suscrito a este curso.');
              } else {
                alert('Suscrito a este curso.');
              }
            }
        });

    } else {
      alert('No se pudo obtener el usuario.');
    }
  }
}