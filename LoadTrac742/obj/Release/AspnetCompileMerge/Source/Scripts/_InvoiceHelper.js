//---------------------------------------------------
//invoices logic
//---------------------------------------------------
//function editInvoice(e) {
//    e.preventDefault();
//    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
//    //alert(dataItem.Id);
//    window.open("./Invoice/Edit/" + dataItem.Id);
//}
function editInvoice2(pInvoiceId) {
    window.open("../../Invoice/Edit/" + pInvoiceId);
}

function loadInvoices() {
    var i = getCustomerId();
    var invoicesWithBalance = $('input[name=chkShowAllInvoices]').is(':checked')
    //alert(invoicesWithBalance);
    var url = "";
    if (invoicesWithBalance == false) {
        url = "/Invoice/IndexOpenInvoices?pCustomerId=" + i + "";
    }
    else {
        url = "/Invoice/IndexJ?pCustomerId=" + i + "";
    }
    getData(url, loadInvoicesCallback);
}
function loadInvoicesCallback(data) {
    $("#gridInvoices").kendoGrid({
        dataSource: data,
        columns: [
            {
                field: "X",
                width: 70,
                template: "<div class='k-button k-grid-edit' onclick=editInvoice2(#= kendo.toString(Id)#)  style='min-width:16px;'><span class='k-icon k-edit'></span></div>"
            }
            , { field: "InvoiceNumber", title: "Invoice #" }
            , {
                field: "InvoiceDate", title: "Invoice Date",
                template: '#= kendo.toString(new Date(parseInt(InvoiceDate.replace(/[A-Za-z$--/]/g, ""))),"MM/dd/yyyy") #'
            }
           , { field: "Subtotal", format: "{0:c}" }
           , { field: "TaxName", title: "Tax", format: "{0:c}" }
           , { field: "TaxAmount", title: "Tax($)", format: "{0:c}" }
            , { field: "Total", format: "{0:c}" }
            , { field: "Balance", format: "{0:c}" }
        ]
        ,sortable: true
     //, scrollable: { virtual: true }
    }).data("kendoGrid");
}
function btnCreateInvoice() {
    var url = "../../Invoice/Create?pCustomerId=" + getCustomerId();        
    window.open(url);
    //$.ajax({
    //    type: 'POST',
    //    url: "/Invoice/Create",
    //    data: {
    //        CustomerId: getCustomerId()
    //    , InvoiceDate: $("#txtInvoiceDate").val()
    //    }
    //    , success: function (data) {
    //        var myurl = "../../Invoice/Edit/" + data;
    //        //alert("Data Loaded: " + data + " url " + myurl)
    //        //'New Invoice','fullscreen=1, menubar=1,toolbar=1'
    //        window.open(myurl);
    //    }
    //});
}
