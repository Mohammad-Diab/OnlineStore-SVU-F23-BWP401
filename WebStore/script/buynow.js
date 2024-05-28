let cartProducts = [];
$(document).ready(function () {
    updatePaymentMethod('cash');
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../welcome');
            return;
        }
        requestToServer('Store/GetCart', 'get', '', (data) => {
            if (data) {
                if (data.length) {
                    fillGrid(data, 'cartTable');
                    let grid = $(`#cartTable`);
                    const totalPrice = data.reduce((a, b) => a + parseInt(b.amount), 5000);
                    let totalHtml = `<tr>`;
                    totalHtml += `<td>رسوم التوصيل</td>`;
                    totalHtml += `<td></td>`;
                    totalHtml += `<td class="text-end">5000 ليرة سورية</td>`;
                    totalHtml += `</tr>\n`;
                    totalHtml += `<tr>`;
                    totalHtml += `<td></td>`;
                    totalHtml += `<td class="fw-bold">السعر الاجمالي:</td>`;
                    totalHtml += `<td class="fw-bold text-end">${totalPrice} ليرة سورية</td>`;
                    totalHtml += `</tr>\n`;
                    grid.html(grid.html() + totalHtml)
                    $('#buyNow').attr('disabled', false).removeClass('disabled')
                } else {
                    $('#buyNow').attr('disabled', true).addClass('disabled')
                    $('#cartTable').empty().html(`<tr><td colspan="6" class="text-center fs-4">لا يوجد عناصر في العربة</td></tr>`);
                }
                cartProducts = data;
            }
            getCards()
        });
    });
});

function getCards() {
    requestToServer('Store/GetCards', 'get', '', (data) => {
        if (data) {
            let selectHtml = `<option value="" selected>اختر بطاقة</option>`;
            let tableHtml = '';
            if (data?.length) {
                for (let i = 0; i < data.length; i++) {
                    selectHtml += `<option value="${data[i].id}">البطاقة التي تنتهي بالرقم ${data[i].lastFourDigits}</option>`;
                    tableHtml += `<tr>`;
                    tableHtml += `<td>${i + 1}</td>`;
                    tableHtml += `<td>**** **** **** ${data[i].lastFourDigits}</td>`;
                    tableHtml += `<td>${data[i].holderName}</td>`;
                    tableHtml += `<td>${data[i].expiryDate}</td>`;
                    tableHtml += `<td><button class="btn btn-danger btn-sm" onclick="deleteCard('${data[i].id}')">حذف</button></td>`;
                    tableHtml += `</tr>\n`;
                }
            } else {
                tableHtml = '<tr><td colspan="5" class="text-center">لا يوجد بطاقات</td></tr>\n';
            }
            $('#cards').empty().append(selectHtml).val('');
            $('#cardsTable').empty().append(tableHtml);
        }
    });
}

getObject = (_, el, index) => {

    let htmlVal = `<tr>`;
    htmlVal += `<td>${el.name}</td>`;
    htmlVal += `<td class="text-center">×${el.quantity}</td>`;
    htmlVal += `<td class="text-end">${el.amount} ليرة سورية</td>`;
    htmlVal += `</tr>\n`;
    return htmlVal
}

function updatePaymentMethod(type) {
    $('#card-div select, #card-div button').attr('disabled', type === 'cash');
}

function buyNow(e) {
    e.preventDefault();
    e.stopPropagation();
    const form = e.target;
    $(form).removeClass('was-validated');

    const payMethod = $('input[name="paymentMethod"]:checked').val();
    const cardId = payMethod === 'card' ? $('#cards').val() : '';
    const reciverName = $('#reciverName').val();
    const phone = $('#phone').val();
    const email = $('#email').val();
    const address = $('#address').val();
    const instructions = $('#instructions').val();

    if (payMethod === 'cash' || cardId) {
        $('#cards')[0].setCustomValidity('');
    } else {
        $('#cards')[0].setCustomValidity('error');
    }

    if (reciverName) {
        $('#cardNumber')[0].setCustomValidity('');
    } else {
        $('#cardNumber')[0].setCustomValidity('error');
    }

    if (phone && /^\d{7,15}$/.test(phone)) {
        $('#phone')[0].setCustomValidity('');
    } else {
        $('#phone')[0].setCustomValidity('error');
    }

    if (email && /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email)) {
        $('#email')[0].setCustomValidity('');
    } else {
        $('#email')[0].setCustomValidity('error');
    }

    if (address) {
        $('#address')[0].setCustomValidity('');
    } else {
        $('#address')[0].setCustomValidity('error');
    }


    if (!form.checkValidity()) {
        $(form).addClass('was-validated');
        return;
    }

    $(form).addClass('was-validated');

    const deleveryOptions = {
        payMethod,
        cardId,
        reciverName,
        phone,
        email,
        address,
        instructions
    }

    requestToServer('Store/BuyNow', 'post', deleveryOptions, (data) => {
        if (data) {
            showMessage('تمت العملية بنجاح سيتم تحويلك للصفحة الرئيسية خلال 3 ثوان');
            setTimeout(() => {
                window.location = '../home';
            }, 3000);
        }
    });
}


function addCard(e) {
    e.preventDefault();
    e.stopPropagation();
    const form = e.target;
    $(form).removeClass('was-validated');

    const cardNumber = $('#cardNumber').val();
    const holderName = $('#cardName').val();
    const expiryDate = $('#expiryDate').val();
    const cvc = $('#cvc').val();

    if (cardNumber && /^\d{16}$/.test(cardNumber)) {
        $('#cardNumber')[0].setCustomValidity('');
    } else {
        $('#cardNumber')[0].setCustomValidity('error');
    }

    if (holderName) {
        $('#cardName')[0].setCustomValidity('');
    } else {
        $('#cardName')[0].setCustomValidity('error');
    }

    if (expiryDate && /^(0[1-9]|1[0-2])([0-9]{2})$/.test(expiryDate)) {
        $('#expiryDate')[0].setCustomValidity('');
    } else {
        $('#expiryDate')[0].setCustomValidity('error');
    }

    if (cvc && /^\d{3,4}$/.test(cvc)) {
        $('#cvc')[0].setCustomValidity('');
    } else {
        $('#cvc')[0].setCustomValidity('error');
    }


    if (!form.checkValidity()) {
        $(form).addClass('was-validated');
        return;
    }

    $(form).addClass('was-validated');

    const newCard = {
        number: cardNumber,
        holderName,
        expiryDate,
        cvc
    }

    requestToServer('Store/AddCard', 'post', newCard, (data) => {
        if (data) {
            $('#addCardForm').removeClass('was-validated');
            $('#cardNumber').val('');
            $('#cardName').val('');
            $('#expiryDate').val('');
            $('#cvc').val('');
            showMessage('تمت اضافة البطاقة بنجاح');
            getCards();
        }
    });
}

function deleteCard(id) {
    if (confirmMessage('هل تريد حذف البطاقة؟', () => {
        requestToServer(`Store/DeleteCard?cardId=${id}`, 'get', '', (data) => {
            if (data) {
                showMessage('تمت العملية بنجاح');
                getCards();
            } else {
                showMessage('فشلت العملية');
            }
        });
    }, () => { })) { }
}