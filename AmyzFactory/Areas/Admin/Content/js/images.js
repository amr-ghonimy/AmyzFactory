var drawImageRaw = function (image, DeleteActionName, Controller, Container) {

    var DeletedParems = "/" + Controller + "/" + DeleteActionName + "?imageName=" + image.Title;

        var trID = Container + image.Id;


        var html = '';
        html += '<tr id="' + trID + '">'
        html += '<td style="color:#1cc88a;">' + image.Title + '</td>'
        html += '<td><a class="btn btn-info" style="margin-right: 4px;" href="' +  image.ImageUrl + '" target="_blank" >Preview</a>'
        html += '<button class="btn btn-danger"  onClick= "deleteImage(\'' + DeletedParems + '\',\'' + trID + '\')">Delete</button></td>'
        html += '</tr>'

         $('#' + Container).append(html);

}

 

var getImages = function (Action, Controller, Container, DeleteActionName) {


        var URL = "/" + Controller + "/" + Action;

         $.ajax({
            url: URL,
            type: "Get",
            dataType: "json",
            success: function (data) {


                $.each(data, function (i, slide) {
                    drawImageRaw(slide, DeleteActionName, Controller, Container)
                })

            }
        });

    }

var viewImageFile = function (inputID, descriptionID, targetImageID, imgPreviewID, imageWidth, imageHeight) {
    $(descriptionID).text('');

    var file = $(inputID).get(0).files;

    if (file && file[0]) {
        readImage(file[0], descriptionID, targetImageID, imgPreviewID, imageWidth, imageHeight, inputID);
    }
}

var readImage = function (file, descriptionID, targetImageID, imgPreviewID, imageWidth, imageHeight, inputFileID) {
        var reader = new FileReader;
        var image = new Image;

        debugger;


        reader.readAsDataURL(file);
        reader.onload = function (_file) {
            image.src = _file.target.result;
            image.onload = function () {
                var height = this.height;
                var width = this.width;
                var type = file.type;
                var size = ~~(file.size / 1024) + "KB";
                if (size > 500) {
                    failedDialog("Maximum Image Size is 500KB");
                    return false;
                }

                // Image validation
                if (imageHeight != 0) {
                    if (height === imageHeight && width === imageWidth) {
                        $(targetImageID).attr('src', _file.target.result);
                        $(descriptionID).text("Size:" + size + ", " + height + ", " + width);
                        $(imgPreviewID).show();
                    } else {
                        clearPreview(inputFileID, descriptionID, imgPreviewID);
                        failedDialog("Please Select an correct image Dimensions (" + imageWidth + "*" + imageWidth + ")");
                    }


                } else {
                    $(targetImageID).attr('src', _file.target.result);
                    $(descriptionID).text("Size:" + size + ", " + height + ", " + width);
                    $(imgPreviewID).show();
                }

            }
        }

    }

var clearPreview = function (inputFileID, descriptionID, imgPreviewID) {
	$(inputFileID).val('');
    $(descriptionID).text('');
    $(imgPreviewID).hide();

}

var startUploadImage = function (inputFileID, actionMethodName, controllerName, TblBodyID, descriptionID, imgPreviewID, DeleteActionName) {

    var URL = "/" + controllerName + "/" + actionMethodName;

    var file = $(inputFileID).get(0).files;
    var data = new FormData;
    data.append("ImageFile", file[0]);

    $.ajax({

        type: "Post",
        url: URL,
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            clearPreview(inputFileID, descriptionID, imgPreviewID);

            if (response.Result.IsSuccess) {
                // update table
                successDialog(response.Result.Message);

                drawImageRaw(response, DeleteActionName, controllerName, TblBodyID)


            } else {
                failedDialog(response.Result.Message);
            }

        }

    });
}

var deleteImage = function (URL, rowID) {

    $.ajax({
        type: "Post",
        url: URL,
        success: function (response) {

            if (response.IsSuccess) {
                successDialog(response.Message);
                $('#' + rowID).remove();
            } else {
                dangerDialog(response.Message);
            }

        }

    });

}

