import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'Home',
    link: '/WMS/Home',
    icon: 'keypad-outline',
  },
  {
    title: 'Security Check',
    link: '/WMS/SecurityCheck',
    icon: 'list-outline'
  },
  {
    title: 'GRN Posting',
    link: '/WMS/GRNPosting',
    icon: 'list-outline'
  },
  {
    title: 'Material Issue Dashboard',
    link: '/WMS/MaterialIssueDashboard',
    icon: 'home-outline'
  },
  {
    title: '"Put Away"  Material wise',
    link: '/WMS/WarehouseIncharge',
    icon: 'list-outline'
  },
  {
    title: 'PM Dashboard',
    icon: 'home-outline',
    link: '/WMS/Dashboard'
  },
  {
    title: 'Gate Pass',
    icon: 'list-outline',
    link: '/WMS/GatePass'
  },
  {
    title: 'Inventory Movement',
    icon: 'list-outline',
    link: '/WMS/InventoryMovement'
  },
  {
    title: 'Obsolete IM',
    icon: 'list-outline',
    link: '/WMS/ObsoleteInventoryMovement'
  },
  {
    title: 'Excess IM',
    icon: 'list-outline',
    link: '/WMS/ExcessInventoryMovement'
  },
  {
    title: 'ABC Category',
    icon: 'list-outline',
    link: '/WMS/ABCCategory'
  },
  {
    title: 'ABC Analysis',
    icon: 'list-outline',
    link: '/WMS/ABCAnalysis'
  },
  {
    title: 'FIFO LIst',
    icon: 'list-outline',
    link: '/WMS/FIFOList'
  },
  {
    title: 'Cycle Count',
    icon: 'list-outline',
    link: '/WMS/Cyclecount'
  },
  //{
  //  title: 'Invoice Details',
  //  icon: 'list-outline',
  //  link: '/WMS/InvoiceDetails'
  //},
  {
    title: 'Cycle Config',
    icon: 'list-outline',
    link: '/WMS/Cycleconfig'
  },
  {
    title: 'PO Status',
    icon: 'list-outline',
    link: '/WMS/POStatus'
  },
  {
    title: 'Auth',
    icon: 'lock-outline',
    expanded: false,
    children: [
      {
        title: 'Assign Role',
        link: '/WMS/AssignRole',
        icon: 'layers-outline'
      },
    ],
  },

];
