import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AuthService } from '../features/auth/auth.service';
import { LoadingService } from '../shared/loading.service'; // Adjust the path as needed

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private loadingService: LoadingService
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Show the loading indicator before sending the request.
    this.loadingService.show();

    const token = this.authService.getToken();
    if (token) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }

    // Use finalize to hide the loading indicator when the request completes.
    return next.handle(request).pipe(
      finalize(() => this.loadingService.hide())
    );
  }
}
