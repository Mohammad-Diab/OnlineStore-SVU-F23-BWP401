$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (islogin) => {
        setTimeout(() => {
            if (islogin) {
                redirectToPage('./home');
                return
            }
            redirectToPage('./welcome');
        }, 2000);
    })
});