
/*===== Begin ajax Function =====*/

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
var ajaxProp = {
    type: 'POST',
    webMethod: '',
    url: function (webMethodName) { return (webMethodName.indexOf('.ashx') == -1 ? location.pathname + "/" + webMethodName : webMethodName); },
    paraMeter: {},
    data: function (arrParameter) { return JSON.stringify(arrParameter); },
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    async: true,
    cache: false,
    success: function () { },
    error: function (xhr, status, err) {
        var err = eval("(" + xhr.responseText + ")");
        alert(err.Message);
        //bootboxAlert("Error", err.Message);
        UnblockUI();
    },
    complete: function () { }
};
var SetAjaxConnectWebMethod = function ($ajaxProp) {
    $.ajax({
        type: $ajaxProp.type,
        url: $ajaxProp.url($ajaxProp.webMethod),
        data: $ajaxProp.data($ajaxProp.paraMeter),
        contentType: $ajaxProp.contentType,
        dataType: $ajaxProp.dataType,
        async: $ajaxProp.async,
        cache: $ajaxProp.cache,
        success: $ajaxProp.success,
        error: $ajaxProp.error,
        complete: $ajaxProp.complete
    });
};
var fnAjaxConnect = function (webMethod, paraMeter, success, error, complete) {
    var $ajaxProp = ajaxProp;
    $ajaxProp.webMethod = webMethod;
    $ajaxProp.paraMeter = paraMeter;
    if (typeof (success) === 'function') $ajaxProp.success = success;
    if (typeof (error) === 'function') $ajaxProp.error = error;
    if (typeof (complete) === 'function') $ajaxProp.complete = complete;
    SetAjaxConnectWebMethod($ajaxProp);
};
/*===== End ajax Function =====*/