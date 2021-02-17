function addToCart(itemID) {

  //  var itemId = $(this).attr("data-catid");

    debugger;

    var formData = new FormData();
    var URL = '/Product/AddToCart'
    formData.append("itemid", itemID);

    $.ajax({
        async: true,
        type: 'Post',
        contentType: false,
        processData: false,
        data: formData,
        url: URL,
        success: function (data) {
            if (data.Success) {
                $("#counterText").text(data.Counter);
            }
        },
        error: function () {
            alert("يوجد بعد المشاكل");
        }




    });

}

 