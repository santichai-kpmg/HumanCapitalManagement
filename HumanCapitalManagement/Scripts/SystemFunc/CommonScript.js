/// <reference path="jquery-1.11.1.min.js" />
/// <reference path="jquery.blockUI.js" />
/// <reference path="tooltip/jquery.tipsy.js" />
/// <reference path="sweetalert-common.js" />

$(document).ready(function () {
    $(".optional").append('<font color=#3276b1></font> :');
    $(".require").append('<font color=red>*</font> :');
    $(".nomallabel").append(' :');
});


$('.modal-dialog').draggable({
    handle: ".modal-header"
});

//$('.modal-content').resizable({
//    //alsoResize: ".modal-dialog",
//    minHeight: 300,
//    minWidth: 300
//});


//<div class="cssload-loader-inner">
//		<div class="cssload-cssload-loader-line-wrap-wrap">
//			<div class="cssload-loader-line-wrap"></div>
//		</div>
//		<div class="cssload-cssload-loader-line-wrap-wrap">
//			<div class="cssload-loader-line-wrap"></div>
//		</div>
//		<div class="cssload-cssload-loader-line-wrap-wrap">
//			<div class="cssload-loader-line-wrap"></div>
//		</div>
//		<div class="cssload-cssload-loader-line-wrap-wrap">
//			<div class="cssload-loader-line-wrap"></div>
//		</div>
//		<div class="cssload-cssload-loader-line-wrap-wrap">
//			<div class="cssload-loader-line-wrap"></div>
//		</div>
//	</div>


function BlockUI() {
    $.blockUI({   
  //  message: '<div class="cssload-loader-inner"><div class="cssload-cssload-loader-line-wrap-wrap"><div class="cssload-loader-line-wrap"></div></div><div class="cssload-cssload-loader-line-wrap-wrap"><div class="cssload-loader-line-wrap"></div>'
		//+'</div><div class="cssload-cssload-loader-line-wrap-wrap"><div class="cssload-loader-line-wrap"></div></div><div class="cssload-cssload-loader-line-wrap-wrap"><div class="cssload-loader-line-wrap"></div>'
		//+ '</div><div class="cssload-cssload-loader-line-wrap-wrap"><div class="cssload-loader-line-wrap"></div></div><span style="font-size:20px; bottom: 0px; position: absolute;margin-left: -35px;left: 50%; ">Loading...</span></div>',
        message: '<div class="cssload-box-loading"><span style="font-size: 20px; position: absolute;margin-left: -43px;left: 50%;z-index: 999;margin-top: 70px;">Loading...</span></div>',
        css: {
            border: 'none',
            padding: '10px',
            backgroundColor: 'none',
            //'-webkit-border-radius': '10px',
            //'-moz-border-radius': '10px',
            //opacity: .5,
            color: '#FFF',
            fontFamily: 'Arial'
        },
        baseZ: 5000
    });

    //setTimeout($.unblockUI, 50000);
} //Void
function UnblockUI() {
    $.unblockUI();
} //Void

function ScrollTo($element, onFinishScrolling) {
    $('body').animate({ scrollTop: $($element).offset().top }, 'fast', onFinishScrolling);
} //Void

function SetTipsy($ctrl) {
    var $ctrlTitle = $($ctrl).length < 1 ? $('*[title]') : $($ctrl);
    $.each($ctrlTitle, function (index, element) {
        $(element).tipsy({ gravity: $(element).attr('tipsy-gravity') || $.fn.tipsy.autoNEWS });
    });
} //Void
