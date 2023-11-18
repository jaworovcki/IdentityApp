import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { NotificationComponent } from './components/modals/notification/notification.component';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  bsModalRef?: BsModalRef;

  constructor(private modalService: BsModalService) { }

  showNotification(isSuccessful: boolean, title: string, message: string) {
    const inititalState: ModalOptions = {
      initialState: {
        isSuccessful,
        title,
        message,
      }
    };

    this.bsModalRef = this.modalService.show(NotificationComponent, inititalState);

  }
}
