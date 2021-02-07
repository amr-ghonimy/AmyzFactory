function addToCart(item) {

    var itemId = $(item).attr("itemid");
    var formData = new FormData();
    var URL = '/Product/AddToCart'
    formData.append("itemId", itemId);

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