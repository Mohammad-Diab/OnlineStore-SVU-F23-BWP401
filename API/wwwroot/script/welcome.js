$(document).ready(function () {
    requestToServer('Users/IsLogin', 'get', '', (userFullName) => {
        if (userFullName) {
            redirectToPage('../home');
        }
    });
});

document.documentElement.style.setProperty('--animate-duration', '1s');