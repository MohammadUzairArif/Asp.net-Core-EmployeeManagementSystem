import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Eye, EyeClosed, LoaderCircle, LucideAngularModule } from 'lucide-angular';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [LucideAngularModule,ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login implements OnInit {
  fb = inject(FormBuilder);
  authService = inject(Auth);
 router = inject(Router);
  loginForm!: FormGroup;
  loading = false;
  showPassword = false; 

  readonly LoaderCircle = LoaderCircle;
  readonly eye = Eye;
  readonly eyeClosed = EyeClosed;

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$/)
        ]
      ]
    });

    if(this.authService.isLoggedIn){
      this.router.navigateByUrl('/')
  }
}

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  login() {
    this.loading = true;
    this.authService.login(this.loginForm.value).subscribe({
      next: (result) => {
        this.loading = false;
        this.authService.saveToken(result);
        console.log(result);
        alert('Login successful!');
        if(result.role === 'Admin'){
        this.router.navigateByUrl('/');
        }else{
          this.router.navigateByUrl('/employee-dashboard');
        }
      },

      error: (err:any) => {
        this.loading = false;
        if (err.error && err.error.message) {
          alert(err.error.message);
        } else {
          alert('Login failed. Please try again.');
        }
      }
    });
  }
}
