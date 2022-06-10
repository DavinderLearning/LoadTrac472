
//load the customers dropdown in the input div
function getData(pUrl, pCallBack) {
    $.ajax({
        type: 'GET',
        url: pUrl,
        cache: false,
        success: pCallBack
        });
}
function postData(pUrl, pData, pCallBack) {
    $.ajax({
        type: 'POST',
        url: pUrl,
        cache: false,
        asynch: false,
        data: pData,
        success: pCallBack
        //error: alert('Unable to post transaction.  ')        
    });       
}
function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&amp;");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
   // alert('Query Variable ' + variable + ' not found');
}
//UI Class
function loadKendoDropdown(pData, pDropdownName, pTextField,pValueField) {
    $(pDropdownName).kendoComboBox({
        dataTextField: pTextField,
        dataValueField: pValueField,
        dataSource: pData,
        //change: onChange,
        filter: "startswith",
        suggest: true,
        index: -1
});
}

function setDropdownValue(pDropdown,pValue) {
    var $cbx = $(pDropdown).data("kendoComboBox")
    if(pValue>0)
        $cbx.value(pValue);
}


function openModalWindow(pDivName, pTitle) {
    $(pDivName).dialog({
        autoOpen: false,
        modal: true,
        resizeable: false,
        height: 650,
        width: 600,
        title: pTitle,
        position: "center"
    });
    $(pDivName).dialog("open");    
}


function formatJSONDate(jsonDate) {
    var date = new Date(parseInt(jsonDate.substr(6)));
    var newDate = date.toLocaleDateString();
    return newDate;
}