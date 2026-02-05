import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppUser} from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  private accountService = inject(AccountService);
  cancelRegister = output<boolean>();
  protected creds = {} as AppUser;
  private router = inject(Router)

  register() {
    this.accountService.register(this.creds).subscribe({
      next: response => {
        this.router.navigateByUrl('/members');
        console.log(response);
        this.cancel();
      },
      error: error => console.log(error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}