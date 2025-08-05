import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LessonComponent } from './lesson/lesson.component';


export const routes: Routes = [
    {path:"", component: HomeComponent},
    {path:"course/:id", component: LessonComponent},
];
