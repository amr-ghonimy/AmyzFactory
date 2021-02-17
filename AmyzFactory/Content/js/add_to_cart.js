
$('.add-to-card').click(function(){

    var itemId = $(this).attr("data-catid");


    var formData = new FormData();
    var URL = '/Product/AddToCart'
    formData.append("itemid", itemId);

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


    
})