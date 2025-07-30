import { Component, OnInit, Input } from '@angular/core';
import { Course, CourseCreateDTO, CourseService } from '../../Services/course.service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-card',
  imports: [CommonModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent implements OnInit {
  @Input() course!: Course;
  
  constructor(private courseService: CourseService) {}

  ngOnInit() {
    // Ya no necesitas cargar un curso aquí si lo recibes por @Input
    console.log('Curso recibido por Input:', this.course);
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
}
