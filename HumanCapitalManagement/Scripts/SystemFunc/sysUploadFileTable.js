var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png"];

/*==== Set Upload ====*/
function SetUploadFileMulti(ObjMulti, Url) {
    // We can attach the `fileselect` event to all file inputs on the page

    if (ObjMulti.beforUpload != undefined && ObjMulti.beforUpload != "") {
        ObjMulti.beforUpload();
    }

    $(document).on('change', '#' + ObjMulti.sID + '', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });


    // We can watch for our custom `fileselect` event like this
    $(document).ready(function () {
        $('#' + ObjMulti.sID + '').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' files selected' : label;

            if (input.length) {
                input.val(log);
            } else {
                if (log) alert(log);
            }
            if (numFiles > 0) {

                if (ObjMulti.btnOK == undefined) {
                    onUploadMulti(ObjMulti, input, Url);
                }
            }
        });
        if (ObjMulti.btnOK != undefined) {
            var inputOK = $('#' + ObjMulti.btnOK + '');
            inputOK.click(
                function () {
                    var val = $('#' + ObjMulti.sID + '').val();
                    if (val == "") {
                        DialogWarning(DialogHeader.Warning, "Please attach file.");
                    }
                    else {
                        DialogConfirm(DialogHeader.Confirm, "Do you want to upload file?", function () {
                            onUploadMulti(ObjMulti, $('#' + ObjMulti.sID + '').parents('.input-group').find(':text'), Url);
                        }, "");

                    }

                }
            );
        }
    });



}

function SetFileSelect(ObjMulti) {

}

function onUploadMulti(ObjMulti, input, Url) {
    $(function () {
        // alert('onUploadMulti');
        var bCheck = true;
        var DConfim = true;
        if (ObjMulti.beforBtnOK != undefined) {
            DConfim = false;
            bCheck = ObjMulti.beforBtnOK();

        }
        if (bCheck) {
            var fileUpload = $('#' + ObjMulti.sID + '').get(0);
            var files = fileUpload.files;
            var sFile = new FormData();
            for (var i = 0; i < files.length; i++) {
                sFile.append(files[i].name, files[i]);
            }

            var sValHdf = $('input[id$=' + ObjMulti.hdf + ']').val();
            sFile.append("sSess", sValHdf);
            sFile.append("IdEncrypt", ko.toJS(viewModel.IdEncrypt));
            if (ObjMulti.txtNote != undefined && ObjMulti.txtNote != "" && GetTextareaByID("ObjMulti.txtNote") != undefined) {
                sFile.append("description", GetValTextArea(ObjMulti.txtNote));
            }

            $.ajax({
                url: Url,
                type: "POST",
                contentType: false,
                processData: false,
                data: sFile,
                success: function (result) {
                    ObjMulti.funcTable(result);
                    input.val('');
                    //var lstFile = Enumerable.From(result).Select(function (x) { return { th1: x['OpenFile'], th2: '', th3: '', th4: '', th5: '', th6: '', th7: '', } }).ToArray(); // object initializer

                    //var lstFile2 = Enumerable.From(result).ToArray();
                    //$('#gvwFileUpload').DataTable().clear().draw();
                    //$('#gvwFileUpload').dataTable().fnAddData(lstFile);
                    //$('#gvwFileUpload').dataTable().fnDraw();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }

    });
}

