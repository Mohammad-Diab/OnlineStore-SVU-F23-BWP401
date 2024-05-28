$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (userFullName) => {
        if (userFullName) {
            redirectToPage('../home');
        }
    });
});