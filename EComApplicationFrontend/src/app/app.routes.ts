import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ProfileComponent} from './components/profile/profile.component';
import { AuthGuard } from './guard/auth.guard';
import { ProductMasterComponent } from './components/product-master/product-master.component';
import {ChangePasswordComponent} from './components/change-password/change-password.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { CartComponent } from './components/cart/cart.component';
import { InvoiceComponent } from './components/invoice/invoice.component';
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
        component: NavbarComponent,
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
        path : 'change-password',
        component: ChangePasswordComponent
    
    },
    {
        path : 'product-list',
        component : ProductListComponent
    },
    {
        path : 'cart',
        component : CartComponent
    },
    {
        path : 'invoice',
        component : InvoiceComponent
    },
    {
        path: '',
        component: LoginComponent        
    }
];
