
let productId;
$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../welcome');
            return;
        }
        const params = new URLSearchParams(window.location.search);
        productId = params.get('id');
        if (!productId) {
            showMessage('لم يتم تحديد المنتج');
            return;
        }
        requestToServer(`Store/GetProduct?productId=${productId}`, 'get', '', (data) => {
            fillObjectData(data);
        });
    })

});

function fillObjectData(el) {
    if (el) {
        document.title = "المنتجات - " + el.name;
        $('#product-name').text(el.name);
        $('#product-description').text(el.description);
        $('#product-seller').text(el.seller);
        $('#product-warranty').text(el.warranty);
        $('#product-price').text(el.price);
        $('#product-quantity').attr('max', el.availableStock);
        $('#product-availableStock').text(el.availableStock);
        $('#product-purchaseCount').text(el.purchaseCount);
        $('#product-image').attr('alt', el.name);
        GetImage(el.image, 'product-image');
    }
}

function addToCart() {
    if (!productId) {
        showMessage('لم يتم تحديد المنتج');
        return;
    }
    const quantity = $('#product-quantity').val();
    if (!quantity) {
        showMessage('الرجاء تحديد الكمية');
        return;
    }
    requestToServer('Store/AddToCart?productId=' + productId + '&quantity=' + quantity, 'get', '', (data) => {
        if (data) {
            requestToServer(`Store/GetProduct?productId=${productId}`, 'get', '', (data) => {
                fillObjectData(data);
            });
            showMessage('تمت العملية بنجاح');
        }
    })
}