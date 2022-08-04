$(function () {
    //Make the dashboard widgets sortable Using jquery UI
    //$(".connectedSortable").sortable({
    //    placeholder: "sort-highlight",
    //    connectWith: ".connectedSortable",
    //    handle: ".box-header, .nav-tabs",
    //    forcePlaceholderSize: true,
    //    zIndex: 999999
    //});
    //$(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");


    //SetInputMask();
    //SetTootip();
    SetICheck();
    //SetColDatatable();
    try {
        $('.dropdownSearch').selectpicker({
            'size': '6'

        });
    } catch (ex) {
    }
});

function viewmode() {

    var obj = $('body').find('#divContent').find('input');
    $.each(obj, function (key, data) {
        $('#' + data.id).prop('disabled', true);
    });
    var obj = $('body').find('#divContent').find('textarea');
    $.each(obj, function (key, data) {
        $('#' + data.id).prop('disabled', true);
    });
    var obj = $('body').find('#divContent').find('button');
    $.each(obj, function (key, data) {
        if (data.id != "btnBack") {
            $('#' + data.id).prop('disabled', true);
        }

    });
    var obj = $('body').find('#divContent').find('select');
    $.each(obj, function (key, data) {
        $('#' + data.id).prop('disabled', true);
    });

}


function LoaddinProcess() {
    $("#loading-div-background").show();
}

function HideLoadding() {
    $("#loading-div-background").hide();
}

function PopupLogin(homeUrl) {
    if (homeUrl != undefined) {
        DialogAlertLogin(DialogHeader.Info, DialogMsg.Login, homeUrl);
    }
    else {
        DialogAlertLogin(DialogHeader.Info, DialogMsg.Login);
    }
}

function PopUpNoPermission() {
    DialogAlertLogin(DialogHeader.Warning, "You don't have permission.", "index.aspx");
}

function PopUpNoPermissionUnit() {
    DialogAlertLogin(DialogHeader.Warning, "You don't have permission.", "index.aspx");
}
function PopUpNoPermissionRedirect(URL) {
    DialogAlertLogin(DialogHeader.Warning, "You don't have permission.", URL);
}
//process code behind return
var SysProcess = {
    FileOversize: "OverSize",
    FileInvalidType: "InvalidType",
    Failed: "Failed",
    Success: "Success",
    SessionExpired: "SSEXP",
    Duplicate: "DUP"
}

var GridEvent = {
    sort: "SORT",
    pageindex: "PAGINDEX",
    pagesize: "PAGESIZE",
    BIND: "BIND"
}

var AshxSysFunc = {
    url: "Ashx/SystemFunction.ashx",
    FuncEncrypt: "encrypt",
    FuncDecrypt: "decrypt",
    FuncEncodeForJavaDecode: "encrypt_decodejava"
}

var AshxSysFuncParam = {
    funcName: "funcName",
    param1: "param1"
}
var EmptyTable = 'Data Not Found.';


//use >> sysEncrypt(empid, function (data) { sysDecrypt(data, function (data) { alert(data); }); });
function sysEncrypt(str, funcSuccess) {
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: AshxSysFunc.FuncEncrypt, param1: str },
        success: function (response) {
            funcSuccess(response);
        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly

        }
    });
}

function sysEncryptForJava(str, funcSuccess) {
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: AshxSysFunc.FuncEncodeForJavaDecode, param1: str },
        success: function (response) {
            funcSuccess(response);
        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly

        }
    });
}

//Use in java only
function sysDecrypt(str, funcSuccess) {
    $.ajax({
        dataType: "html",
        type: AjaxCall.type,
        url: AshxSysFunc.url,
        data: { funcName: AshxSysFunc.FuncDecrypt, param1: str },
        success: function (response) {
            funcSuccess(response);
        },
        error: AjaxCall.error,
        complete: function (jqXHR, status) {//finaly
        }
    });
}

/*==== เซทค่า datatable====*/
function SetColDatatable() {
    $('div[class*=dataTables_length]').addClass("col-md-6");
    $('div[class*=dataTables_info]').addClass("col-md-6");
    $('div[class*=dataTables_paginate]').addClass("col-md-6 text-right");
    return "s";
    //$('div[class*=dataTables_paginate]').addClass("text-right");
}

function SetICheck() {
    //$('input').iCheck('destroy');
    //iCheck for checkbox and radio inputs
    $('.minimal input[type="checkbox"],.minimal input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
    $('.minimal-red input[type="checkbox"],.minimal-red input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });
    $('.flat-red input[type="checkbox"],.flat-red input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });
    $('.minimal-green input[type="checkbox"],.minimal-green input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_minimal-green',
        radioClass: 'iradio_minimal-green'
    });
    $('.flat-green input[type="checkbox"],.flat-green input[type="radio"]').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });

    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal,input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
    $('input[type="checkbox"].minimal-red,input[type="radio"].minimal-red').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });
    $('input[type="checkbox"].flat-red,input[type="radio"].flat-red').iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });
    $('input[type="checkbox"].minimal-green,input[type="radio"].minimal-green').iCheck({
        checkboxClass: 'icheckbox_minimal-green',
        radioClass: 'iradio_minimal-green'
    });
    $('input[type="checkbox"].flat-green,input[type="radio"].flat-green').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });
}
function SetICheckCheckBox() {
    //$('input').iCheck('destroy');
    //iCheck for checkbox and radio inputs
    $('.minimal input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
    $('.minimal-red input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });
    $('.flat-red input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });
    $('.minimal-green input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_minimal-green',
        radioClass: 'iradio_minimal-green'
    });
    $('.flat-green input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });

    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });
    $('input[type="checkbox"].minimal-red').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });
    $('input[type="checkbox"].flat-red').iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });
    $('input[type="checkbox"].minimal-green').iCheck({
        checkboxClass: 'icheckbox_minimal-green',
        radioClass: 'iradio_minimal-green'
    });
    $('input[type="checkbox"].flat-green').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });
}
//tooltip
function SetTootip() {
    //$('[data-toggle="tooltip"],[title]').tooltip();
}

//set mask format textbox
function SetInputMask() {
    $("[data-mask]").inputmask();
}

//integerDigits : จำนวนหลักของของค่าจำนวนเต็ม,digits : จำนวนหลักของค่าทศนิยม
function InputMaskDecimal(objCtrl, integerDigits, digits, allowPlus, allowMinus) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: digits, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        allowPlus: Boolean(allowPlus), //อนุญาตใส่เครื่องหมายบวก
        allowMinus: Boolean(allowMinus), //อนุญาตใส่เครื่องหมายลบ,
        rightAlign: false,
    });
}

function InputMaskIntegerNotMoney(objCtrl, integerDigits) {
    //Inputmask
    $(objCtrl).inputmask("decimal", {
        integerDigits: integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        allowPlus: false, //อนุญาตใส่เครื่องหมายบวก
        allowMinus: false, //อนุญาตใส่เครื่องหมายลบ,    
        rightAlign: false
    });
}

function SetCheckBoxSelectRowInGrid(tableID, ckbHeadID, ckbListID, fMode) {
    $("input[id$=" + ckbHeadID + "]").on("ifClicked", function (event) {
        //console.log('-->SetCheckBoxSelectRowInGrid()');
        checkboxSelectOrUnSelectAll(tableID, ckbListID, event);
    });
    $("div[id$=" + tableID + "_wrapper] div.icheckbox_flat-green input[id*=" + ckbListID + "]").on("ifClicked", function (event) {
        checkUnCheckHeadCheckbox(tableID, ckbHeadID, ckbListID, event, fMode);
        var sCheckValue = event.target.checked;
        var sID = $(this)[0].id;
        if (!sCheckValue) {
            $('input[id=' + sID + ']').iCheck('check');
        } else {
            $('input[id=' + sID + ']').iCheck('uncheck');
        }
    });

    //$("table[id$=" + tableID + "] div.icheckbox_flat-green input[id$=" + ckbHeadID + "]").on("ifClicked", function (event) { checkboxSelectOrUnSelectAll(tableID, ckbListID, event); });
    //$("table[id$=" + tableID + "] div.icheckbox_flat-green input[id*=" + ckbListID + "]").on("ifClicked", function (event) { checkUnCheckHeadCheckbox(tableID, ckbHeadID, ckbListID, event); });
}

function checkboxSelectOrUnSelectAll(containID, elementBodyID, event) {
    //debugger;
    if (!event.currentTarget.checked) {
        $("div[id$=" + containID + "_wrapper] div.icheckbox_flat-green input[id*=" + elementBodyID + "]").iCheck('check');
    }
    else {
        $("div[id$=" + containID + "_wrapper] div.icheckbox_flat-green input[id*=" + elementBodyID + "]").iCheck('uncheck');
    }
}

function checkUnCheckHeadCheckbox(containID, elementHeadID, elementBodyID, event, fMode) {

    var nLengthAll = 0
    if (fMode != undefined && fMode == "fix") {
        nLengthAll = $("div[id$=" + containID + "_wrapper] div.DTFC_LeftWrapper div.icheckbox_flat-green input[id*=" + elementBodyID + "]").length;

    }
    else {
        nLengthAll = $("div[id$=" + containID + "_wrapper] div.icheckbox_flat-green input[id*=" + elementBodyID + "]").length;
    }
    var nLengthCheck = 0
    if (fMode != undefined && fMode == "fix") {
        nLengthCheck = $("div[id$=" + containID + "_wrapper] div.DTFC_LeftWrapper div.icheckbox_flat-green[aria-checked=true] input[id*=" + elementBodyID + "]").length;

    }
    else {
        nLengthCheck = $("div[id$=" + containID + "_wrapper] div.icheckbox_flat-green[aria-checked=true] input[id*=" + elementBodyID + "]").length;
    }
    //var nLengthCheck =
    if (!event.currentTarget.checked) {//event ifClicked value checked is berfor click
        nLengthCheck = nLengthCheck + 1;
    }
    else {
        nLengthCheck = nLengthCheck - 1;
    }

    if (nLengthAll == 0 || nLengthAll != nLengthCheck) {
        $("input[id$=" + elementHeadID + "]").iCheck('uncheck');
    }
    else {
        $("input[id$=" + elementHeadID + "]").iCheck('check');
    }
}

function UnCheckCheckBoxHeader(elementHeadID) {
    $("input[id$=" + elementHeadID + "]").iCheck('uncheck');
}

function CheckDeleteDataInGrid(containID, elementBodyID, funcYes, funcNo) {
    var nLengthCheck = $("table[id$=" + containID + "] div.icheckbox_flat-green[aria-checked=true] input[id*=" + elementBodyID + "]").length;
    if (nLengthCheck == 0) {
        DialogWarning(DialogHeader.Warning, "กรุณาเลือกข้อมูลที่ต้องการลบ");
    }
    else {
        DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmDel, funcYes, funcNo);
    }
}

function CheckApproveDataInGrid(containID, elementBodyID, funcYes, funcNo) {
    var nLengthCheck = $("table[id$=" + containID + "] div.icheckbox_flat-green[aria-checked=true] input[id*=" + elementBodyID + "]").length;
    if (nLengthCheck == 0) {
        DialogWarning(DialogHeader.Warning, "กรุณาเลือกข้อมูลที่ต้องการอนุมัติ");
    }
    else {
        DialogConfirm(DialogHeader.Confirm, DialogMsg.ConfirmApprove, funcYes, funcNo);
    }
}

/*********** Jquery Datatable  ***********/
var DataLengthMenu = [10, 20, 30, 50, 100];
var objJSTableDefaul = {
    retrieve: true,
    bPaginate: true,
    bLengthChange: true,
    aLengthMenu: DataLengthMenu,
    bFilter: false,
    bSort: true,
    bInfo: true,
    bAutoWidth: false,
    bProcessing: true,
    sDom: '<r>t<"cSet1"<"cSet2"l><"cSet2"i><"cSet2"p>>',
    sDomBtnDel: '<r>t<"cSet1"<"cBtnDel"><"cSet2"l><"cSet2"i><"cSet2"p>>',
    oLanguage: {
        "sInfoEmpty": "",
        "sEmptyTable": "No data.",
        "sInfo": "Showing _START_ to _END_ (_TOTAL_ item(s))",
        "sLengthMenu": "List _MENU_ items",
        //"sProcessing": "",
    }
    /*
    "bJQueryUI": true,
    "sPaginationType": "full_numbers",
    "aaSorting": [[1, "asc"]],//เนื่องจาก ไม่สามาถปิดการ sort index 0 ได้หากไม่ได้กำหนด
    "aoColumnDefs": [
        { 'bSortable': false, 'aTargets': [0] }
    ]*/
    /*"fnInitComplete": function (oSettings, json) {
                            HideLoadding();
                        },
                        /*"fnPreDrawCallback": function () {
                            LoaddinProcess();
                        },
                        "fnDrawCallback": function () {
                            HideLoadding();
                        }*/
};

function SetCommonPropertyTable(obj) {
    obj.bPaginate = objJSTableDefaul.bPaginate;
    obj.bLengthChange = objJSTableDefaul.bLengthChange;
    obj.aLengthMenu = objJSTableDefaul.aLengthMenu;
    $(obj).bFilter = objJSTableDefaul.bFilter;
    $(obj).bSort = objJSTableDefaul.bSort;
    $(obj).bInfo = objJSTableDefaul.bInfo;
    $(obj).bAutoWidth = objJSTableDefaul.bAutoWidth;
    $(obj).bProcessing = objJSTableDefaul.bProcessing;
    obj.sDom = objJSTableDefaul.sDom;
    obj.oLanguage = objJSTableDefaul.oLanguage;
    /*
    "bPaginate": objJSTableDefaul.bPaginate,
                "bLengthChange": objJSTableDefaul.bLengthChange,
                "": objJSTableDefaul.aLengthMenu,
                "": objJSTableDefaul.bFilter,
                "": objJSTableDefaul.bSort,
                "": objJSTableDefaul.bInfo,
                "": objJSTableDefaul.bAutoWidth,
                "": objJSTableDefaul.bProcessing,
                "": objJSTableDefaul.sDom,
                "": objJSTableDefaul.oLanguage
    */
}
/*********** End Jquery Datatable  ***********/


var AjaxCall = {
    dataType: "json",
    type: "POST",
    contentType: "application/json; charset=utf-8",
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
    },
    complete: function (jqXHR, status) {//finaly
    }
};

/************Get Data *************/
function GetValTextBox(txtID) {
    //return reEscape($("input[id$=" + txtID + "]").val());

    return $("input[id$=" + txtID + "]").val();
}
function GetValDropdown(ddlID) {
    return $("select[id$=" + ddlID + "]").val();
}
function GetTextDropdownText(ddlID) {
    return $("select[id$=" + ddlID + "] option:selected").text();
}
function GetTextRadio(rdlID) {
    return $('input[id$=' + rdlID + ']').parent()[0].innerText
}

function GetValRadioListICheck(rblID) {
    return $("input[id*=" + rblID + "]:checked").val();
    //$("input[id*=rdlChanel][value=E]").iCheck('check');
}
function GetValCheckBox(cbID) {
    return $("input[id*=" + cbID + "]").is(':checked');
}
function GetValTextArea(txtID) {
    return reEscape($("textarea[id$=" + txtID + "]").val());
}
function GetValueAttrValue(txtID) {
    return $("input[id$=" + txtID + "]").attr("value");
}

function GetValRadioListNotValidate(name) {
    var sVal = $('input[name=' + name + ']:checked').val();
    if (typeof sVal !== "undefined") {
        return sVal;
    }
    else {
        return "";
    }
}

function GetValueChkGroupSingelValue(sGroup) {

    var values = ""; if (sGroup != undefined && sGroup != "") {
        $.each($("input:checkbox[name='" + sGroup + "']:checked"), function () {
            // values.push($(this).val());
            values = $(this).val();
            // or you can do something to the actual checked checkboxes by working directly with  'this'
            // something like $(this).hide() (only something useful, probably) :P
        });
    }
    return values;
}
function GetValueChkGroup(sGroup) {

    var values = []; if (sGroup != undefined && sGroup != "") {
        $.each($("input:checkbox[name='" + sGroup + "']:checked"), function () {
            values.push($(this).val());
            // values = $(this).val();
            // or you can do something to the actual checked checkboxes by working directly with  'this'
            // something like $(this).hide() (only something useful, probably) :P
        });
    }
    return values;
}

function reEscape(s) {
    return s.replace(/<(?:.|\n)*?>/gm, '');
}
/*********** Dialog  ***********/
var DialogHeader = {
    Info: "Process Result",
    Error: "An error occur, please contact administrator",
    Confirm: "Process Confirmation",
    Warning: "Process Result"
}

var DialogMsg = {
    ConfirmDel: "Do you want to delete?",
    ConfirmApprove: "Do you want to approve?",
    ConfirmSave: "Do you want to submit?",
    AlertDel: "Please select item you want to delete.",
    SaveComplete: "Saving Completed",
    DelComplete: "Delete Completed",
    ApproveComplete: "Approve Completed",
    Login: "กรุณา Login เข้าใช้งานระบบ",
    Duplicate: "Cannot save duplicate data.",
    SaveFail: "Cannot save data.",
}

var btnOKText = "OK";
var btnCancelText = "Cancel";

function DialogInfo(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_INFO,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-info',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}

function DialogInfoRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_INFO,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-info',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogError(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_DANGER,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-danger',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}
function DialogErrorCallback(head, msg, funcCallback) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_DANGER,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-danger',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                funcCallback();
            }
        }]
    });
}

function DialogErrorRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_DANGER,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-danger',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogWarning(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-warning',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}

function DialogWarningRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        buttons: [{
            id: 'btn-ok',
            label: btnOKText,
            cssClass: 'btn btn-warning',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogConfirm(head, msg, funcYes, funcNo) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_PRIMARY,
        closable: true,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            //icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-primary',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                //LoaddinProcess();
                funcYes();
            }
        },
        {
            id: 'btn-cancel',
            //icon: 'glyphicon glyphicon-remove',
            label: btnCancelText,
            cssClass: 'btn btn-default',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                if (funcNo != null && funcNo != undefined && funcNo != "") {
                    funcNo();
                }
            }
        }
        ]
    });
}
function DialogWarningConfirm(head, msg, funcYes, funcNo) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        closable: true,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            //icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-warning',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                //LoaddinProcess();
                funcYes(dialogRef);
            }
        },
        {
            id: 'btn-cancel',
            //icon: 'glyphicon glyphicon-remove',
            label: btnCancelText,
            cssClass: 'btn btn-default',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                if (funcNo != null && funcNo != undefined && funcNo != "") {
                    funcNo(dialogRef);
                }
            }
        }
        ]
    });
}
function DialogSuccess(head, msg) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_SUCCESS,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-success',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}
function DialogSuccessCallback(head, msg, funcCallback) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_SUCCESS,
        closable: false,
        draggable: true,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-success',
            autospin: false,
            action: function (dialogRef) {
                dialogRef.close();
                funcCallback();
            }
        }]
    });
}
function DialogSuccessRedirect(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_SUCCESS,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-success',
            autospin: false,
            action: function (dialogRef) {
                window.location = redirto;
            }
        }]
    });
}

function DialogAlertLogin(head, msg, redirto) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_WARNING,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-warning',
            autospin: false,
            action: function (dialogRef) {
                if (redirto != undefined) {
                    window.location = redirto;
                }
                else {
                    location.reload()
                }
            }
        }]
    });
}

function DialogSuccessOk(head, msg, redirto, func) {
    BootstrapDialog.show({
        title: head,
        message: msg,
        type: BootstrapDialog.TYPE_SUCCESS,
        closable: false,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            id: 'btn-ok',
            icon: 'glyphicon glyphicon-check',
            label: btnOKText,
            cssClass: 'btn btn-success',
            autospin: false,
            action: function (dialogRef) {
                if (redirto == undefined || redirto == "") {
                    dialogRef.close();
                }
                else if (redirto == "true") {
                    window.location.reload();
                }
                else if (redirto == "func") {
                    if (func != undefined && func != "") {
                        func();
                    }
                }
                else {
                    window.location = redirto;
                }
                dialogRef.close();
            }
        }]
    });
}
/***********End Dialog  ***********/

/*********** Form Validation ************/

function BindValidate(sContainer, objValidate) {
    $("#" + sContainer).formValidation({
        framework: 'bootstrap',
        excluded: ':disabled',
        err: {
            //container: 'tooltip'
        },
        //icon: {
        //    valid: 'glyphicon glyphicon-ok',
        //    invalid: 'glyphicon glyphicon-remove',
        //    validating: 'glyphicon glyphicon-refresh'
        //},
        fields: objValidate,
        autoFocus: true,
    }).on('err.validator.fv', function (e, data) {
        data.element
            .data('fv.messages')
            // Hide all the messages
            .find('.help-block[data-fv-for="' + data.field + '"]').hide()
            // Show only message associated with current validator
            .filter('[data-fv-validator="' + data.validator + '"]').show();
    })
        .on('change', '[name="countrySelectBox"]', function (e) {
            $('#idForm').formValidation('revalidateField', 'idNumber');
        });
}

function BindValidateBootstrap(sContainer, objValidate, objICheck) {
    var validateF = $("#" + sContainer).bootstrapValidator({
        err: {
            //container: 'tooltip'
        },
        //feedbackIcons: {
        //    valid: 'glyphicon glyphicon-ok',
        //    invalid: 'glyphicon glyphicon-remove',
        //    validating: 'glyphicon glyphicon-refresh'
        //},
        fields: objValidate,
        autoFocus: true,
    }).on('err.validator.fv', function (e, data) {

        data.element
            .data('fv.messages')
            // Hide all the messages
            .find('.help-block[data-fv-for="' + data.field + '"]').hide()
            // Show only message associated with current validator
            .filter('[data-fv-validator="' + data.validator + '"]').show();
    });
    if (objICheck != undefined) {
        validateF.find(objICheck)
            // Init iCheck elements

            // Called when the radios/checkboxes are changed
            .on('ifChanged', function (e) {

                // Get the field name
                var field = $(this).attr('name');
                $('#' + sContainer).bootstrapValidator('revalidateField', field);
            });
    }

}

function CheckValidate(sContainer) {
    return $("#" + sContainer).data('formValidation').validate().isValid();
}
function CheckValidateBootstrap(sContainer) {

    return $("#" + sContainer).data('bootstrapValidator').validate().isValid();
}
function FocusValidate(sContainer) {
    return $("#" + sContainer).data('formValidation').validate().getInvalidFields()[0].focus();
}
function AlertValidate(sContainer) {
    var sText = $($("#" + sContainer).data('formValidation').validate().getInvalidFields()[0]).parents('.form-group').find('label').text().replace(" ", "").replace(":", "");
    if (sText != undefined && sText != "") {
        DialogWarning(DialogHeader.Warning, "กรุณากรอกข้อมูล : " + sText);
        return;
    }
}
function FocusValidateBootstrap(sContainer) {
    return $("#" + sContainer).data('bootstrapValidator').validate().getInvalidFields()[0].focus();
}
function AlertValidateBootstrap(sContainer) {

    var sText = $($("#" + sContainer).data('bootstrapValidator').validate().getInvalidFields()[0]).parents('.form-group').find('label').text().replace(" ", "").replace(":", "");
    if (sText != undefined && sText != "") {
        DialogWarning(DialogHeader.Warning, "กรุณากรอกข้อมูล : " + sText);
        return;
    }
}
function addValidateEmail_notEmpty() {//กรณีที่มีการกำหนด data-inputmask เนื่องจากไม่สามารถใช้ notEmpty ได้
    return {
        validators: {
            regexp: {
                regexp: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
                message: "Your email address is invalid. Example abc@mail.com"
            },

            callback: {
                message: "กรุณาระบุ อีเมล์",
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }

    };
}

function addValidate_Email() {
    return {
        validators: {
            regexp: {
                regexp: "^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$",
                message: "Your email address is invalid. Example abc@mail.com"
            }
        }
    };
}




function setValidate(thisInput, isError) {
    if (isError) {
        thisInput.parent().find('small').css('display', 'block');
        thisInput.parent().find('i').css('display', 'block');
        thisInput.parent().parent().removeClass().addClass('form-group has-feedback has-error');
    }
    else {
        thisInput.parent().parent().removeClass().addClass('form-group has-feedback');
        thisInput.parent().find('i').css('display', 'none');
        thisInput.parent().find('small').css('display', 'none');
    }
}

//กรณีที่มีการกำหนด data-inputmask เนื่องจากไม่สามารถใช้ notEmpty ได้
function addValidate_CustomNotEmpty(msg) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function addValidate_rblICheckNotEmpty(msg) {
    return {
        validators: {
            choice: {
                min: 1,
                message: msg
            }
        }
    };
}

function addValidate_TextArea(msg, minLength, maxLength) {
    return {
        validators: {
            stringLength: {
                min: minLength,
                max: maxLength,
                message: msg
            }
        }
    };
}

function addValidate_TextAreaMin(msg, minLength) {
    return {
        validators: {
            stringLength: {
                min: minLength,
                message: msg
            }
        }
    };
}

function addValidate_TextAreaMax(msg, minLength, maxLength) {
    return {
        validators: {
            stringLength: {
                max: maxLength,
                message: msg
            }
        }
    };
}

function addValidate_notEmpty(msg) {
    return {
        validators: {
            notEmpty: {
                message: msg
            }
        }
    };
}




function addValidateMask_notEmpty(msg) {
    return {
        validators: {
            callback: {
                message: msg,
                callback: function (value, validator, $field) {
                    alert(value);
                    return !(value + "" == "" || value == null || value == undefined);
                }
            }
        }
    };
}

function addValidate_ConfirmPassword(msgEmpty, CompareCtrlID, msgInvalidPassword) {
    return {
        validators: {
            notEmpty: {
                message: msgEmpty
            },
            callback: {
                message: msgInvalidPassword,
                callback: function (value, validator, $field) {
                    var sValCheck = $("input[id$=" + CompareCtrlID + "]").val();
                    return (value == sValCheck);
                }
            }
        }
    };
}

function addValidate_DateNotEmpty(msgEmpty, msgFormat) {
    return {
        validators: {
            notEmpty: {
                message: msgEmpty
            },
            date: {
                format: 'DD/MM/YYYY',
                message: msgFormat == "" ? 'The date is not a valid' : msgFormat
            }
        }
    }
}

function addValidate_BirthDate(msgEmpty, msgFormat) {
    return {
        validators: {
            

            date: {
                format: 'DD/MM/YYYY',
                message: msgFormat == "" ? 'The date is not a valid' : msgFormat
            },

        }
    }
}



function GetElementName(sElement, objType) {
    return $(objType + "[id$=" + sElement + "]").attr("name");
}

function GetElementNameICheck(sElement) {
    return $("input[name$=" + sElement + "").attr("name");
}

function GetElementID(sElement, objType) {
    return $(objType + "[id$=" + sElement + "]").attr("id");
}

var objControl = {
    txtbox: "input",
    txtarea: "textarea",
    dropdown: "select",
    div: "div",
    span: "span",
    rblICheck: "input",
    btn: "input"
};

/***********End Form Validation ************/

//Check Input
function NumberOnly(e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}

function FLOAT_TextBox_Init($txt, allowZero, allowNegative, _integerDigits, _digits) {
    var isAllowZero = Boolean(allowZero); //default is FALSE
    var isAllowNegative = Boolean(allowNegative); //default is FALSE
    $txt.inputmask('decimal', {
        integerDigits: _integerDigits, //จำนวนหลักของของค่าจำนวนเต็ม
        digits: _digits, //จำนวนหลักของค่าทศนิยม
        radixPoint: '.', //จุดทศนิยม
        groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
        autoGroup: true, //การจัดกลุ่มอัตโนมัตื
        allowPlus: false, //อนุญาตใส่เครื่องหมายบวก
        allowMinus: isAllowNegative, //อนุญาตใส่เครื่องหมายลบ
        rightAlign: true
    });
}

function ddlChangeChkOtherValidate(ddlControl, txtControl, divID) {
    var txt = $('input[id$=' + txtControl + ']');
    var arrDDL;
    if (ddlControl.value == undefined) {
        arrDDL = ddlControl.val().split("_");
    } else {
        arrDDL = ddlControl.value.split("_");
    }
    if (arrDDL.length > 0) {
        if (arrDDL[1] == 'Y') {
            txt.show();
            txt.prop('disabled', false);
            $("#" + divID + "").formValidation('revalidateField', '' + txt.attr("name") + '');
        } else {
            txt.val("");
            $("#" + divID + "").formValidation("updateStatus", GetElementName("'" + txtControl + "'", objControl.txtbox), "NOT_VALIDATED")
            txt.hide();
            txt.prop('disabled', true);
        }
    } else {
        txt.val("");
        $("#" + divID + "").formValidation("updateStatus", GetElementName("'" + txtControl + "'", objControl.txtbox), "NOT_VALIDATED")
        txt.hide();
        txt.prop('disabled', true);
    }
}

function ddlOnChangeSetValueToHiddenField(sIDddl, sIDHiddenField) {
    $("select[id$=" + sIDddl + "]").on("change", function () {
        $("input[id$=" + sIDHiddenField + "]").val($(this).val());
    });
}


function SetDatePicker($txtDate, $startDate, $endDate) {
    //var sDateDef = $('input[id$=hdfDate]').val();
    $txtDate.datepicker({
        format: 'dd-M-yyyy',
        todayHighlight: true,
        autoclose: true,
        startDate: ($startDate === undefined ? -Infinity : $startDate),
        endDate: ($endDate === undefined ? Infinity : $endDate),
    })
        .keydown(function (e) {
            //48-57 = "0-9" บนคีย์บอร์ด //96-105 = "0-9" คีย์ตัวเลข // 111 191 = "/" //8 46 = Del
            if (!(e.which >= 48 && e.which <= 57) && !(e.which >= 96 && e.which <= 105) && e.which != 111 && e.which != 191 && e.which == 8 && e.which == 46) {
                return false;
            }
            if (e.which == 8 || e.which == 46) {
                $txtDate.val('');
            }
        });
    $txtDate.next().click(function () { $txtDate.focus(); });
    $txtDate.MaxLength({
        MaxLength: 11,
        DisplayCharacterCount: false,
    });
    //if (sDateDef != undefined && sDateDef != "") {
    //    $txtDate.val(sDateDef);
    //}
}
function setDatePickerFromTo(_dateFrom, _dateTo) {
    SetDatePicker(_dateFrom);
    SetDatePicker(_dateTo);
    var dateStart = "";
    var dateEnd = "";
    _dateFrom
        .change(function () {
            var thisVal = $(this).val();
            // alert("start" + thisVal);
            if (thisVal != '') _dateTo.datepicker('setStartDate', thisVal);
            else { SetDatePicker(_dateTo.datepicker('remove')); }
        }).keydown(function (e) {
            if (e.which == 8 || e.which == 46) {
                SetDatePicker(_dateTo.datepicker('remove'));
            }
        });
    _dateTo
        .change(function () {
            var thisVal = $(this).val();
            if (thisVal != '') _dateFrom.datepicker('setEndDate', thisVal);
            else SetDatePicker(_dateFrom.datepicker('remove'));
        }).keydown(function (e) {
            if (e.which == 8 || e.which == 46) {
                SetDatePicker(_dateFrom.datepicker('remove'));
            }
        });
};


function SetDatePickerYear($txtDate, $startDate, $endDate) {
    $txtDate.datepicker({
        format: " yyyy", // Notice the Extra space at the beginning
        viewMode: "years",
        minViewMode: "years",
        autoclose: true,
        startDate: ($startDate === undefined ? -Infinity : $startDate),
        endDate: ($endDate === undefined ? Infinity : $endDate),
    })
        .keydown(function (e) {
            //48-57 = "0-9" บนคีย์บอร์ด //96-105 = "0-9" คีย์ตัวเลข // 111 191 = "/" //8 46 = Del
            if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
                return false;
            }
            if (e.which == 8 || e.which == 46) {
                $txtDate.val('');
            }
        });
    $txtDate.next().click(function () { $txtDate.focus(); });
}
function SetDatePickerNoLimit($txtDate) {
    $txtDate.watermark('-- / -- / ----')
        .datepicker({
            format: 'dd/mm/yyyy', autoclose: true
        })
        .keydown(function (e) {
            //48-57 = "0-9" บนคีย์บอร์ด //96-105 = "0-9" คีย์ตัวเลข // 111 191 = "/" //8 46 = Del
            if (!(e.which >= 48 && e.which <= 57) && !(e.which >= 96 && e.which <= 105) && e.which != 111 && e.which != 191 && e.which == 8 && e.which == 46) {
                return false;
            }
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
}
function SetDatePickerValidate($txtDate, divID) {
    $txtDate.watermark('-- / -- / ----')
        .datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
        });
    $txtDate.prev().click(function () { $txtDate.focus(); });
    $txtDate.change(function () {
        $('#' + divID + '').formValidation('revalidateField', '' + $txtDate.attr("name") + '');
    }).keydown(function (e) {
        if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
            return false;
        }
        if (e.which == 8 || e.which == 46) {
            $txtDate.val('');
            $('#' + divID + '').formValidation('revalidateField', '' + $txtDate.attr("name") + '');
        }
    });
}

//Old 7/20/2017
//function SetDatePicker($txtDate, $startDate, $endDate) {
//    //var sDateDef = $('input[id$=hdfDate]').val();
//    $txtDate.datepicker({
//        format: 'dd/mm/yyyy',
//        //orientation: "top",
//        autoclose: true,
//        startDate: ($startDate === undefined ? -Infinity : $startDate),
//        endDate: ($endDate === undefined ? Infinity : $endDate),
//    })
//        .keydown(function (e) {
//            //48-57 = "0-9" บนคีย์บอร์ด //96-105 = "0-9" คีย์ตัวเลข // 111 191 = "/" //8 46 = Del
//            if ((e.which >= 1 && e.which <= 7) || (e.which >= 9 && e.which <= 45) || (e.which >= 47 && e.which <= 255)) {
//                return false;
//            }
//            if (e.which == 8 || e.which == 46) {
//                $txtDate.val('');
//            }
//        });
//    $txtDate.next().click(function () { $txtDate.focus(); });
//    //if (sDateDef != undefined && sDateDef != "") {
//    //    $txtDate.val(sDateDef);
//    //}
//}
//function setDatePickerFromTo(_dateFrom, _dateTo) {
//    SetDatePicker(_dateFrom);
//    SetDatePicker(_dateTo);
//    _dateFrom
//        .change(function () {
//            var thisVal = $(this).val();
//            if (thisVal != '') _dateTo.datepicker('setStartDate', thisVal);
//            else { SetDatePicker(_dateTo.datepicker('remove')); }
//        }).keydown(function (e) {
//            if (e.which == 8 || e.which == 46) {
//                SetDatePicker(_dateTo.datepicker('remove'));
//            }
//        });
//    _dateTo
//        .change(function () {
//            var thisVal = $(this).val();
//            if (thisVal != '') _dateFrom.datepicker('setEndDate', thisVal);
//            else SetDatePicker(_dateFrom.datepicker('remove'));
//        }).keydown(function (e) {
//            if (e.which == 8 || e.which == 46) {
//                SetDatePicker(_dateFrom.datepicker('remove'));
//            }
//        });
//};

function ConvertStringToDate(inputFormat) {
    var from = inputFormat.split("/");
    var f = new Date(from[2], from[1] - 1, from[0]);
    var d = new Date(f);
    function pad(s) { return (s < 10) ? '0' + s : s; }
    return [d.getFullYear(), pad(d.getMonth() + 1), pad(d.getDate())].join('/');
}

/***********End Check Input ************/

//=============Start Set Menu Active (not hava link in database) ==============
function ActiveMenuWithoutDB(sMenuLevel0, sMenuLevel1, sMenuLevel2) {
    if (sMenuLevel0 != "") {
        $('#myNavbar> ul> li[id=' + sMenuLevel0 + ']').addClass('active');

        if (sMenuLevel1 != "") {
            $('#myNavbar> ul> li[id=' + sMenuLevel0 + '] > ul >li[id$=' + sMenuLevel1 + ']').addClass('active');

            if (sMenuLevel2 != "") {
                $('#myNavbar> ul> li[id=' + sMenuLevel0 + '] > ul >li[id$=' + sMenuLevel1 + ']  > ul >li[id$=' + sMenuLevel2 + ']').addClass('active');
            }
        }
    }
}
//=============End Set Menu Active (not hava link in database) ==============



/*===== Begin DateTime Control =====*/
function SetDatePicker2($txtDate, $startDate, $endDate) {

    $txtDate
        .datepicker({
            format: 'dd/mm/yyyy',
            //clearBtn: true,
            autoclose: true,
            startDate: ($startDate === undefined ? -Infinity : $startDate),
            endDate: ($endDate === undefined ? Infinity : $endDate),
        }).keydown(function (e) {
            if ($(this).is('[readonly]')) {
                if (e.which === 8 || e.which === 46) {
                    $(this).val('').change();
                    return false;
                }
            }
        });

    $txtDate.prev('.input-group-addon').click(function () { $txtDate.focus(); });
}
function SetMonthYearPicker($txtDate, $startDate, $endDate) {

    $txtDate
        .datepicker({
            format: "mm/yyyy",
            viewMode: "months",
            minViewMode: "months",
            autoclose: true,
            startDate: ($startDate === undefined ? -Infinity : $startDate),
            endDate: ($endDate === undefined ? Infinity : $endDate)
        })
        .keydown(function (e) {
            //48-57 = "0-9" บนคีย์บอร์ด //96-105 = "0-9" คีย์ตัวเลข // 111 191 = "/" //8 46 = Del
            if (!(e.which >= 48 && e.which <= 57) && !(e.which >= 96 && e.which <= 105) && e.which != 111 && e.which != 191 && e.which == 8 && e.which == 46) {
                return false;
            }
            if (e.which == 8 || e.which == 46) {
                $txtDate.val('');
            }
        });
        //.watermark('-- / ----');

    $txtDate.prev('.input-group-addon').click(function () { $txtDate.focus(); });
}
function SetTimePicker($txtTime, $minTime, $maxTime) {

    $txtTime
        .timepicker({
            'step': 60,
            'timeFormat': 'H:i',
            'minTime': ($minTime === undefined ? '00:00' : $minTime),
            'maxTime': ($maxTime === undefined ? '23:59' : $maxTime)
        }).change(function () {
            if ($(this).val() === '24:00') $(this).val('00:00');
        })
        .watermark('-- : --');

    $txtTime.prev('.input-group-addon').click(function () { $txtTime.focus(); });
}
/*===== End DateTime Control =====*/


/*===== Begin Inputmask Control =====*/
function SetTextBoxNumber($txt, sMode, allowComma, allowZero, allowNegative, nDigit, nMin, nMax) {
    var isAllowComma = IsEmpty(allowComma) ? true : Boolean(allowComma); //default is FALSE
    var isAllowZero = Boolean(allowZero); //default is FALSE
    var isAllowNegative = Boolean(allowNegative); //default is FALSE

    var nMaxinteger = 14;

    switch (sMode) {
        case "integer":
            $txt.inputmask('integer', {
                integerDigits: nMaxinteger, //จำนวนหลักของของค่าจำนวนเต็ม
                groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
                autoGroup: isAllowComma, //การจัดกลุ่มอัตโนมัตื
                allowPlus: false, //อนุญาตใส่เครื่องหมายบวก
                allowMinus: isAllowNegative, //อนุญาตใส่เครื่องหมายลบ
                min: $.isNumeric(nMin) ? parseInt(nMin) : null,
                max: $.isNumeric(nMax) ? parseInt(nMax) : null,
            });
            break;
        case "decimal":
            $txt.inputmask('decimal', {
                integerDigits: nMaxinteger, //จำนวนหลักของของค่าจำนวนเต็ม
                digits: $.isNumeric(nDigit) ? parseInt(nDigit) : "*", //จำนวนหลักของค่าทศนิยม
                radixPoint: '.', //จุดทศนิยม
                groupSeparator: ',', //สัญลักษณ์แบ่งหลัก
                autoGroup: isAllowComma, //การจัดกลุ่มอัตโนมัตื
                allowPlus: false, //อนุญาตใส่เครื่องหมายบวก
                allowMinus: isAllowNegative, //อนุญาตใส่เครื่องหมายลบ
                min: $.isNumeric(nMin) ? parseFloat(nMin) : null,
                max: $.isNumeric(nMax) ? parseFloat(nMax) : null,
            });
            break;
        case "percentage":
            $txt.inputmask('percentage', { suffix: '', placeholder: '' });
            break;
        case "currency":
            $txt.inputmask('currency', {
                prefix: '',
                min: $.isNumeric(nMin) ? parseFloat(nMin) : null,
                max: $.isNumeric(nMax) ? parseFloat(nMax) : null
            });
            break;
        case "year":
            $txt.inputmask('integer', {
                groupSeparator: ',',
                autoGroup: IsEmpty(allowComma) ? false : isAllowComma,
                allowPlus: false,
                min: $.isNumeric(nMin) ? parseInt(nMin) : 1900,
                max: $.isNumeric(nMax) ? parseInt(nMax) : ((new Date()).getFullYear() + 5)
            });
            break;
    }

    $txt
        .change(function () {
            if (!isAllowZero) {
                var sThisVal = $(this).val();
                if (sThisVal.length >= 1) {
                    var isInt1Digit = sThisVal.split('.')[0].length === 1; //มีหลักหน่วย 1 หลัก
                    if (isInt1Digit) {
                        var nVal = Math.abs(sThisVal);
                        if (nVal === 0) $(this).val('');
                    }
                }
            }
        })
        .blur(function () { $(this).change(); });
}
function GetValue_InputMask($txt) {
    return $txt.inputmask('unmaskedvalue'); //nullable string
}
/*===== End Inputmask Control =====*/


/*===== Begin bootStrap Controller =====*/
function CheckBox_LabelText(isChecked) {
    return '<i class="glyphicon glyphicon-' + (isChecked ? 'ok' : 'remove') + '"></i>';
}
function SetSwitchCheckbox($checkbox, size, onColor, offColor, onText, offText) {
    $checkbox.bootstrapSwitch({
        size: IsEmpty(size) ? 'small' : size,
        onColor: IsEmpty(onColor) ? 'success' : onColor, offColor: IsEmpty(offColor) ? 'danger' : offColor,
        onText: IsEmpty(onText) ? 'Active' : onText, offText: IsEmpty(offText) ? 'Non Active' : offText,
        labelText: CheckBox_LabelText($checkbox.IsChecked()),
        handleWidth: 70,
        labelWidth: 10,
        onSwitchChange: function (event, state) {
            $(this).bootstrapSwitch('labelText', CheckBox_LabelText(state));
        }
    });
}
/*===== End bootStrap Controller =====*/


/*===== Begin iCheck =====*/
function SetiCheck($cb) {
    $cb.iCheck({
        checkboxClass: 'icheckbox_flat-green'
    });
}
function SetiRadio($rb) {
    $rb.iCheck({
        radioClass: 'iradio_flat-green'
    });
}

$.fn.iClicked = function (fn) { $(this).bind("ifClicked", fn); };
$.fn.iChanged = function (fn) { $(this).bind("ifChanged", fn); };
$.fn.iChecked = function (fn) { $(this).bind("ifChecked", fn); };
$.fn.iUnchecked = function (fn) { $(this).bind("ifUnchecked", fn); };
$.fn.SetiChecked = function (state) { $(this).iCheck(state ? 'check' : 'uncheck'); };
$.fn.SetiDisabled = function (state) { $(this).iCheck(state ? 'disable' : 'enable'); };

/*===== End iCheck =====*/


/*===== Begin bootStrap Validation =====*/
var sExcluded = ':disabled, :hidden:not([type=hidden])';
var validatorObj_NotEmpty = { notEmpty: { message: 'This field is required' } };
function ValidateObj_SelectorNotempty(strSelector, strNotEmpty) {
    return { selector: strSelector, validators: (!IsEmpty(strNotEmpty) ? { notEmpty: { message: strNotEmpty } } : validatorObj_NotEmpty) };
}
function IsValidate($dvBody) {
    return $dvBody.data('bootstrapValidator').validate().isValid();
}
function RevalidateField($dvBody, arrfields) {
    $.each(arrfields, function (index, value) {
        $dvBody.data('bootstrapValidator').revalidateField(value);
    });
}
function UpdatevalidateField($dvBody, arrfields, isRejectExclude) {

    var isExcluded = $dvBody.data('bootstrapValidator').options.excluded !== '';
    if (isExcluded && isRejectExclude) $dvBody.data('bootstrapValidator').options.excluded = '';

    $.each(arrfields, function (index, value) {
        $dvBody.data('bootstrapValidator').updateStatus(value, 'NOT_VALIDATED', null);
    });

    if (isExcluded && isRejectExclude) $dvBody.data('bootstrapValidator').options.excluded = sExcluded;
}
function SetbootstrapValidator($dvBody, arrfields, isNotExclude) {
    $dvBody.bootstrapValidator({
        excluded: isNotExclude ? '' : sExcluded, //ยกเว้นเมื่อถูก disabled
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: arrfields
    });
}
/*===== End bootStrap Validation =====*/


/*===== jQuery Function =====*/
$.fn.IsChecked = function () { return $(this).prop('checked'); };
$.fn.SetChecked = function (state) { return $(this).prop('checked', state); };
$.fn.IsDisabled = function () { return $(this).prop('disabled'); };
$.fn.SetDisabled = function (state) { return $(this).attr('disabled', state); };
$.fn.IsVisible = function () { return $(this).css('display') != 'none'; };
$.fn.SetVisible = function (state) { return state ? $(this).show('fast') : $(this).hide('fast'); };
$.fn.SetMaxLength = function (nMaxLength) { return $(this).prop('maxlength', nMaxLength); };


function GetLabelByID(sID) {
    return $("label[id$='" + sID + "']");
}
function GetTextareaByID(sID) {
    return $("textarea[id$='" + sID + "']");
}
function GetInputByID(sID) {
    return $("input[id$='" + sID + "']");
}
function GetInputByName(sName) {
    return $("input[name$='" + sName + "']");
}
function GetSelectByID(sID) {
    return $("select[id$='" + sID + "']");
}
function GetSelectByName(sName) {
    return $("select[name$='" + sName + "']");
}
function GetButtonByID(sID) {
    return $("button[id$='" + sID + "']");
}
function GetButtonByName(sName) {
    return $("button[name$='" + sName + "']");
}
function GetHyperLinkByID(sID) {
    return $("a[id$='" + sID + "']");
}
function GetCheckboxList(sName) {
    return $("input[name*='$" + sName + "$']");
}
function GetRadio(sName) {
    return $("input[name='" + sName + "']");
}
function GetTableByID(sID) {
    return $("table[id$='" + sID + "']");
}
function GetTHeadByID(sID) {
    return $("table[id$='" + sID + "']>thead");
}
function GetTBodyByID(sID) {
    return $("table[id$='" + sID + "']>tbody");
}
function GetValueTextBox(sID) {
    return GetInputByID(sID).val();
}

function IsEmpty(str) {
    var _empty = (str === undefined || str === null || str === '' || str === 'null');
    return _empty;
}
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}
function SetFormatNumber(nNumber, nDecimal, sEmpty) {
    if ($.isNumeric(nNumber)) {
        if ($.isNumeric(nDecimal))
            return addCommas(nNumber.toFixed(nDecimal));
        else
            return addCommas(nNumber);
    }
    else {
        return IsEmpty(nNumber) ? (sEmpty === undefined ? "" : sEmpty) : nNumber;
    }
}
function SetMaxLength($ctrl, nMaxLength) {
    $ctrl.prop('maxlength', nMaxLength);
}
function SetCheckedList(cbl, lstValue) {
    var $cbl = GetCheckboxList(cbl);
    $.each($cbl, function (index, element) {
        var $cb = $(element);
        var isChecked = Enumerable.From(lstValue).Any(function (w) { return w === $cb.val(); });
        $cb.SetChecked(isChecked);
    });
}
function scrollToAnchor($element) {
    $('html,body').animate({ scrollTop: $element.offset().top - 148 }, 'slow');
}

/*==== แปลง DateTime C# to java และจัดฟอร์แมต ====*/
function ConvertDateTimeToString(_sDate) {

    if (_sDate !== null) {
        var dDate;

        var _Date = '';
        var _Time = '';

        if (_sDate.indexOf('/Date(') > -1) {
            var sDate = _sDate.replace('/Date(', '').replace(')/', '');
            dDate = new Date(parseInt(sDate));

            _Date += (dDate.getDate() < 10 ? '0' : '') + dDate.getDate() + '/';
            _Date += (dDate.getMonth() < 10 ? '0' : '') + dDate.getMonth() + '/';
            _Date += (dDate.getFullYear() < 10 ? '0' : '') + (dDate.getFullYear());


            _Time += (dDate.getHours() < 10 ? '0' : '') + dDate.getHours() + ':';
            _Time += (dDate.getMinutes() < 10 ? '0' : '') + dDate.getMinutes();
        }
        else {
            var sDate = _sDate.split('/');
            dDate = new Date(parseInt(sDate[2]) - 543, parseInt(sDate[1]) - 1, parseInt(sDate[0]));

            _Date += (dDate.getDate() < 10 ? '0' : '') + dDate.getDate() + '/';
            _Date += ((dDate.getMonth() + 1) < 10 ? '0' : '') + (dDate.getMonth() + 1) + '/';
            _Date += (dDate.getFullYear() < 10 ? '0' : '') + (dDate.getFullYear());


            _Time += (dDate.getHours() < 10 ? '0' : '') + dDate.getHours() + ':';
            _Time += (dDate.getMinutes() < 10 ? '0' : '') + dDate.getMinutes();
        }

        return { 'Date': _Date, 'Time': _Time };
    }
    else {
        return { 'Date': '-', 'Time': '-' };
    }
}
/*==== แปลง DateTime java to C#  ====*/
function ConvertStringToDateTime(textDate, textTime) {

    var dDate = null;

    var arrDate = textDate.split("/");
    var arrTime = textTime.split(":");

    if (arrDate.length === 3) {
        if (arrTime.length === 2) dDate = new Date(arrDate[2], arrDate[1], arrDate[0], arrTime[0], arrTime[1], 0, 0);
        else dDate = new Date(arrDate[2], arrDate[1], arrDate[0]);
    }

    return dDate;
}
/*==== แปลง DateTime C# to java และจัดฟอร์แมต ====*/
function ConvertDateTimeCSToDateTime(_sDate) {

    if (_sDate !== null) {

        var _Date = new Date();

        if (_sDate.indexOf('/Date(') > -1) {
            var sDate = _sDate.replace('/Date(', '').replace(')/', '');
            var dDate = new Date(parseInt(sDate));

            _Date = dDate;
        }
        else {
            _Date = _sDate;
        }

        return _Date;
    }
    else {
        return null;
    }
}

function isNumeric(nStr) {
    return $.isNumeric(nStr);
}
function ConvertToIntNullable(nStr) {
    return $.isNumeric(nStr) ? parseInt(nStr) : null;
}
function ConvertToIntNotnull(nStr) {
    return $.isNumeric(nStr) ? parseInt(nStr) : 0;
}
function ConvertToFloatNullable(nStr) {
    return $.isNumeric(nStr) ? parseFloat(nStr) : null;
}
function ConvertToFloatNotnull(nStr) {
    return $.isNumeric(nStr) ? parseFloat(nStr) : 0;
}
function ConvertBlankToNull(str) {
    return IsEmpty(str) ? null : str;
}
/*==== Set Value To Control ====*/
function SetValTextBox(txtID, sValue) {
    return $("input[id$=" + txtID + "]").val(sValue);
}

/*==== Creart Table datatable.net ====*/
function contains(a, obj) {
    var i = a.length;
    while (i--) {
        if ((a[i] + "").toLowerCase() === obj) {
            return true;
        }
    }
    return false;
}
function containsNumber(a, obj) {
    var i = a.length;
    while (i--) {
        if (a[i] === obj) {
            return true;
        }
    }
    return false;
}
function arrayContainsAnotherArray(needle, haystack) {
    for (var i = 0; i < needle.length; i++) {
        if (haystack.indexOf(needle[i]) === -1)
            return false;
    }
    return true;
}
function CreatTableJS(objOption) {
    var dTable2;

    if (objOption.objHeadCl != undefined
        && objOption.sTableID != undefined && objOption.sTableID != ""
        && objOption.objHeadCl != null && objOption.objHeadCl != "") {
        //add render text
        var bRender = true;
        if (objOption.objBeforeClRender != undefined && !objOption.objBeforeClRender == true) {
            bRender = objOption.objBeforeClRender;
        }
        var aColumnotText = ["sview", "edit", "view", "chk", "submit"]
        var aColumnDef = [];
        if (objOption.columnDefs == undefined) {
            objOption.columnDefs = [];

        }
        else {
            aColumnDef = objOption.columnDefs.map(function (a) { return a.targets; });
            if (contains(aColumnDef, "_all")) {
                bRender = false;
            }
        }

        if (objOption.objHeadClRender != undefined) {
            aColumnotText = objOption.objHeadClRender;
        }
        if (bRender) {

            $.each(objOption.objHeadCl, function (key, data) {
                if (!contains(aColumnotText, data.data.toLowerCase()) && !containsNumber(aColumnDef, key)) {
                    if (!data.hasOwnProperty("render")) {
                        objOption.objHeadCl[key]["render"] = $.fn.dataTable.render.text();
                    }
                }
            });
        }

        //Option FixHeader
        var bFixHeader = true;
        var nFixHeadder = 0;
        var bProcess = false;
        var bScorllY = false;
        var bScorllX = false;
        var srowGroup = {};
        var sfixedHeader = {
            header: false,
            footer: false,
        };
        var sfooter = null;
        var sresponsive = false;

        if (objOption.processing != undefined) { bProcess = true; }

        if (objOption.createdRow == undefined) { objOption.createdRow = function () { }; }
        if (objOption.nHeadFix != undefined && objOption.nHeadFix > 0) {
            bFixHeader = true;
            bScorllX = true;
            nFixHeadder = objOption.nHeadFix;
        }
        if (objOption.nDis != undefined && objOption.nDis == "") { nDis = []; }
        var lengthMenu = [[50, 100, 150, -1], [50, 100, 150, "All"]];
        if (objOption.lengthMN != undefined) {
            lengthMenu = objOption.lengthMN;
        }
        if (objOption.bFixHeader != undefined) {
            bFixHeader = objOption.bFixHeader;
            bScorllX = true;
        }
        if (objOption.rowGroup != undefined) {
            srowGroup = objOption.rowGroup;
        }
        if (objOption.fixedHeader != undefined) {
            sfixedHeader = objOption.fixedHeader;
        }

        if (objOption.footerCallback != undefined) {
            sfooter = objOption.footerCallback;
        }

        if (objOption.responsive != undefined) {
            sresponsive = objOption.responsive;
        }


        if (objOption.scrollY != undefined) {
            if (objOption.scrollY) {
                bScorllY = objOption.scrollY;
            }
        }


        var dTable = $('#' + objOption.sTableID).DataTable({
            "data": objOption.objDataSet != undefined ? objOption.objDataSet : [],
            "bFilter": false,
            "dom": objOption.sProperty != undefined ? objOption.sProperty : "",
            "language": {
                "emptyTable": objOption.EmptyTable,
            },
            "zeroRecords": "No matching records found",
            "scrollX": bScorllX,
            "scrollY": bScorllY,
            "scrollCollapse": bFixHeader,
            "fixedColumns": {
                "leftColumns": nFixHeadder
            },
            "fixedHeader": sfixedHeader,
            "columns": objOption.objHeadCl,
            "columnDefs": objOption.columnDefs,
            "createdRow": objOption.createdRow,
            "order": objOption.order,
            "lengthMenu": lengthMenu,
            "bRetrieve": true,
            "drawCallback": function () {

                if (objOption.drawCallback != undefined) {

                    objOption.drawCallback();
                }
                //$("table >tbody input[type=checkbox] ").iCheck({
                //    checkboxClass: 'icheckbox_flat-green',
                //    radioClass: 'iradio_flat-green'
                //});
            },
            //"fnDrawCallback": function () {
            //    //EDIT OFFICE FORM
            //    // SetICheck();
            //    UnCheckCheckBoxHeader("ckbAll");

            //    //$("div[id$=" + objOption.sTableID + "_wrapper]   input[type=checkbox]").iCheck({
            //    //    checkboxClass: 'icheckbox_flat-green',
            //    //    radioClass: 'iradio_flat-green'
            //    //});
            //    // SetCheckBoxSelectRowInGrid("gvwData", "ckbAll", "chkRow", "fix");
            "processing": bProcess,
            //"initComplete": function (settings, json) {
            //    setTimeout(function () {
            //        $('#' + objOption.sTableID).DataTable().columns.adjust().draw()
            //    }, 500);
            //}
            //},
            "rowGroup": srowGroup,
            "responsive": sresponsive,
            "bAutoWidth": true,
            "footerCallback": sfooter

        });

        //dTable.dataTable
        if (objOption.nRunningNumber != undefined && objOption.nRunningNumber[0] == "true") {
            dTable.on('order.dt search.dt', function () {
                dTable.column(objOption.nRunningNumber[1], { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
        dTable2 = dTable;
        //$('#' + objOption.sTableID + "_info").addClass("col-md-6");
        //$('#' + objOption.sTableID + "_paginate").addClass("col-md-6 text-right");
    }
    return dTable2;
};
/*==== Creart control for Table datatable.net ====*/
function CreatTextbox(sID, sMaxlength, sValue, sFixClr) {

    $("input[id$=" + sID + "]").unbind();
    var sSetValue = sValue != undefined && sValue != "" ? "value='" + sValue + "'" : "";
    var sMaxlength = sMaxlength != undefined && sMaxlength != "" ? "maxlength='" + sMaxlength + "'" : "";
    var sHtml = "<input type='text' id='" + sID + "' name='" + sID + "' class='form-control input-sm' " + sMaxlength + " " + sSetValue + " style='width:100%;'  >";


    //Set Event 
    if (sFixClr != undefined && sFixClr == true) {

    }
    else {
        $("input[id$=" + sID + "]").bind("propertychange change keyup paste input", function () {
            //alert($(this).val());
            $("input[id$=" + sID + "]").val($(this).val());
        });
    }


    return sHtml;
};

/*==== Creart control for Table datatable.net ====*/
function CreatTextboxreadonly(sID, sMaxlength, sValue, sFixClr,readonly) {

    $("input[id$=" + sID + "]").unbind();
    var sSetValue = sValue != undefined && sValue != "" ? "value='" + sValue + "'" : "";
    var sMaxlength = sMaxlength != undefined && sMaxlength != "" ? "maxlength='" + sMaxlength + "'" : "";
    var sHtml = "<input type='text' id='" + sID + "' name='" + sID + "' class='form-control input-sm' " + sMaxlength + " " + sSetValue + " style='width:100%;'  readonly='readonly'>";


  


    return sHtml;
};
function CreatDatePicker(sID, sMaxlength, sValue, sFixClr) {

    var sSetValue = sValue != undefined && sValue != "" ? "value='" + sValue + "'" : "";
    var sMaxlength = 10;//sMaxlength != undefined && sMaxlength != "" ? "maxlength='" + sMaxlength + "'" : "";
    var sHtml = "<div class='input-group form-inline' style='float: left;'>";
    sHtml += "<input type='text' id='" + sID + "' placeholder='-- / -- / ----' name='" + sID + "' class='form-control text-center input-sm' " + sMaxlength + " " + sSetValue + " style='width:100%;'  >";
    sHtml += "<span class='input-group-addon'><i class='glyphicon glyphicon-calendar'></i></span></div>";

    return sHtml;
};
function CreatSelect(sID, objJson, sSelectValue, sDisable) {
    $("select[id$=" + sID + "]").unbind();
    var sDis = "";
    if (sDisable != undefined && sDisable == "Y") {
        sDis = "disabled";
    }
    var sHtml = "<select id='" + sID + "' class='form-control input-sm' style='width:100%;' " + sDis + " >";
    if (objJson != undefined) {
        if (sSelectValue != undefined && sSelectValue != null && sSelectValue != "") {
            $.each(JSON.parse(objJson), function (i, item) {
                var optionhtml = "";
                if (item.value + "" == sSelectValue + "") {
                    optionhtml = '<option value="' +
                        item.value + '" selected="selected">' + item.text + '</option>';
                    sHtml += optionhtml;
                }
                else {
                    optionhtml = '<option value="' +
                        item.value + '">' + item.text + '</option>';
                    sHtml += optionhtml;
                }
            });
        }
        else {
            var optionhtml = "";
            $.each(JSON.parse(objJson), function (i, item) {
                optionhtml = '<option value="' +
                    item.value + '">' + item.text + '</option>';
                sHtml += optionhtml;
            });
        }
    }
    sHtml += "</select>";

    //Set Event dropdows
    $("select[id$=" + sID + "]").bind("change", function () {
        $("select[id$=" + sID + "]").val($(this).val());
    });

    return sHtml;
}
function CreatCheckbox(sID, sValue) {
    //var sSetValue = sValue != undefined && sValue != "" ? "value='" + sValue + "'" : "";
    var sSetValue = "";
    if (sValue != undefined && sValue != "") {
        if (sValue == true || sValue == "true") {
            sSetValue = "checked"
        }
    }
    var sHtml = "<input type='checkbox' id='" + sID + "' name='" + sID + "' class='checkbox flat-green no-margin' " + sSetValue + "  >";
    return sHtml;
};
function CreatButtonView(sOnClick) {
    var sHtml = "<button id='btnView' type='button' onclick='View(\"" + sOnClick + "\")'   class='btn btn-xs btn-info'><i class='glyphicon glyphicon-search'></i></button>";
    return sHtml;
};
function CreateLinkInfo(sOnClick) {
    var sHtml = "<a href='#' onclick='View(\"" + sOnClick + "\")'   class='fancybox-table btn btn-xs  btn-info'><i class='glyphicon glyphicon-exclamation-sign'></i></a>";
    return sHtml;
};

/*==== Creart add edit del for Table datatable.net ====*/
function AddItem(objList) {
    // var nCount = $('#' + objList.sTableID + '').dataTable().fnGetData().length;

    var lstJson = objList.objListItem;
    $('#' + objList.sTableID + '').dataTable().fnAddData(
        lstJson
    );
    $('#' + objList.sTableID + '').DataTable().columns.adjust().draw()
    //$('#gvwData1').dataTable().fnAddData([{
    //    "id": "xx",
    //    "country_code": "<select id='ddlEx' class='form-control'>"
    //        + "<option value='1'>1</option><option value='2'>2</option></select>",
    //    "title": "<input type='text' id='txttitle' class='form-control'>",
    //    "pubdate": "<input type='text' id='txtpubdate' class='form-control'>",
    //    "url": "http://..."
    //  , "": "<button id='btnEdit' type='button' onclick='AddItemSave(this)' class='btn btn-success'>บันทึก</button>&nbsp;<button id='btnDel' type='button'  onclick='AddItemCancel(this)' class='btn btn-warning'>ยกเลิก</button>"
    //}]);
};
function AddItemSave($btnRow, sRow, objData, sTableID) {

    var form = $($btnRow).closest('tr');
    //$('#gvwDataCar').dataTable().fnUpdate( ['a', 'b', 'c', 'd', 'e'], 1 ); // Row
    var nRowIndex = sRow;//form[0]._DT_RowIndex;

    //objData.rID = 9;
    var newRow = $('#' + sTableID.id + '').dataTable().fnFindCellRowIndexes(nRowIndex, 24);
    $('#' + sTableID.id + '').dataTable().fnUpdate(objData, newRow); // Row

};
function AddItemCancel($btnRow, sRow, sTableID) {
    var $form = $($btnRow).closest('tr');
    $('#' + sTableID.id + '').dataTable().fnDeleteRow($form);
};

function DelectItem($btnRow, sRow, sTableID) {
    var $form = $($btnRow).closest('tr');
    $('#' + sTableID.id + '').dataTable().fnDeleteRow($form);
};


/*==== Action Control ====*/

function DDLOnChangeSetvalueToControl(objDDl) {
    var aDDLID = Enumerable.From(objDDl).ToArray();
    if (aDDLID.length > 0) {
        aDDLID.forEach(function (value) {
            var $aDDLID = $('select[id$=' + value.InputID + ']');
            $aDDLID.on("change", function () {
                var sValue = $(this).val();
                var lstTarget = value.InputTarget;
                if (sValue != "") {
                    var sValueText = GetTextDropdownText(value.InputID);
                    lstTarget.forEach(function (value2) {

                        if (value2.ValueType + "" == "val") $("" + value2.InputType + "[id$='" + value2.InputID + "']").val(sValue);
                        else {
                            $("" + value2.InputType + "[id$='" + value2.InputID + "']").val(sValueText);
                        }
                    });
                }
                else {
                    lstTarget.forEach(function (value2) {

                        if (value2.ValueType + "" == "val") $("" + value2.InputType + "[id$='" + value2.InputID + "']").val('');
                        else {
                            $("" + value2.InputType + "[id$='" + value2.InputID + "']").val('');
                        }
                    });
                }
            });
        });
    }
}
/*==== Set FancyBOX ====*/
function SetFancyBox() {
    $(".fancybox").fancybox({
        openEffect: 'none',
        closeEffect: 'none',
        helpers: {
            overlay: {
                locked: true,
            }
        }
    });
}

function SetFancyBoxInTable() {
    $(".fancybox-table").fancybox({
        //autoScale: false,
        'scrolling': 'auto',
        // href : $('.fancybox').attr('id'), // don't need this
        type: 'iframe',
        padding: 0,
        //width: "100%",
        autoSize: true,
        autoScale: true,
        scrolling: true,
    });
}
/*==== Number to Money format ====*/
/*==== Ex 1 (123456789.12345).formatMoney(2, '.', ','); ====*/
/*==== Ex 2 (123456789.12345).formatMoney(2); ====*/
function StringtoCount(strNumber) {
    var nNumber = Number(ConvertToIntNotnull(strNumber));
    //$Accept.text((1000).formatMoney(2));
    return nNumber.formatMoney(0);
}
function Stringtomoney(strNumber) {
    var nNumber = Number(ConvertToIntNotnull(strNumber));
    //$Accept.text((1000).formatMoney(2));
    return nNumber.formatMoney(2);
}
function StringtomoneyDecimal(strNumber) {
    var nNumber = Number(ConvertToFloatNotnull(strNumber));
    //$Accept.text((1000).formatMoney(2));
    return nNumber.formatMoney(2);
}
Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};
function StringtoInt(strNumber) {
    var number = 0;
    if (strNumber != undefined && strNumber != null) {
        number = Number(strNumber.replace(/(?!^-)[^0-9.]/g, ""));
    }
    return number;
}
function StringtoIntNb(strNumber) {
    var nNumber = Number(ConvertToIntNotnull(strNumber));
    //$Accept.text((1000).formatMoney(2));
    return nNumber.formatMoney(0);
}


function PopupCenter(url, title, w, h, tab2) {
    // Fixes dual-screen position                         Most browsers      Firefox  
    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
    var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

    width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
    height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

    var left = ((width / 2) - (w / 2)) + dualScreenLeft;
    var top = ((height / 2) - (h / 2)) + dualScreenTop;
    if (tab2 != undefined && ConvertToIntNotnull(tab2) != 0) {
        left = left + (tab2 * 20);
        top = top + (tab2 * 20);
    }
    var newWindow = window.open(url, title, 'scrollbars=yes,status=yes,resizable=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

    // Puts focus on the newWindow  
    if (window.focus) {
        newWindow.focus();
    }
}


//delect File
function DelFileFunc(sValue) {
    var Param = {
        sSess: sValue + "",
    };
    $.ajax({
        type: "POST",
        url: UrlUploadfile() + "DelectFile",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: JSON.stringify({ ItemSearch: Param }),
        success: function (response) {

            if (response.d.Status == SysProcess.SessionExpired) {
                UnblockUI();
                PopupLogin();
            }
            else {
                var lstFile = Enumerable.From(response.d.lstData).Select(function (x) {
                    return {
                        th1: x['DelFile'] + '',
                        th2: x['Cl_RefName'] + '',
                        th3: x['sFileTypeDetail'] + '',
                        th4: x['sFileName'] + '',
                        th5: x['sFileDetail'] + '',
                        th6: x['sDateCreat'] + '',
                        th7: x['sUserCreat'] + '',
                        th8: x['OpenFile'] + '',
                    }
                }).ToArray(); // object initializer
                $('#gvwFileUpload').DataTable().clear().draw();
                if (lstFile.length > 0) {
                    $('#gvwFileUpload').dataTable().fnAddData(lstFile);
                }
                $('#gvwFileUpload').dataTable().fnDraw();
            }
        },
        complete: function (jqXHR, status) {//finaly
            UnblockUI();
        }
    });
}

function UrlUploadfile() {

    // Asmx for this page
    return "../Asmx/OPCDCM02101.asmx/";

}

function setTime() {
    $('.inpTime')
        .keydown(function (e) {
            if (e.which == 13) { //keyCode - ENTER : 13
                return false;
            }
        })
        .clockpicker({
            placement: 'bottom',
            align: 'right',
            autoclose: true,
            'default': 'now'
        })
        .change(function () {
            var thisVal = $(this).val().replace(':', '');
            var setVal = "";
            if (thisVal.length == 4) {
                setVal = thisVal.substr(0, 2) + ":" + thisVal.substr(2, 3);
                if (setVal == '24:00') $(this).val('00:00');
                var isValid = false;
                var arrTime = setVal.split(':');
                if (arrTime.length == 2) {
                    var hour = arrTime[0].trim();
                    var minute = arrTime[1].trim();
                    if (hour.length >= 1 && hour.length <= 2 && minute.length >= 1 && minute.length <= 2) {
                        if (!isNaN(hour) && !isNaN(minute)) {
                            isValid = hour >= 0 && hour < 24 && minute >= 0 && minute <= 59;
                            $(this).val(setVal);

                        }
                    }
                } else if ($(this).val('')) isValid = true;
                if (!isValid) $(this).val('');
            } else {
                $(this).val('');
            }
        }).next().click(function () {

            $(this).clockpicker('show');
        });
}

function CreateRadioOfPage(sID) {
    var sHtml = '<input type="checkbox" id="' + sID + 'chkRow" name="' + sID + '" class="flat-green" value="Y"  />';
    return sHtml;
}
function CreateRadioOfPageX(sID) {
    var sHtml = "";
    if (sID != undefined && sID != "") {
        sHtml = '<input type="checkbox" id="chkX' + sID + '-chkRow" name="chk' + sID + '" class="flat-green" value="Y"  />&nbsp;รับ&nbsp;&nbsp;';
    }
    else {
        sHtml = "";
    }


    return sHtml;
}
function CreateRadioOfPageXY(sID) {
    var sHtml = "";
    if (sID != undefined && sID != "") {
        sHtml = '<input type="checkbox" id="chkX' + sID + '-chkRow" name="chk' + sID + '" class="flat-green" value="Y"  />&nbsp;รับ&nbsp;&nbsp;' +
            '<input type="checkbox" id="chkY' + sID + '-chkRow"  name="chk' + sID + '" id="rdBlue" class="flat-green" value="N"   />&nbsp;ไม่รับ';
    }
    else {
        sHtml = "";
    }


    return sHtml;
}

function CreateRadioOfPageAB(sID) {
    var sHtml = "";
    if (sID != undefined && sID != "") {
        sHtml = '<input type="checkbox" id="chkX' + sID + '-chkRow" name="chk' + sID + '" class="flat-green" value="Y"  />&nbsp;อนุมัติ&nbsp;&nbsp;' +
            '<input type="checkbox" id="chkY' + sID + '"  name="chk' + sID + '" id="rdBlue" class="flat-green" value="N"   />&nbsp;ไม่อนุมัติ';
    }
    else {
        sHtml = "";
    }


    return sHtml;
}

function CreateRadioOfPageAB(sID) {
    var sHtml = "";
    if (sID != undefined && sID != "") {
        sHtml = '<input type="checkbox" id="chkX' + sID + '-chkRow" name="chk' + sID + '" class="flat-green" value="Y"  />&nbsp;อนุมัติ&nbsp;&nbsp;' +
            '<input type="checkbox" id="chkY' + sID + '"  name="chk' + sID + '" id="rdBlue" class="flat-green" value="N"   />&nbsp;ไม่อนุมัติ';
    }
    else {
        sHtml = "";
    }


    return sHtml;
}
function CreateSingleCheckBok(sID, sValue) {
    var sHtml = "";
    var sChecked = "";
    if (sID != undefined && sID != "") {
        if (sValue != undefined && sValue == "Y") {
            sChecked = "checked";
        }
        sHtml = '<input type="checkbox" id="' + sID + '" name="' + sID + '" class="flat-green" value="Y" ' + sChecked + '  />';
    }
    else {
        sHtml = "";
    }
    return sHtml;
}

function chkAll() {
    var sHtml = "<input type='checkbox' id='ckbAll'  class='flat-green' />";
    return sHtml;
}
function chkAllDefChecked() {
    var sHtml = "<input type='checkbox' id='ckbAll'  class='flat-green' checked='true'  value='Y' />";
    return sHtml;
}
//Set Maxlength by MaxLength.min.js

function SetMaxLeangthJS(ObjControl) {
    //  [{ ctrlID: "txtName", ctrlType: "txt,area", MaxLength: "0"},]
    if (ObjControl != undefined) {
        $.each(ObjControl, function (key, data) {
            if (data.ctrlID != undefined && data.ctrlType != undefined && data.MaxLength != undefined) {
                if (data.ctrlType != null && data.ctrlType != "") {
                    if (data.ctrlType == "txt") {
                        GetInputByID(data.ctrlID + "").MaxLength({
                            MaxLength: data.MaxLength,
                            DisplayCharacterCount: false,
                        });
                    }
                    else if (data.ctrlType == "area") {
                        GetTextareaByID(data.ctrlID + "").MaxLength({
                            MaxLength: data.MaxLength,
                            DisplayCharacterCount: false,
                        });
                    }
                }
            }
        });
        //$.each(ObjControl, function (key, data) {
        //    if (data.ctrlID != undefined && data.ctrlType != undefined && data.MaxLength != undefined) {
        //        if (data.ctrlType != null && data.ctrlType != "") {
        //            if (data.ctrlType == "txt") {
        //                $("input[id$=" + data.ctrlID + "]").MaxLength({
        //                    MaxLength: data.MaxLength,
        //                    DisplayCharacterCount: false,
        //                });
        //            }
        //            else if (data.ctrlType == "area") {
        //                $("textarea[id$=" + data.ctrlID + "]").MaxLength({
        //                    MaxLength: data.MaxLength,
        //                    DisplayCharacterCount: false,
        //                });
        //            }
        //        }
        //    }
        //});
    }

}

jQuery.fn.extend({
    renameAttr: function (name, newName, removeData) {
        var val;
        return this.each(function () {
            val = jQuery.attr(this, name);
            jQuery.attr(this, newName, val);
            jQuery.removeAttr(this, name);
            // remove original data
            if (removeData !== false) {
                jQuery.removeData(this, name.replace('data-', ''));
            }
        });
    }
});


$(window).scroll(function () {
    if ($(this).scrollTop() != 0) {
        $(".scrollToTop").addClass("fadeToTop");
        $(".scrollToTop").removeClass("fadeToBottom");
    } else {
        $(".scrollToTop").removeClass("fadeToTop");
        $(".scrollToTop").addClass("fadeToBottom");
    }
});

$(".scrollToTop").click(function () {
    $("body,html").animate({ scrollTop: 0 }, 800);
});


function GenarateKPI(sVal, sSbu, sSu, sType) {
    var sReturn = "";
    var nSbu = StringtoInt((sSbu + "").replace("%", ""));
    var nSu = StringtoInt((sSu + "").replace("%", ""));
    if (sType == "N") {
        if (nSbu > 0 || nSu > 0) {
            sReturn = "P/G : " + StringtomoneyDecimal(sVal);
            if (nSbu > 0) {
                sReturn += "<br/><hr style='margin-top: 5px !important;' />SBU : " + StringtomoneyDecimal(nSbu);
            }
            if (nSu > 0) {
                sReturn += "<br/><hr style='margin-top: 5px !important;' />SU : " + StringtomoneyDecimal(nSu);
            }
        }
        else {
            sReturn = StringtomoneyDecimal(sVal);
        }
    } else {
        if (nSbu > 0 || nSu > 0) {
            sReturn = "P/G : " + sVal;
            if (nSbu > 0) {
                sReturn += "<br/><hr style='margin-top: 5px !important;' />SBU : " + nSbu;
            }
            if (nSu > 0) {
                sReturn += "<br/><hr style='margin-top: 5px !important;' />SU : " + nSu;
            }
        }
        else {
            sReturn = sVal;
        }

    }
    return sReturn;


}