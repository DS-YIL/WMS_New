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
import { GatePassApproverComponent } from './WMS/GatepassApproverForm.component';
import { GatePassPrintComponent } from './WMS/GatepassPrint.component';
import { InventoryMovementComponent } from './WMS/InventoryMovement.component';
import { ObsoleteInventoryMovementComponent } from './WMS/ObsoleteInventoryMovement.component';
import { ExcessInventoryMovementComponent } from './WMS/ExcessInventoryMovement.component';
import { ABCAnalysisComponent } from './WMS/ABCAnalysis.component';
import { ABCCategoryComponent } from './WMS/ABCCategory.component';
import { FIFOComponent } from './WMS/FIFO.component';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CyclecountComponent } from './WMS/Cyclecount.component';
import { CycleconfigComponent } from './WMS/Cycleconfig.component';
import { AssignRoleComponent } from './WMS/AssignRole.component';
import { POStatusComponent } from './WMS/POStatus.component';
import { InvoiceDetailsComponent } from './WMS/InvoiceDetails.component';
import { MaterialDetailsComponent } from './WMS/MaterialDetails.component';
import { LocationDetailsComponent } from './WMS/LocationDetails.component';
import { MaterialRequestDetailsComponent } from './WMS/MaterialRequestDetails.component';
import { MaterialRequestViewComponent } from './WMS/MaterialRequestView.component';
import { MaterialReserveComponent } from './WMS/MaterialReserve.component';

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
    AutoCompleteModule,
    ConfirmDialogModule
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
    GatePassComponent,
    GatePassApproverComponent,
    GatePassPrintComponent,
    InventoryMovementComponent,
    ObsoleteInventoryMovementComponent,
    ExcessInventoryMovementComponent,
    ABCCategoryComponent,
    ABCAnalysisComponent,
    FIFOComponent,
    CyclecountComponent,
    CycleconfigComponent,
    AssignRoleComponent,
    POStatusComponent,
    InvoiceDetailsComponent,
    MaterialDetailsComponent,
    LocationDetailsComponent,
    MaterialRequestDetailsComponent,
    MaterialRequestViewComponent,
    MaterialReserveComponent
  ],
  providers: [MessageService, ConfirmationService],
  entryComponents: [ConfirmationDialogComponent]
})
export class PagesModule {
}
