import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { Auth } from './services/auth';
import { LogOut, LucideAngularModule} from "lucide-angular";
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, LucideAngularModule],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App {
  protected readonly title = signal('web-app');
  authService = inject(Auth);
  router = inject(Router)

  readonly logOut = LogOut;
  // Sidebar toggle state
  isSidebarOpen: boolean = false;

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  closeSidebar() {
    this.isSidebarOpen = false;
  }

  logout() {
    this.authService.logOut();
    this.router.navigateByUrl('/login');
  }
}
