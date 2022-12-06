import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavbarComponent } from './components/common/navbar/navbar.component';
import { MatModule } from './modules/mat-module/mat-module.module';
import { HomeComponent } from './components/home/home.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CustomerService } from './services/data/customer.service';
import { LayoutModule } from '@angular/cdk/layout';
import { CustomerViewComponent } from './components/customer/customer-view/customer-view.component';
import { NotifyService } from './services/common/notify.service';
import { CustomerCreateComponent } from './components/customer/customer-create/customer-create.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomerEditComponent } from './components/customer/customer-edit/customer-edit.component';
import { ConfirmDialogComponent } from './components/common/confirm-dialog/confirm-dialog.component';

import { ProductService } from './services/data/product.service';
import { ProductViewComponent } from './components/product/product-view/product-view.component';
import { MaterialFileInputModule } from 'ngx-material-file-input';
import { ProductCreateComponent } from './components/product/product-create/product-create.component';
import { OrderViewComponent } from './components/order/order-view/order-view.component';
import { OrderCreateComponent } from './components/order/order-create/order-create.component';
import { ProductEditComponent } from './components/product/product-edit/product-edit.component';
import { OrderService } from './services/data/order.service';
import { OrderDetailsComponent } from './components/order/order-details/order-details.component';
import { OrderEditComponent } from './components/order/order-edit/order-edit.component';



@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    CustomerViewComponent,
    CustomerCreateComponent,
    CustomerEditComponent,
    ConfirmDialogComponent,
    ProductViewComponent,
    ProductCreateComponent,
    OrderViewComponent,
    OrderCreateComponent,
    ProductEditComponent,
    OrderDetailsComponent,
    OrderEditComponent


  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatModule,
    LayoutModule,
    HttpClientModule,
    ReactiveFormsModule,
    MaterialFileInputModule

    
    

  ],
  entryComponents:[ConfirmDialogComponent],
  providers: [HttpClient,CustomerService, NotifyService,ProductService,OrderService],
  bootstrap: [AppComponent]
})
export class AppModule { }
