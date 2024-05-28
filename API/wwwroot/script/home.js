$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        if (!islogin) {
            redirectToPage('../welcome');
            return;
        }
        requestToServer('Store/GetTodayDeals', 'get', '', (data) => {
            requestToServer('Store/GetTopProducts', 'get', '', (data2) => {
                if (data2) {
                    fillGrid(data2, 'topSeller');
                }
            });
            if (data) {
                fillGrid(data, 'todayDeals');
            }
        });
    })
});

let imageLoadingCount = 0

