import { Component, inject, OnInit } from '@angular/core';
import { Auth } from '../../services/auth';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Eye, EyeClosed, LoaderCircle, LucideAngularModule, Pencil } from 'lucide-angular';
import { Router } from '@angular/router';
import { HttpService } from '../../services/http';

@Component({
  selector: 'app-profile',
  imports: [LucideAngularModule,ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile implements OnInit {
  fb = inject(FormBuilder);
  httpService = inject(HttpService);
  authService = inject(Auth);
  router = inject(Router);

  profileForm!: FormGroup;
  loading = false;
  showOldPassword = false;
  showNewPassword = false;

  readonly LoaderCircle = LoaderCircle;
  readonly eye = Eye;
  readonly eyeClosed = EyeClosed;
   readonly Pencil = Pencil;

   

  ngOnInit(): void {
    this.getProfile();
    this.profileForm = this.fb.group({
      userId: [this.authService.AuthDetail?.id || ''], // abhi dummy
     name: ['', [Validators.required, Validators.minLength(3)]],
      
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]{10,15}$/)]],
      profileImage: [''],
     oldPassword: [
  '',
  [
    Validators.required,
    Validators.minLength(8),
    Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$/)
  ]
],
newPassword: [
  '',
  [
    Validators.required,
    Validators.minLength(8),
    Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$/)
  ]
]
    });
  }

  toggleOldPassword() {
    this.showOldPassword = !this.showOldPassword;
  }

  toggleNewPassword() {
    this.showNewPassword = !this.showNewPassword;
  }

  updateProfile() {
    this.loading = true;
    this.httpService.updateProfile(this.profileForm.value).subscribe({
      next: (res:any) => {
        this.loading = false;
        alert(res.message);
        this.router.navigate(['/']);
      },
      error: (err: any) => {
        this.loading = false;
        if (err.error && err.error.message) {
          alert(err.error.message);
        } else {
          alert('Profile update failed. Please try again.');
        }
      }
    });
  }

  getProfile() {
    this.httpService.getProfile(this.authService.AuthDetail?.id || '').subscribe({
      next: (res) => {
        
        this.profileForm.patchValue(
          res
         
        );
      },
      error: (err) => {
  
        alert('Failed to load profile. Please try again.');
    
      }

    });
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      const base64 = reader.result as string;
      this.profileForm.patchValue({
        profileImage: base64
      });
    };
    reader.readAsDataURL(file);
  }

}