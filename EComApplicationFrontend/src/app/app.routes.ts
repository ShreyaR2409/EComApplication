import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent} from './components/profile/profile.component';
import { AuthGuard } from './guard/auth.guard';
import { ProductMasterComponent } from './components/product-master/product-master.component';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegistrationComponent
    },
    {
        path:'home',
        component: HomeComponent,
        canActivate: [AuthGuard]
    },
    {
        path : 'profile',
        component: ProfileComponent
    },
    {
        path : 'product-master',
        component: ProductMasterComponent
    },
    {
        path: '',
        component: LoginComponent        
    }
];
