$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../welcome');
            return;
        }
        requestToServer('Store/GetProducts', 'get', '', (data) => {
            if (data) {
                fillGrid(data, 'products');
            }
        });

    })
});

let imageLoadingCount = 0