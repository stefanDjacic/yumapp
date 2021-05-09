////<script>
////    //signalR connection
////        var connection = new signalR.HubConnectionBuilder()
////            .configureLogging(signalR.LogLevel.Debug)
////            .withUrl('/User')
////            .withAutomaticReconnect()
////            .build();

////        connection.start().then(function () {
////            console.log("SignalR connected!")
////        }).catch(function (err) {
////            console.log(err.toString())
////        });
////    //signalR conneciton

////    //Start of method called by server
////        connection.on('AddNewNotificationsFE', function (userId) {
////        $.ajax({
////            type: 'GET',
////            url: `/User/GetNotifications/${userId}`,
////            success: function (notifications) {

////                console.log(notifications);

////                var ddm = document.getElementById('dropdown-menu');
////                ddm.innerHTML = '';

////                notifications.forEach((element) => {
////                    var div = document.createElement('div');
////                    div.classList.add('dropdown-item', 'dropdown-item-notification', 'p-2');

////                    if (element.notificationText.includes('followed')) {
////                        var html = `
////                            <a class="notification-link" href="/User/Profile/${element.idForRedirecting}">
////                                <figure class="d-flex mb-1">
////                                    <img src="${element.initiatorPhotoPath}" alt="User photo" class="dropdown-item-notification-photo rounded-circle">
////                                    <figcaption class="ml-3">
////                                    ${element.notificationText}
////                                    </figcaption>
////                                </figure>
////                                <p class="small mb-0">
////                                ${element.timeOfNotification}
////                                </p>
////                            </a>
////                            `;
////                    } else {
////                        var html = `
////                            <a class="notification-link" href="/User/SinglePost/${element.idForRedirecting}">
////                                <figure class="d-flex mb-1">
////                                    <img src="${element.initiatorPhotoPath}" alt="User photo" class="dropdown-item-notification-photo rounded-circle">
////                                    <figcaption class="ml-3">
////                                    ${element.notificationText}
////                                    </figcaption>
////                                </figure>
////                                <p class="small mb-0">
////                                ${element.timeOfNotification}
////                                </p>
////                            </a>
////                            `;
////                    }
////                    console.log(html);
////                    div.innerHTML += html;

////                    ddm.appendChild(div);
////                    $('#notification-alarm').text(' ').removeClass('bg-transparent').addClass('bg-danger');
////                });
////            },
////            error: function (response) {
////                alert(response.responseText);
////            }
////        })
////    });
////    //End of method called by server

////     //Start of ajax call for liking a post
////        var postIdForLike;
////        var userIdForLike;
////            function YumAPost(btn) {
////        postIdForLike = $(btn).attr('id');
////                userIdForLike = $(btn).attr('data-userid');

////                $.ajax({
////        type: 'POST',
////                    url: `/User/YumAPost/${postIdForLike}`,
////                    success: function (response) {
////        $(`#${postIdForLike}`).remove();
////                        $(`#yum-counter-${postIdForLike}`).text(response).addClass('text-white');
////                        $(`#yum-counter-${postIdForLike}`).parent().addClass('bg-success');

////                        connection.invoke('AddNewNotificationsBE', userIdForLike.toString()).catch(function (err) {
////                            return console.log(err.toString());
////                        });
////                    },
////                    error: function (response) {
////        alert(response.responseText);
////                    }
////                });
////            }
////        //End of ajax call for liking a post
////</script>