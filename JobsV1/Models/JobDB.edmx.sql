
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/22/2020 14:45:13
-- Generated from EDMX file: C:\Users\VILLOSA\Documents\GitHub\eJob20\JobsV1\Models\JobDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-JobsV1-20160528101923];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_JobMainJobType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobTypes] DROP CONSTRAINT [FK_JobMainJobType];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainJobSupplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobServices] DROP CONSTRAINT [FK_JobMainJobSupplier];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierJobSupplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobServices] DROP CONSTRAINT [FK_SupplierJobSupplier];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobMains] DROP CONSTRAINT [FK_CustomerJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_ServicesJobServices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobServices] DROP CONSTRAINT [FK_ServicesJobServices];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainJobItinerary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobItineraries] DROP CONSTRAINT [FK_JobMainJobItinerary];
GO
IF OBJECT_ID(N'[dbo].[FK_DestinationJobItinerary]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobItineraries] DROP CONSTRAINT [FK_DestinationJobItinerary];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainJobPickup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobPickups] DROP CONSTRAINT [FK_JobMainJobPickup];
GO
IF OBJECT_ID(N'[dbo].[FK_CityBranch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Branches] DROP CONSTRAINT [FK_CityBranch];
GO
IF OBJECT_ID(N'[dbo].[FK_CitySupplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK_CitySupplier];
GO
IF OBJECT_ID(N'[dbo].[FK_BranchJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobMains] DROP CONSTRAINT [FK_BranchJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_CityDestination]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Destinations] DROP CONSTRAINT [FK_CityDestination];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierTypeSupplier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK_SupplierTypeSupplier];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierItems] DROP CONSTRAINT [FK_SupplierSupplierItem];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierItemJobServices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobServices] DROP CONSTRAINT [FK_SupplierItemJobServices];
GO
IF OBJECT_ID(N'[dbo].[FK_JobServicesJobServicePickup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobServicePickups] DROP CONSTRAINT [FK_JobServicesJobServicePickup];
GO
IF OBJECT_ID(N'[dbo].[FK_JobStatusJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobMains] DROP CONSTRAINT [FK_JobStatusJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_JobThruJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobMains] DROP CONSTRAINT [FK_JobThruJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainJobPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobPayments] DROP CONSTRAINT [FK_JobMainJobPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_BankJobPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobPayments] DROP CONSTRAINT [FK_BankJobPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_CarCategoryCarUnit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarUnits] DROP CONSTRAINT [FK_CarCategoryCarUnit];
GO
IF OBJECT_ID(N'[dbo].[FK_CityCarDestination]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarDestinations] DROP CONSTRAINT [FK_CityCarDestination];
GO
IF OBJECT_ID(N'[dbo].[FK_CarUnitCarRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarRates] DROP CONSTRAINT [FK_CarUnitCarRate];
GO
IF OBJECT_ID(N'[dbo].[FK_CarUnitCarReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarReservations] DROP CONSTRAINT [FK_CarUnitCarReservation];
GO
IF OBJECT_ID(N'[dbo].[FK_CarUnitCarImage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarImages] DROP CONSTRAINT [FK_CarUnitCarImage];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductPrice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductPrices] DROP CONSTRAINT [FK_ProductProductPrice];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductImages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductImages1] DROP CONSTRAINT [FK_ProductProductImages];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductCondition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductConditions] DROP CONSTRAINT [FK_ProductProductCondition];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductCategoryProductProdCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductProdCats] DROP CONSTRAINT [FK_ProductCategoryProductProdCat];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductProdCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductProdCats] DROP CONSTRAINT [FK_ProductProductProdCat];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainJobNote]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobNotes] DROP CONSTRAINT [FK_JobMainJobNote];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustCats] DROP CONSTRAINT [FK_CustomerCustCat];
GO
IF OBJECT_ID(N'[dbo].[FK_CustCategoryCustCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustCats] DROP CONSTRAINT [FK_CustCategoryCustCat];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerSalesLead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeads] DROP CONSTRAINT [FK_CustomerSalesLead];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesActCodeSalesActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesActivities] DROP CONSTRAINT [FK_SalesActCodeSalesActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesActivities] DROP CONSTRAINT [FK_SalesLeadSalesActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntities] DROP CONSTRAINT [FK_CustEntMainCustEntity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustEntity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntities] DROP CONSTRAINT [FK_CustomerCustEntity];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesActStatusSalesActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesActivities] DROP CONSTRAINT [FK_SalesActStatusSalesActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadCatCodeSalesLeadCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadCategories] DROP CONSTRAINT [FK_SalesLeadCatCodeSalesLeadCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesLeadCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadCategories] DROP CONSTRAINT [FK_SalesLeadSalesLeadCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadCatCodeCustSalesCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustSalesCategories] DROP CONSTRAINT [FK_SalesLeadCatCodeCustSalesCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustSalesCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustSalesCategories] DROP CONSTRAINT [FK_CustomerCustSalesCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesStatusCodeSalesStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesStatus] DROP CONSTRAINT [FK_SalesStatusCodeSalesStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesStatus] DROP CONSTRAINT [FK_SalesLeadSalesStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_JobServicesJobAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobActions] DROP CONSTRAINT [FK_JobServicesJobAction];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemCatInvItemCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvItemCategories] DROP CONSTRAINT [FK_InvItemCatInvItemCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvItemCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvItemCategories] DROP CONSTRAINT [FK_InvItemInvItemCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_JobServicesJobServiceItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobServiceItems] DROP CONSTRAINT [FK_JobServicesJobServiceItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemJobServiceItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobServiceItems] DROP CONSTRAINT [FK_InvItemJobServiceItem];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierInvItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierInvItems] DROP CONSTRAINT [FK_SupplierSupplierInvItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemSupplierInvItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierInvItems] DROP CONSTRAINT [FK_InvItemSupplierInvItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ServicesSrvActionItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SrvActionItems] DROP CONSTRAINT [FK_ServicesSrvActionItem];
GO
IF OBJECT_ID(N'[dbo].[FK_SrvActionItemJobAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobActions] DROP CONSTRAINT [FK_SrvActionItemJobAction];
GO
IF OBJECT_ID(N'[dbo].[FK_SrvActionCodeSrvActionItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SrvActionItems] DROP CONSTRAINT [FK_SrvActionCodeSrvActionItem];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustFiles] DROP CONSTRAINT [FK_CustomerCustFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierPoHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierPoHdrs] DROP CONSTRAINT [FK_SupplierSupplierPoHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierPoStatusSupplierPoHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierPoHdrs] DROP CONSTRAINT [FK_SupplierPoStatusSupplierPoHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierPoHdrSupplierPoDtl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierPoDtls] DROP CONSTRAINT [FK_SupplierPoHdrSupplierPoDtl];
GO
IF OBJECT_ID(N'[dbo].[FK_JobServicesSupplierPoDtl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierPoDtls] DROP CONSTRAINT [FK_JobServicesSupplierPoDtl];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierPoDtlSupplierPoItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierPoItems] DROP CONSTRAINT [FK_SupplierPoDtlSupplierPoItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemSupplierPoItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierPoItems] DROP CONSTRAINT [FK_InvItemSupplierPoItem];
GO
IF OBJECT_ID(N'[dbo].[FK_CustFilesCustFileRef]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustFileRefs] DROP CONSTRAINT [FK_CustFilesCustFileRef];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesLeadLink]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadLinks] DROP CONSTRAINT [FK_SalesLeadSalesLeadLink];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainSalesLeadLink]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadLinks] DROP CONSTRAINT [FK_JobMainSalesLeadLink];
GO
IF OBJECT_ID(N'[dbo].[FK_InvCarRecordTypeInvCarRecord]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvCarRecords] DROP CONSTRAINT [FK_InvCarRecordTypeInvCarRecord];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvCarRecord]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvCarRecords] DROP CONSTRAINT [FK_InvItemInvCarRecord];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvCarGateControl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvCarGateControls] DROP CONSTRAINT [FK_InvItemInvCarGateControl];
GO
IF OBJECT_ID(N'[dbo].[FK_CarUnitCarViewPage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarViewPages] DROP CONSTRAINT [FK_CarUnitCarViewPage];
GO
IF OBJECT_ID(N'[dbo].[FK_CarRatePackageCarRateUnitPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarRateUnitPackages] DROP CONSTRAINT [FK_CarRatePackageCarRateUnitPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_CarUnitCarRateUnitPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarRateUnitPackages] DROP CONSTRAINT [FK_CarUnitCarRateUnitPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_CarRateUnitPackageCarResPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarResPackages] DROP CONSTRAINT [FK_CarRateUnitPackageCarResPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_CarReservationCarResPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarResPackages] DROP CONSTRAINT [FK_CarReservationCarResPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_CarUnitCarUnitMeta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarUnitMetas] DROP CONSTRAINT [FK_CarUnitCarUnitMeta];
GO
IF OBJECT_ID(N'[dbo].[FK_CoopMemberCoopMemberItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CoopMemberItems] DROP CONSTRAINT [FK_CoopMemberCoopMemberItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemCoopMemberItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CoopMemberItems] DROP CONSTRAINT [FK_InvItemCoopMemberItem];
GO
IF OBJECT_ID(N'[dbo].[FK_RateGroupCarRateGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarRateGroups] DROP CONSTRAINT [FK_RateGroupCarRateGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_CarRatePackageCarRateGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarRateGroups] DROP CONSTRAINT [FK_CarRatePackageCarRateGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_EmailBlasterLogsBlasterLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlasterLogs] DROP CONSTRAINT [FK_EmailBlasterLogsBlasterLog];
GO
IF OBJECT_ID(N'[dbo].[FK_EmailBlasterTemplateBlasterLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BlasterLogs] DROP CONSTRAINT [FK_EmailBlasterTemplateBlasterLog];
GO
IF OBJECT_ID(N'[dbo].[FK_JobEntMainJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobEntMains] DROP CONSTRAINT [FK_JobEntMainJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_JobEntMainCustEntMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobEntMains] DROP CONSTRAINT [FK_JobEntMainCustEntMain];
GO
IF OBJECT_ID(N'[dbo].[FK_CashExpenseJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CashExpenses] DROP CONSTRAINT [FK_CashExpenseJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerPortalCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PortalCustomers] DROP CONSTRAINT [FK_CustomerPortalCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpensesJobExpenses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobExpenses] DROP CONSTRAINT [FK_ExpensesJobExpenses];
GO
IF OBJECT_ID(N'[dbo].[FK_JobServicesJobExpenses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobExpenses] DROP CONSTRAINT [FK_JobServicesJobExpenses];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpensesCategoryExpenses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Expenses] DROP CONSTRAINT [FK_ExpensesCategoryExpenses];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainJobPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobPosts] DROP CONSTRAINT [FK_JobMainJobPost];
GO
IF OBJECT_ID(N'[dbo].[FK_OnlineReservationRsvPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RsvPayments] DROP CONSTRAINT [FK_OnlineReservationRsvPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_PickupInstructionsDriverInstructions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DriverInsJobServices] DROP CONSTRAINT [FK_PickupInstructionsDriverInstructions];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesLeadCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadCompanies] DROP CONSTRAINT [FK_SalesLeadSalesLeadCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainSalesLeadCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadCompanies] DROP CONSTRAINT [FK_CustEntMainSalesLeadCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntAddresses] DROP CONSTRAINT [FK_CustEntMainCustEntAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntCats] DROP CONSTRAINT [FK_CustEntMainCustEntCat];
GO
IF OBJECT_ID(N'[dbo].[FK_CustCategoryCustEntCat]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntCats] DROP CONSTRAINT [FK_CustCategoryCustEntCat];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntClauses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntClauses] DROP CONSTRAINT [FK_CustEntMainCustEntClauses];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierContacts] DROP CONSTRAINT [FK_SupplierSupplierContact];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierInvItemSupplierItemRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierItemRates] DROP CONSTRAINT [FK_SupplierInvItemSupplierItemRate];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierUnitSupplierItemRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierItemRates] DROP CONSTRAINT [FK_SupplierUnitSupplierItemRate];
GO
IF OBJECT_ID(N'[dbo].[FK_JobServicesPickupInstructions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DriverInsJobServices] DROP CONSTRAINT [FK_JobServicesPickupInstructions];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadSalesLeadItems]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadItems] DROP CONSTRAINT [FK_SalesLeadSalesLeadItems];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemSalesLeadItems]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadItems] DROP CONSTRAINT [FK_InvItemSalesLeadItems];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesLeadItemsSalesLeadQuotedItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadQuotedItems] DROP CONSTRAINT [FK_SalesLeadItemsSalesLeadQuotedItem];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustSocialAcc]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustSocialAccs] DROP CONSTRAINT [FK_CustomerCustSocialAcc];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierContactStatusSupplierContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierContacts] DROP CONSTRAINT [FK_SupplierContactStatusSupplierContact];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierCountry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK_SupplierCountry];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntAssign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntAssigns] DROP CONSTRAINT [FK_CustEntMainCustEntAssign];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierActivities] DROP CONSTRAINT [FK_SupplierSupplierActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierDocuments] DROP CONSTRAINT [FK_SupplierSupplierDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_SupDocumentSupplierDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierDocuments] DROP CONSTRAINT [FK_SupDocumentSupplierDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntDocumentsSupDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntDocuments] DROP CONSTRAINT [FK_CustEntDocumentsSupDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntDocuments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntDocuments] DROP CONSTRAINT [FK_CustEntMainCustEntDocuments];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainCustEntActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntActivities] DROP CONSTRAINT [FK_CustEntMainCustEntActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustNotifRecipient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustNotifRecipients] DROP CONSTRAINT [FK_CustomerCustNotifRecipient];
GO
IF OBJECT_ID(N'[dbo].[FK_CustNotifCustNotifRecipient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustNotifRecipients] DROP CONSTRAINT [FK_CustNotifCustNotifRecipient];
GO
IF OBJECT_ID(N'[dbo].[FK_NotifRecipientCustNotifRecipient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustNotifRecipients] DROP CONSTRAINT [FK_NotifRecipientCustNotifRecipient];
GO
IF OBJECT_ID(N'[dbo].[FK_CustNotifRecipientCustNotifActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustNotifActivities] DROP CONSTRAINT [FK_CustNotifRecipientCustNotifActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleJobVehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobVehicles] DROP CONSTRAINT [FK_VehicleJobVehicle];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainJobVehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobVehicles] DROP CONSTRAINT [FK_JobMainJobVehicle];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerVehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vehicles] DROP CONSTRAINT [FK_CustomerVehicle];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntMainVehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vehicles] DROP CONSTRAINT [FK_CustEntMainVehicle];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleBrandVehicleMake]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VehicleModels] DROP CONSTRAINT [FK_VehicleBrandVehicleMake];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleTypeVehicleMake]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VehicleModels] DROP CONSTRAINT [FK_VehicleTypeVehicleMake];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleTransmissionVehicleMake]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VehicleModels] DROP CONSTRAINT [FK_VehicleTransmissionVehicleMake];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleFuelVehicleMake]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VehicleModels] DROP CONSTRAINT [FK_VehicleFuelVehicleMake];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleModelVehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vehicles] DROP CONSTRAINT [FK_VehicleModelVehicle];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleDriveVehicleModel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VehicleModels] DROP CONSTRAINT [FK_VehicleDriveVehicleModel];
GO
IF OBJECT_ID(N'[dbo].[FK_JobPostSaleJobServices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobPostSales] DROP CONSTRAINT [FK_JobPostSaleJobServices];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntAccountTypeCustEntMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntMains] DROP CONSTRAINT [FK_CustEntAccountTypeCustEntMain];
GO
IF OBJECT_ID(N'[dbo].[FK_JobMainPaymentStatusJobMain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobMainPaymentStatus] DROP CONSTRAINT [FK_JobMainPaymentStatusJobMain];
GO
IF OBJECT_ID(N'[dbo].[FK_JobPaymentStatusJobMainPaymentStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobMainPaymentStatus] DROP CONSTRAINT [FK_JobPaymentStatusJobMainPaymentStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemCommiInvItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvItemCommis] DROP CONSTRAINT [FK_InvItemCommiInvItem];
GO
IF OBJECT_ID(N'[dbo].[FK_JobPaymentTypeJobPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobPayments] DROP CONSTRAINT [FK_JobPaymentTypeJobPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_JobPostSalesStatusJobPostSale]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JobPostSales] DROP CONSTRAINT [FK_JobPostSalesStatusJobPostSale];
GO
IF OBJECT_ID(N'[dbo].[FK_ServicesSvcGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SvcGroups] DROP CONSTRAINT [FK_ServicesSvcGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_SvcDetailSvcGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SvcGroups] DROP CONSTRAINT [FK_SvcDetailSvcGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierActStatusSupplierActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierActivities] DROP CONSTRAINT [FK_SupplierActStatusSupplierActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntActPostSaleStatusCustEntActPostSale]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntActPostSales] DROP CONSTRAINT [FK_CustEntActPostSaleStatusCustEntActPostSale];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierItemRateSalesLeadQuotedItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesLeadQuotedItems] DROP CONSTRAINT [FK_SupplierItemRateSalesLeadQuotedItem];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntActStatusCustEntActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntActivities] DROP CONSTRAINT [FK_CustEntActStatusCustEntActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntActActionStatusCustEntActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntActivities] DROP CONSTRAINT [FK_CustEntActActionStatusCustEntActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CustEntActActionCodesCustEntActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustEntActivities] DROP CONSTRAINT [FK_CustEntActActionCodesCustEntActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CarDetailCarUnit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarDetails] DROP CONSTRAINT [FK_CarDetailCarUnit];
GO
IF OBJECT_ID(N'[dbo].[FK_CarResTypeCarReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarReservations] DROP CONSTRAINT [FK_CarResTypeCarReservation];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[JobMains]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobMains];
GO
IF OBJECT_ID(N'[dbo].[Suppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suppliers];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[JobTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobTypes];
GO
IF OBJECT_ID(N'[dbo].[Services]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Services];
GO
IF OBJECT_ID(N'[dbo].[JobServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobServices];
GO
IF OBJECT_ID(N'[dbo].[JobItineraries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobItineraries];
GO
IF OBJECT_ID(N'[dbo].[Destinations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Destinations];
GO
IF OBJECT_ID(N'[dbo].[JobPickups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPickups];
GO
IF OBJECT_ID(N'[dbo].[Branches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Branches];
GO
IF OBJECT_ID(N'[dbo].[Cities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cities];
GO
IF OBJECT_ID(N'[dbo].[SupplierTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierTypes];
GO
IF OBJECT_ID(N'[dbo].[SupplierItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierItems];
GO
IF OBJECT_ID(N'[dbo].[JobServicePickups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobServicePickups];
GO
IF OBJECT_ID(N'[dbo].[JobStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobStatus];
GO
IF OBJECT_ID(N'[dbo].[JobThrus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobThrus];
GO
IF OBJECT_ID(N'[dbo].[Banks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Banks];
GO
IF OBJECT_ID(N'[dbo].[JobPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPayments];
GO
IF OBJECT_ID(N'[dbo].[CarCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarCategories];
GO
IF OBJECT_ID(N'[dbo].[CarUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarUnits];
GO
IF OBJECT_ID(N'[dbo].[CarDestinations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarDestinations];
GO
IF OBJECT_ID(N'[dbo].[CarRates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarRates];
GO
IF OBJECT_ID(N'[dbo].[CarReservations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarReservations];
GO
IF OBJECT_ID(N'[dbo].[CarImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarImages];
GO
IF OBJECT_ID(N'[dbo].[JobContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobContacts];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[ProductPrices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductPrices];
GO
IF OBJECT_ID(N'[dbo].[ProductImages1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductImages1];
GO
IF OBJECT_ID(N'[dbo].[ProductConditions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductConditions];
GO
IF OBJECT_ID(N'[dbo].[ProductCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductCategories];
GO
IF OBJECT_ID(N'[dbo].[ProductProdCats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductProdCats];
GO
IF OBJECT_ID(N'[dbo].[PreDefinedNotes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PreDefinedNotes];
GO
IF OBJECT_ID(N'[dbo].[JobNotes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobNotes];
GO
IF OBJECT_ID(N'[dbo].[JobChecklists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobChecklists];
GO
IF OBJECT_ID(N'[dbo].[CustCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustCategories];
GO
IF OBJECT_ID(N'[dbo].[CustCats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustCats];
GO
IF OBJECT_ID(N'[dbo].[CustEntMains]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntMains];
GO
IF OBJECT_ID(N'[dbo].[SalesLeads]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeads];
GO
IF OBJECT_ID(N'[dbo].[SalesStatusCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesStatusCodes];
GO
IF OBJECT_ID(N'[dbo].[SalesStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesStatus];
GO
IF OBJECT_ID(N'[dbo].[SalesActCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesActCodes];
GO
IF OBJECT_ID(N'[dbo].[SalesActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesActivities];
GO
IF OBJECT_ID(N'[dbo].[CustEntities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntities];
GO
IF OBJECT_ID(N'[dbo].[SalesActStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesActStatus];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadCatCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadCatCodes];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadCategories];
GO
IF OBJECT_ID(N'[dbo].[CustSalesCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustSalesCategories];
GO
IF OBJECT_ID(N'[dbo].[SrvActionCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SrvActionCodes];
GO
IF OBJECT_ID(N'[dbo].[SrvActionItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SrvActionItems];
GO
IF OBJECT_ID(N'[dbo].[JobActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobActions];
GO
IF OBJECT_ID(N'[dbo].[InvItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvItems];
GO
IF OBJECT_ID(N'[dbo].[InvItemCats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvItemCats];
GO
IF OBJECT_ID(N'[dbo].[InvItemCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvItemCategories];
GO
IF OBJECT_ID(N'[dbo].[JobServiceItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobServiceItems];
GO
IF OBJECT_ID(N'[dbo].[SupplierInvItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierInvItems];
GO
IF OBJECT_ID(N'[dbo].[JobNotificationRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobNotificationRequests];
GO
IF OBJECT_ID(N'[dbo].[CustFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustFiles];
GO
IF OBJECT_ID(N'[dbo].[SupplierPoHdrs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierPoHdrs];
GO
IF OBJECT_ID(N'[dbo].[SupplierPoDtls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierPoDtls];
GO
IF OBJECT_ID(N'[dbo].[SupplierPoStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierPoStatus];
GO
IF OBJECT_ID(N'[dbo].[SupplierPoItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierPoItems];
GO
IF OBJECT_ID(N'[dbo].[CustFileRefs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustFileRefs];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadLinks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadLinks];
GO
IF OBJECT_ID(N'[dbo].[InvCarRecords]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvCarRecords];
GO
IF OBJECT_ID(N'[dbo].[InvCarRecordTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvCarRecordTypes];
GO
IF OBJECT_ID(N'[dbo].[InvCarGateControls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvCarGateControls];
GO
IF OBJECT_ID(N'[dbo].[JobTrails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobTrails];
GO
IF OBJECT_ID(N'[dbo].[CarViewPages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarViewPages];
GO
IF OBJECT_ID(N'[dbo].[CarRatePackages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarRatePackages];
GO
IF OBJECT_ID(N'[dbo].[CarRateUnitPackages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarRateUnitPackages];
GO
IF OBJECT_ID(N'[dbo].[CarResPackages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarResPackages];
GO
IF OBJECT_ID(N'[dbo].[CarUnitMetas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarUnitMetas];
GO
IF OBJECT_ID(N'[dbo].[CoopMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CoopMembers];
GO
IF OBJECT_ID(N'[dbo].[CoopMemberItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CoopMemberItems];
GO
IF OBJECT_ID(N'[dbo].[PaypalTransactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaypalTransactions];
GO
IF OBJECT_ID(N'[dbo].[PaypalAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaypalAccounts];
GO
IF OBJECT_ID(N'[dbo].[RateGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RateGroups];
GO
IF OBJECT_ID(N'[dbo].[CarRateGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarRateGroups];
GO
IF OBJECT_ID(N'[dbo].[EmailBlasterTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailBlasterTemplates];
GO
IF OBJECT_ID(N'[dbo].[BlasterLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlasterLogs];
GO
IF OBJECT_ID(N'[dbo].[EmailBlasterLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailBlasterLogs];
GO
IF OBJECT_ID(N'[dbo].[JobEntMains]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobEntMains];
GO
IF OBJECT_ID(N'[dbo].[CashExpenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CashExpenses];
GO
IF OBJECT_ID(N'[dbo].[PortalCustomers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PortalCustomers];
GO
IF OBJECT_ID(N'[dbo].[Expenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Expenses];
GO
IF OBJECT_ID(N'[dbo].[JobExpenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobExpenses];
GO
IF OBJECT_ID(N'[dbo].[ExpensesCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpensesCategories];
GO
IF OBJECT_ID(N'[dbo].[PkgDestinations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PkgDestinations];
GO
IF OBJECT_ID(N'[dbo].[JobPosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPosts];
GO
IF OBJECT_ID(N'[dbo].[OnlineReservations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OnlineReservations];
GO
IF OBJECT_ID(N'[dbo].[RsvPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RsvPayments];
GO
IF OBJECT_ID(N'[dbo].[DriverInstructions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DriverInstructions];
GO
IF OBJECT_ID(N'[dbo].[DriverInsJobServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DriverInsJobServices];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadCompanies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadCompanies];
GO
IF OBJECT_ID(N'[dbo].[CustEntAddresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntAddresses];
GO
IF OBJECT_ID(N'[dbo].[CustEntCats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntCats];
GO
IF OBJECT_ID(N'[dbo].[CustEntClauses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntClauses];
GO
IF OBJECT_ID(N'[dbo].[SupplierContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierContacts];
GO
IF OBJECT_ID(N'[dbo].[SupplierItemRates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierItemRates];
GO
IF OBJECT_ID(N'[dbo].[SupplierUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierUnits];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadItems];
GO
IF OBJECT_ID(N'[dbo].[SalesLeadQuotedItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesLeadQuotedItems];
GO
IF OBJECT_ID(N'[dbo].[CustSocialAccs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustSocialAccs];
GO
IF OBJECT_ID(N'[dbo].[AdminEmails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdminEmails];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO
IF OBJECT_ID(N'[dbo].[SupplierContactStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierContactStatus];
GO
IF OBJECT_ID(N'[dbo].[CustEntAssigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntAssigns];
GO
IF OBJECT_ID(N'[dbo].[SupplierActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierActivities];
GO
IF OBJECT_ID(N'[dbo].[SupDocuments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupDocuments];
GO
IF OBJECT_ID(N'[dbo].[SupplierDocuments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierDocuments];
GO
IF OBJECT_ID(N'[dbo].[CustEntActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActivities];
GO
IF OBJECT_ID(N'[dbo].[CustEntDocuments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntDocuments];
GO
IF OBJECT_ID(N'[dbo].[CustNotifs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustNotifs];
GO
IF OBJECT_ID(N'[dbo].[CustNotifActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustNotifActivities];
GO
IF OBJECT_ID(N'[dbo].[CustNotifRecipients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustNotifRecipients];
GO
IF OBJECT_ID(N'[dbo].[CustNotifRecipientLists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustNotifRecipientLists];
GO
IF OBJECT_ID(N'[dbo].[CustEntActStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActStatus];
GO
IF OBJECT_ID(N'[dbo].[CustEntActTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActTypes];
GO
IF OBJECT_ID(N'[dbo].[SupplierActStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierActStatus];
GO
IF OBJECT_ID(N'[dbo].[CustEntActivityTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActivityTypes];
GO
IF OBJECT_ID(N'[dbo].[SupplierActivityTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierActivityTypes];
GO
IF OBJECT_ID(N'[dbo].[JobVehicles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobVehicles];
GO
IF OBJECT_ID(N'[dbo].[Vehicles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vehicles];
GO
IF OBJECT_ID(N'[dbo].[VehicleTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleTypes];
GO
IF OBJECT_ID(N'[dbo].[VehicleModels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleModels];
GO
IF OBJECT_ID(N'[dbo].[VehicleBrands]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleBrands];
GO
IF OBJECT_ID(N'[dbo].[VehicleTransmissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleTransmissions];
GO
IF OBJECT_ID(N'[dbo].[VehicleFuels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleFuels];
GO
IF OBJECT_ID(N'[dbo].[VehicleDrives]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleDrives];
GO
IF OBJECT_ID(N'[dbo].[JobPostSales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPostSales];
GO
IF OBJECT_ID(N'[dbo].[CustEntAccountTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntAccountTypes];
GO
IF OBJECT_ID(N'[dbo].[JobPaymentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPaymentStatus];
GO
IF OBJECT_ID(N'[dbo].[JobMainPaymentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobMainPaymentStatus];
GO
IF OBJECT_ID(N'[dbo].[InvItemCommis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvItemCommis];
GO
IF OBJECT_ID(N'[dbo].[JobPaymentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPaymentTypes];
GO
IF OBJECT_ID(N'[dbo].[JobPostSalesStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobPostSalesStatus];
GO
IF OBJECT_ID(N'[dbo].[SvcGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SvcGroups];
GO
IF OBJECT_ID(N'[dbo].[SvcDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SvcDetails];
GO
IF OBJECT_ID(N'[dbo].[CustEntActPostSales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActPostSales];
GO
IF OBJECT_ID(N'[dbo].[CustEntActPostSaleStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActPostSaleStatus];
GO
IF OBJECT_ID(N'[dbo].[CustEntActActionCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActActionCodes];
GO
IF OBJECT_ID(N'[dbo].[CustEntActActionStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustEntActActionStatus];
GO
IF OBJECT_ID(N'[dbo].[CarDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarDetails];
GO
IF OBJECT_ID(N'[dbo].[CarResTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarResTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'JobMains'
CREATE TABLE [dbo].[JobMains] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobDate] datetime  NOT NULL,
    [CustomerId] int  NOT NULL,
    [Description] nvarchar(180)  NOT NULL,
    [NoOfPax] int  NOT NULL,
    [NoOfDays] int  NOT NULL,
    [JobRemarks] nvarchar(180)  NULL,
    [JobStatusId] int  NOT NULL,
    [StatusRemarks] nvarchar(max)  NULL,
    [BranchId] int  NOT NULL,
    [JobThruId] int  NOT NULL,
    [AgreedAmt] decimal(18,0)  NULL,
    [CustContactEmail] nvarchar(150)  NULL,
    [CustContactNumber] nvarchar(120)  NULL,
    [AssignedTo] nvarchar(80)  NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Contact1] nvarchar(30)  NULL,
    [Contact2] nvarchar(30)  NULL,
    [Contact3] nvarchar(30)  NULL,
    [Email] nvarchar(50)  NULL,
    [Details] nvarchar(250)  NULL,
    [CityId] int  NOT NULL,
    [SupplierTypeId] int  NOT NULL,
    [Status] nvarchar(10)  NULL,
    [Website] nvarchar(80)  NULL,
    [Address] nvarchar(150)  NULL,
    [CountryId] int  NOT NULL,
    [Code] nvarchar(30)  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Email] nvarchar(80)  NULL,
    [Contact1] nvarchar(20)  NULL,
    [Contact2] nvarchar(20)  NULL,
    [Remarks] nvarchar(120)  NULL,
    [Status] nvarchar(3)  NULL
);
GO

-- Creating table 'JobTypes'
CREATE TABLE [dbo].[JobTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [TTicket] int  NULL,
    [TTransport] int  NULL,
    [TTour] int  NULL,
    [THotel] int  NULL,
    [TOthers] int  NULL,
    [Remarks] nvarchar(80)  NULL
);
GO

-- Creating table 'Services'
CREATE TABLE [dbo].[Services] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Description] nvarchar(150)  NULL,
    [Status] nvarchar(5)  NOT NULL
);
GO

-- Creating table 'JobServices'
CREATE TABLE [dbo].[JobServices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [ServicesId] int  NOT NULL,
    [SupplierId] int  NOT NULL,
    [Particulars] nvarchar(80)  NULL,
    [QuotedAmt] decimal(18,0)  NULL,
    [SupplierAmt] decimal(18,0)  NULL,
    [ActualAmt] decimal(18,2)  NULL,
    [Remarks] nvarchar(80)  NULL,
    [SupplierItemId] int  NOT NULL,
    [DtStart] datetime  NULL,
    [DtEnd] datetime  NULL
);
GO

-- Creating table 'JobItineraries'
CREATE TABLE [dbo].[JobItineraries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [DestinationId] int  NOT NULL,
    [ActualRate] decimal(18,0)  NULL,
    [Remarks] nvarchar(80)  NULL,
    [ItiDate] datetime  NULL,
    [SvcId] int  NULL
);
GO

-- Creating table 'Destinations'
CREATE TABLE [dbo].[Destinations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [PubRate] decimal(18,0)  NULL,
    [ConRate] decimal(18,0)  NULL,
    [Remarks] nvarchar(150)  NULL,
    [CityId] int  NOT NULL
);
GO

-- Creating table 'JobPickups'
CREATE TABLE [dbo].[JobPickups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [puDate] datetime  NOT NULL,
    [puTime] nvarchar(5)  NOT NULL,
    [puLocation] nvarchar(80)  NOT NULL,
    [ContactName] nvarchar(80)  NOT NULL,
    [ContactNumber] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Branches'
CREATE TABLE [dbo].[Branches] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(30)  NOT NULL,
    [CityId] int  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [Address] nvarchar(180)  NULL,
    [Landline] nvarchar(20)  NULL,
    [Mobile] nvarchar(20)  NULL
);
GO

-- Creating table 'Cities'
CREATE TABLE [dbo].[Cities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'SupplierTypes'
CREATE TABLE [dbo].[SupplierTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SupplierItems'
CREATE TABLE [dbo].[SupplierItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(30)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [SupplierId] int  NOT NULL,
    [InCharge] nvarchar(30)  NULL,
    [Tel1] nvarchar(max)  NULL,
    [Tel2] nvarchar(max)  NULL,
    [Tel3] nvarchar(max)  NULL,
    [Status] nvarchar(3)  NULL,
    [Interval] int  NULL
);
GO

-- Creating table 'JobServicePickups'
CREATE TABLE [dbo].[JobServicePickups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobServicesId] int  NOT NULL,
    [JsDate] datetime  NOT NULL,
    [JsTime] nvarchar(10)  NULL,
    [JsLocation] nvarchar(80)  NULL,
    [ClientName] nvarchar(80)  NULL,
    [ClientContact] nvarchar(50)  NULL,
    [ProviderName] nvarchar(80)  NULL,
    [ProviderContact] nvarchar(50)  NULL
);
GO

-- Creating table 'JobStatus'
CREATE TABLE [dbo].[JobStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JobThrus'
CREATE TABLE [dbo].[JobThrus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Banks'
CREATE TABLE [dbo].[Banks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BankName] nvarchar(max)  NOT NULL,
    [BankBranch] nvarchar(max)  NOT NULL,
    [AccntName] nvarchar(max)  NOT NULL,
    [AccntNo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JobPayments'
CREATE TABLE [dbo].[JobPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [DtPayment] datetime  NOT NULL,
    [PaymentAmt] decimal(18,2)  NOT NULL,
    [Remarks] nvarchar(max)  NULL,
    [BankId] int  NOT NULL,
    [JobPaymentTypeId] int  NOT NULL
);
GO

-- Creating table 'CarCategories'
CREATE TABLE [dbo].[CarCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Remarks] nvarchar(max)  NULL
);
GO

-- Creating table 'CarUnits'
CREATE TABLE [dbo].[CarUnits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Remarks] nvarchar(max)  NULL,
    [CarCategoryId] int  NOT NULL,
    [SelfDrive] int  NULL,
    [SortOrder] int  NULL
);
GO

-- Creating table 'CarDestinations'
CREATE TABLE [dbo].[CarDestinations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CityId] int  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Kms] int  NOT NULL
);
GO

-- Creating table 'CarRates'
CREATE TABLE [dbo].[CarRates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Daily] decimal(18,0)  NOT NULL,
    [Weekly] decimal(18,0)  NOT NULL,
    [Monthly] decimal(18,0)  NOT NULL,
    [KmFree] int  NOT NULL,
    [KmRate] int  NOT NULL,
    [CarUnitId] int  NOT NULL,
    [OtRate] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'CarReservations'
CREATE TABLE [dbo].[CarReservations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtTrx] datetime  NOT NULL,
    [CarUnitId] int  NOT NULL,
    [DtStart] nvarchar(max)  NOT NULL,
    [LocStart] nvarchar(max)  NULL,
    [DtEnd] nvarchar(max)  NOT NULL,
    [LocEnd] nvarchar(max)  NULL,
    [BaseRate] nvarchar(max)  NOT NULL,
    [Destinations] nvarchar(max)  NOT NULL,
    [UseFor] nvarchar(max)  NOT NULL,
    [RenterName] nvarchar(80)  NOT NULL,
    [RenterCompany] nvarchar(50)  NULL,
    [RenterEmail] nvarchar(50)  NOT NULL,
    [RenterMobile] nvarchar(20)  NOT NULL,
    [RenterAddress] nvarchar(80)  NULL,
    [RenterFbAccnt] nvarchar(50)  NULL,
    [RenterLinkedInAccnt] nvarchar(50)  NULL,
    [EstHrPerDay] int  NULL,
    [EstKmTravel] int  NULL,
    [JobRefNo] int  NULL,
    [SelfDrive] int  NULL,
    [CarResTypeId] int  NOT NULL,
    [NoDays] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'CarImages'
CREATE TABLE [dbo].[CarImages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CarUnitId] int  NOT NULL,
    [ImgUrl] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [SysCode] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JobContacts'
CREATE TABLE [dbo].[JobContacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ShortName] nvarchar(20)  NULL,
    [Company] nvarchar(50)  NULL,
    [Position] nvarchar(50)  NULL,
    [Tel1] nvarchar(50)  NULL,
    [Tel2] nvarchar(50)  NULL,
    [Email] nvarchar(100)  NULL,
    [AddInfo] nvarchar(250)  NULL,
    [Remarks] nvarchar(250)  NULL,
    [ContactType] nvarchar(5)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(30)  NOT NULL,
    [TemplateId] int  NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [Sort] int  NOT NULL,
    [Status] nvarchar(3)  NOT NULL
);
GO

-- Creating table 'ProductPrices'
CREATE TABLE [dbo].[ProductPrices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Uom] nvarchar(20)  NOT NULL,
    [Qty] int  NOT NULL,
    [Rate] decimal(18,0)  NOT NULL,
    [Rate1] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'ProductImages1'
CREATE TABLE [dbo].[ProductImages1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Path] nvarchar(250)  NOT NULL
);
GO

-- Creating table 'ProductConditions'
CREATE TABLE [dbo].[ProductConditions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Description] nvarchar(250)  NOT NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'ProductCategories'
CREATE TABLE [dbo].[ProductCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProductProdCats'
CREATE TABLE [dbo].[ProductProdCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductCategoryId] int  NOT NULL,
    [ProductId] int  NOT NULL
);
GO

-- Creating table 'PreDefinedNotes'
CREATE TABLE [dbo].[PreDefinedNotes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Note] nvarchar(250)  NULL
);
GO

-- Creating table 'JobNotes'
CREATE TABLE [dbo].[JobNotes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [Sort] int  NULL,
    [Note] nvarchar(250)  NULL
);
GO

-- Creating table 'JobChecklists'
CREATE TABLE [dbo].[JobChecklists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dtEntered] datetime  NOT NULL,
    [dtDue] datetime  NOT NULL,
    [dtNotification] datetime  NOT NULL,
    [Description] nvarchar(250)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [RefId] int  NULL,
    [Status] int  NOT NULL
);
GO

-- Creating table 'CustCategories'
CREATE TABLE [dbo].[CustCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'CustCats'
CREATE TABLE [dbo].[CustCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [CustCategoryId] int  NOT NULL
);
GO

-- Creating table 'CustEntMains'
CREATE TABLE [dbo].[CustEntMains] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Address] nvarchar(180)  NULL,
    [Contact1] nvarchar(80)  NULL,
    [Contact2] nvarchar(80)  NULL,
    [iconPath] nvarchar(150)  NULL,
    [Website] nvarchar(180)  NULL,
    [Remarks] nvarchar(80)  NULL,
    [CityId] int  NULL,
    [Status] nvarchar(10)  NULL,
    [AssignedTo] nvarchar(80)  NULL,
    [Mobile] nvarchar(max)  NULL,
    [Code] nvarchar(20)  NULL,
    [Exclusive] nvarchar(10)  NULL,
    [CustEntAccountTypeId] int  NOT NULL
);
GO

-- Creating table 'SalesLeads'
CREATE TABLE [dbo].[SalesLeads] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Details] nvarchar(250)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [CustomerId] int  NOT NULL,
    [CustName] nvarchar(80)  NULL,
    [DtEntered] datetime  NOT NULL,
    [EnteredBy] nvarchar(80)  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [AssignedTo] nvarchar(80)  NULL,
    [CustPhone] nvarchar(20)  NULL,
    [CustEmail] nvarchar(80)  NULL,
    [SalesCode] nvarchar(40)  NULL
);
GO

-- Creating table 'SalesStatusCodes'
CREATE TABLE [dbo].[SalesStatusCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SeqNo] int  NULL,
    [Name] nvarchar(80)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'SalesStatus'
CREATE TABLE [dbo].[SalesStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtStatus] datetime  NOT NULL,
    [SalesStatusCodeId] int  NOT NULL,
    [SalesLeadId] int  NOT NULL
);
GO

-- Creating table 'SalesActCodes'
CREATE TABLE [dbo].[SalesActCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Desc] nvarchar(40)  NOT NULL,
    [SysCode] nvarchar(20)  NULL,
    [iconPath] nvarchar(150)  NULL,
    [DefaultActStatus] int  NULL
);
GO

-- Creating table 'SalesActivities'
CREATE TABLE [dbo].[SalesActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadId] int  NOT NULL,
    [SalesActCodeId] int  NOT NULL,
    [Particulars] nvarchar(250)  NOT NULL,
    [DtActivity] datetime  NOT NULL,
    [SalesActStatusId] int  NOT NULL,
    [DtEntered] datetime  NOT NULL,
    [EnteredBy] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'CustEntities'
CREATE TABLE [dbo].[CustEntities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustEntMainId] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [Position] nvarchar(50)  NULL
);
GO

-- Creating table 'SalesActStatus'
CREATE TABLE [dbo].[SalesActStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'SalesLeadCatCodes'
CREATE TABLE [dbo].[SalesLeadCatCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CatName] nvarchar(80)  NOT NULL,
    [SysCode] nvarchar(20)  NOT NULL,
    [iconPath] nvarchar(150)  NULL
);
GO

-- Creating table 'SalesLeadCategories'
CREATE TABLE [dbo].[SalesLeadCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadCatCodeId] int  NOT NULL,
    [SalesLeadId] int  NOT NULL
);
GO

-- Creating table 'CustSalesCategories'
CREATE TABLE [dbo].[CustSalesCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadCatCodeId] int  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'SrvActionCodes'
CREATE TABLE [dbo].[SrvActionCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CatCode] nvarchar(80)  NOT NULL,
    [SortNo] int  NOT NULL
);
GO

-- Creating table 'SrvActionItems'
CREATE TABLE [dbo].[SrvActionItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(180)  NULL,
    [SortNo] int  NOT NULL,
    [ServicesId] int  NOT NULL,
    [SrvActionCodeId] int  NOT NULL
);
GO

-- Creating table 'JobActions'
CREATE TABLE [dbo].[JobActions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobServicesId] int  NOT NULL,
    [AssignedTo] nvarchar(80)  NULL,
    [PerformedBy] nvarchar(80)  NULL,
    [DtAssigned] datetime  NULL,
    [DtPerformed] datetime  NULL,
    [SrvActionItemId] int  NOT NULL,
    [Remarks] nvarchar(150)  NULL
);
GO

-- Creating table 'InvItems'
CREATE TABLE [dbo].[InvItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemCode] nvarchar(30)  NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [ImgPath] nvarchar(80)  NULL,
    [ContactInfo] nvarchar(50)  NULL,
    [ViewLabel] nvarchar(20)  NULL,
    [OrderNo] int  NULL
);
GO

-- Creating table 'InvItemCats'
CREATE TABLE [dbo].[InvItemCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [ImgPath] nvarchar(150)  NULL,
    [SysCode] nvarchar(20)  NULL
);
GO

-- Creating table 'InvItemCategories'
CREATE TABLE [dbo].[InvItemCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemCatId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'JobServiceItems'
CREATE TABLE [dbo].[JobServiceItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobServicesId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'SupplierInvItems'
CREATE TABLE [dbo].[SupplierInvItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'JobNotificationRequests'
CREATE TABLE [dbo].[JobNotificationRequests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ServiceId] int  NOT NULL,
    [ReqDt] datetime  NOT NULL,
    [RefId] nvarchar(20)  NULL
);
GO

-- Creating table 'CustFiles'
CREATE TABLE [dbo].[CustFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(180)  NOT NULL,
    [Folder] nvarchar(150)  NOT NULL,
    [Path] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(180)  NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'SupplierPoHdrs'
CREATE TABLE [dbo].[SupplierPoHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PoDate] datetime  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [SupplierId] int  NOT NULL,
    [SupplierPoStatusId] int  NOT NULL,
    [RequestBy] nvarchar(max)  NOT NULL,
    [DtRequest] datetime  NOT NULL,
    [ApprovedBy] nvarchar(80)  NULL,
    [DtApproved] datetime  NULL
);
GO

-- Creating table 'SupplierPoDtls'
CREATE TABLE [dbo].[SupplierPoDtls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierPoHdrId] int  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [JobServicesId] int  NOT NULL
);
GO

-- Creating table 'SupplierPoStatus'
CREATE TABLE [dbo].[SupplierPoStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [OrderNo] int  NOT NULL
);
GO

-- Creating table 'SupplierPoItems'
CREATE TABLE [dbo].[SupplierPoItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierPoDtlId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'CustFileRefs'
CREATE TABLE [dbo].[CustFileRefs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RefTable] nvarchar(80)  NOT NULL,
    [RefId] int  NOT NULL,
    [CustFilesId] int  NOT NULL
);
GO

-- Creating table 'SalesLeadLinks'
CREATE TABLE [dbo].[SalesLeadLinks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadId] int  NOT NULL,
    [JobMainId] int  NOT NULL
);
GO

-- Creating table 'InvCarRecords'
CREATE TABLE [dbo].[InvCarRecords] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvCarRecordTypeId] int  NOT NULL,
    [Odometer] int  NOT NULL,
    [dtDone] datetime  NOT NULL,
    [NextOdometer] int  NOT NULL,
    [NextSched] datetime  NOT NULL,
    [Remarks] nvarchar(150)  NULL
);
GO

-- Creating table 'InvCarRecordTypes'
CREATE TABLE [dbo].[InvCarRecordTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [SysCode] nvarchar(50)  NULL,
    [OdoInterval] int  NOT NULL,
    [DaysInterval] int  NOT NULL
);
GO

-- Creating table 'InvCarGateControls'
CREATE TABLE [dbo].[InvCarGateControls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [In_Out_flag] int  NOT NULL,
    [Odometer] nvarchar(max)  NOT NULL,
    [dtControl] datetime  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [Driver] nvarchar(50)  NULL,
    [Inspector] nvarchar(50)  NULL,
    [JobMainId] int  NULL,
    [CustomerId] int  NULL,
    [DriverId] int  NULL
);
GO

-- Creating table 'JobTrails'
CREATE TABLE [dbo].[JobTrails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RefTable] nvarchar(50)  NOT NULL,
    [RefId] nvarchar(max)  NOT NULL,
    [dtTrail] datetime  NOT NULL,
    [user] nvarchar(50)  NOT NULL,
    [Action] nvarchar(80)  NOT NULL,
    [IPAddress] nvarchar(40)  NULL
);
GO

-- Creating table 'CarViewPages'
CREATE TABLE [dbo].[CarViewPages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CarUnitId] int  NOT NULL,
    [Viewname] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'CarRatePackages'
CREATE TABLE [dbo].[CarRatePackages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [DailyMeals] decimal(18,0)  NOT NULL,
    [DailyRoom] decimal(18,0)  NOT NULL,
    [DaysMin] int  NOT NULL,
    [Status] nvarchar(5)  NULL
);
GO

-- Creating table 'CarRateUnitPackages'
CREATE TABLE [dbo].[CarRateUnitPackages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CarRatePackageId] int  NOT NULL,
    [CarUnitId] int  NOT NULL,
    [DailyRate] decimal(18,0)  NOT NULL,
    [FuelLonghaul] decimal(18,0)  NOT NULL,
    [FuelDaily] decimal(18,0)  NOT NULL,
    [DailyAddon] decimal(18,0)  NULL,
    [Status] nvarchar(10)  NULL
);
GO

-- Creating table 'CarResPackages'
CREATE TABLE [dbo].[CarResPackages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CarRateUnitPackageId] int  NOT NULL,
    [CarReservationId] int  NOT NULL,
    [DrvMealByClient] int  NOT NULL,
    [DrvRoomByClient] int  NOT NULL,
    [FuelByClient] int  NOT NULL
);
GO

-- Creating table 'CarUnitMetas'
CREATE TABLE [dbo].[CarUnitMetas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CarUnitId] int  NOT NULL,
    [PageTitle] nvarchar(120)  NOT NULL,
    [MetaDesc] nvarchar(300)  NOT NULL,
    [HomeDesc] nvarchar(300)  NULL
);
GO

-- Creating table 'CoopMembers'
CREATE TABLE [dbo].[CoopMembers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Contact1] nvarchar(20)  NULL,
    [Contact2] nvarchar(20)  NULL,
    [Contact3] nvarchar(20)  NULL,
    [Email] nvarchar(50)  NULL,
    [Details] nvarchar(80)  NULL,
    [Status] nvarchar(10)  NULL
);
GO

-- Creating table 'CoopMemberItems'
CREATE TABLE [dbo].[CoopMemberItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CoopMemberId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'PaypalTransactions'
CREATE TABLE [dbo].[PaypalTransactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TrxId] nvarchar(50)  NOT NULL,
    [JobId] int  NOT NULL,
    [TrxDate] datetime  NOT NULL,
    [DatePosted] datetime  NOT NULL,
    [Status] nvarchar(30)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [Amount] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'PaypalAccounts'
CREATE TABLE [dbo].[PaypalAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SysCode] nvarchar(50)  NOT NULL,
    [Key] nvarchar(100)  NOT NULL,
    [Secret] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'RateGroups'
CREATE TABLE [dbo].[RateGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'CarRateGroups'
CREATE TABLE [dbo].[CarRateGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RateGroupId] int  NOT NULL,
    [CarRatePackageId] int  NOT NULL
);
GO

-- Creating table 'EmailBlasterTemplates'
CREATE TABLE [dbo].[EmailBlasterTemplates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailCategory] nvarchar(50)  NOT NULL,
    [RecipientsCategory] nvarchar(50)  NOT NULL,
    [EmailTitle] nvarchar(150)  NOT NULL,
    [EmailBody] nvarchar(750)  NOT NULL,
    [ContentPicture] nvarchar(250)  NULL,
    [AttachmentLink] nvarchar(200)  NULL,
    [Company] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BlasterLogs'
CREATE TABLE [dbo].[BlasterLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailBlasterLogsId] int  NOT NULL,
    [EmailBlasterTemplateId] int  NOT NULL,
    [ReportId] int  NOT NULL
);
GO

-- Creating table 'EmailBlasterLogs'
CREATE TABLE [dbo].[EmailBlasterLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(120)  NOT NULL,
    [DateTime] datetime  NOT NULL,
    [Status] nvarchar(20)  NOT NULL,
    [CustId] int  NOT NULL
);
GO

-- Creating table 'JobEntMains'
CREATE TABLE [dbo].[JobEntMains] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [CustEntMainId] int  NOT NULL
);
GO

-- Creating table 'CashExpenses'
CREATE TABLE [dbo].[CashExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [DtExpense] datetime  NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [RecievedBy] nvarchar(30)  NULL,
    [ReleasedBy] nvarchar(30)  NULL
);
GO

-- Creating table 'PortalCustomers'
CREATE TABLE [dbo].[PortalCustomers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContactNum] nvarchar(15)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [ExpiryDt] datetime  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'Expenses'
CREATE TABLE [dbo].[Expenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [SeqNo] int  NOT NULL,
    [ExpensesCategoryId] int  NOT NULL
);
GO

-- Creating table 'JobExpenses'
CREATE TABLE [dbo].[JobExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [JobMainId] int  NOT NULL,
    [ExpensesId] int  NOT NULL,
    [JobServicesId] int  NOT NULL,
    [DtExpense] datetime  NULL,
    [IsReleased] bit  NULL,
    [ForRelease] bit  NULL
);
GO

-- Creating table 'ExpensesCategories'
CREATE TABLE [dbo].[ExpensesCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'PkgDestinations'
CREATE TABLE [dbo].[PkgDestinations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [SmProductId] nvarchar(max)  NOT NULL,
    [JobId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JobPosts'
CREATE TABLE [dbo].[JobPosts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPost] datetime  NOT NULL,
    [PaymentAmt] decimal(18,0)  NOT NULL,
    [ExpensesAmt] decimal(18,0)  NOT NULL,
    [CarRentalInc] decimal(18,0)  NOT NULL,
    [TourInc] decimal(18,0)  NOT NULL,
    [OthersInc] decimal(18,0)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [JobMainId] int  NOT NULL
);
GO

-- Creating table 'OnlineReservations'
CREATE TABLE [dbo].[OnlineReservations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPosted] datetime  NOT NULL,
    [ProductCode] nvarchar(15)  NOT NULL,
    [DtStart] datetime  NOT NULL,
    [Name] nvarchar(120)  NOT NULL,
    [ContactNum] nvarchar(15)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Qty] int  NOT NULL,
    [PickupDtls] nvarchar(80)  NOT NULL,
    [PaymentAmt] decimal(18,0)  NULL
);
GO

-- Creating table 'RsvPayments'
CREATE TABLE [dbo].[RsvPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPayment] datetime  NOT NULL,
    [Status] nvarchar(20)  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [PaypaPaymentId] nvarchar(max)  NOT NULL,
    [OnlineReservationId] int  NOT NULL
);
GO

-- Creating table 'DriverInstructions'
CREATE TABLE [dbo].[DriverInstructions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(120)  NOT NULL,
    [OrderNo] int  NOT NULL
);
GO

-- Creating table 'DriverInsJobServices'
CREATE TABLE [dbo].[DriverInsJobServices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DriverInstructionsId] int  NOT NULL,
    [JobServicesId] int  NOT NULL
);
GO

-- Creating table 'SalesLeadCompanies'
CREATE TABLE [dbo].[SalesLeadCompanies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadId] int  NOT NULL,
    [CustEntMainId] int  NOT NULL
);
GO

-- Creating table 'CustEntAddresses'
CREATE TABLE [dbo].[CustEntAddresses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustEntMainId] int  NOT NULL,
    [Line1] nvarchar(80)  NULL,
    [Line2] nvarchar(80)  NULL,
    [Line3] nvarchar(80)  NULL,
    [Line4] nvarchar(80)  NULL,
    [Line5] nvarchar(80)  NULL,
    [isBilling] bit  NOT NULL,
    [isPrimary] bit  NOT NULL
);
GO

-- Creating table 'CustEntCats'
CREATE TABLE [dbo].[CustEntCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustEntMainId] int  NOT NULL,
    [CustCategoryId] int  NOT NULL
);
GO

-- Creating table 'CustEntClauses'
CREATE TABLE [dbo].[CustEntClauses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustEntMainId] int  NOT NULL,
    [Title] nvarchar(50)  NOT NULL,
    [ValidStart] datetime  NOT NULL,
    [ValidEnd] datetime  NOT NULL,
    [Desc1] nvarchar(250)  NULL,
    [Desc2] nvarchar(max)  NULL,
    [Desc3] nvarchar(250)  NULL,
    [DtEncoded] datetime  NOT NULL,
    [EncodedBy] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SupplierContacts'
CREATE TABLE [dbo].[SupplierContacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Mobile] nvarchar(40)  NULL,
    [Landline] nvarchar(40)  NULL,
    [SkypeId] nvarchar(40)  NULL,
    [ViberId] nvarchar(40)  NULL,
    [Remarks] nvarchar(max)  NULL,
    [SupplierId] int  NOT NULL,
    [WhatsApp] nvarchar(80)  NULL,
    [Email] nvarchar(80)  NULL,
    [SupplierContactStatusId] int  NOT NULL,
    [WeChat] nvarchar(40)  NULL,
    [Position] nvarchar(40)  NULL,
    [Department] nvarchar(40)  NULL
);
GO

-- Creating table 'SupplierItemRates'
CREATE TABLE [dbo].[SupplierItemRates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierInvItemId] int  NOT NULL,
    [ItemRate] nvarchar(max)  NOT NULL,
    [SupplierUnitId] int  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [DtValidFrom] nvarchar(max)  NOT NULL,
    [DtValidTo] nvarchar(max)  NOT NULL,
    [Particulars] nvarchar(80)  NOT NULL,
    [By] nvarchar(80)  NOT NULL,
    [Material] nvarchar(40)  NULL,
    [ProcBy] nvarchar(40)  NULL,
    [TradeTerm] nvarchar(80)  NULL,
    [Tolerance] nvarchar(80)  NULL,
    [DtEntered] nvarchar(30)  NULL
);
GO

-- Creating table 'SupplierUnits'
CREATE TABLE [dbo].[SupplierUnits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Unit] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SalesLeadItems'
CREATE TABLE [dbo].[SalesLeadItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QuotedPrice] decimal(18,0)  NULL,
    [Remarks] nvarchar(80)  NULL,
    [SalesLeadId] int  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'SalesLeadQuotedItems'
CREATE TABLE [dbo].[SalesLeadQuotedItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesLeadItemsId] int  NOT NULL,
    [SupplierItemRateId] int  NOT NULL
);
GO

-- Creating table 'CustSocialAccs'
CREATE TABLE [dbo].[CustSocialAccs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Facebook] nvarchar(80)  NOT NULL,
    [Viber] nvarchar(80)  NOT NULL,
    [Skype] nvarchar(80)  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'AdminEmails'
CREATE TABLE [dbo].[AdminEmails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(80)  NOT NULL,
    [AccCode] nvarchar(20)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Code] nvarchar(4)  NOT NULL
);
GO

-- Creating table 'SupplierContactStatus'
CREATE TABLE [dbo].[SupplierContactStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CustEntAssigns'
CREATE TABLE [dbo].[CustEntAssigns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Assigned] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [CustEntMainId] int  NOT NULL
);
GO

-- Creating table 'SupplierActivities'
CREATE TABLE [dbo].[SupplierActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(20)  NULL,
    [DtActivity] datetime  NOT NULL,
    [Assigned] nvarchar(40)  NULL,
    [Remarks] nvarchar(250)  NULL,
    [SupplierId] int  NOT NULL,
    [Amount] decimal(18,0)  NULL,
    [Type] nvarchar(20)  NULL,
    [ActivityType] nvarchar(20)  NOT NULL,
    [SupplierActStatusId] int  NOT NULL
);
GO

-- Creating table 'SupDocuments'
CREATE TABLE [dbo].[SupDocuments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'SupplierDocuments'
CREATE TABLE [dbo].[SupplierDocuments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupplierId] int  NOT NULL,
    [SupDocumentId] int  NOT NULL
);
GO

-- Creating table 'CustEntActivities'
CREATE TABLE [dbo].[CustEntActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Assigned] nvarchar(40)  NOT NULL,
    [ProjectName] nvarchar(80)  NULL,
    [SalesCode] nvarchar(20)  NULL,
    [Amount] decimal(18,0)  NULL,
    [Status] nvarchar(20)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [CustEntMainId] int  NOT NULL,
    [Type] nvarchar(20)  NULL,
    [ActivityType] nvarchar(30)  NULL,
    [SalesLeadId] int  NULL,
    [CustEntActStatusId] int  NOT NULL,
    [CustEntActActionStatusId] int  NOT NULL,
    [CustEntActActionCodesId] int  NOT NULL
);
GO

-- Creating table 'CustEntDocuments'
CREATE TABLE [dbo].[CustEntDocuments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SupDocumentId] int  NOT NULL,
    [CustEntMainId] int  NOT NULL,
    [IsApproved] int  NULL
);
GO

-- Creating table 'CustNotifs'
CREATE TABLE [dbo].[CustNotifs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MsgTitle] nvarchar(80)  NULL,
    [MsgBody] nvarchar(360)  NOT NULL,
    [DtEncoded] datetime  NULL,
    [DtScheduled] datetime  NOT NULL,
    [Occurence] nvarchar(20)  NOT NULL,
    [IsEmail] bit  NOT NULL,
    [IsSms] bit  NOT NULL,
    [Status] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'CustNotifActivities'
CREATE TABLE [dbo].[CustNotifActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtActivity] datetime  NOT NULL,
    [Status] nvarchar(40)  NOT NULL,
    [CustNotifRecipientId] int  NOT NULL
);
GO

-- Creating table 'CustNotifRecipients'
CREATE TABLE [dbo].[CustNotifRecipients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [CustNotifId] int  NOT NULL,
    [NotifRecipientId] int  NOT NULL
);
GO

-- Creating table 'CustNotifRecipientLists'
CREATE TABLE [dbo].[CustNotifRecipientLists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NULL,
    [Mobile] nvarchar(max)  NULL
);
GO

-- Creating table 'CustEntActStatus'
CREATE TABLE [dbo].[CustEntActStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'CustEntActTypes'
CREATE TABLE [dbo].[CustEntActTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SupplierActStatus'
CREATE TABLE [dbo].[SupplierActStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CustEntActivityTypes'
CREATE TABLE [dbo].[CustEntActivityTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(20)  NOT NULL,
    [Points] int  NOT NULL
);
GO

-- Creating table 'SupplierActivityTypes'
CREATE TABLE [dbo].[SupplierActivityTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(20)  NOT NULL,
    [Points] int  NOT NULL
);
GO

-- Creating table 'JobVehicles'
CREATE TABLE [dbo].[JobVehicles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VehicleId] int  NOT NULL,
    [JobMainId] int  NOT NULL,
    [Mileage] int  NOT NULL
);
GO

-- Creating table 'Vehicles'
CREATE TABLE [dbo].[Vehicles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VehicleModelId] int  NOT NULL,
    [YearModel] nvarchar(4)  NOT NULL,
    [PlateNo] nvarchar(20)  NOT NULL,
    [Conduction] nvarchar(10)  NULL,
    [EngineNo] nvarchar(50)  NULL,
    [ChassisNo] nvarchar(50)  NULL,
    [Color] nvarchar(30)  NULL,
    [CustomerId] int  NOT NULL,
    [CustEntMainId] int  NOT NULL,
    [Remarks] nvarchar(250)  NULL
);
GO

-- Creating table 'VehicleTypes'
CREATE TABLE [dbo].[VehicleTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'VehicleModels'
CREATE TABLE [dbo].[VehicleModels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Make] nvarchar(50)  NOT NULL,
    [Variant] nvarchar(30)  NOT NULL,
    [VehicleBrandId] int  NOT NULL,
    [VehicleTypeId] int  NOT NULL,
    [Remarks] nvarchar(50)  NULL,
    [VehicleTransmissionId] int  NOT NULL,
    [VehicleFuelId] int  NOT NULL,
    [VehicleDriveId] int  NOT NULL,
    [MotorOil] nvarchar(10)  NULL,
    [GearOil] nvarchar(10)  NULL,
    [TransmissionOil] nvarchar(10)  NULL
);
GO

-- Creating table 'VehicleBrands'
CREATE TABLE [dbo].[VehicleBrands] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Brand] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'VehicleTransmissions'
CREATE TABLE [dbo].[VehicleTransmissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(5)  NOT NULL
);
GO

-- Creating table 'VehicleFuels'
CREATE TABLE [dbo].[VehicleFuels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fuel] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'VehicleDrives'
CREATE TABLE [dbo].[VehicleDrives] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Drive] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'JobPostSales'
CREATE TABLE [dbo].[JobPostSales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DtPost] datetime  NOT NULL,
    [DoneBy] nvarchar(50)  NOT NULL,
    [Remarks] nvarchar(150)  NULL,
    [JobServicesId] int  NOT NULL,
    [DtDone] datetime  NULL,
    [JobPostSalesStatusId] int  NOT NULL
);
GO

-- Creating table 'CustEntAccountTypes'
CREATE TABLE [dbo].[CustEntAccountTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [SysCode] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'JobPaymentStatus'
CREATE TABLE [dbo].[JobPaymentStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'JobMainPaymentStatus'
CREATE TABLE [dbo].[JobMainPaymentStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobMainId] int  NOT NULL,
    [JobPaymentStatusId] int  NOT NULL
);
GO

-- Creating table 'InvItemCommis'
CREATE TABLE [dbo].[InvItemCommis] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [Unit] nvarchar(30)  NOT NULL,
    [Source] nvarchar(20)  NOT NULL,
    [InvItemId] int  NOT NULL
);
GO

-- Creating table 'JobPaymentTypes'
CREATE TABLE [dbo].[JobPaymentTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'JobPostSalesStatus'
CREATE TABLE [dbo].[JobPostSalesStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SvcGroups'
CREATE TABLE [dbo].[SvcGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ServicesId] int  NOT NULL,
    [SvcDetailId] int  NOT NULL
);
GO

-- Creating table 'SvcDetails'
CREATE TABLE [dbo].[SvcDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL
);
GO

-- Creating table 'CustEntActPostSales'
CREATE TABLE [dbo].[CustEntActPostSales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [By] nvarchar(80)  NOT NULL,
    [DateEncoded] datetime  NOT NULL,
    [Remarks] nvarchar(80)  NULL,
    [SalesCode] nvarchar(40)  NOT NULL,
    [CustEntActPostSaleStatusId] int  NOT NULL
);
GO

-- Creating table 'CustEntActPostSaleStatus'
CREATE TABLE [dbo].[CustEntActPostSaleStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'CustEntActActionCodes'
CREATE TABLE [dbo].[CustEntActActionCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Desc] nvarchar(80)  NOT NULL,
    [SysCode] nvarchar(20)  NOT NULL,
    [IconPath] nvarchar(80)  NOT NULL,
    [DefaultActStatus] int  NOT NULL
);
GO

-- Creating table 'CustEntActActionStatus'
CREATE TABLE [dbo].[CustEntActActionStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ActionStatus] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'CarDetails'
CREATE TABLE [dbo].[CarDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fuel] nvarchar(30)  NOT NULL,
    [Class] nvarchar(20)  NULL,
    [Transmission] nvarchar(40)  NULL,
    [Usage] nvarchar(30)  NULL,
    [Passengers] nvarchar(30)  NULL,
    [Remarks] nvarchar(50)  NULL,
    [CarUnitId] int  NOT NULL
);
GO

-- Creating table 'CarResTypes'
CREATE TABLE [dbo].[CarResTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(20)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'JobMains'
ALTER TABLE [dbo].[JobMains]
ADD CONSTRAINT [PK_JobMains]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobTypes'
ALTER TABLE [dbo].[JobTypes]
ADD CONSTRAINT [PK_JobTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [PK_Services]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobServices'
ALTER TABLE [dbo].[JobServices]
ADD CONSTRAINT [PK_JobServices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobItineraries'
ALTER TABLE [dbo].[JobItineraries]
ADD CONSTRAINT [PK_JobItineraries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Destinations'
ALTER TABLE [dbo].[Destinations]
ADD CONSTRAINT [PK_Destinations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPickups'
ALTER TABLE [dbo].[JobPickups]
ADD CONSTRAINT [PK_JobPickups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [PK_Branches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cities'
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [PK_Cities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierTypes'
ALTER TABLE [dbo].[SupplierTypes]
ADD CONSTRAINT [PK_SupplierTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierItems'
ALTER TABLE [dbo].[SupplierItems]
ADD CONSTRAINT [PK_SupplierItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobServicePickups'
ALTER TABLE [dbo].[JobServicePickups]
ADD CONSTRAINT [PK_JobServicePickups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobStatus'
ALTER TABLE [dbo].[JobStatus]
ADD CONSTRAINT [PK_JobStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobThrus'
ALTER TABLE [dbo].[JobThrus]
ADD CONSTRAINT [PK_JobThrus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Banks'
ALTER TABLE [dbo].[Banks]
ADD CONSTRAINT [PK_Banks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPayments'
ALTER TABLE [dbo].[JobPayments]
ADD CONSTRAINT [PK_JobPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarCategories'
ALTER TABLE [dbo].[CarCategories]
ADD CONSTRAINT [PK_CarCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarUnits'
ALTER TABLE [dbo].[CarUnits]
ADD CONSTRAINT [PK_CarUnits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarDestinations'
ALTER TABLE [dbo].[CarDestinations]
ADD CONSTRAINT [PK_CarDestinations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarRates'
ALTER TABLE [dbo].[CarRates]
ADD CONSTRAINT [PK_CarRates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarReservations'
ALTER TABLE [dbo].[CarReservations]
ADD CONSTRAINT [PK_CarReservations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarImages'
ALTER TABLE [dbo].[CarImages]
ADD CONSTRAINT [PK_CarImages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobContacts'
ALTER TABLE [dbo].[JobContacts]
ADD CONSTRAINT [PK_JobContacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductPrices'
ALTER TABLE [dbo].[ProductPrices]
ADD CONSTRAINT [PK_ProductPrices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductImages1'
ALTER TABLE [dbo].[ProductImages1]
ADD CONSTRAINT [PK_ProductImages1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductConditions'
ALTER TABLE [dbo].[ProductConditions]
ADD CONSTRAINT [PK_ProductConditions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductCategories'
ALTER TABLE [dbo].[ProductCategories]
ADD CONSTRAINT [PK_ProductCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductProdCats'
ALTER TABLE [dbo].[ProductProdCats]
ADD CONSTRAINT [PK_ProductProdCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PreDefinedNotes'
ALTER TABLE [dbo].[PreDefinedNotes]
ADD CONSTRAINT [PK_PreDefinedNotes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobNotes'
ALTER TABLE [dbo].[JobNotes]
ADD CONSTRAINT [PK_JobNotes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobChecklists'
ALTER TABLE [dbo].[JobChecklists]
ADD CONSTRAINT [PK_JobChecklists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustCategories'
ALTER TABLE [dbo].[CustCategories]
ADD CONSTRAINT [PK_CustCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustCats'
ALTER TABLE [dbo].[CustCats]
ADD CONSTRAINT [PK_CustCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntMains'
ALTER TABLE [dbo].[CustEntMains]
ADD CONSTRAINT [PK_CustEntMains]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeads'
ALTER TABLE [dbo].[SalesLeads]
ADD CONSTRAINT [PK_SalesLeads]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesStatusCodes'
ALTER TABLE [dbo].[SalesStatusCodes]
ADD CONSTRAINT [PK_SalesStatusCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesStatus'
ALTER TABLE [dbo].[SalesStatus]
ADD CONSTRAINT [PK_SalesStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesActCodes'
ALTER TABLE [dbo].[SalesActCodes]
ADD CONSTRAINT [PK_SalesActCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [PK_SalesActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntities'
ALTER TABLE [dbo].[CustEntities]
ADD CONSTRAINT [PK_CustEntities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesActStatus'
ALTER TABLE [dbo].[SalesActStatus]
ADD CONSTRAINT [PK_SalesActStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadCatCodes'
ALTER TABLE [dbo].[SalesLeadCatCodes]
ADD CONSTRAINT [PK_SalesLeadCatCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadCategories'
ALTER TABLE [dbo].[SalesLeadCategories]
ADD CONSTRAINT [PK_SalesLeadCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustSalesCategories'
ALTER TABLE [dbo].[CustSalesCategories]
ADD CONSTRAINT [PK_CustSalesCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SrvActionCodes'
ALTER TABLE [dbo].[SrvActionCodes]
ADD CONSTRAINT [PK_SrvActionCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SrvActionItems'
ALTER TABLE [dbo].[SrvActionItems]
ADD CONSTRAINT [PK_SrvActionItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobActions'
ALTER TABLE [dbo].[JobActions]
ADD CONSTRAINT [PK_JobActions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItems'
ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [PK_InvItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemCats'
ALTER TABLE [dbo].[InvItemCats]
ADD CONSTRAINT [PK_InvItemCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemCategories'
ALTER TABLE [dbo].[InvItemCategories]
ADD CONSTRAINT [PK_InvItemCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobServiceItems'
ALTER TABLE [dbo].[JobServiceItems]
ADD CONSTRAINT [PK_JobServiceItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierInvItems'
ALTER TABLE [dbo].[SupplierInvItems]
ADD CONSTRAINT [PK_SupplierInvItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobNotificationRequests'
ALTER TABLE [dbo].[JobNotificationRequests]
ADD CONSTRAINT [PK_JobNotificationRequests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustFiles'
ALTER TABLE [dbo].[CustFiles]
ADD CONSTRAINT [PK_CustFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoHdrs'
ALTER TABLE [dbo].[SupplierPoHdrs]
ADD CONSTRAINT [PK_SupplierPoHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoDtls'
ALTER TABLE [dbo].[SupplierPoDtls]
ADD CONSTRAINT [PK_SupplierPoDtls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoStatus'
ALTER TABLE [dbo].[SupplierPoStatus]
ADD CONSTRAINT [PK_SupplierPoStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierPoItems'
ALTER TABLE [dbo].[SupplierPoItems]
ADD CONSTRAINT [PK_SupplierPoItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustFileRefs'
ALTER TABLE [dbo].[CustFileRefs]
ADD CONSTRAINT [PK_CustFileRefs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadLinks'
ALTER TABLE [dbo].[SalesLeadLinks]
ADD CONSTRAINT [PK_SalesLeadLinks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCarRecords'
ALTER TABLE [dbo].[InvCarRecords]
ADD CONSTRAINT [PK_InvCarRecords]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCarRecordTypes'
ALTER TABLE [dbo].[InvCarRecordTypes]
ADD CONSTRAINT [PK_InvCarRecordTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCarGateControls'
ALTER TABLE [dbo].[InvCarGateControls]
ADD CONSTRAINT [PK_InvCarGateControls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobTrails'
ALTER TABLE [dbo].[JobTrails]
ADD CONSTRAINT [PK_JobTrails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarViewPages'
ALTER TABLE [dbo].[CarViewPages]
ADD CONSTRAINT [PK_CarViewPages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarRatePackages'
ALTER TABLE [dbo].[CarRatePackages]
ADD CONSTRAINT [PK_CarRatePackages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarRateUnitPackages'
ALTER TABLE [dbo].[CarRateUnitPackages]
ADD CONSTRAINT [PK_CarRateUnitPackages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarResPackages'
ALTER TABLE [dbo].[CarResPackages]
ADD CONSTRAINT [PK_CarResPackages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarUnitMetas'
ALTER TABLE [dbo].[CarUnitMetas]
ADD CONSTRAINT [PK_CarUnitMetas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CoopMembers'
ALTER TABLE [dbo].[CoopMembers]
ADD CONSTRAINT [PK_CoopMembers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CoopMemberItems'
ALTER TABLE [dbo].[CoopMemberItems]
ADD CONSTRAINT [PK_CoopMemberItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaypalTransactions'
ALTER TABLE [dbo].[PaypalTransactions]
ADD CONSTRAINT [PK_PaypalTransactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaypalAccounts'
ALTER TABLE [dbo].[PaypalAccounts]
ADD CONSTRAINT [PK_PaypalAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RateGroups'
ALTER TABLE [dbo].[RateGroups]
ADD CONSTRAINT [PK_RateGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarRateGroups'
ALTER TABLE [dbo].[CarRateGroups]
ADD CONSTRAINT [PK_CarRateGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmailBlasterTemplates'
ALTER TABLE [dbo].[EmailBlasterTemplates]
ADD CONSTRAINT [PK_EmailBlasterTemplates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BlasterLogs'
ALTER TABLE [dbo].[BlasterLogs]
ADD CONSTRAINT [PK_BlasterLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmailBlasterLogs'
ALTER TABLE [dbo].[EmailBlasterLogs]
ADD CONSTRAINT [PK_EmailBlasterLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobEntMains'
ALTER TABLE [dbo].[JobEntMains]
ADD CONSTRAINT [PK_JobEntMains]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CashExpenses'
ALTER TABLE [dbo].[CashExpenses]
ADD CONSTRAINT [PK_CashExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PortalCustomers'
ALTER TABLE [dbo].[PortalCustomers]
ADD CONSTRAINT [PK_PortalCustomers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Expenses'
ALTER TABLE [dbo].[Expenses]
ADD CONSTRAINT [PK_Expenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobExpenses'
ALTER TABLE [dbo].[JobExpenses]
ADD CONSTRAINT [PK_JobExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpensesCategories'
ALTER TABLE [dbo].[ExpensesCategories]
ADD CONSTRAINT [PK_ExpensesCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PkgDestinations'
ALTER TABLE [dbo].[PkgDestinations]
ADD CONSTRAINT [PK_PkgDestinations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPosts'
ALTER TABLE [dbo].[JobPosts]
ADD CONSTRAINT [PK_JobPosts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OnlineReservations'
ALTER TABLE [dbo].[OnlineReservations]
ADD CONSTRAINT [PK_OnlineReservations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RsvPayments'
ALTER TABLE [dbo].[RsvPayments]
ADD CONSTRAINT [PK_RsvPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DriverInstructions'
ALTER TABLE [dbo].[DriverInstructions]
ADD CONSTRAINT [PK_DriverInstructions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DriverInsJobServices'
ALTER TABLE [dbo].[DriverInsJobServices]
ADD CONSTRAINT [PK_DriverInsJobServices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadCompanies'
ALTER TABLE [dbo].[SalesLeadCompanies]
ADD CONSTRAINT [PK_SalesLeadCompanies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntAddresses'
ALTER TABLE [dbo].[CustEntAddresses]
ADD CONSTRAINT [PK_CustEntAddresses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntCats'
ALTER TABLE [dbo].[CustEntCats]
ADD CONSTRAINT [PK_CustEntCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntClauses'
ALTER TABLE [dbo].[CustEntClauses]
ADD CONSTRAINT [PK_CustEntClauses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierContacts'
ALTER TABLE [dbo].[SupplierContacts]
ADD CONSTRAINT [PK_SupplierContacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierItemRates'
ALTER TABLE [dbo].[SupplierItemRates]
ADD CONSTRAINT [PK_SupplierItemRates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierUnits'
ALTER TABLE [dbo].[SupplierUnits]
ADD CONSTRAINT [PK_SupplierUnits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadItems'
ALTER TABLE [dbo].[SalesLeadItems]
ADD CONSTRAINT [PK_SalesLeadItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesLeadQuotedItems'
ALTER TABLE [dbo].[SalesLeadQuotedItems]
ADD CONSTRAINT [PK_SalesLeadQuotedItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustSocialAccs'
ALTER TABLE [dbo].[CustSocialAccs]
ADD CONSTRAINT [PK_CustSocialAccs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdminEmails'
ALTER TABLE [dbo].[AdminEmails]
ADD CONSTRAINT [PK_AdminEmails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierContactStatus'
ALTER TABLE [dbo].[SupplierContactStatus]
ADD CONSTRAINT [PK_SupplierContactStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntAssigns'
ALTER TABLE [dbo].[CustEntAssigns]
ADD CONSTRAINT [PK_CustEntAssigns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierActivities'
ALTER TABLE [dbo].[SupplierActivities]
ADD CONSTRAINT [PK_SupplierActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupDocuments'
ALTER TABLE [dbo].[SupDocuments]
ADD CONSTRAINT [PK_SupDocuments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierDocuments'
ALTER TABLE [dbo].[SupplierDocuments]
ADD CONSTRAINT [PK_SupplierDocuments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActivities'
ALTER TABLE [dbo].[CustEntActivities]
ADD CONSTRAINT [PK_CustEntActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntDocuments'
ALTER TABLE [dbo].[CustEntDocuments]
ADD CONSTRAINT [PK_CustEntDocuments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustNotifs'
ALTER TABLE [dbo].[CustNotifs]
ADD CONSTRAINT [PK_CustNotifs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustNotifActivities'
ALTER TABLE [dbo].[CustNotifActivities]
ADD CONSTRAINT [PK_CustNotifActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustNotifRecipients'
ALTER TABLE [dbo].[CustNotifRecipients]
ADD CONSTRAINT [PK_CustNotifRecipients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustNotifRecipientLists'
ALTER TABLE [dbo].[CustNotifRecipientLists]
ADD CONSTRAINT [PK_CustNotifRecipientLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActStatus'
ALTER TABLE [dbo].[CustEntActStatus]
ADD CONSTRAINT [PK_CustEntActStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActTypes'
ALTER TABLE [dbo].[CustEntActTypes]
ADD CONSTRAINT [PK_CustEntActTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierActStatus'
ALTER TABLE [dbo].[SupplierActStatus]
ADD CONSTRAINT [PK_SupplierActStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActivityTypes'
ALTER TABLE [dbo].[CustEntActivityTypes]
ADD CONSTRAINT [PK_CustEntActivityTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierActivityTypes'
ALTER TABLE [dbo].[SupplierActivityTypes]
ADD CONSTRAINT [PK_SupplierActivityTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobVehicles'
ALTER TABLE [dbo].[JobVehicles]
ADD CONSTRAINT [PK_JobVehicles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [PK_Vehicles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VehicleTypes'
ALTER TABLE [dbo].[VehicleTypes]
ADD CONSTRAINT [PK_VehicleTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VehicleModels'
ALTER TABLE [dbo].[VehicleModels]
ADD CONSTRAINT [PK_VehicleModels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VehicleBrands'
ALTER TABLE [dbo].[VehicleBrands]
ADD CONSTRAINT [PK_VehicleBrands]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VehicleTransmissions'
ALTER TABLE [dbo].[VehicleTransmissions]
ADD CONSTRAINT [PK_VehicleTransmissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VehicleFuels'
ALTER TABLE [dbo].[VehicleFuels]
ADD CONSTRAINT [PK_VehicleFuels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VehicleDrives'
ALTER TABLE [dbo].[VehicleDrives]
ADD CONSTRAINT [PK_VehicleDrives]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPostSales'
ALTER TABLE [dbo].[JobPostSales]
ADD CONSTRAINT [PK_JobPostSales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntAccountTypes'
ALTER TABLE [dbo].[CustEntAccountTypes]
ADD CONSTRAINT [PK_CustEntAccountTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPaymentStatus'
ALTER TABLE [dbo].[JobPaymentStatus]
ADD CONSTRAINT [PK_JobPaymentStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobMainPaymentStatus'
ALTER TABLE [dbo].[JobMainPaymentStatus]
ADD CONSTRAINT [PK_JobMainPaymentStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemCommis'
ALTER TABLE [dbo].[InvItemCommis]
ADD CONSTRAINT [PK_InvItemCommis]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPaymentTypes'
ALTER TABLE [dbo].[JobPaymentTypes]
ADD CONSTRAINT [PK_JobPaymentTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobPostSalesStatus'
ALTER TABLE [dbo].[JobPostSalesStatus]
ADD CONSTRAINT [PK_JobPostSalesStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SvcGroups'
ALTER TABLE [dbo].[SvcGroups]
ADD CONSTRAINT [PK_SvcGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SvcDetails'
ALTER TABLE [dbo].[SvcDetails]
ADD CONSTRAINT [PK_SvcDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActPostSales'
ALTER TABLE [dbo].[CustEntActPostSales]
ADD CONSTRAINT [PK_CustEntActPostSales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActPostSaleStatus'
ALTER TABLE [dbo].[CustEntActPostSaleStatus]
ADD CONSTRAINT [PK_CustEntActPostSaleStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActActionCodes'
ALTER TABLE [dbo].[CustEntActActionCodes]
ADD CONSTRAINT [PK_CustEntActActionCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustEntActActionStatus'
ALTER TABLE [dbo].[CustEntActActionStatus]
ADD CONSTRAINT [PK_CustEntActActionStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarDetails'
ALTER TABLE [dbo].[CarDetails]
ADD CONSTRAINT [PK_CarDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarResTypes'
ALTER TABLE [dbo].[CarResTypes]
ADD CONSTRAINT [PK_CarResTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [JobMainId] in table 'JobTypes'
ALTER TABLE [dbo].[JobTypes]
ADD CONSTRAINT [FK_JobMainJobType]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobType'
CREATE INDEX [IX_FK_JobMainJobType]
ON [dbo].[JobTypes]
    ([JobMainId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobServices'
ALTER TABLE [dbo].[JobServices]
ADD CONSTRAINT [FK_JobMainJobSupplier]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobSupplier'
CREATE INDEX [IX_FK_JobMainJobSupplier]
ON [dbo].[JobServices]
    ([JobMainId]);
GO

-- Creating foreign key on [SupplierId] in table 'JobServices'
ALTER TABLE [dbo].[JobServices]
ADD CONSTRAINT [FK_SupplierJobSupplier]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierJobSupplier'
CREATE INDEX [IX_FK_SupplierJobSupplier]
ON [dbo].[JobServices]
    ([SupplierId]);
GO

-- Creating foreign key on [CustomerId] in table 'JobMains'
ALTER TABLE [dbo].[JobMains]
ADD CONSTRAINT [FK_CustomerJobMain]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerJobMain'
CREATE INDEX [IX_FK_CustomerJobMain]
ON [dbo].[JobMains]
    ([CustomerId]);
GO

-- Creating foreign key on [ServicesId] in table 'JobServices'
ALTER TABLE [dbo].[JobServices]
ADD CONSTRAINT [FK_ServicesJobServices]
    FOREIGN KEY ([ServicesId])
    REFERENCES [dbo].[Services]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServicesJobServices'
CREATE INDEX [IX_FK_ServicesJobServices]
ON [dbo].[JobServices]
    ([ServicesId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobItineraries'
ALTER TABLE [dbo].[JobItineraries]
ADD CONSTRAINT [FK_JobMainJobItinerary]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobItinerary'
CREATE INDEX [IX_FK_JobMainJobItinerary]
ON [dbo].[JobItineraries]
    ([JobMainId]);
GO

-- Creating foreign key on [DestinationId] in table 'JobItineraries'
ALTER TABLE [dbo].[JobItineraries]
ADD CONSTRAINT [FK_DestinationJobItinerary]
    FOREIGN KEY ([DestinationId])
    REFERENCES [dbo].[Destinations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DestinationJobItinerary'
CREATE INDEX [IX_FK_DestinationJobItinerary]
ON [dbo].[JobItineraries]
    ([DestinationId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobPickups'
ALTER TABLE [dbo].[JobPickups]
ADD CONSTRAINT [FK_JobMainJobPickup]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobPickup'
CREATE INDEX [IX_FK_JobMainJobPickup]
ON [dbo].[JobPickups]
    ([JobMainId]);
GO

-- Creating foreign key on [CityId] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [FK_CityBranch]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[Cities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityBranch'
CREATE INDEX [IX_FK_CityBranch]
ON [dbo].[Branches]
    ([CityId]);
GO

-- Creating foreign key on [CityId] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK_CitySupplier]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[Cities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CitySupplier'
CREATE INDEX [IX_FK_CitySupplier]
ON [dbo].[Suppliers]
    ([CityId]);
GO

-- Creating foreign key on [BranchId] in table 'JobMains'
ALTER TABLE [dbo].[JobMains]
ADD CONSTRAINT [FK_BranchJobMain]
    FOREIGN KEY ([BranchId])
    REFERENCES [dbo].[Branches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BranchJobMain'
CREATE INDEX [IX_FK_BranchJobMain]
ON [dbo].[JobMains]
    ([BranchId]);
GO

-- Creating foreign key on [CityId] in table 'Destinations'
ALTER TABLE [dbo].[Destinations]
ADD CONSTRAINT [FK_CityDestination]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[Cities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityDestination'
CREATE INDEX [IX_FK_CityDestination]
ON [dbo].[Destinations]
    ([CityId]);
GO

-- Creating foreign key on [SupplierTypeId] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK_SupplierTypeSupplier]
    FOREIGN KEY ([SupplierTypeId])
    REFERENCES [dbo].[SupplierTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierTypeSupplier'
CREATE INDEX [IX_FK_SupplierTypeSupplier]
ON [dbo].[Suppliers]
    ([SupplierTypeId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierItems'
ALTER TABLE [dbo].[SupplierItems]
ADD CONSTRAINT [FK_SupplierSupplierItem]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierItem'
CREATE INDEX [IX_FK_SupplierSupplierItem]
ON [dbo].[SupplierItems]
    ([SupplierId]);
GO

-- Creating foreign key on [SupplierItemId] in table 'JobServices'
ALTER TABLE [dbo].[JobServices]
ADD CONSTRAINT [FK_SupplierItemJobServices]
    FOREIGN KEY ([SupplierItemId])
    REFERENCES [dbo].[SupplierItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierItemJobServices'
CREATE INDEX [IX_FK_SupplierItemJobServices]
ON [dbo].[JobServices]
    ([SupplierItemId]);
GO

-- Creating foreign key on [JobServicesId] in table 'JobServicePickups'
ALTER TABLE [dbo].[JobServicePickups]
ADD CONSTRAINT [FK_JobServicesJobServicePickup]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesJobServicePickup'
CREATE INDEX [IX_FK_JobServicesJobServicePickup]
ON [dbo].[JobServicePickups]
    ([JobServicesId]);
GO

-- Creating foreign key on [JobStatusId] in table 'JobMains'
ALTER TABLE [dbo].[JobMains]
ADD CONSTRAINT [FK_JobStatusJobMain]
    FOREIGN KEY ([JobStatusId])
    REFERENCES [dbo].[JobStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobStatusJobMain'
CREATE INDEX [IX_FK_JobStatusJobMain]
ON [dbo].[JobMains]
    ([JobStatusId]);
GO

-- Creating foreign key on [JobThruId] in table 'JobMains'
ALTER TABLE [dbo].[JobMains]
ADD CONSTRAINT [FK_JobThruJobMain]
    FOREIGN KEY ([JobThruId])
    REFERENCES [dbo].[JobThrus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobThruJobMain'
CREATE INDEX [IX_FK_JobThruJobMain]
ON [dbo].[JobMains]
    ([JobThruId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobPayments'
ALTER TABLE [dbo].[JobPayments]
ADD CONSTRAINT [FK_JobMainJobPayment]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobPayment'
CREATE INDEX [IX_FK_JobMainJobPayment]
ON [dbo].[JobPayments]
    ([JobMainId]);
GO

-- Creating foreign key on [BankId] in table 'JobPayments'
ALTER TABLE [dbo].[JobPayments]
ADD CONSTRAINT [FK_BankJobPayment]
    FOREIGN KEY ([BankId])
    REFERENCES [dbo].[Banks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BankJobPayment'
CREATE INDEX [IX_FK_BankJobPayment]
ON [dbo].[JobPayments]
    ([BankId]);
GO

-- Creating foreign key on [CarCategoryId] in table 'CarUnits'
ALTER TABLE [dbo].[CarUnits]
ADD CONSTRAINT [FK_CarCategoryCarUnit]
    FOREIGN KEY ([CarCategoryId])
    REFERENCES [dbo].[CarCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarCategoryCarUnit'
CREATE INDEX [IX_FK_CarCategoryCarUnit]
ON [dbo].[CarUnits]
    ([CarCategoryId]);
GO

-- Creating foreign key on [CityId] in table 'CarDestinations'
ALTER TABLE [dbo].[CarDestinations]
ADD CONSTRAINT [FK_CityCarDestination]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[Cities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityCarDestination'
CREATE INDEX [IX_FK_CityCarDestination]
ON [dbo].[CarDestinations]
    ([CityId]);
GO

-- Creating foreign key on [CarUnitId] in table 'CarRates'
ALTER TABLE [dbo].[CarRates]
ADD CONSTRAINT [FK_CarUnitCarRate]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarRate'
CREATE INDEX [IX_FK_CarUnitCarRate]
ON [dbo].[CarRates]
    ([CarUnitId]);
GO

-- Creating foreign key on [CarUnitId] in table 'CarReservations'
ALTER TABLE [dbo].[CarReservations]
ADD CONSTRAINT [FK_CarUnitCarReservation]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarReservation'
CREATE INDEX [IX_FK_CarUnitCarReservation]
ON [dbo].[CarReservations]
    ([CarUnitId]);
GO

-- Creating foreign key on [CarUnitId] in table 'CarImages'
ALTER TABLE [dbo].[CarImages]
ADD CONSTRAINT [FK_CarUnitCarImage]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarImage'
CREATE INDEX [IX_FK_CarUnitCarImage]
ON [dbo].[CarImages]
    ([CarUnitId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductPrices'
ALTER TABLE [dbo].[ProductPrices]
ADD CONSTRAINT [FK_ProductProductPrice]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductPrice'
CREATE INDEX [IX_FK_ProductProductPrice]
ON [dbo].[ProductPrices]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductImages1'
ALTER TABLE [dbo].[ProductImages1]
ADD CONSTRAINT [FK_ProductProductImages]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductImages'
CREATE INDEX [IX_FK_ProductProductImages]
ON [dbo].[ProductImages1]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductConditions'
ALTER TABLE [dbo].[ProductConditions]
ADD CONSTRAINT [FK_ProductProductCondition]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductCondition'
CREATE INDEX [IX_FK_ProductProductCondition]
ON [dbo].[ProductConditions]
    ([ProductId]);
GO

-- Creating foreign key on [ProductCategoryId] in table 'ProductProdCats'
ALTER TABLE [dbo].[ProductProdCats]
ADD CONSTRAINT [FK_ProductCategoryProductProdCat]
    FOREIGN KEY ([ProductCategoryId])
    REFERENCES [dbo].[ProductCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCategoryProductProdCat'
CREATE INDEX [IX_FK_ProductCategoryProductProdCat]
ON [dbo].[ProductProdCats]
    ([ProductCategoryId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductProdCats'
ALTER TABLE [dbo].[ProductProdCats]
ADD CONSTRAINT [FK_ProductProductProdCat]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductProdCat'
CREATE INDEX [IX_FK_ProductProductProdCat]
ON [dbo].[ProductProdCats]
    ([ProductId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobNotes'
ALTER TABLE [dbo].[JobNotes]
ADD CONSTRAINT [FK_JobMainJobNote]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobNote'
CREATE INDEX [IX_FK_JobMainJobNote]
ON [dbo].[JobNotes]
    ([JobMainId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustCats'
ALTER TABLE [dbo].[CustCats]
ADD CONSTRAINT [FK_CustomerCustCat]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustCat'
CREATE INDEX [IX_FK_CustomerCustCat]
ON [dbo].[CustCats]
    ([CustomerId]);
GO

-- Creating foreign key on [CustCategoryId] in table 'CustCats'
ALTER TABLE [dbo].[CustCats]
ADD CONSTRAINT [FK_CustCategoryCustCat]
    FOREIGN KEY ([CustCategoryId])
    REFERENCES [dbo].[CustCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustCategoryCustCat'
CREATE INDEX [IX_FK_CustCategoryCustCat]
ON [dbo].[CustCats]
    ([CustCategoryId]);
GO

-- Creating foreign key on [CustomerId] in table 'SalesLeads'
ALTER TABLE [dbo].[SalesLeads]
ADD CONSTRAINT [FK_CustomerSalesLead]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerSalesLead'
CREATE INDEX [IX_FK_CustomerSalesLead]
ON [dbo].[SalesLeads]
    ([CustomerId]);
GO

-- Creating foreign key on [SalesActCodeId] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [FK_SalesActCodeSalesActivity]
    FOREIGN KEY ([SalesActCodeId])
    REFERENCES [dbo].[SalesActCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesActCodeSalesActivity'
CREATE INDEX [IX_FK_SalesActCodeSalesActivity]
ON [dbo].[SalesActivities]
    ([SalesActCodeId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [FK_SalesLeadSalesActivity]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesActivity'
CREATE INDEX [IX_FK_SalesLeadSalesActivity]
ON [dbo].[SalesActivities]
    ([SalesLeadId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntities'
ALTER TABLE [dbo].[CustEntities]
ADD CONSTRAINT [FK_CustEntMainCustEntity]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntity'
CREATE INDEX [IX_FK_CustEntMainCustEntity]
ON [dbo].[CustEntities]
    ([CustEntMainId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustEntities'
ALTER TABLE [dbo].[CustEntities]
ADD CONSTRAINT [FK_CustomerCustEntity]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustEntity'
CREATE INDEX [IX_FK_CustomerCustEntity]
ON [dbo].[CustEntities]
    ([CustomerId]);
GO

-- Creating foreign key on [SalesActStatusId] in table 'SalesActivities'
ALTER TABLE [dbo].[SalesActivities]
ADD CONSTRAINT [FK_SalesActStatusSalesActivity]
    FOREIGN KEY ([SalesActStatusId])
    REFERENCES [dbo].[SalesActStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesActStatusSalesActivity'
CREATE INDEX [IX_FK_SalesActStatusSalesActivity]
ON [dbo].[SalesActivities]
    ([SalesActStatusId]);
GO

-- Creating foreign key on [SalesLeadCatCodeId] in table 'SalesLeadCategories'
ALTER TABLE [dbo].[SalesLeadCategories]
ADD CONSTRAINT [FK_SalesLeadCatCodeSalesLeadCategory]
    FOREIGN KEY ([SalesLeadCatCodeId])
    REFERENCES [dbo].[SalesLeadCatCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadCatCodeSalesLeadCategory'
CREATE INDEX [IX_FK_SalesLeadCatCodeSalesLeadCategory]
ON [dbo].[SalesLeadCategories]
    ([SalesLeadCatCodeId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesLeadCategories'
ALTER TABLE [dbo].[SalesLeadCategories]
ADD CONSTRAINT [FK_SalesLeadSalesLeadCategory]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesLeadCategory'
CREATE INDEX [IX_FK_SalesLeadSalesLeadCategory]
ON [dbo].[SalesLeadCategories]
    ([SalesLeadId]);
GO

-- Creating foreign key on [SalesLeadCatCodeId] in table 'CustSalesCategories'
ALTER TABLE [dbo].[CustSalesCategories]
ADD CONSTRAINT [FK_SalesLeadCatCodeCustSalesCategory]
    FOREIGN KEY ([SalesLeadCatCodeId])
    REFERENCES [dbo].[SalesLeadCatCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadCatCodeCustSalesCategory'
CREATE INDEX [IX_FK_SalesLeadCatCodeCustSalesCategory]
ON [dbo].[CustSalesCategories]
    ([SalesLeadCatCodeId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustSalesCategories'
ALTER TABLE [dbo].[CustSalesCategories]
ADD CONSTRAINT [FK_CustomerCustSalesCategory]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustSalesCategory'
CREATE INDEX [IX_FK_CustomerCustSalesCategory]
ON [dbo].[CustSalesCategories]
    ([CustomerId]);
GO

-- Creating foreign key on [SalesStatusCodeId] in table 'SalesStatus'
ALTER TABLE [dbo].[SalesStatus]
ADD CONSTRAINT [FK_SalesStatusCodeSalesStatus]
    FOREIGN KEY ([SalesStatusCodeId])
    REFERENCES [dbo].[SalesStatusCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesStatusCodeSalesStatus'
CREATE INDEX [IX_FK_SalesStatusCodeSalesStatus]
ON [dbo].[SalesStatus]
    ([SalesStatusCodeId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesStatus'
ALTER TABLE [dbo].[SalesStatus]
ADD CONSTRAINT [FK_SalesLeadSalesStatus]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesStatus'
CREATE INDEX [IX_FK_SalesLeadSalesStatus]
ON [dbo].[SalesStatus]
    ([SalesLeadId]);
GO

-- Creating foreign key on [JobServicesId] in table 'JobActions'
ALTER TABLE [dbo].[JobActions]
ADD CONSTRAINT [FK_JobServicesJobAction]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesJobAction'
CREATE INDEX [IX_FK_JobServicesJobAction]
ON [dbo].[JobActions]
    ([JobServicesId]);
GO

-- Creating foreign key on [InvItemCatId] in table 'InvItemCategories'
ALTER TABLE [dbo].[InvItemCategories]
ADD CONSTRAINT [FK_InvItemCatInvItemCategory]
    FOREIGN KEY ([InvItemCatId])
    REFERENCES [dbo].[InvItemCats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemCatInvItemCategory'
CREATE INDEX [IX_FK_InvItemCatInvItemCategory]
ON [dbo].[InvItemCategories]
    ([InvItemCatId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvItemCategories'
ALTER TABLE [dbo].[InvItemCategories]
ADD CONSTRAINT [FK_InvItemInvItemCategory]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemCategory'
CREATE INDEX [IX_FK_InvItemInvItemCategory]
ON [dbo].[InvItemCategories]
    ([InvItemId]);
GO

-- Creating foreign key on [JobServicesId] in table 'JobServiceItems'
ALTER TABLE [dbo].[JobServiceItems]
ADD CONSTRAINT [FK_JobServicesJobServiceItem]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesJobServiceItem'
CREATE INDEX [IX_FK_JobServicesJobServiceItem]
ON [dbo].[JobServiceItems]
    ([JobServicesId]);
GO

-- Creating foreign key on [InvItemId] in table 'JobServiceItems'
ALTER TABLE [dbo].[JobServiceItems]
ADD CONSTRAINT [FK_InvItemJobServiceItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemJobServiceItem'
CREATE INDEX [IX_FK_InvItemJobServiceItem]
ON [dbo].[JobServiceItems]
    ([InvItemId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierInvItems'
ALTER TABLE [dbo].[SupplierInvItems]
ADD CONSTRAINT [FK_SupplierSupplierInvItem]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierInvItem'
CREATE INDEX [IX_FK_SupplierSupplierInvItem]
ON [dbo].[SupplierInvItems]
    ([SupplierId]);
GO

-- Creating foreign key on [InvItemId] in table 'SupplierInvItems'
ALTER TABLE [dbo].[SupplierInvItems]
ADD CONSTRAINT [FK_InvItemSupplierInvItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemSupplierInvItem'
CREATE INDEX [IX_FK_InvItemSupplierInvItem]
ON [dbo].[SupplierInvItems]
    ([InvItemId]);
GO

-- Creating foreign key on [ServicesId] in table 'SrvActionItems'
ALTER TABLE [dbo].[SrvActionItems]
ADD CONSTRAINT [FK_ServicesSrvActionItem]
    FOREIGN KEY ([ServicesId])
    REFERENCES [dbo].[Services]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServicesSrvActionItem'
CREATE INDEX [IX_FK_ServicesSrvActionItem]
ON [dbo].[SrvActionItems]
    ([ServicesId]);
GO

-- Creating foreign key on [SrvActionItemId] in table 'JobActions'
ALTER TABLE [dbo].[JobActions]
ADD CONSTRAINT [FK_SrvActionItemJobAction]
    FOREIGN KEY ([SrvActionItemId])
    REFERENCES [dbo].[SrvActionItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SrvActionItemJobAction'
CREATE INDEX [IX_FK_SrvActionItemJobAction]
ON [dbo].[JobActions]
    ([SrvActionItemId]);
GO

-- Creating foreign key on [SrvActionCodeId] in table 'SrvActionItems'
ALTER TABLE [dbo].[SrvActionItems]
ADD CONSTRAINT [FK_SrvActionCodeSrvActionItem]
    FOREIGN KEY ([SrvActionCodeId])
    REFERENCES [dbo].[SrvActionCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SrvActionCodeSrvActionItem'
CREATE INDEX [IX_FK_SrvActionCodeSrvActionItem]
ON [dbo].[SrvActionItems]
    ([SrvActionCodeId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustFiles'
ALTER TABLE [dbo].[CustFiles]
ADD CONSTRAINT [FK_CustomerCustFiles]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustFiles'
CREATE INDEX [IX_FK_CustomerCustFiles]
ON [dbo].[CustFiles]
    ([CustomerId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierPoHdrs'
ALTER TABLE [dbo].[SupplierPoHdrs]
ADD CONSTRAINT [FK_SupplierSupplierPoHdr]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierPoHdr'
CREATE INDEX [IX_FK_SupplierSupplierPoHdr]
ON [dbo].[SupplierPoHdrs]
    ([SupplierId]);
GO

-- Creating foreign key on [SupplierPoStatusId] in table 'SupplierPoHdrs'
ALTER TABLE [dbo].[SupplierPoHdrs]
ADD CONSTRAINT [FK_SupplierPoStatusSupplierPoHdr]
    FOREIGN KEY ([SupplierPoStatusId])
    REFERENCES [dbo].[SupplierPoStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierPoStatusSupplierPoHdr'
CREATE INDEX [IX_FK_SupplierPoStatusSupplierPoHdr]
ON [dbo].[SupplierPoHdrs]
    ([SupplierPoStatusId]);
GO

-- Creating foreign key on [SupplierPoHdrId] in table 'SupplierPoDtls'
ALTER TABLE [dbo].[SupplierPoDtls]
ADD CONSTRAINT [FK_SupplierPoHdrSupplierPoDtl]
    FOREIGN KEY ([SupplierPoHdrId])
    REFERENCES [dbo].[SupplierPoHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierPoHdrSupplierPoDtl'
CREATE INDEX [IX_FK_SupplierPoHdrSupplierPoDtl]
ON [dbo].[SupplierPoDtls]
    ([SupplierPoHdrId]);
GO

-- Creating foreign key on [JobServicesId] in table 'SupplierPoDtls'
ALTER TABLE [dbo].[SupplierPoDtls]
ADD CONSTRAINT [FK_JobServicesSupplierPoDtl]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesSupplierPoDtl'
CREATE INDEX [IX_FK_JobServicesSupplierPoDtl]
ON [dbo].[SupplierPoDtls]
    ([JobServicesId]);
GO

-- Creating foreign key on [SupplierPoDtlId] in table 'SupplierPoItems'
ALTER TABLE [dbo].[SupplierPoItems]
ADD CONSTRAINT [FK_SupplierPoDtlSupplierPoItem]
    FOREIGN KEY ([SupplierPoDtlId])
    REFERENCES [dbo].[SupplierPoDtls]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierPoDtlSupplierPoItem'
CREATE INDEX [IX_FK_SupplierPoDtlSupplierPoItem]
ON [dbo].[SupplierPoItems]
    ([SupplierPoDtlId]);
GO

-- Creating foreign key on [InvItemId] in table 'SupplierPoItems'
ALTER TABLE [dbo].[SupplierPoItems]
ADD CONSTRAINT [FK_InvItemSupplierPoItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemSupplierPoItem'
CREATE INDEX [IX_FK_InvItemSupplierPoItem]
ON [dbo].[SupplierPoItems]
    ([InvItemId]);
GO

-- Creating foreign key on [CustFilesId] in table 'CustFileRefs'
ALTER TABLE [dbo].[CustFileRefs]
ADD CONSTRAINT [FK_CustFilesCustFileRef]
    FOREIGN KEY ([CustFilesId])
    REFERENCES [dbo].[CustFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustFilesCustFileRef'
CREATE INDEX [IX_FK_CustFilesCustFileRef]
ON [dbo].[CustFileRefs]
    ([CustFilesId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesLeadLinks'
ALTER TABLE [dbo].[SalesLeadLinks]
ADD CONSTRAINT [FK_SalesLeadSalesLeadLink]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesLeadLink'
CREATE INDEX [IX_FK_SalesLeadSalesLeadLink]
ON [dbo].[SalesLeadLinks]
    ([SalesLeadId]);
GO

-- Creating foreign key on [JobMainId] in table 'SalesLeadLinks'
ALTER TABLE [dbo].[SalesLeadLinks]
ADD CONSTRAINT [FK_JobMainSalesLeadLink]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainSalesLeadLink'
CREATE INDEX [IX_FK_JobMainSalesLeadLink]
ON [dbo].[SalesLeadLinks]
    ([JobMainId]);
GO

-- Creating foreign key on [InvCarRecordTypeId] in table 'InvCarRecords'
ALTER TABLE [dbo].[InvCarRecords]
ADD CONSTRAINT [FK_InvCarRecordTypeInvCarRecord]
    FOREIGN KEY ([InvCarRecordTypeId])
    REFERENCES [dbo].[InvCarRecordTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCarRecordTypeInvCarRecord'
CREATE INDEX [IX_FK_InvCarRecordTypeInvCarRecord]
ON [dbo].[InvCarRecords]
    ([InvCarRecordTypeId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvCarRecords'
ALTER TABLE [dbo].[InvCarRecords]
ADD CONSTRAINT [FK_InvItemInvCarRecord]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvCarRecord'
CREATE INDEX [IX_FK_InvItemInvCarRecord]
ON [dbo].[InvCarRecords]
    ([InvItemId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvCarGateControls'
ALTER TABLE [dbo].[InvCarGateControls]
ADD CONSTRAINT [FK_InvItemInvCarGateControl]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvCarGateControl'
CREATE INDEX [IX_FK_InvItemInvCarGateControl]
ON [dbo].[InvCarGateControls]
    ([InvItemId]);
GO

-- Creating foreign key on [CarUnitId] in table 'CarViewPages'
ALTER TABLE [dbo].[CarViewPages]
ADD CONSTRAINT [FK_CarUnitCarViewPage]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarViewPage'
CREATE INDEX [IX_FK_CarUnitCarViewPage]
ON [dbo].[CarViewPages]
    ([CarUnitId]);
GO

-- Creating foreign key on [CarRatePackageId] in table 'CarRateUnitPackages'
ALTER TABLE [dbo].[CarRateUnitPackages]
ADD CONSTRAINT [FK_CarRatePackageCarRateUnitPackage]
    FOREIGN KEY ([CarRatePackageId])
    REFERENCES [dbo].[CarRatePackages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarRatePackageCarRateUnitPackage'
CREATE INDEX [IX_FK_CarRatePackageCarRateUnitPackage]
ON [dbo].[CarRateUnitPackages]
    ([CarRatePackageId]);
GO

-- Creating foreign key on [CarUnitId] in table 'CarRateUnitPackages'
ALTER TABLE [dbo].[CarRateUnitPackages]
ADD CONSTRAINT [FK_CarUnitCarRateUnitPackage]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarRateUnitPackage'
CREATE INDEX [IX_FK_CarUnitCarRateUnitPackage]
ON [dbo].[CarRateUnitPackages]
    ([CarUnitId]);
GO

-- Creating foreign key on [CarRateUnitPackageId] in table 'CarResPackages'
ALTER TABLE [dbo].[CarResPackages]
ADD CONSTRAINT [FK_CarRateUnitPackageCarResPackage]
    FOREIGN KEY ([CarRateUnitPackageId])
    REFERENCES [dbo].[CarRateUnitPackages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarRateUnitPackageCarResPackage'
CREATE INDEX [IX_FK_CarRateUnitPackageCarResPackage]
ON [dbo].[CarResPackages]
    ([CarRateUnitPackageId]);
GO

-- Creating foreign key on [CarReservationId] in table 'CarResPackages'
ALTER TABLE [dbo].[CarResPackages]
ADD CONSTRAINT [FK_CarReservationCarResPackage]
    FOREIGN KEY ([CarReservationId])
    REFERENCES [dbo].[CarReservations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarReservationCarResPackage'
CREATE INDEX [IX_FK_CarReservationCarResPackage]
ON [dbo].[CarResPackages]
    ([CarReservationId]);
GO

-- Creating foreign key on [CarUnitId] in table 'CarUnitMetas'
ALTER TABLE [dbo].[CarUnitMetas]
ADD CONSTRAINT [FK_CarUnitCarUnitMeta]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarUnitCarUnitMeta'
CREATE INDEX [IX_FK_CarUnitCarUnitMeta]
ON [dbo].[CarUnitMetas]
    ([CarUnitId]);
GO

-- Creating foreign key on [CoopMemberId] in table 'CoopMemberItems'
ALTER TABLE [dbo].[CoopMemberItems]
ADD CONSTRAINT [FK_CoopMemberCoopMemberItem]
    FOREIGN KEY ([CoopMemberId])
    REFERENCES [dbo].[CoopMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CoopMemberCoopMemberItem'
CREATE INDEX [IX_FK_CoopMemberCoopMemberItem]
ON [dbo].[CoopMemberItems]
    ([CoopMemberId]);
GO

-- Creating foreign key on [InvItemId] in table 'CoopMemberItems'
ALTER TABLE [dbo].[CoopMemberItems]
ADD CONSTRAINT [FK_InvItemCoopMemberItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemCoopMemberItem'
CREATE INDEX [IX_FK_InvItemCoopMemberItem]
ON [dbo].[CoopMemberItems]
    ([InvItemId]);
GO

-- Creating foreign key on [RateGroupId] in table 'CarRateGroups'
ALTER TABLE [dbo].[CarRateGroups]
ADD CONSTRAINT [FK_RateGroupCarRateGroup]
    FOREIGN KEY ([RateGroupId])
    REFERENCES [dbo].[RateGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RateGroupCarRateGroup'
CREATE INDEX [IX_FK_RateGroupCarRateGroup]
ON [dbo].[CarRateGroups]
    ([RateGroupId]);
GO

-- Creating foreign key on [CarRatePackageId] in table 'CarRateGroups'
ALTER TABLE [dbo].[CarRateGroups]
ADD CONSTRAINT [FK_CarRatePackageCarRateGroup]
    FOREIGN KEY ([CarRatePackageId])
    REFERENCES [dbo].[CarRatePackages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarRatePackageCarRateGroup'
CREATE INDEX [IX_FK_CarRatePackageCarRateGroup]
ON [dbo].[CarRateGroups]
    ([CarRatePackageId]);
GO

-- Creating foreign key on [EmailBlasterLogsId] in table 'BlasterLogs'
ALTER TABLE [dbo].[BlasterLogs]
ADD CONSTRAINT [FK_EmailBlasterLogsBlasterLog]
    FOREIGN KEY ([EmailBlasterLogsId])
    REFERENCES [dbo].[EmailBlasterLogs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmailBlasterLogsBlasterLog'
CREATE INDEX [IX_FK_EmailBlasterLogsBlasterLog]
ON [dbo].[BlasterLogs]
    ([EmailBlasterLogsId]);
GO

-- Creating foreign key on [EmailBlasterTemplateId] in table 'BlasterLogs'
ALTER TABLE [dbo].[BlasterLogs]
ADD CONSTRAINT [FK_EmailBlasterTemplateBlasterLog]
    FOREIGN KEY ([EmailBlasterTemplateId])
    REFERENCES [dbo].[EmailBlasterTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmailBlasterTemplateBlasterLog'
CREATE INDEX [IX_FK_EmailBlasterTemplateBlasterLog]
ON [dbo].[BlasterLogs]
    ([EmailBlasterTemplateId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobEntMains'
ALTER TABLE [dbo].[JobEntMains]
ADD CONSTRAINT [FK_JobEntMainJobMain]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobEntMainJobMain'
CREATE INDEX [IX_FK_JobEntMainJobMain]
ON [dbo].[JobEntMains]
    ([JobMainId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'JobEntMains'
ALTER TABLE [dbo].[JobEntMains]
ADD CONSTRAINT [FK_JobEntMainCustEntMain]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobEntMainCustEntMain'
CREATE INDEX [IX_FK_JobEntMainCustEntMain]
ON [dbo].[JobEntMains]
    ([CustEntMainId]);
GO

-- Creating foreign key on [JobMainId] in table 'CashExpenses'
ALTER TABLE [dbo].[CashExpenses]
ADD CONSTRAINT [FK_CashExpenseJobMain]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CashExpenseJobMain'
CREATE INDEX [IX_FK_CashExpenseJobMain]
ON [dbo].[CashExpenses]
    ([JobMainId]);
GO

-- Creating foreign key on [CustomerId] in table 'PortalCustomers'
ALTER TABLE [dbo].[PortalCustomers]
ADD CONSTRAINT [FK_CustomerPortalCustomer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerPortalCustomer'
CREATE INDEX [IX_FK_CustomerPortalCustomer]
ON [dbo].[PortalCustomers]
    ([CustomerId]);
GO

-- Creating foreign key on [ExpensesId] in table 'JobExpenses'
ALTER TABLE [dbo].[JobExpenses]
ADD CONSTRAINT [FK_ExpensesJobExpenses]
    FOREIGN KEY ([ExpensesId])
    REFERENCES [dbo].[Expenses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpensesJobExpenses'
CREATE INDEX [IX_FK_ExpensesJobExpenses]
ON [dbo].[JobExpenses]
    ([ExpensesId]);
GO

-- Creating foreign key on [JobServicesId] in table 'JobExpenses'
ALTER TABLE [dbo].[JobExpenses]
ADD CONSTRAINT [FK_JobServicesJobExpenses]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesJobExpenses'
CREATE INDEX [IX_FK_JobServicesJobExpenses]
ON [dbo].[JobExpenses]
    ([JobServicesId]);
GO

-- Creating foreign key on [ExpensesCategoryId] in table 'Expenses'
ALTER TABLE [dbo].[Expenses]
ADD CONSTRAINT [FK_ExpensesCategoryExpenses]
    FOREIGN KEY ([ExpensesCategoryId])
    REFERENCES [dbo].[ExpensesCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpensesCategoryExpenses'
CREATE INDEX [IX_FK_ExpensesCategoryExpenses]
ON [dbo].[Expenses]
    ([ExpensesCategoryId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobPosts'
ALTER TABLE [dbo].[JobPosts]
ADD CONSTRAINT [FK_JobMainJobPost]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobPost'
CREATE INDEX [IX_FK_JobMainJobPost]
ON [dbo].[JobPosts]
    ([JobMainId]);
GO

-- Creating foreign key on [OnlineReservationId] in table 'RsvPayments'
ALTER TABLE [dbo].[RsvPayments]
ADD CONSTRAINT [FK_OnlineReservationRsvPayment]
    FOREIGN KEY ([OnlineReservationId])
    REFERENCES [dbo].[OnlineReservations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OnlineReservationRsvPayment'
CREATE INDEX [IX_FK_OnlineReservationRsvPayment]
ON [dbo].[RsvPayments]
    ([OnlineReservationId]);
GO

-- Creating foreign key on [DriverInstructionsId] in table 'DriverInsJobServices'
ALTER TABLE [dbo].[DriverInsJobServices]
ADD CONSTRAINT [FK_PickupInstructionsDriverInstructions]
    FOREIGN KEY ([DriverInstructionsId])
    REFERENCES [dbo].[DriverInstructions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PickupInstructionsDriverInstructions'
CREATE INDEX [IX_FK_PickupInstructionsDriverInstructions]
ON [dbo].[DriverInsJobServices]
    ([DriverInstructionsId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesLeadCompanies'
ALTER TABLE [dbo].[SalesLeadCompanies]
ADD CONSTRAINT [FK_SalesLeadSalesLeadCompany]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesLeadCompany'
CREATE INDEX [IX_FK_SalesLeadSalesLeadCompany]
ON [dbo].[SalesLeadCompanies]
    ([SalesLeadId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'SalesLeadCompanies'
ALTER TABLE [dbo].[SalesLeadCompanies]
ADD CONSTRAINT [FK_CustEntMainSalesLeadCompany]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainSalesLeadCompany'
CREATE INDEX [IX_FK_CustEntMainSalesLeadCompany]
ON [dbo].[SalesLeadCompanies]
    ([CustEntMainId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntAddresses'
ALTER TABLE [dbo].[CustEntAddresses]
ADD CONSTRAINT [FK_CustEntMainCustEntAddress]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntAddress'
CREATE INDEX [IX_FK_CustEntMainCustEntAddress]
ON [dbo].[CustEntAddresses]
    ([CustEntMainId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntCats'
ALTER TABLE [dbo].[CustEntCats]
ADD CONSTRAINT [FK_CustEntMainCustEntCat]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntCat'
CREATE INDEX [IX_FK_CustEntMainCustEntCat]
ON [dbo].[CustEntCats]
    ([CustEntMainId]);
GO

-- Creating foreign key on [CustCategoryId] in table 'CustEntCats'
ALTER TABLE [dbo].[CustEntCats]
ADD CONSTRAINT [FK_CustCategoryCustEntCat]
    FOREIGN KEY ([CustCategoryId])
    REFERENCES [dbo].[CustCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustCategoryCustEntCat'
CREATE INDEX [IX_FK_CustCategoryCustEntCat]
ON [dbo].[CustEntCats]
    ([CustCategoryId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntClauses'
ALTER TABLE [dbo].[CustEntClauses]
ADD CONSTRAINT [FK_CustEntMainCustEntClauses]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntClauses'
CREATE INDEX [IX_FK_CustEntMainCustEntClauses]
ON [dbo].[CustEntClauses]
    ([CustEntMainId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierContacts'
ALTER TABLE [dbo].[SupplierContacts]
ADD CONSTRAINT [FK_SupplierSupplierContact]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierContact'
CREATE INDEX [IX_FK_SupplierSupplierContact]
ON [dbo].[SupplierContacts]
    ([SupplierId]);
GO

-- Creating foreign key on [SupplierInvItemId] in table 'SupplierItemRates'
ALTER TABLE [dbo].[SupplierItemRates]
ADD CONSTRAINT [FK_SupplierInvItemSupplierItemRate]
    FOREIGN KEY ([SupplierInvItemId])
    REFERENCES [dbo].[SupplierInvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierInvItemSupplierItemRate'
CREATE INDEX [IX_FK_SupplierInvItemSupplierItemRate]
ON [dbo].[SupplierItemRates]
    ([SupplierInvItemId]);
GO

-- Creating foreign key on [SupplierUnitId] in table 'SupplierItemRates'
ALTER TABLE [dbo].[SupplierItemRates]
ADD CONSTRAINT [FK_SupplierUnitSupplierItemRate]
    FOREIGN KEY ([SupplierUnitId])
    REFERENCES [dbo].[SupplierUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierUnitSupplierItemRate'
CREATE INDEX [IX_FK_SupplierUnitSupplierItemRate]
ON [dbo].[SupplierItemRates]
    ([SupplierUnitId]);
GO

-- Creating foreign key on [JobServicesId] in table 'DriverInsJobServices'
ALTER TABLE [dbo].[DriverInsJobServices]
ADD CONSTRAINT [FK_JobServicesPickupInstructions]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobServicesPickupInstructions'
CREATE INDEX [IX_FK_JobServicesPickupInstructions]
ON [dbo].[DriverInsJobServices]
    ([JobServicesId]);
GO

-- Creating foreign key on [SalesLeadId] in table 'SalesLeadItems'
ALTER TABLE [dbo].[SalesLeadItems]
ADD CONSTRAINT [FK_SalesLeadSalesLeadItems]
    FOREIGN KEY ([SalesLeadId])
    REFERENCES [dbo].[SalesLeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadSalesLeadItems'
CREATE INDEX [IX_FK_SalesLeadSalesLeadItems]
ON [dbo].[SalesLeadItems]
    ([SalesLeadId]);
GO

-- Creating foreign key on [InvItemId] in table 'SalesLeadItems'
ALTER TABLE [dbo].[SalesLeadItems]
ADD CONSTRAINT [FK_InvItemSalesLeadItems]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemSalesLeadItems'
CREATE INDEX [IX_FK_InvItemSalesLeadItems]
ON [dbo].[SalesLeadItems]
    ([InvItemId]);
GO

-- Creating foreign key on [SalesLeadItemsId] in table 'SalesLeadQuotedItems'
ALTER TABLE [dbo].[SalesLeadQuotedItems]
ADD CONSTRAINT [FK_SalesLeadItemsSalesLeadQuotedItem]
    FOREIGN KEY ([SalesLeadItemsId])
    REFERENCES [dbo].[SalesLeadItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesLeadItemsSalesLeadQuotedItem'
CREATE INDEX [IX_FK_SalesLeadItemsSalesLeadQuotedItem]
ON [dbo].[SalesLeadQuotedItems]
    ([SalesLeadItemsId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustSocialAccs'
ALTER TABLE [dbo].[CustSocialAccs]
ADD CONSTRAINT [FK_CustomerCustSocialAcc]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustSocialAcc'
CREATE INDEX [IX_FK_CustomerCustSocialAcc]
ON [dbo].[CustSocialAccs]
    ([CustomerId]);
GO

-- Creating foreign key on [SupplierContactStatusId] in table 'SupplierContacts'
ALTER TABLE [dbo].[SupplierContacts]
ADD CONSTRAINT [FK_SupplierContactStatusSupplierContact]
    FOREIGN KEY ([SupplierContactStatusId])
    REFERENCES [dbo].[SupplierContactStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierContactStatusSupplierContact'
CREATE INDEX [IX_FK_SupplierContactStatusSupplierContact]
ON [dbo].[SupplierContacts]
    ([SupplierContactStatusId]);
GO

-- Creating foreign key on [CountryId] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK_SupplierCountry]
    FOREIGN KEY ([CountryId])
    REFERENCES [dbo].[Countries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierCountry'
CREATE INDEX [IX_FK_SupplierCountry]
ON [dbo].[Suppliers]
    ([CountryId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntAssigns'
ALTER TABLE [dbo].[CustEntAssigns]
ADD CONSTRAINT [FK_CustEntMainCustEntAssign]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntAssign'
CREATE INDEX [IX_FK_CustEntMainCustEntAssign]
ON [dbo].[CustEntAssigns]
    ([CustEntMainId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierActivities'
ALTER TABLE [dbo].[SupplierActivities]
ADD CONSTRAINT [FK_SupplierSupplierActivity]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierActivity'
CREATE INDEX [IX_FK_SupplierSupplierActivity]
ON [dbo].[SupplierActivities]
    ([SupplierId]);
GO

-- Creating foreign key on [SupplierId] in table 'SupplierDocuments'
ALTER TABLE [dbo].[SupplierDocuments]
ADD CONSTRAINT [FK_SupplierSupplierDocument]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierDocument'
CREATE INDEX [IX_FK_SupplierSupplierDocument]
ON [dbo].[SupplierDocuments]
    ([SupplierId]);
GO

-- Creating foreign key on [SupDocumentId] in table 'SupplierDocuments'
ALTER TABLE [dbo].[SupplierDocuments]
ADD CONSTRAINT [FK_SupDocumentSupplierDocument]
    FOREIGN KEY ([SupDocumentId])
    REFERENCES [dbo].[SupDocuments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupDocumentSupplierDocument'
CREATE INDEX [IX_FK_SupDocumentSupplierDocument]
ON [dbo].[SupplierDocuments]
    ([SupDocumentId]);
GO

-- Creating foreign key on [SupDocumentId] in table 'CustEntDocuments'
ALTER TABLE [dbo].[CustEntDocuments]
ADD CONSTRAINT [FK_CustEntDocumentsSupDocument]
    FOREIGN KEY ([SupDocumentId])
    REFERENCES [dbo].[SupDocuments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntDocumentsSupDocument'
CREATE INDEX [IX_FK_CustEntDocumentsSupDocument]
ON [dbo].[CustEntDocuments]
    ([SupDocumentId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntDocuments'
ALTER TABLE [dbo].[CustEntDocuments]
ADD CONSTRAINT [FK_CustEntMainCustEntDocuments]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntDocuments'
CREATE INDEX [IX_FK_CustEntMainCustEntDocuments]
ON [dbo].[CustEntDocuments]
    ([CustEntMainId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'CustEntActivities'
ALTER TABLE [dbo].[CustEntActivities]
ADD CONSTRAINT [FK_CustEntMainCustEntActivity]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainCustEntActivity'
CREATE INDEX [IX_FK_CustEntMainCustEntActivity]
ON [dbo].[CustEntActivities]
    ([CustEntMainId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustNotifRecipients'
ALTER TABLE [dbo].[CustNotifRecipients]
ADD CONSTRAINT [FK_CustomerCustNotifRecipient]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustNotifRecipient'
CREATE INDEX [IX_FK_CustomerCustNotifRecipient]
ON [dbo].[CustNotifRecipients]
    ([CustomerId]);
GO

-- Creating foreign key on [CustNotifId] in table 'CustNotifRecipients'
ALTER TABLE [dbo].[CustNotifRecipients]
ADD CONSTRAINT [FK_CustNotifCustNotifRecipient]
    FOREIGN KEY ([CustNotifId])
    REFERENCES [dbo].[CustNotifs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustNotifCustNotifRecipient'
CREATE INDEX [IX_FK_CustNotifCustNotifRecipient]
ON [dbo].[CustNotifRecipients]
    ([CustNotifId]);
GO

-- Creating foreign key on [NotifRecipientId] in table 'CustNotifRecipients'
ALTER TABLE [dbo].[CustNotifRecipients]
ADD CONSTRAINT [FK_NotifRecipientCustNotifRecipient]
    FOREIGN KEY ([NotifRecipientId])
    REFERENCES [dbo].[CustNotifRecipientLists]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NotifRecipientCustNotifRecipient'
CREATE INDEX [IX_FK_NotifRecipientCustNotifRecipient]
ON [dbo].[CustNotifRecipients]
    ([NotifRecipientId]);
GO

-- Creating foreign key on [CustNotifRecipientId] in table 'CustNotifActivities'
ALTER TABLE [dbo].[CustNotifActivities]
ADD CONSTRAINT [FK_CustNotifRecipientCustNotifActivity]
    FOREIGN KEY ([CustNotifRecipientId])
    REFERENCES [dbo].[CustNotifRecipients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustNotifRecipientCustNotifActivity'
CREATE INDEX [IX_FK_CustNotifRecipientCustNotifActivity]
ON [dbo].[CustNotifActivities]
    ([CustNotifRecipientId]);
GO

-- Creating foreign key on [VehicleId] in table 'JobVehicles'
ALTER TABLE [dbo].[JobVehicles]
ADD CONSTRAINT [FK_VehicleJobVehicle]
    FOREIGN KEY ([VehicleId])
    REFERENCES [dbo].[Vehicles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleJobVehicle'
CREATE INDEX [IX_FK_VehicleJobVehicle]
ON [dbo].[JobVehicles]
    ([VehicleId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobVehicles'
ALTER TABLE [dbo].[JobVehicles]
ADD CONSTRAINT [FK_JobMainJobVehicle]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainJobVehicle'
CREATE INDEX [IX_FK_JobMainJobVehicle]
ON [dbo].[JobVehicles]
    ([JobMainId]);
GO

-- Creating foreign key on [CustomerId] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [FK_CustomerVehicle]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerVehicle'
CREATE INDEX [IX_FK_CustomerVehicle]
ON [dbo].[Vehicles]
    ([CustomerId]);
GO

-- Creating foreign key on [CustEntMainId] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [FK_CustEntMainVehicle]
    FOREIGN KEY ([CustEntMainId])
    REFERENCES [dbo].[CustEntMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntMainVehicle'
CREATE INDEX [IX_FK_CustEntMainVehicle]
ON [dbo].[Vehicles]
    ([CustEntMainId]);
GO

-- Creating foreign key on [VehicleBrandId] in table 'VehicleModels'
ALTER TABLE [dbo].[VehicleModels]
ADD CONSTRAINT [FK_VehicleBrandVehicleMake]
    FOREIGN KEY ([VehicleBrandId])
    REFERENCES [dbo].[VehicleBrands]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleBrandVehicleMake'
CREATE INDEX [IX_FK_VehicleBrandVehicleMake]
ON [dbo].[VehicleModels]
    ([VehicleBrandId]);
GO

-- Creating foreign key on [VehicleTypeId] in table 'VehicleModels'
ALTER TABLE [dbo].[VehicleModels]
ADD CONSTRAINT [FK_VehicleTypeVehicleMake]
    FOREIGN KEY ([VehicleTypeId])
    REFERENCES [dbo].[VehicleTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleTypeVehicleMake'
CREATE INDEX [IX_FK_VehicleTypeVehicleMake]
ON [dbo].[VehicleModels]
    ([VehicleTypeId]);
GO

-- Creating foreign key on [VehicleTransmissionId] in table 'VehicleModels'
ALTER TABLE [dbo].[VehicleModels]
ADD CONSTRAINT [FK_VehicleTransmissionVehicleMake]
    FOREIGN KEY ([VehicleTransmissionId])
    REFERENCES [dbo].[VehicleTransmissions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleTransmissionVehicleMake'
CREATE INDEX [IX_FK_VehicleTransmissionVehicleMake]
ON [dbo].[VehicleModels]
    ([VehicleTransmissionId]);
GO

-- Creating foreign key on [VehicleFuelId] in table 'VehicleModels'
ALTER TABLE [dbo].[VehicleModels]
ADD CONSTRAINT [FK_VehicleFuelVehicleMake]
    FOREIGN KEY ([VehicleFuelId])
    REFERENCES [dbo].[VehicleFuels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleFuelVehicleMake'
CREATE INDEX [IX_FK_VehicleFuelVehicleMake]
ON [dbo].[VehicleModels]
    ([VehicleFuelId]);
GO

-- Creating foreign key on [VehicleModelId] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [FK_VehicleModelVehicle]
    FOREIGN KEY ([VehicleModelId])
    REFERENCES [dbo].[VehicleModels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleModelVehicle'
CREATE INDEX [IX_FK_VehicleModelVehicle]
ON [dbo].[Vehicles]
    ([VehicleModelId]);
GO

-- Creating foreign key on [VehicleDriveId] in table 'VehicleModels'
ALTER TABLE [dbo].[VehicleModels]
ADD CONSTRAINT [FK_VehicleDriveVehicleModel]
    FOREIGN KEY ([VehicleDriveId])
    REFERENCES [dbo].[VehicleDrives]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleDriveVehicleModel'
CREATE INDEX [IX_FK_VehicleDriveVehicleModel]
ON [dbo].[VehicleModels]
    ([VehicleDriveId]);
GO

-- Creating foreign key on [JobServicesId] in table 'JobPostSales'
ALTER TABLE [dbo].[JobPostSales]
ADD CONSTRAINT [FK_JobPostSaleJobServices]
    FOREIGN KEY ([JobServicesId])
    REFERENCES [dbo].[JobServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobPostSaleJobServices'
CREATE INDEX [IX_FK_JobPostSaleJobServices]
ON [dbo].[JobPostSales]
    ([JobServicesId]);
GO

-- Creating foreign key on [CustEntAccountTypeId] in table 'CustEntMains'
ALTER TABLE [dbo].[CustEntMains]
ADD CONSTRAINT [FK_CustEntAccountTypeCustEntMain]
    FOREIGN KEY ([CustEntAccountTypeId])
    REFERENCES [dbo].[CustEntAccountTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntAccountTypeCustEntMain'
CREATE INDEX [IX_FK_CustEntAccountTypeCustEntMain]
ON [dbo].[CustEntMains]
    ([CustEntAccountTypeId]);
GO

-- Creating foreign key on [JobMainId] in table 'JobMainPaymentStatus'
ALTER TABLE [dbo].[JobMainPaymentStatus]
ADD CONSTRAINT [FK_JobMainPaymentStatusJobMain]
    FOREIGN KEY ([JobMainId])
    REFERENCES [dbo].[JobMains]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobMainPaymentStatusJobMain'
CREATE INDEX [IX_FK_JobMainPaymentStatusJobMain]
ON [dbo].[JobMainPaymentStatus]
    ([JobMainId]);
GO

-- Creating foreign key on [JobPaymentStatusId] in table 'JobMainPaymentStatus'
ALTER TABLE [dbo].[JobMainPaymentStatus]
ADD CONSTRAINT [FK_JobPaymentStatusJobMainPaymentStatus]
    FOREIGN KEY ([JobPaymentStatusId])
    REFERENCES [dbo].[JobPaymentStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobPaymentStatusJobMainPaymentStatus'
CREATE INDEX [IX_FK_JobPaymentStatusJobMainPaymentStatus]
ON [dbo].[JobMainPaymentStatus]
    ([JobPaymentStatusId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvItemCommis'
ALTER TABLE [dbo].[InvItemCommis]
ADD CONSTRAINT [FK_InvItemCommiInvItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemCommiInvItem'
CREATE INDEX [IX_FK_InvItemCommiInvItem]
ON [dbo].[InvItemCommis]
    ([InvItemId]);
GO

-- Creating foreign key on [JobPaymentTypeId] in table 'JobPayments'
ALTER TABLE [dbo].[JobPayments]
ADD CONSTRAINT [FK_JobPaymentTypeJobPayment]
    FOREIGN KEY ([JobPaymentTypeId])
    REFERENCES [dbo].[JobPaymentTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobPaymentTypeJobPayment'
CREATE INDEX [IX_FK_JobPaymentTypeJobPayment]
ON [dbo].[JobPayments]
    ([JobPaymentTypeId]);
GO

-- Creating foreign key on [JobPostSalesStatusId] in table 'JobPostSales'
ALTER TABLE [dbo].[JobPostSales]
ADD CONSTRAINT [FK_JobPostSalesStatusJobPostSale]
    FOREIGN KEY ([JobPostSalesStatusId])
    REFERENCES [dbo].[JobPostSalesStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JobPostSalesStatusJobPostSale'
CREATE INDEX [IX_FK_JobPostSalesStatusJobPostSale]
ON [dbo].[JobPostSales]
    ([JobPostSalesStatusId]);
GO

-- Creating foreign key on [ServicesId] in table 'SvcGroups'
ALTER TABLE [dbo].[SvcGroups]
ADD CONSTRAINT [FK_ServicesSvcGroup]
    FOREIGN KEY ([ServicesId])
    REFERENCES [dbo].[Services]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServicesSvcGroup'
CREATE INDEX [IX_FK_ServicesSvcGroup]
ON [dbo].[SvcGroups]
    ([ServicesId]);
GO

-- Creating foreign key on [SvcDetailId] in table 'SvcGroups'
ALTER TABLE [dbo].[SvcGroups]
ADD CONSTRAINT [FK_SvcDetailSvcGroup]
    FOREIGN KEY ([SvcDetailId])
    REFERENCES [dbo].[SvcDetails]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SvcDetailSvcGroup'
CREATE INDEX [IX_FK_SvcDetailSvcGroup]
ON [dbo].[SvcGroups]
    ([SvcDetailId]);
GO

-- Creating foreign key on [SupplierActStatusId] in table 'SupplierActivities'
ALTER TABLE [dbo].[SupplierActivities]
ADD CONSTRAINT [FK_SupplierActStatusSupplierActivity]
    FOREIGN KEY ([SupplierActStatusId])
    REFERENCES [dbo].[SupplierActStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierActStatusSupplierActivity'
CREATE INDEX [IX_FK_SupplierActStatusSupplierActivity]
ON [dbo].[SupplierActivities]
    ([SupplierActStatusId]);
GO

-- Creating foreign key on [CustEntActPostSaleStatusId] in table 'CustEntActPostSales'
ALTER TABLE [dbo].[CustEntActPostSales]
ADD CONSTRAINT [FK_CustEntActPostSaleStatusCustEntActPostSale]
    FOREIGN KEY ([CustEntActPostSaleStatusId])
    REFERENCES [dbo].[CustEntActPostSaleStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntActPostSaleStatusCustEntActPostSale'
CREATE INDEX [IX_FK_CustEntActPostSaleStatusCustEntActPostSale]
ON [dbo].[CustEntActPostSales]
    ([CustEntActPostSaleStatusId]);
GO

-- Creating foreign key on [SupplierItemRateId] in table 'SalesLeadQuotedItems'
ALTER TABLE [dbo].[SalesLeadQuotedItems]
ADD CONSTRAINT [FK_SupplierItemRateSalesLeadQuotedItem]
    FOREIGN KEY ([SupplierItemRateId])
    REFERENCES [dbo].[SupplierItemRates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierItemRateSalesLeadQuotedItem'
CREATE INDEX [IX_FK_SupplierItemRateSalesLeadQuotedItem]
ON [dbo].[SalesLeadQuotedItems]
    ([SupplierItemRateId]);
GO

-- Creating foreign key on [CustEntActStatusId] in table 'CustEntActivities'
ALTER TABLE [dbo].[CustEntActivities]
ADD CONSTRAINT [FK_CustEntActStatusCustEntActivity]
    FOREIGN KEY ([CustEntActStatusId])
    REFERENCES [dbo].[CustEntActStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntActStatusCustEntActivity'
CREATE INDEX [IX_FK_CustEntActStatusCustEntActivity]
ON [dbo].[CustEntActivities]
    ([CustEntActStatusId]);
GO

-- Creating foreign key on [CustEntActActionStatusId] in table 'CustEntActivities'
ALTER TABLE [dbo].[CustEntActivities]
ADD CONSTRAINT [FK_CustEntActActionStatusCustEntActivity]
    FOREIGN KEY ([CustEntActActionStatusId])
    REFERENCES [dbo].[CustEntActActionStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntActActionStatusCustEntActivity'
CREATE INDEX [IX_FK_CustEntActActionStatusCustEntActivity]
ON [dbo].[CustEntActivities]
    ([CustEntActActionStatusId]);
GO

-- Creating foreign key on [CustEntActActionCodesId] in table 'CustEntActivities'
ALTER TABLE [dbo].[CustEntActivities]
ADD CONSTRAINT [FK_CustEntActActionCodesCustEntActivity]
    FOREIGN KEY ([CustEntActActionCodesId])
    REFERENCES [dbo].[CustEntActActionCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustEntActActionCodesCustEntActivity'
CREATE INDEX [IX_FK_CustEntActActionCodesCustEntActivity]
ON [dbo].[CustEntActivities]
    ([CustEntActActionCodesId]);
GO

-- Creating foreign key on [CarUnitId] in table 'CarDetails'
ALTER TABLE [dbo].[CarDetails]
ADD CONSTRAINT [FK_CarDetailCarUnit]
    FOREIGN KEY ([CarUnitId])
    REFERENCES [dbo].[CarUnits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarDetailCarUnit'
CREATE INDEX [IX_FK_CarDetailCarUnit]
ON [dbo].[CarDetails]
    ([CarUnitId]);
GO

-- Creating foreign key on [CarResTypeId] in table 'CarReservations'
ALTER TABLE [dbo].[CarReservations]
ADD CONSTRAINT [FK_CarResTypeCarReservation]
    FOREIGN KEY ([CarResTypeId])
    REFERENCES [dbo].[CarResTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarResTypeCarReservation'
CREATE INDEX [IX_FK_CarResTypeCarReservation]
ON [dbo].[CarReservations]
    ([CarResTypeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------