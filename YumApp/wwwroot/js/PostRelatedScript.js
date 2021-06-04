//Start of adding comment to the post
    var commentHtml
    connection.on('AddCommentToPostFE', function (commentModel) {
        console.log(commentModel);

        var allCommentsDiv = document.getElementById(`all-comments-${commentModel.postId}`);
        var divForComment = document.createElement('div');
        divForComment.classList.add('d-flex', 'mb-3');

        commentHtml = `
                    <div>
                        <a href="/User/Profile/${commentModel.commentator.id}">
                            <img src="${commentModel.commentator.photoPath}" class="comment-user-photo" alt="User photo" />
                        </a>
                    </div>
                    <div class="comment-user bg-light ml-2 p-2 d-flex align-items-center flex-column">
                        <a href="/User/Profile/${commentModel.commentator.id}" class="align-self-start">
                            <p class="mb-1 font-weight-bold">
                                ${commentModel.commentator.firstName} ${commentModel.commentator.lastName}
                            </p>
                        </a>
                        <p class="mb-0 align-self-start">
                            ${commentModel.content}
                        </p>
                        </div>
                    </div>
                `;

        divForComment.innerHTML += commentHtml;
        allCommentsDiv.appendChild(divForComment);
    });
//End of adding comment to the post

//Start of ajax call for liking a post
    var postIdForLike;
    var userIdForLike;
    function YumAPost(btn) {
        postIdForLike = $(btn).attr('id');
        userIdForLike = $(btn).attr('data-userid');

        $.ajax({
            type: 'POST',
            url: `/User/YumAPost/${postIdForLike}`,
            success: function (response) {
                $(`#${postIdForLike}`).remove();
                $(`#yum-counter-${postIdForLike}`).text(response).addClass('text-white');
                $(`#yum-counter-${postIdForLike}`).parent().addClass('bg-success');

                connection.invoke('AddNewNotificationsBE', userIdForLike.toString()).catch(function (err) {
                    return console.log(err.toString());
                });
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
//End of ajax call for liking a post

//Start of ajax call for posting a new comment
    var postIdForComment;
    var textAreaValue;
    function PostAComment(btn) {
        postIdForComment = $(btn).attr('data-postid');
        textAreaValue = $(`#textarea-comment-${postIdForComment}`).val();

        $.ajax({
            type: 'POST',
            url: `/User/PostAComment/${postIdForComment}`,
            contentType: 'application/x-www-form-urlencoded',
            data: { commentText: textAreaValue },
            /*data: JSON.stringify({ commentText : textAreaValue }),*/
            success: function (commentModel) {
                console.log(commentModel);

                connection.invoke('AddNewNotificationsBE', `${commentModel.appUserId.toString()}`).catch(function (err) {
                    return console.log(err.toString());
                });

                connection.invoke('AddCommentToPostBE', commentModel).catch(function (err) {
                    return console.log(err.toString());
                });

                $(`#textarea-comment-${postIdForComment}`).val('');
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
//End of ajax call for posting a new comment

//Start of ajax call for reporting a post
    function ReportAPost(btn) {
        var postId = $(btn).attr('data-postid');

        $.ajax({
            type: 'POST',
            url: `/User/ReportAPost/${postId}`,
            success: function () {            
                alert('Post reported!');
            },
            error: function () {
                alert('There was a problem with reportiing a post, please try again later.');
            }
        });
    }
//End of ajax call for reporting a post

////Start of ajax call for reporting a post
//function RepostAPost(btn) {                  //CHANGE WHOLE FUNCTION
//    var postId = $(btn).attr('data-postid');

//    $.ajax({
//        type: 'POST',
//        url: `/User/ReportAPost/${postId}`,
//        success: function () {
//            connection.invoke('AddNewGroupNotificationsBE').catch(function (err) {
//                return console.log(err.toString());
//            });
//            alert('Post reported!');
//        },
//        error: function () {
//            alert('There was a problem with reportiing a post, please try again later.');
//        }
//    });
//}
////End of ajax call for reporting a post

////Start of ajax call for removing a post
//function RemoveAPost(btn) {
//    var postId = $(btn).attr('data-postid');

//    $.ajax({
//        type: 'POST',
//        url: `/Admin/RemoveAPost/${postId}`,
//        success: function (response) {
//            window.location.href = response.redirectToUrl;
//            alert('Post removed!');
//        },
//        error: function (response) {
//            alert(response.responseText);
//        }
//    });
//}
////End of ajax call for removing a post