//global page variables
var blnIsEditMode = false;

var selectTrucks ;
var selectLoadTypes;
var selectEquips;
var selectProducts;

var newTicketObject;

//used for adding/removing items to the invoice in the invoice edit screen
var selectedDataItems = [];
var selectedDataItemsInvoice = [];
var invoiceId = 0;
var invoiceLinesAvailableData;
var invoiceLinesData;

$(document).ready(function () {    
 
    getTruckData();
    getLoadTypeData();
    getEquipData();
    getProductData();
});

///gets the current selected customer id
function getCustomerId() {
    return txtCustomerId.value;
}

///creates in instance of a new ticket with the desired default values
function newTicket() {
    blnIsEditMode = false;
    // $.getJSON("/InvoiceLine/New", function (data) { viewTicketWindowCallback(data) });
    getData("/InvoiceLine/New", viewTicketWindowCallback);

}

function loadOpenTickets() {
    var i = getCustomerId();
    var url = "/InvoiceLine/GetOpenTickets?CustomerId=" + i + "";
    getData(url, loadOpenTicketsCallback);
}
function loadOpenTicketsCallback(data) {
    $("#gridTickets").kendoGrid({
        dataSource: data ,
        columns: [
            {
                field: "Id",
                title: "",
                width: 60,
                template: "<div class='k-button k-grid-edit' onclick=editTicket(#= kendo.toString(Id)#)  style='min-width:16px;'><span class='k-icon k-edit'></span></div>"             
            }
            , {
                field: "Id",
                title: " ",
                width: 60,
                template: "<div class='k-button k-grid-delete' onclick=deleteTicket(#= kendo.toString(Id)#)  style='min-width:16px;'><span class='k-icon k-delete'></span></div>"
            }
            , {
                field: "TicketDate", title: "Date"
                , width: 120
                , template: '#= kendo.toString(new Date(parseInt(TicketDate.replace(/[A-Za-z$--/]/g, ""))),"MM/dd/yyyy") #'
            }
            , { field: "TicketNumber", title: "Ticket#" }
            , { field: "TicketTruck.Name", title: "Truck", width: 100 }
            , { field: "TicketLoadType.Name", title: "Type", width: 120 }
            , { field: "TicketEquip.Name", title: "Equip", width: 100 }
            , { field: "TicketProduct.Name", title: "Product" }
            , { field: "Description", title: "Desc" }
            , { field: "Quantity", title: "Qty", format: "{0:n}", width: 100 }
            , { field: "Price", title: "Price", format: "{0:c}", width: 100 }
            , { field: "ExtendedAmount", title: "Total", format: "{0:c}", width: 110 }
        ]       
       , selectable: "multiple row"
        //put the selected item in the selected item collection
       , change: function () {
           var selectedRows = this.select();
           selectedDataItems = [];
           for (var i = 0; i < selectedRows.length; i++) {
               var dataItem = this.dataItem(selectedRows[i]);
               selectedDataItems.push(dataItem);
           }
       }
    }).data("kendoGrid");
    //alert(data.length);
    invoiceLinesAvailableData = data;
};


//call to load the edit ticket window
function editTicket(pTicketId) {
    //$.getJSON("/InvoiceLine/Get?pId=" + pTicketId, function (data) { viewTicketWindowCallback(data) });
    var url = "/InvoiceLine/Get?pId=" + pTicketId;
    getData(url, viewTicketWindowCallback);
    blnIsEditMode = true;
}

///newTicket and editTicket callBack handler
///will the selected ticket in a dialog window
//using the templating in Jquery
function viewTicketWindowCallback(data) {
    //clear the div where template is being appended
    $("#dvCreateTicket").empty();
    //append the template
    $("#templTicket").tmpl(data).appendTo("#dvCreateTicket");
    //load the dropdowns into the new div with templated fields
    loadKendoDropdown(selectTrucks, "#cboTruck", "Name", "Id");
    cboTruck.value = data.TruckId;
    setDropdownValue("#cboTruck", data.TruckId);

    loadKendoDropdown(selectLoadTypes, "#cboLoadType", "Name", "Id");
    setDropdownValue("#cboLoadType", data.LoadTypeId);

   loadKendoDropdown(selectEquips, "#cboEquip", "Name", "Id");
   setDropdownValue("#cboEquip", data.EquipId);

    $("#cboEquip").buttonset();

    loadKendoDropdown(selectProducts, "#cboProduct", "Name", "Id");
    setDropdownValue("#cboProduct", data.ProductId);

    txtTicketId.value = data.Id;
    $("#txtTicketCreate_Date").kendoDatePicker();

    var title = "Ticket Entry: <New Ticket>"
    if (blnIsEditMode) {
        title = "Ticket Entry: <Editing Ticket Id: " + data.Id + ">"
        //set the invoiceId for saving the ticket
        invoiceId = data.InvoiceId;
        //set the datepicker default value
        txtTicketCreate_Date.value = formatJSONDate(data.TicketDate);

    }
    else {
        //new ticket window opening
       // loadOpenTickets();
    }
    openModalWindow("#dvCreateTicket", title);
}
function postEditTicket() {
    var url = "/InvoiceLine/CreateJ";
    if (blnIsEditMode) {
        url = "/InvoiceLine/EditJ";
    }
    var tDate=$("#txtTicketCreate_Date").val();
    if (tDate == "Mon Jan 1 1") {

        alert($("#txtTicketCreate_Date").val() + ' is not a proper date.  Please select a proper date and try again.');
        return;
    }
    var data = {
        InvoiceId: invoiceId
            , AddToInvoice: 0
            , Id: $("#txtTicketId").val()
            , TicketDate: $("#txtTicketCreate_Date").val()
            , TicketNumber: $("#txtTicketCreate_TicketNumber").val()
            , ProductId: $("#cboProduct").val()
            , Description: $("#txtTicketCreate_Description").val()
            , Quantity: $("#txtTicketCreate_Quantity").val()
            , Price: $("#txtTicketCreate_Price").val()
            , LoadTypeId: $("#cboLoadType").val()
            , EquipId: $("#cboEquip").val()
            , TruckId: $("#cboTruck").val()
            , CustomerId: getCustomerId()
            , IsLineTaxable: $("#txtTicketCreate_Taxable").is(':checked')
    };
    postData(url, data, postEditTicketCallback);
    
}

function postEditTicketCallback(data)
{
    $("#txtStatus").val(data);
    loadOpenTickets();
    //if new ticket mode then open a new window with ticket
    if (blnIsEditMode == false) {
        newTicket();
    }
    else {
        //if this and invoiced ticket 
        //then cause recalc and loadInovicedTickets
        if (invoiceId != 0) {
            //alert('causing recalc and reload of invoices '+ invoiceId);
            recalc();
            loadInvoicedTickets(invoiceId);
        }
        //Close the ticket dialog window
        $("#dvCreateTicket").dialog("close");

    }
}
function validateTicketNumber() {
    //only on new ticket
    if (!blnIsEditMode) {
        var url = "/InvoiceLine/ValidateTicketNumber";
        var data = {
            TicketNumber: $("#txtTicketCreate_TicketNumber").val()
                , CustomerId: getCustomerId()
        };
        postData(url, data, validateTicketNumber_Callback);
    }
}
function validateTicketNumber_Callback(data) {
    if (data != "")
        alert(data);
}

function loadInvoicedTickets(i) {
   // alert(i);
    var url = "/InvoiceLine/GetInvoiceLines?InvoiceId=" + i + ""
    getData(url, loadInvoicedTicketsCallback);    
}
function loadInvoicedTicketsCallback(data) {
    //alert('load ticket data' + data);
    $("#gridInvoiceLines").kendoGrid({
        dataSource: data,
        columns: [
            {
                field: "Id",
                title: "",
                width: 70,
                template: "<div class='k-button k-grid-edit' onclick=editTicket(#= kendo.toString(Id)#)  style='min-width:16px;'><span class='k-icon k-edit'></span></div>"
            }
            , {
                field: "TicketDate", title: "Date"
                , width: 120
                , template: '#= kendo.toString(new Date(parseInt(TicketDate.replace(/[A-Za-z$--/]/g, ""))),"MM/dd/yyyy") #'
            }
            , { field: "TicketNumber", title: "Ticket#" }
            , { field: "TicketTruck.Name", title: "Truck", width: 80 }
            , { field: "TicketLoadType.Name", title: "Type", width: 70 }
            , { field: "TicketEquip.Name", title: "Equip", width: 70 }
            , { field: "TicketProduct.Name", title: "Product" }
            , { field: "Description", title: "Desc" }
            , { field: "Quantity", title: "Qty", format: "{0:n}", width: 100 }
            , { field: "Price", title: "Price", format: "{0:c}", width: 100 }
            , { field: "ExtendedAmount", title: "Total", format: "{0:c}", width: 110 }
        ]
       , selectable: "multiple row"
        //put the selected item in the selected item collection
       , change: function () {
           var selectedRows = this.select();
           selectedDataItems = [];
           for (var i = 0; i < selectedRows.length; i++) {
               var dataItem = this.dataItem(selectedRows[i]);
               selectedDataItemsInvoice.push(dataItem);
           }
       }
    }).data("kendoGrid");
    invoiceLinesData = data;
};

function deleteTicket(pTicketId) {
    //$.getJSON("/InvoiceLine/Get?pId=" + pTicketId, function (data) { showDeleteTicketDialog(data) });
    getData("/InvoiceLine/Get?pId=" + pTicketId, showDeleteTicketDialog);
}

function showDeleteTicketDialog(data) {
    $("#dvCreateTicket").empty();
    $("#templDeleteTicket").tmpl(data).appendTo("#dvCreateTicket");
    openModalWindow("#dvCreateTicket", "Delete Ticket Id " + data.Id);
}


//#region 
///call the server method for ticket deletion
function postDeleteTicket()
{
    //call the post method for delete
    $.ajax({
        type: 'POST',
        url: "/InvoiceLine/Delete",
        cache: false,
        data: {
            Id: $("#txtDeleteTicketId").val()
        }
    });
    //close the dialog after delete
    $("#dvCreateTicket").dialog("close");

    loadOpenTickets();
}

//Dropdowns
function loadCustomers() {
    getData("/Customer/IndexJ", loadCustomersCallback);
}
function loadCustomersCallback(pData) {
    loadKendoDropdown(pData, "#input", "CustomerName", "Id");
}
function getTruckData() {
    getData("/DataService/GetTrucks", loadTrucksCallback);
}
function loadTrucksCallback(pData) {    
    selectTrucks = pData;
}
function getLoadTypeData() {
    getData("/DataService/GetLoadTypes", loadLoadTypesCallback);
}
function loadLoadTypesCallback(pData) {
    selectLoadTypes = pData;
}
function getEquipData() {
    getData("/DataService/GetEquips", loadEquipsCallback);
}
function loadEquipsCallback(pData) {
     selectEquips = pData;
}
function getProductData() {
    getData("/DataService/GetProducts", loadProductsCallback);
}
function loadProductsCallback(pData) {
    selectProducts = pData;
}
//#endregion 
