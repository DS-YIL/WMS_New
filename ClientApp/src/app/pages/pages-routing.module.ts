import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from '../Wmscommon/auth.guard';
import { PagesComponent } from './pages.component';
import { DashboardComponent } from './Home/Dashboard.component';
import { LoginComponent } from './Home/Login.component';
import { POListComponent } from './WMS/POList.component';
import { SecurityHomeComponent } from './WMS/SecurityHome.component';
import { StoreClerkComponent } from './WMS/StoreClerk.component';
import { WarehouseInchargeComponent } from './WMS/WarehouseIncharge.component';
import { MaterialRequestComponent } from './WMS/MaterialRequest.component';
import { MaterialIssueDashBoardComponent } from './WMS/MaterialIssueDashBoard.component';
import { MaterialIssueComponent } from './WMS/MaterialIssue.component';
import { HomeComponent } from './Home/Home.component';
import { GatePassComponent } from './WMS/Gatepass.component';
import { GatePassApproverComponent } from './WMS/GatepassApproverForm.component';
import { GatePassPrintComponent } from './WMS/GatepassPrint.component';
import { InventoryMovementComponent } from './WMS/InventoryMovement.component';
import { ObsoleteInventoryMovementComponent } from './WMS/ObsoleteInventoryMovement.component';
import { ExcessInventoryMovementComponent } from './WMS/ExcessInventoryMovement.component';
import { ABCAnalysisComponent } from './WMS/ABCAnalysis.component';
import { ABCCategoryComponent } from './WMS/ABCCategory.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: 'Login',
      component: LoginComponent,
    },
    {
      path: '',
      redirectTo: 'Login',
      pathMatch: 'full',
    },
    { path: "Dashboard", component: DashboardComponent, canActivate: [AuthGuard] },
    { path: "Home", component: HomeComponent, canActivate: [AuthGuard] },
    { path: "POList", component: POListComponent, canActivate: [AuthGuard] },
    { path: "SecurityCheck", component: SecurityHomeComponent, canActivate: [AuthGuard] },
    { path: "GRNPosting", component: StoreClerkComponent, canActivate: [AuthGuard] },
    { path: "WarehouseIncharge", component: WarehouseInchargeComponent, canActivate: [AuthGuard] },
    { path: "MaterialRequest", component: MaterialRequestComponent, canActivate: [AuthGuard] },
    { path: 'MaterialRequest/:pono', component: MaterialRequestComponent, canActivate: [AuthGuard] },
    { path: "MaterialIssueDashboard", component: MaterialIssueDashBoardComponent, canActivate: [AuthGuard] },
    { path: "MaterialIssue/:requestid", component: MaterialIssueComponent, canActivate: [AuthGuard] },
    { path: "GatePass", component: GatePassComponent, canActivate: [AuthGuard] },
    { path: "GatePassApprover/:gatepassid", component: GatePassApproverComponent, canActivate: [AuthGuard] },
    { path: "GatePassPrint/:gatepassid", component: GatePassPrintComponent, canActivate: [AuthGuard] },
    { path: "InventoryMovement", component: InventoryMovementComponent, canActivate: [AuthGuard] },
    { path: "ObsoleteInventoryMovement", component: ObsoleteInventoryMovementComponent, canActivate: [AuthGuard] },
    { path: "ExcessInventoryMovement", component: ExcessInventoryMovementComponent, canActivate: [AuthGuard] },
    { path: "ABCCategory", component: ABCCategoryComponent, canActivate: [AuthGuard] },
    { path: "ABCAnalysis", component: ABCAnalysisComponent, canActivate: [AuthGuard] },
    { path: "ABCAnalysis/:material", component: ABCAnalysisComponent, canActivate: [AuthGuard] }
  ],
}];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
