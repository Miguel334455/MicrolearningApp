import { Component, OnInit, Input } from '@angular/core';
import { Course, CourseCreateDTO, CourseService } from '../../Services/course.service';
import { CommonModule } from '@angular/common';
import { LessonComponent } from '../lesson/lesson.component';
import { Lesson } from '../../Services/lesson.service';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-card',
  imports: [CommonModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent implements OnInit {

  @Input() course!: Course;
  showlesson: boolean = false;

  fakeLesson: Lesson = {
    id: 1,
    content: 'This is a fake lesson content',
    courseId: this.course?.id || 0 // si el curso ya está definido
  };


  constructor(private courseService: CourseService, private router: Router) {}
  ngOnInit(): void {
      
  }

  addCourse() {
    const newCourse: CourseCreateDTO = {
      id: 0,
      name: 'New Course',
      description: 'This is a new course',
      idCategory: 1
    };

    this.courseService.createCourse(newCourse).subscribe(course => {
      console.log('Course created:', course);
      this.course = course;
    });
  }
  
  onCourseClick() {
    this.router.navigate(['/course', this.course.id]);
  }
}
