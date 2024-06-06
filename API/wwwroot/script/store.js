$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../welcome');
            return;
        }
        getAllProducts();
    })
});

function getAllProducts() {
    requestToServer('Store/GetMyProducts', 'get', '', (data) => {
        let table = $('#product-table');
        table.empty();
        let htmlVal = '';
        if (data?.length) {
            for (let i = 0; i < data.length; i++) {
                const el = data[i];
                htmlVal += '<tr>'
                htmlVal += `<td>${i + 1}</td>`
                htmlVal += `<td>${el.name}</td>`
                htmlVal += `<td>${el.description}</td>`
                htmlVal += `<td>${el.warranty} يوم</td>`
                htmlVal += `<td class="text-end">${el.price} ليرة سورية</td>`
                htmlVal += `<td>${el.availableStock}</td>`
                htmlVal += `<td>${el.purchaseCount}</td>`

                htmlVal += '<td>';
                htmlVal += `<button class="btn btn-sm btn-danger" onclick="deleteProduct('${el.id}')">حذف</button>`
                htmlVal += '</td>';

                htmlVal += '</tr>'
            }
        } else {
            htmlVal += '<tr><td colspan="8" class="text-center">'
            htmlVal += 'لم تقم باضافة اي عنصر بعد'
            htmlVal += '</td></tr>'
        }
        table.html(htmlVal);
    })
}

function deleteProduct(id) {
    if (!confirm("هل تريد حذف المنتج الحالي")) {
        return;
    }
    requestToServer(`Store/DeleteProduct?id=${id}`, 'get', '', (result) => {
        if (result) {
            getAllProducts();
        }
    })
}

(() => {
    'use strict'
    const form = document.querySelector('form')
    form.addEventListener('submit', event => {
        event.preventDefault()
        event.stopPropagation()
        form.classList.add('was-validated');

        if (!form.checkValidity()) {
            return;
        }
        const name = $('#name').val();
        const description = $('#description').val();
        const warranty = $('#warranty').val();
        const price = $('#price').val();
        const availableStock = $('#stock').val();
        const image = $("#image").get(0).files[0];

        var formData = new FormData();
        formData.append("name", name);
        formData.append("description", description);
        formData.append("warranty", warranty);
        formData.append("price", price);
        formData.append("availableStock", availableStock);
        formData.append("image", image);

        requestToServer('Store/CreateProduct', 'post', formData, (result) => {
            if (result) {
                getAllProducts();
                $('#addProductModal').modal('hide');
                form.reset();
                form.classList.remove('was-validated');
            } else {
                showMessage("فشل حفظ المنتج")
            }
        }, false);
    }, false)
})()