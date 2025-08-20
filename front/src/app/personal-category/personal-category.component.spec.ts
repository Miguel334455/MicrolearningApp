import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalCategoryComponent } from './personal-category.component';

describe('PersonalCategoryComponent', () => {
  let component: PersonalCategoryComponent;
  let fixture: ComponentFixture<PersonalCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PersonalCategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PersonalCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
