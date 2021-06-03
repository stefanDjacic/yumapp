//Start of method called by server for group notifications
connection.on('AddNewGroupNotificationsBE', function () {

});
//End of method called by server for group notifications

//Start of method called by server for notifications
    connection.on('AddNewNotificationsFE', function (userId) {
        $.ajax({
            type: 'GET',
            url: `/User/GetNotifications/${userId}`,
            success: function (notifications) {

                console.log(notifications);

                var ddm = document.getElementById('dropdown-menu');
                ddm.innerHTML = '';

                notifications.forEach((element) => {
                    var div = document.createElement('div');
                    div.classList.add('dropdown-item', 'dropdown-item-notification', 'p-2');

                    if (element.notificationText.includes('followed')) {
                        var html = `
                                    <a class="notification-link" href="/User/Profile/${element.idForRedirecting}">
                                        <figure class="d-flex mb-1">
                                            <img src="${element.initiatorPhotoPath}" alt="User photo" class="dropdown-item-notification-photo rounded-circle">
                                            <figcaption class="ml-3">
                                            ${element.notificationText}
                                            </figcaption>
                                        </figure>
                                        <p class="small mb-0">
                                        ${element.timeOfNotification}
                                        </p>
                                    </a>
                                `;
                    } else {
                        var html = `
                                    <a class="notification-link" href="/User/SinglePost/${element.idForRedirecting}">
                                        <figure class="d-flex mb-1">
                                            <img src="${element.initiatorPhotoPath}" alt="User photo" class="dropdown-item-notification-photo rounded-circle">
                                            <figcaption class="ml-3">
                                            ${element.notificationText}
                                            </figcaption>
                                        </figure>
                                        <p class="small mb-0">
                                        ${element.timeOfNotification}
                                        </p>
                                    </a>
                                `;
                    }
                    console.log(html);
                    div.innerHTML += html;

                    ddm.appendChild(div);
                    $('#notification-alarm').text(' ').removeClass('bg-transparent').addClass('bg-danger');
                });
            },
            error: function (response) {
                alert(response.responseText);
            }
        })
    });
//End of method called by server  for notifications

//For removing indicator for new received notifications
$(document).ready(function () {
    $('#dropdownMenuLink').click(function () {
        $('#notification-alarm').text('').removeClass('bg-danger').addClass('bg-transparent');
    });
});
//End of new notifications indicator