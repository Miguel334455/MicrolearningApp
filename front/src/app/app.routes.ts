import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LessonComponent } from './lesson/lesson.component';
import { LoginComponent } from './login/login.component';
import { PersonalComponent } from './personal/personal.component';


export const routes: Routes = [
    {path:"", component: HomeComponent},
    {path:"course/:id", component: LessonComponent},
    {path: "login", component: LoginComponent},
    {path: "personal", component: PersonalComponent}
];
