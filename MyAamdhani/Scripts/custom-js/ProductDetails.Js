var DataContent = {
    SingleElementId: {
        btn_addtocart: '[id="btn_addtocart"]',
        drp_size: '[id="drp_size"]',
        drp_qty: '[id="drp_qty"]',
    },
    _AddToCart: function () {
        var _this = this;
        var modelData = {
            ProductKey: $(_this).attr("data-productkey"),
            Quantity: $(DataContent.SingleElementId.drp_qty +" :selected").val(),
            Size: $(DataContent.SingleElementId.drp_size + " :selected").val()
    };
        debugger 
        //console.log(model);
        commonFunctions.blockPage().showLoader();
        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: { 'model': modelData },
            success: function (result) {
                commonFunctions.blockPage().hideLoader();
                if (!result.isError) {
                    swal({
                            icon: "success",
                            title: "Product Added",
                        text: result.Message,
                        }
                        );
                    setTimeout(function() { location.href = result.RedirectUrl; }, 2000);

                }
            },
            error: function (result) {
                commonFunctions.blockPage().hideLoader();
                swal({
                    icon: "error",
                    title: "Print",
                    text: "Something went wrong",
                },
                    function () {
                        location.replace(result.RedirectUrl)
                    });
            }
        });
    }
}
$(document).on('click', DataContent.SingleElementId.btn_addtocart, DataContent._AddToCart);