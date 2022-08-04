
/*==== Set Upload ====*/
function SetImportCandidate(sID, sIMGTarget, hdf, Url) {

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
            onImportCandidate(sID, sIMGTarget, hdf, Url);
        });
    });

}

function onImportCandidate(sID, sIMGTarget, hdf, Url) {
    $(function () {

        var fileUpload = $('#' + sID + '').get(0);
        var files = fileUpload.files;
        if (!files) {
            files = handleFilesPRCandidate(fileUpload);
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
            if (hdf != undefined && hdf != "") {
                var sValHdf = $('input[id$=' + hdf + ']').val();
                sFile.append("sSess", sValHdf);
               sFile.append("IdEncrypt", ko.toJS(viewModel.IdEncrypt));
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
                         
                            $('#gvwDataOld').DataTable().clear().draw();
                            $('#gvwDataOld').dataTable().fnDraw();
                            lstObj = [];
                            $('#gvwDataOld').DataTable().columns.adjust().draw()
                        }
                        else {
                            //  $("div[id$=divTable]").show();
                          
                            $('#gvwDataOld').DataTable().clear().draw();
                            $('#gvwDataOld').dataTable().fnAddData(lstData);
                            $('#gvwDataOld').dataTable().fnDraw();
                            lstObj = lstData;
                            $('#gvwDataOld').DataTable().columns.adjust().draw()
                        }

                    }
                    else if (sStatusresponse == "Clear") {
                        var lstData = result.result.lstNewData;
                        if (lstData == undefined || lstData.length <= 0) {

                            $('#gvwDataOld').DataTable().clear().draw();
                            $('#gvwDataOld').dataTable().fnDraw();
                            lstObj = [];
                            $('#gvwDataOld').DataTable().columns.adjust().draw()
                        }
                        else {
                            //  $("div[id$=divTable]").show();

                            $('#gvwDataOld').DataTable().clear().draw();
                            $('#gvwDataOld').dataTable().fnAddData(lstData);
                            $('#gvwDataOld').dataTable().fnDraw();
                            lstObj = lstData;
                            $('#gvwDataOld').DataTable().columns.adjust().draw()
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
                        $('#gvwDataOld').DataTable().columns.adjust().draw();
                    }, 500);

                }
            });
        }

    });
}

function handleFilesImportCandidate(input) {
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
