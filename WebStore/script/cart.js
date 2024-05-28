let cartProducts = [];
$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../welcome');
            return;
        }
        loadPage();
    })
});

function loadPage() {
    requestToServer('Store/GetCart', 'get', '', (data) => {
        if (data) {
            if(data.length){
                fillGrid(data, 'cartTable');
                $('#buyNow').attr('disabled', false).removeClass('disabled')
            } else {
                $('#buyNow').attr('disabled', true).addClass('disabled')
                $('#cartTable').empty().html(`<tr><td colspan="6" class="text-center fs-4">لا يوجد عناصر في العربة</td></tr>`);
            }
            cartProducts = data;
            const totalPrice = data.reduce((a, b) => a + b.amount, 0);
            $('#totalPrice').text(totalPrice + ' ليرة سورية');
        }
    });
}

getObject = (_, el, index) => {

    let htmlVal = `<tr>`
    htmlVal += `<td>${index + 1}</td>`;
    htmlVal += `<td>${el.name}</td>`;
    htmlVal += `<td>${el.quantity}</td>`;
    htmlVal += `<td>${el.price} ليرة سورية</td>`;
    htmlVal += `<td>${el.amount} ليرة سورية</td>`;
    htmlVal += `<td>`;
    htmlVal += `<button onclick="changeQuantity(\'${el.id}\')" class="btn btn-sm btn-primary">تغيير الكمية</button> `;
    htmlVal += `<button onclick="deleteItem(\'${el.id}\')" class="btn btn-sm btn-danger">حذف</button>`;
    htmlVal += `</td>`;
    htmlVal += `</tr>\n`;
    return htmlVal
}

function changeQuantity(id) {
    const currentProd = cartProducts.find(p => p.id == id);
    $('#quantity').val(currentProd.quantity);
    $('#stock').val(currentProd.availableStock);
    $('#productTitle').text(currentProd.name);
    $('#submitBtn').data('id', id);
    $('#changeQuantityModal').modal('show');
    $('form').removeClass('was-validated');
}
function updateQuantity(e) {
    e.preventDefault();
    e.stopPropagation();
    const form = e.target;
    const id = $('#submitBtn').data('id');
    if (id) {
        const quantity = parseInt($('#quantity').val());
        const stock = parseInt($('#stock').val());
        if (!quantity) {
            $('#quantityError').text('هذا الحقل مطلوب');
            $('#quantity')[0].setCustomValidity('error');
        } else if (quantity < 0) {
            $('#quantityError').text('قيمة غير صحيحة');
            $('#quantity')[0].setCustomValidity('error');
        } else if (quantity > stock) {
            $('#quantityError').text('الكمية المطلوبة غير متوفرة');
            $('#quantity')[0].setCustomValidity('error');
        } else {
            $('#quantityError').text('');
            $('#quantity')[0].setCustomValidity('');
        }

        
        if (!form.checkValidity()) {
            $('form').addClass('was-validated');
            return;
        }
        
        $('form').addClass('was-validated');
        $('#changeQuantityModal').modal('hide');

        requestToServer(`Store/ChangeQuantity?productCartId=${id}&quantity=${quantity}`, 'get', '', (data) => {
            if (data) {
                showMessage('تم تغيير الكمية بنجاح');
                loadPage();
            }
        });
    }

}

function deleteItem(id) {
    const currentProd = cartProducts.find(p => p.id == id);
    if (confirmMessage(`هل تريد حذف ${currentProd.name} من العربة؟`, () => {
        requestToServer(`Store/DeleteFromCart?productCartId=${id}`, 'get', '', (data) => {
            if (data) {
                showMessage('تمت العملية بنجاح');
                loadPage();
            }
        });
    }, () => { })) { }
}