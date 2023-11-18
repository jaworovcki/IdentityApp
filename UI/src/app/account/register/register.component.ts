import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessages: string[] = [];

  constructor(private accountService: AccountService,
    private formBuilder: FormBuilder,) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$')]],
      password : ['', [Validators.required, Validators.maxLength(50), Validators.minLength(2)]],
    });
  }

  register() {
    this.submitted = true;
    this.errorMessages = [];

    this.accountService.register(this.registerForm.value).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }
}
