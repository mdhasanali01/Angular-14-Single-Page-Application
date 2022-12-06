import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { throwError } from 'rxjs';
import { Customer } from 'src/app/models/data/customer';
import { baseUrl } from 'src/app/models/shared/app-constants';
import { NotifyService } from 'src/app/services/common/notify.service';
import { CustomerService } from 'src/app/services/data/customer.service';

@Component({
  selector: 'app-customer-edit',
  templateUrl: './customer-edit.component.html',
  styleUrls: ['./customer-edit.component.css']
})
export class CustomerEditComponent implements OnInit {
  customer:Customer = null!;
  imgPath:string= baseUrl;
  customerForm:FormGroup = new FormGroup({
    customerName: new FormControl('', [Validators.required, Validators.maxLength(40)]),
    address:new FormControl('', Validators.required),
    email:new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]),
    picture:new FormControl('',Validators.required)
  });
  constructor(
    private customerService:CustomerService,
    private notifyService:NotifyService,
    private activatedRoute:ActivatedRoute
  ) { }
  file: File = null!;
save(){
  if(this.customerForm.invalid) return;
      Object.assign(this.customer, this.customerForm.value);
      this.customerService.update(this.customer)
      .subscribe({
        next:r=>{
          this.notifyService.message('Data saved', 'DISMISS');
        },
        error:err=> {
          this.notifyService.message('Failed to save data', 'DISMISS');
          throwError(()=>err);
        }
      })
}
handleFileInputChange(event: any): void {
  if (event.target.files.length) {
    this.file = event.target.files[0];
    this.customerForm.controls['picture'].patchValue(this.file.name);
  }
  else {
    this.customerForm.controls['picture'].patchValue("");
  }
  
}
  ngOnInit(): void {
    let id:number=this.activatedRoute.snapshot.params['id'];
    this.customerService.getById(id)
    .subscribe({
      next: r=> {
        this.customer=r;
        //console.log(this.customer);
        this.customerForm.patchValue(this.customer);
      },
      error: err=>{
        this.notifyService.message('Failed to load customer data', 'DISMISS');
        throwError(()=>err);
      }
    })
  }

}