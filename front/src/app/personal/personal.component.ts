import { Component } from '@angular/core';
import { PersonalCategoryComponent } from '../personal-category/personal-category.component';


@Component({
  selector: 'app-personal',
  imports: [PersonalCategoryComponent],
  templateUrl: './personal.component.html',
  styleUrl: './personal.component.css'
})
export class PersonalComponent {

}
