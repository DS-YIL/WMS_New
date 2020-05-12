import { NgModule } from '@angular/core';
import { NbMenuModule } from '@nebular/theme';
import { NbCardModule } from '@nebular/theme';
import { ThemeModule } from '../@theme/theme.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PagesRoutingModule } from './pages-routing.module';
import { ConfirmationDialogComponent } from '../WmsCommon/confirmationdialog/confirmation-dialog.component';
import { SelectfilterPipe } from '../WmsCommon/selectfilter.pipe';
import { ConfirmationService, MessageService } from 'primeng/api';
import { MatDialogModule, MatButtonModule, MatExpansionModule } from '@angular/material';
import { ToastModule } from 'primeng/toast';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ListboxModule } from 'primeng/listbox';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { TooltipModule } from 'primeng/tooltip';
import { AutoCompleteModule } from 'primeng/autocomplete';

import { PagesComponent } from './pages.component';
import { LoginComponent } from './Home/Login.component';
import { HomeComponent } from './Home/Home.component';
import { DashboardComponent } from './Home/Dashboard.component'; 
import { POListComponent } from './WMS/POList.component';
import { SecurityHomeComponent } from './WMS/SecurityHome.component';
import { StoreClerkComponent } from './WMS/StoreClerk.component';
import { WarehouseInchargeComponent } from './WMS/WarehouseIncharge.component';
import { MaterialRequestComponent } from './WMS/MaterialRequest.component';
import { MaterialIssueDashBoardComponent } from './WMS/MaterialIssueDashBoard.component';
import { MaterialIssueComponent } from './WMS/MaterialIssue.component';
import { GatePassComponent } from './WMS/Gatepass.component';

@NgModule({
  imports: [
    NbMenuModule,
    NbCardModule,
    FormsModule,
    ReactiveFormsModule,
    PagesRoutingModule,
    ThemeModule,
    MatDialogModule,
    MatButtonModule,
    MatExpansionModule,
    ToastModule,
    CardModule,
    TableModule,
    DialogModule,
    ListboxModule,
    CalendarModule,
    CheckboxModule,
    TooltipModule,
    AutoCompleteModule
  ],
  declarations: [
    SelectfilterPipe,
    ConfirmationDialogComponent,
    PagesComponent,
    HomeComponent,
    LoginComponent,
    DashboardComponent,
    POListComponent,
    SecurityHomeComponent,
    StoreClerkComponent,
    WarehouseInchargeComponent,
    MaterialRequestComponent,
    MaterialIssueDashBoardComponent,
    MaterialIssueComponent,
    GatePassComponent
  ],
  providers: [MessageService, ConfirmationService],
  entryComponents: [ConfirmationDialogComponent]
})
export class PagesModule {
}
