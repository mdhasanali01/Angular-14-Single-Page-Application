import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { throwError } from 'rxjs';
import { Customer } from 'src/app/models/data/customer';
import { CustomerInputModel } from 'src/app/models/view-models/input/customer-input-model';
import { NotifyService } from 'src/app/services/common/notify.service';
import { CustomerService } from 'src/app/services/data/customer.service';

@Component({
  selector: 'app-customer-create',
  templateUrl: './customer-create.component.html',
  styleUrls: ['./customer-create.component.css']
})
export class CustomerCreateComponent implements OnInit {
  customer: CustomerInputModel = { customerName: undefined, address:undefined,email:undefined};
  customerForm:FormGroup = new FormGroup({
    customerName: new FormControl('', [Validators.required, Validators.maxLength(40)]),
    address:new FormControl('', Validators.required),
    email:new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]),
    picture: new FormControl(undefined, Validators.required)
  });
  file: File = null!;
  save() {
    if (this.customerForm.invalid) return;
    Object.assign(this.customer, this.customerForm.value)
    
    var _self = this;
    
    this.customerService.insert(this.customer)
      .subscribe({
        next: r => {
          _self.notifyService.message('Data saved', 'DISMISS');
         
          var reader = new FileReader();
          
          reader.onload = function (e: any) {
            console.log(e);
            _self.customerService.uploadImage(<number>r.customerID, _self.file)
              .subscribe({
                next: r => {
                  console.log(r);
                  _self.notifyService.message('Picture uploaded', 'DISMISS');
                  
                  console.log(_self.customer)
                },
                error: err => {
                  _self.notifyService.message('Picture upload failed', 'DISMISS');
                }
              });
          }
          reader.readAsArrayBuffer(_self.file);
        },
        error: err => {
        _self.notifyService.message('Failed to save product', 'DISMISS')
        }
      });


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
  constructor(
    private customerService:CustomerService,
    private notifyService:NotifyService
  ) { }
  
  ngOnInit(): void {
  }

}
