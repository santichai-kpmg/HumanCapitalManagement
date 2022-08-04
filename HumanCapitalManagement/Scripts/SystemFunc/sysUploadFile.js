
/*==== Set Upload ====*/
function SetUploadFile(sID, sIMGTarget, hdf, Url) {

    // We can attach the `fileselect` event to all file inputs on the page
    $(document).on('change', '#' + sID + '', function () {

        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });

    // We can watch for our custom `fileselect` event like this
    $(document).ready(function () {

        $('#' + sID + '').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' files selected' : label;

            if (input.length) {
                input.val(log);
                $(this).disabled = true;
            } else {
                if (log) alert(log);
            }
            onUpload(sID, sIMGTarget, hdf, Url);
        });
    });

}
function onUpload(sID, sIMGTarget, hdf, Url) {
    $(function () {

        var fileUpload = $('#' + sID + '').get(0);
        var files = fileUpload.files;
        if (!files) {
            files = handleFiles(fileUpload);
        }

        if (typeof FormData == "undefined") {
            var test = [];
            DialogWarning(DialogHeader.Warning, "การอัพโหลดไฟล์ยังไม่สามารถรองรับ IE9 !");
        }
        else {


            var sFile = new FormData();
            for (var i = 0; i < files.length; i++) {
                sFile.append(files[i].name, files[i]);
            }
            sFile.append("sIDupload", sID);
            if (hdf != undefined && hdf != "") {
                var sValHdf = $('input[id$=' + hdf + ']').val();
                sFile.append("sSess", sValHdf);
               sFile.append("IdEncrypt", ko.toJS(viewModel.IdEncrypt));
            }
            if (sIMGTarget != undefined && sIMGTarget != "") {
                sFile.append("sIDImg", sIMGTarget);
            }
            // var data = new FormData();
            $.ajax({
                url: Url,
                type: "POST",
                contentType: false,
                processData: false,
                data: sFile,
                success: function (result) {

                    var sStatusresponse = result.result.Status + "";
                    if (sStatusresponse == SysProcess.SessionExpired) {
                        PopupLogin();
                    }
                    else if (sStatusresponse == SysProcess.Success) {
                        var lstData = result.result.lstNewData;
                        if (lstData == undefined || lstData.length <= 0) {
                         
                            $('#gvwData').DataTable().clear().draw();
                            $('#gvwData').dataTable().fnDraw();
                            lstObj = [];
                            $('#gvwData').DataTable().columns.adjust().draw()
                        }
                        else {
                            //  $("div[id$=divTable]").show();
                          
                            $('#gvwData').DataTable().clear().draw();
                            $('#gvwData').dataTable().fnAddData(lstData);
                            $('#gvwData').dataTable().fnDraw();
                            lstObj = lstData;
                            $('#gvwData').DataTable().columns.adjust().draw()
                        }

                    }
                    else if (sStatusresponse == "Clear") {
                        var lstData = result.result.lstNewData;
                        if (lstData == undefined || lstData.length <= 0) {

                            $('#gvwData').DataTable().clear().draw();
                            $('#gvwData').dataTable().fnDraw();
                            lstObj = [];
                            $('#gvwData').DataTable().columns.adjust().draw()
                        }
                        else {
                            //  $("div[id$=divTable]").show();

                            $('#gvwData').DataTable().clear().draw();
                            $('#gvwData').dataTable().fnAddData(lstData);
                            $('#gvwData').dataTable().fnDraw();
                            lstObj = lstData;
                            $('#gvwData').DataTable().columns.adjust().draw()
                        }
                    }
                    else {
                        DialogWarning(DialogHeader.Warning, result.result.Msg);
                    }
                },
                error: function (err) {
                    alert(err.statusText);
                },
                complete: function (jqXHR, status) {//finaly
                    UnblockUI();
                    setTimeout(function () {
                        $('#gvwData').DataTable().columns.adjust().draw();
                    }, 500);

                }
            });
        }

    });
}

function handleFiles(input) {
    var files = input.files;
    if (!files) {
        // workaround for IE9
        files = [];
        files.push({
            name: input.value.substring(input.value.lastIndexOf("\\") + 1),
            size: 0,  // it's not possible to get file size w/o flash or so
            type: input.value.substring(input.value.lastIndexOf(".") + 1)
        });
    }

    // do whatever you need to with the `files` variable
    return files;
}

function DelectSingleFile(sValue) {
    BlockUI();

    var Param = {
        sSess: sValue + "",
    };
    $.ajax({
        type: "POST",
        url: "../Asmx/OPCDCM02101.asmx/DelectSingleFile",
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        data: JSON.stringify({ ItemSearch: Param }),
        success: function (response) {
            if (response.d.Status == SysProcess.SessionExpired) {
                UnblockUI();
                PopupLogin();
            }
            else {

                //../../Image/avatar-504.png
                var cUpload = $('#' + response.d.sIDUpload + '');
                var sIDImg = $('#' + response.d.sIDImg + '');
                var input = cUpload.parents('.input-group').find(':text');
                cUpload.disabled = false;

                if (input.length) {
                    input.val('');
                }
                var inputDel = cUpload.parents('.input-group').find('#btnDelFile');
                inputDel.hide('fast');
                inputDel.click(function () {

                });
                //alert(inputDel);
                cUpload.parent().show();
                sIDImg.attr("src", "../../Image/avatar-504.png");
                sIDImg.parent().attr('href', "../../Image/avatar-504.png");
                //ทำให้สามารถอัพไฟล์เดิมเข้าไปได้

                cUpload.replaceWith(cUpload.val('').clone(true));
            }
        },
        complete: function (jqXHR, status) {//finaly
            UnblockUI();
        }
    });
}