; (function (window, undefined, $) {

    $(function () {
        $("#photo").makeAsyncUploader({
            upload_url: "/Customer/AsyncUpload",
            flash_url: '/assets/fileupload/swfupload.swf',
            button_image_url: '/Content/blankButton.png',
            disableDuringUpload: 'INPUT[type="submit"]'
        });
    });
})(window, undefined, jQuery);